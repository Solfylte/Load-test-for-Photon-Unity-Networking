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
        private const string PUN1_SCRIPTING_DEFINE_SYMBOLS = "IS_PUN1";
        private const string PUN2_SCRIPTING_DEFINE_SYMBOLS = "IS_PUN2";

        static string pun1TypeName = "Photon.MonoBehaviour";
        static string pun2TypeName = "Photon.Pun.MonoBehaviourPun";

        public static PUNVersion DetectedPUNVersion { get; private set; }

        static PUNCompatibilityController()
        {
            bool isPUN1 = IsTypeExist(pun1TypeName);
            bool isPUN2 = IsTypeExist(pun2TypeName);

            if (isPUN1)
                SwitchToPUN1();
            else if(isPUN2)
                SwitchToPUN2();
        }

        private static void SwitchToPUN1()
        {
            Debug.Log(">>SwitchToPUN1");
            // var scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(Application.platform);

            ScriptingDefineEditor defineEditor = new ScriptingDefineEditor(EditorUserBuildSettings.selectedBuildTargetGroup);
            defineEditor.Add(PUN1_SCRIPTING_DEFINE_SYMBOLS);

            DetectedPUNVersion = PUNVersion.PUN1;
        }

        private static void SwitchToPUN2()
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, 
                                                                PUN1_SCRIPTING_DEFINE_SYMBOLS);
            DetectedPUNVersion = PUNVersion.PUN1;
        }

        private static bool IsTypeExist(string fullTypeName)
        {
            var type = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from myType in assembly.GetTypes()
                        where myType.FullName == fullTypeName
                        select myType).FirstOrDefault();

            return type != null;
        }
    }
}
