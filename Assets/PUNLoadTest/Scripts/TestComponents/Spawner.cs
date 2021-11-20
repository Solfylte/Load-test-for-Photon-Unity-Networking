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
        private bool isRPCSync;

        private List<GameObject> testObjects; 

        private void Awake()
        {
            testObjects = new List<GameObject>();
            configuration = LoadTestConfiguration.Instance;
            spawnTimeout = new WaitForSeconds(configuration.SpawnDelay);
        }

        public void SpawnObjects(int count, bool isLoopInstantiating, bool isRPCSync)
        {
            this.objectsCount = count;
            this.isLoopInstantiating = isLoopInstantiating;
            this.isRPCSync = isRPCSync;

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

            testObjects.Add(testObject);

            if (isLoopInstantiating)
            {
                IMovementController movementController = testObject.GetComponent<IMovementController>();
                movementController.OnArrivedToDestination += MovementController_OnArrivedToDestination;
            }
        }

        private void MovementController_OnArrivedToDestination(PhotonView testObjectPhotonView)
        {
            testObjects.Remove(testObjectPhotonView.gameObject);
            PhotonNetwork.Destroy(testObjectPhotonView);
            SpawnObject(testObjectPhotonView.transform.position);            
        }

        public void DestroyAll()
        {
            StopAllCoroutines();
            for (int i = 0; i < testObjects.Count; i++)
                PhotonNetwork.Destroy(testObjects[i]);

            testObjects.Clear();
        }
    }
}
