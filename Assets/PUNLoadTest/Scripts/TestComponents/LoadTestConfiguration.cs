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

    public string TestObjectName => testObjectPrefab.name;

    [SerializeField] private GameObject testObjectPrefab;
}
