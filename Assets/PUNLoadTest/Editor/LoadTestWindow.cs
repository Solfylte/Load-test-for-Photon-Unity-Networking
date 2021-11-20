using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using PunLoadTest;

public class LoadTestWindow : EditorWindow
{
	private LoadTestConfiguration config;
	private ILoadTest loadTest;

	private int objectsCount = 50;
	private int testTime = 60;
	private bool isLoopInstantiating = false;
	private bool isRPCSync = false;

	private Color background;

	private bool isTestCanBeRun;
	private bool isTestCanBeInterrupted;

	[MenuItem("Tools/Testing/PUN load test", false, 20)]
	internal static void OpenLoadTestWindow()
	{
		LoadTestWindow window = (LoadTestWindow)EditorWindow.GetWindow<LoadTestWindow>(false, "PUN load test", true);
		window.Show();
	}

    private void OnEnable()
    {
		config = LoadTestConfiguration.Instance;
	}

    void OnGUI()
    {
		background = GUI.backgroundColor;

		CheckTestAccessibility();

		DrawButtons();
		DrawConfiguration();
		DrawInfoMessage();
	}

    private void CheckTestAccessibility()
    {
		isTestCanBeRun = PhotonNetworkFacade.InRoom && Application.isPlaying;
		isTestCanBeInterrupted = Application.isPlaying && loadTest != null && loadTest.IsRun;
	}

    private void DrawButtons()
    {
		GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

		GUI.color = Color.cyan;

		EditorGUI.BeginDisabledGroup(!isTestCanBeRun);
		if (GUILayout.Button("RUN", GUILayout.Width(150f), GUILayout.Height(50f)))
			Run();
		EditorGUI.EndDisabledGroup();

		GUILayout.Space(5f);

		GUI.color = new Color(0.75f,0.2f,0.2f);

		EditorGUI.BeginDisabledGroup(!isTestCanBeInterrupted);
		if (GUILayout.Button("INTERRUPT", GUILayout.Width(150f), GUILayout.Height(50f)))
			Interrupt();
		EditorGUI.EndDisabledGroup();

		GUI.color = background;

		GUILayout.EndHorizontal();
	}

    private void Run()
    {
		loadTest = LoadTest.Instance;
		LoadTest.Instance.Run(testTime, objectsCount, isLoopInstantiating, isRPCSync);
	}

	private void Interrupt()
	{
		loadTest = LoadTest.Instance;
		loadTest.Interrupt();
	}

	private void DrawConfiguration()
	{
		EditorGUILayout.Space(10f);
		GUI.color = Color.cyan;
		EditorGUILayout.HelpBox("Configuration", MessageType.None);
		GUI.color = background;

		objectsCount = EditorGUILayout.IntSlider("Objects count: ", objectsCount, config.CountClamp.Min, config.CountClamp.Max);
		testTime = EditorGUILayout.IntField("Test time: ", testTime);

		EditorGUILayout.BeginHorizontal();
		isLoopInstantiating = EditorGUILayout.Toggle("Loop Instant.: ", isLoopInstantiating);
		isRPCSync = EditorGUILayout.Toggle("Sync via RPC: ", isRPCSync);
		EditorGUILayout.EndHorizontal();
	}

	private void DrawInfoMessage()
	{
		if(isLoopInstantiating)
			EditorGUILayout.HelpBox("Loop Instantiating - forces PUN to generate new objects each time, instead of teleporting old ones.", MessageType.Info);

		if(isRPCSync)
			EditorGUILayout.HelpBox("Sync via RPC - position synchronization by sending RPC, instead of normal way.", MessageType.Info);

		if(!isTestCanBeRun)
			EditorGUILayout.HelpBox("For test start, application should be in playing mode and connected to room.", MessageType.Warning);
	}
}
