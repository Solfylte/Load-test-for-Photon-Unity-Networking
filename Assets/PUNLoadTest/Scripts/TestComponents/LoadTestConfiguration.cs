using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadTestConfiguration", menuName = "PUN Load Test/Configuration")]
public class LoadTestConfiguration : ScriptableObject
{
    #region singleton
    private static LoadTestConfiguration instance;

    public static LoadTestConfiguration Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<LoadTestConfiguration>(nameof(LoadTestConfiguration));

            return instance;
        }
    }
    #endregion

#if IS_PUN1
    public string TestObjectName => testObjectPUN1Prefab.name;
#elif IS_PUN2
    public string TestObjectName => testObjectPUN2Prefab.name;
#endif

    public float SpawnStep => spawnStep;
    public float SpawnDelay => spawnDelay;
    public int ObjectsCount { get => objectsCount; set => objectsCount = value; }
    public bool IsLoopInstantiating { get => isLoopInstantiating; set => isLoopInstantiating = value; }

    [Header("Prefabs")]
    [SerializeField] private GameObject testObjectPUN1Prefab;
    [SerializeField] private GameObject testObjectPUN2Prefab;
    [Header("Test values")]
    [SerializeField] private float spawnStep = 1f;
    [SerializeField] private float spawnDelay = 0.5f;
    [SerializeField] private int objectsCount = 400;
    [SerializeField] private bool isLoopInstantiating = false;

}
