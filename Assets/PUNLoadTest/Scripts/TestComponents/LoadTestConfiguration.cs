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
    public string TestObjectTVsyncName => testObjectPUN1TVSync.name;
    public string TestObjectRPCSyncName => testObjectPUN1RPCSync.name;
#elif IS_PUN2
    public string TestObjectTVsyncName => testObjectPUN2TVSync.name;
    public string TestObjectRPCSyncName => testObjectPUN2RPCSync.name;
#endif

    public float SpawnStep => spawnStep;
    public float SpawnDelay => spawnDelay;
    public (int Min, int Max) CountClamp => (minCount, maxCount);

    [Header("Prefabs")]
    [SerializeField] private GameObject testObjectPUN1TVSync;
    [SerializeField] private GameObject testObjectPUN1RPCSync;   
    [SerializeField] private GameObject testObjectPUN2TVSync;
    [SerializeField] private GameObject testObjectPUN2RPCSync;
    [Header("Test values")]
    [SerializeField] private float spawnStep = 1f;
    [SerializeField] private float spawnDelay = 0.5f;
    [Header("Test windows values")]
    [SerializeField] private int minCount = 1;
    [SerializeField] private int maxCount = 100;

}
