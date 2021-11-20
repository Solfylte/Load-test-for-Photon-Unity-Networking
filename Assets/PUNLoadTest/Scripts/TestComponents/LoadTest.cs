using System;
using System.Collections;
using UnityEngine;

namespace PunLoadTest
{
    [RequireComponent(typeof(PhotonView))]
    public class LoadTest : MonoBehaviour, ILoadTest
    {
        private const string PrefabName = "PunLoadTest";

        private LoadTestConfiguration configuration;
        private PhotonView photonView;
        private ISpawner spawner;

        public bool IsRun { get; private set; }

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
        }
    }
}
