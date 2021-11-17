using System;
using System.Collections;
using UnityEngine;

namespace PunLoadTest
{
    [RequireComponent(typeof(PhotonView))]
    public class LoadTest : MonoBehaviour
    {
        private LoadTestConfiguration configuration;
        private PhotonView photonView;
        private ISpawner spawner;

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            configuration = LoadTestConfiguration.Instance;
            photonView = GetComponent<PhotonView>();
            spawner = GetComponent<Spawner>();
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(5f);

            if (PhotonNetworkFacade.IsMasterClient)
                RunTest();
            else
                PhotonNetworkFacade.RPC(photonView, nameof(RunTest), PunLoadTest.RpcTarget.MasterClient);
        }

        [PunRPC]
        public void RunTest()
        {
            spawner.SpawnObjects(400);
        }
    }
}
