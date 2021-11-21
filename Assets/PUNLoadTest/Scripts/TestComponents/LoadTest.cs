using System;
using System.Collections;
using UnityEngine;
using PunLoadTest.UI;
using System.Collections.Generic;

namespace PunLoadTest
{
    [RequireComponent(typeof(PhotonView))]
    public class LoadTest : MonoBehaviour, ILoadTest
    {
        public bool IsRun { get; private set; }

        public IReadOnlyList<ReportInfo> Reports => reports;
        public List<ReportInfo> reports = new List<ReportInfo>();

        private const string PrefabName = "PunLoadTest";

        private LoadTestConfiguration configuration;
        private PhotonView photonView;
        private ISpawner spawner;

        private IFpsCounter fpsCounter;

        private int startTotalIncomingBytes;
        private int startTotalOutdoingBytes;

        private static ILoadTest instance;
        public static ILoadTest Instance
        {
            get 
            {
                if (instance == null)
                {
                    GameObject loadTestGO = PhotonNetwork.InstantiateSceneObject(PrefabName,
                                                                                 Vector3.zero,
                                                                                 Quaternion.identity,
                                                                                 0,
                                                                                 null);

                    instance = loadTestGO.GetComponent<ILoadTest>();
                }

                return instance;
            }
        }

        private void Awake()
        {
            configuration = LoadTestConfiguration.Instance;
            photonView = GetComponent<PhotonView>();
            spawner = GetComponent<ISpawner>();
            fpsCounter = GetComponent<IFpsCounter>();
        }

        [PunRPC]
        public void Run(float testTime, int count, bool isLoopInstantiating, bool isRPCSync)
        {
            if (PhotonNetworkFacade.IsMasterClient)
            {
                Interrupt();
                StartCoroutine(DelayedEnd(testTime));
                IsRun = true;
                spawner.SpawnObjects(count, isLoopInstantiating, isRPCSync);

                startTotalIncomingBytes = PhotonNetworkFacade.TotalIncomingBytes;
                startTotalOutdoingBytes = PhotonNetworkFacade.TotalOutgoingBytes;
            }
            else
            {
                PhotonNetworkFacade.RPC(photonView, nameof(Run), PunLoadTest.RpcTarget.MasterClient,
                                        testTime, count, isLoopInstantiating, isRPCSync);
            }
        }

        [PunRPC]
        public void Interrupt()
        {
            if (PhotonNetworkFacade.IsMasterClient)
            {
                StopAllCoroutines();
                IsRun = false;
                spawner.DestroyAll();
            }
            else
            {
                PhotonNetworkFacade.RPC(photonView, nameof(Interrupt), PunLoadTest.RpcTarget.MasterClient);
            }
        }

        private IEnumerator DelayedEnd(float testTime)
        {
            yield return new WaitForSeconds(testTime);
            Interrupt();

            CreateReport();
        }
        
        private void CreateReport()
        {
            ReportInfo report = new ReportInfo(fpsCounter.TotalAverage,
                                                PhotonNetworkFacade.TotalIncomingBytes - startTotalIncomingBytes,
                                                PhotonNetworkFacade.TotalOutgoingBytes - startTotalOutdoingBytes);

            PhotonNetworkFacade.RPC(photonView, nameof(ReceiveReport), RpcTarget.All, report.PackToString());
        }

        [PunRPC]
        public void ReceiveReport(string receivedString)
        {
            ReportInfo report = new ReportInfo(receivedString);
            reports.Add(report);

            Debug.Log($">> Received report: device: {report.DeviceName}" +
                                           $" IsMaster={report.IsMasterClient}" +
                                           $" FPS={report.Fps}" +
                                           $" TotalIncomingBytes = {report.InBytesDelta}" +
                                           $" TotalOutgoingBytes = {report.OutBytesDelta}");
        }
    }
}
