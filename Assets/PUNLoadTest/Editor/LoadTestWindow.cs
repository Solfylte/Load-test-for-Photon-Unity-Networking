using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LoadTestWindow : EditorWindow
{
	[MenuItem("Tools/Testing/PUN load test", false, 20)]
	internal static void ShowWindow()
	{
		var myself = GetWindow<LoadTestWindow>(false, "PUN load test", true);
		myself.minSize = new Vector2(500, 300);
	}
}
