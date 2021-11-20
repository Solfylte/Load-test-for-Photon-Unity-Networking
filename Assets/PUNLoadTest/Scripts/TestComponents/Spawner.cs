using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunLoadTest
{
    public class Spawner : MonoBehaviour, ISpawner
    {
        private LoadTestConfiguration configuration;

        private WaitForSeconds spawnTimeout;
        private int objectsCount;
        private bool isLoopInstantiating;
        private Transform spawnPool;

        private void Awake()
        {
            InitializeComponents();
            CreateSpawnPool();
        }

        private void InitializeComponents()
        {
            configuration = LoadTestConfiguration.Instance;
            spawnTimeout = new WaitForSeconds(configuration.SpawnDelay);
        }

        private void CreateSpawnPool()
        {
            spawnPool = new GameObject("SpawnPool").transform;
        }

        public void SpawnObjects()
        {
            objectsCount = configuration.ObjectsCount;
            isLoopInstantiating = configuration.IsLoopInstantiating;

            StartCoroutine(SpawnObjectsDelayed());
        }

        private IEnumerator SpawnObjectsDelayed()
        {
            Vector3 position = Vector3.zero;
            int offsetX = 1;
            int offsetZ = 0;
            int spiraleStep = 0;
            int countInLine = 1;
            int x = 0, z  = 0;

            for (int i = 0; i < objectsCount; i++)
            {
                position.x = x * configuration.SpawnStep;
                position.z = z * configuration.SpawnStep;

                SpawnObject(position);
                yield return spawnTimeout;

                countInLine--;
                if (countInLine == 0)
                {
                    spiraleStep++;
                    countInLine = spiraleStep;
                    int bufer = offsetX;
                    offsetX = -offsetZ;
                    offsetZ = bufer;
                }

                x += offsetX;
                z += offsetZ;
            }
        }

        public void SpawnObject(Vector3 position)
        {
            GameObject testObject = PhotonNetwork.InstantiateSceneObject(configuration.TestObjectName,
                                                                         position,
                                                                         Quaternion.identity,
                                                                         0,
                                                                         null);

            testObject.transform.SetParent(spawnPool);

            if(isLoopInstantiating)
            {
                IMovementController movementController = testObject.GetComponent<IMovementController>();
                movementController.OnArrivedToDestination += MovementController_OnArrivedToDestination;
            }
        }

        private void MovementController_OnArrivedToDestination(PhotonView testObjectPhotonView)
        {
            PhotonNetwork.Destroy(testObjectPhotonView);
            SpawnObject(testObjectPhotonView.transform.position);            
        }
    }
}
