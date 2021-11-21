using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using PunLoadTest;
using System.Text;

public class LoadTestWindow : EditorWindow
{
	private LoadTestConfiguration config;
	private LoadTest loadTest;

	private int objectsCount = 50;
	private int testTime = 60;
	private bool isLoopInstantiating = false;
	private bool isRPCSync = false;

	private bool isTestCanBeRun;
	private bool isTestCanBeInterrupted;
	private int selectedReportIndex;
	private StringBuilder reportText = new StringBuilder();

	private Color background;
	private Texture2D runIcon;
	private Texture2D interruptIcon;
	private Texture2D copyIcon;

	[MenuItem("Tools/Testing/PUN load test", false, 20)]
	internal static void OpenLoadTestWindow()
	{
		LoadTestWindow window = (LoadTestWindow)EditorWindow.GetWindow<LoadTestWindow>(false, "PUN load test", true);
		window.Show();
	}

    private void Awake()
    {
		runIcon = Resources.Load("Editor/runIcon", typeof(Texture2D)) as Texture2D;
		interruptIcon = Resources.Load("Editor/interruptIcon", typeof(Texture2D)) as Texture2D;
		copyIcon = Resources.Load("Editor/copyIcon", typeof(Texture2D)) as Texture2D;		
	}

    private void OnEnable()
    {
		config = LoadTestConfiguration.Instance;
	}

    private void Update()
	{
		if (loadTest == null)
			loadTest = FindObjectOfType<LoadTest>();

		CheckTestAccessibility();
		this.Repaint();
	}

    private void OnGUI()
    {
		background = GUI.backgroundColor;

		DrawButtons();
		DrawConfiguration();
		DrawInfoMessage();

		DrawReports();
	}

    private void CheckTestAccessibility()
    {
		isTestCanBeRun = PhotonNetworkFacade.InRoom && Application.isPlaying;
		isTestCanBeInterrupted = Application.isPlaying && loadTest!=null && loadTest.IsRun;
	}

    private void DrawButtons()
    {
		GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

		GUI.color = background;
		if (isTestCanBeRun)
			GUI.color = Color.cyan;

		EditorGUI.BeginDisabledGroup(!isTestCanBeRun);
		if (GUILayout.Button(runIcon, GUILayout.Width(150f), GUILayout.Height(50f)))
			Run();
		EditorGUI.EndDisabledGroup();

		GUILayout.Space(5f);

		GUI.color = background;

		if (isTestCanBeInterrupted)
			GUI.color = new Color(0.75f,0.2f,0.2f);

		EditorGUI.BeginDisabledGroup(!isTestCanBeInterrupted);
		if (GUILayout.Button(interruptIcon, GUILayout.Width(150f), GUILayout.Height(50f)))
			Interrupt();
		EditorGUI.EndDisabledGroup();

		GUI.color = background;

		GUILayout.EndHorizontal();
	}

    private void Run()
    {
		reportText.Clear();
		loadTest.Run(testTime, objectsCount, isLoopInstantiating, isRPCSync);
	}

	private void Interrupt()
	{
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
		
		if(loadTest==null)
			EditorGUILayout.HelpBox("LoadTest component must be preadded on scene", MessageType.Error);
	}

	private void DrawReports()
	{
		EditorGUILayout.Space(10f);
		GUI.color = Color.cyan;
		EditorGUILayout.HelpBox("Reports", MessageType.None);
		GUI.color = background;

		if (loadTest == null || loadTest.Reports.Count == 0)
			return;

		if (selectedReportIndex >= loadTest.Reports.Count)
			selectedReportIndex = 0;

		selectedReportIndex = EditorGUILayout.Popup(selectedReportIndex, GetDevicesNames());

		ReportInfo report = loadTest.Reports[selectedReportIndex];
		UpdateReportText(report);

		GUILayout.Space(5f);
		EditorGUILayout.BeginHorizontal(GUI.skin.textArea);
		EditorGUILayout.LabelField(reportText.ToString(), GUILayout.Height(120f));
		if (GUILayout.Button(copyIcon, GUILayout.Width(36f), GUILayout.Height(36f)))
			CopySelectedReport();
		EditorGUILayout.EndHorizontal();
	}

    private void UpdateReportText(ReportInfo report)
    {
		reportText.Clear();
		reportText.AppendLine($"Device:\t{selectedReportIndex+1}. {report.DeviceName}");
		reportText.AppendLine($"Is Master:\t{report.IsMasterClient}");
		reportText.AppendLine($"Average FPS:\t{report.Fps}");
		reportText.AppendLine($"Total In bytes:\t{report.InBytesDelta}");
		reportText.AppendLine($"Total Out bytes:\t{report.OutBytesDelta}");
	}

    private string[] GetDevicesNames()
    {
		string[] devices = new string[loadTest.Reports.Count];

		for (int i = 0; i < loadTest.Reports.Count; i++)
			devices[i] = $"{i+1}. {loadTest.Reports[i].DeviceName}";

		return devices;
	}

	private void CopySelectedReport()
	{
		EditorGUIUtility.systemCopyBuffer = reportText.ToString();
	}
}
