using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PunLoadTest.CompatibilityControl
{
    public enum PUNVersion {NONE, PUN1, PUN2 }

    [InitializeOnLoad]
    public class PUNCompatibilityController
    {
        private static Dictionary<PUNVersion, (string defineName, string typeName)> punVersions = new Dictionary<PUNVersion, (string, string)>() 
        { 
            { PUNVersion.PUN1, ("IS_PUN1", "Photon.MonoBehaviour")},
            { PUNVersion.PUN2, ("IS_PUN2", "Photon.Pun.MonoBehaviourPun")},
        };

        public static PUNVersion DetectedPUNVersion { get; private set; }

        static PUNCompatibilityController()
        {
            UpdatePUNVersion();
            Application.logMessageReceived += Application_logMessageReceived;
        }

        private static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Error && condition.Contains("CS1022"))
                UpdatePUNVersion();
        }

        private static void UpdatePUNVersion()
        {
            if (CheckVersion(PUNVersion.PUN1))
                SwitchToPUN(PUNVersion.PUN1);
            else if (CheckVersion(PUNVersion.PUN2))
                SwitchToPUN(PUNVersion.PUN2);
            else
                SwitchToNone();
        }

        private static bool CheckVersion(PUNVersion punVersion) => TypeChecker.IsTypeExist(punVersions[punVersion].typeName);

        private static void SwitchToNone()
        {
            DetectedPUNVersion = PUNVersion.NONE;
            ScriptingDefineEditor defineEditor = new ScriptingDefineEditor(EditorUserBuildSettings.selectedBuildTargetGroup);
            defineEditor.Remove(punVersions[PUNVersion.PUN1].defineName);
            defineEditor.Remove(punVersions[PUNVersion.PUN2].defineName);
        }

        private static void SwitchToPUN(PUNVersion punVersion)
        {
            ScriptingDefineEditor defineEditor = new ScriptingDefineEditor(EditorUserBuildSettings.selectedBuildTargetGroup);
            defineEditor.Add(punVersions[punVersion].defineName);

            DetectedPUNVersion = punVersion;
        }
    }
}
