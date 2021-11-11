using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PunLoadTest.CompatibilityControl
{
    public class ScriptingDefineEditor
    {
        private BuildTargetGroup buildTargetGroup;
        private string[] defineSymbols;

        public ScriptingDefineEditor(BuildTargetGroup buildTargetGroup)
        {
            this.buildTargetGroup = buildTargetGroup;
            defineSymbols = GetDefineSymbols();
        }

        public bool Contain(string defineSymbol)
        {
            for (int i = 0; i < defineSymbols.Length; i++)
                if (defineSymbols[i] == defineSymbol)
                    return true;

            return false;
        }

        public void Add(string defineSymbol)
        {
            if (!Contain(defineSymbol))
            {
                string defineSymbolsLine = GetDefineSymbolsLine();
                defineSymbolsLine += $";{defineSymbol}";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defineSymbolsLine);

                defineSymbols = GetDefineSymbols();
            }
            else
                Debug.LogWarning($"Define '{defineSymbol}' already exist for '{buildTargetGroup}' target group.");
        }

        public void Remove(string defineSymbol)
        {
            if (Contain(defineSymbol))
            {
                List<string> newDefineSymbols = new List<string>();
                for (int i = 0; i < defineSymbols.Length; i++)
                    if (defineSymbols[i] != defineSymbol)
                        newDefineSymbols.Add(defineSymbols[i]);

                string defineSymbolsLine = ListToLine(newDefineSymbols);

                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defineSymbolsLine);
            }
            else
                Debug.LogError($"Unable to remove define '{defineSymbol}', " +
                                $"since '{buildTargetGroup}' target group don't contain it.");
        }

        private string[] GetDefineSymbols()
        {
            string defineSymbolsLine = GetDefineSymbolsLine();
            return defineSymbolsLine.Split(';');
        }

        private string GetDefineSymbolsLine()
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        }

        private string ListToLine(List<string> defineSymbols)
        {
            string line ="";
            for (int i = 0; i < defineSymbols.Count; i++)
                line += $"{defineSymbols[i]};";

            line = line.Substring(0, line.Length);

            return line;
        }
    }
}
