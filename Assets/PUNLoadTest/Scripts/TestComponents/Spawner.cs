using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunLoadTest
{
    public class Spawner : MonoBehaviour, ISpawner
    {
        private LoadTestConfiguration configuration;
        private PhotonView photonView;

        private WaitForSeconds spawnWaitForSeconds;
        private int objectsCount = 400;
        private int spawnIndex;
        private bool isAlwaysInstantiatingNewObjects;
        private Transform spawnPool;

        private void Awake()
        {
            InitializeComponents();
            CreateSpawnPool();
        }

        private void InitializeComponents()
        {
            configuration = LoadTestConfiguration.Instance;
            spawnWaitForSeconds = new WaitForSeconds(configuration.SpawnDelay);
            photonView = GetComponent<PhotonView>();
        }

        private void CreateSpawnPool()
        {
            spawnPool = new GameObject("SpawnPool").transform;
        }

        public void SpawnObjects(int count, bool isAlwaysInstantiatingNewObjects = false)
        {
            this.objectsCount = count;
            this.isAlwaysInstantiatingNewObjects = isAlwaysInstantiatingNewObjects;
            StartCoroutine(SpawnObjectsDelayed());
        }

        private IEnumerator SpawnObjectsDelayed()
        {
            for (int i = 0; i < objectsCount; i++)
            {
                SpawnObject();
                yield return spawnWaitForSeconds;
            }
        }

        public void SpawnObject()
        {
            GameObject testObject = PhotonNetwork.InstantiateSceneObject(configuration.TestObjectName,
                                                                         GetSpawnObjectPosition(),
                                                                         Quaternion.identity,
                                                                         0,
                                                                         null);

            testObject.transform.SetParent(spawnPool);

            if(isAlwaysInstantiatingNewObjects)
            {
                IMovementController movementController = testObject.GetComponent<IMovementController>();
                movementController.OnArrivedToDestination += MovementController_OnArrivedToDestination;
            }
        }

        private void MovementController_OnArrivedToDestination(PhotonView testObjectPhotonView)
        {
            PhotonNetwork.Destroy(testObjectPhotonView);
            SpawnObject();            
        }

        private Vector3 GetSpawnObjectPosition()
        {
            Vector3 spawnPosition = configuration.FirstSpawnPoint;
            int spawnSide = spawnIndex % 2 == 0 ? 1 : -1;
            spawnPosition.x += configuration.SpawnStep * spawnIndex * spawnSide;

            if (spawnIndex >= configuration.SpawnWidht)
                spawnIndex = 0;
            else
                spawnIndex++;

            return spawnPosition;
        }
    }
}
