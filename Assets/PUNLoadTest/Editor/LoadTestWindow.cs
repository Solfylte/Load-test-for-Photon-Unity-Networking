using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class LoadTestWindow : EditorWindow
{
	[MenuItem("Tools/Testing/PUN load test", false, 20)]
	internal static void OpenLoadTestWindow()
	{
		EditorWindow.GetWindow<LoadTestWindow>(false, "PUN load test", true);
	}

	internal void Initialize()
    {

	}
}
