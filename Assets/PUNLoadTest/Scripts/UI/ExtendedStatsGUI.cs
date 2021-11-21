using ExitGames.Client.Photon;
using System;
using UnityEngine;
using UnityEngine.Profiling;
#if IS_PUN2
using Photon.Pun.UtilityScripts;
using Photon.Pun;
#endif

namespace PunLoadTest.UI
{
    public class ExtendedStatsGUI : MonoBehaviour
    {
        private Rect statsRect = new Rect(10, 100, 200, 50);
        private int WindowId = 101;

        private bool isStatsWindowOn = true;
        private bool isStatsOn = true;

        private IFpsCounter fpsCounter;
        private ITrafficCounter trafficCounter;

        private bool isShowFps;
        private bool isShowTraffic;

        private GUIStyle labelStyle;
        private GUIStyle valueStyle;

        private void Awake()
        {
            InitializeComponents();
            InitializeStyle();
            InitializePUNStatGUI();
        }

        private void InitializeComponents()
        {
            fpsCounter = GetComponent<IFpsCounter>();
            trafficCounter = GetComponent<ITrafficCounter>();
            isShowFps = fpsCounter != null;
            isShowTraffic = trafficCounter != null;
        }

        private void InitializeStyle()
        {
            labelStyle = new GUIStyle();
            labelStyle.fontSize = 16;
            labelStyle.normal.textColor = Color.white;

            valueStyle = new GUIStyle(labelStyle);
            valueStyle.fontStyle = FontStyle.Bold;
        }

        // can't be add on scene directly, because the project is released without PUN1/PUN2 sources.
        private void InitializePUNStatGUI()
        {
            PhotonStatsGui photonStatsGui = gameObject.AddComponent<PhotonStatsGui>();
            photonStatsGui.trafficStatsOn = true;
        }

        private void Start()
        {
            if (this.statsRect.x <= 0)
                this.statsRect.x = Screen.width - this.statsRect.width;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
            {
                isStatsWindowOn = !isStatsWindowOn;
                isStatsOn = true;
            }
        }

        private void OnGUI()
        {
            if (PhotonNetworkFacade.TrafficStatsEnabled != isStatsOn)
                PhotonNetworkFacade.TrafficStatsEnabled = isStatsOn;

            if (isStatsWindowOn)
                statsRect = GUILayout.Window(WindowId, this.statsRect, ExtendedStatsWindow,
                                                $"Extended PUN stats \n({SystemInfo.deviceName})");
        }

        private void ExtendedStatsWindow(int windowID)
        {
            GUILayout.Space(15f);
            AddCompositeLabel("Time: ", Timer.FormatedTime);

            DrawPerformanceGUI();
            DrawBandwidthGUI();
            DrawMemoryGUI();

            GUI.DragWindow();
        }

        private void DrawPerformanceGUI()
        {
            if (isShowFps)
                AddCompositeLabel("FPS: ", fpsCounter.Average.ToString());

            int playersCount = 0;
            if (PhotonNetworkFacade.InRoom)
                playersCount = PhotonNetworkFacade.PlayersInRoom;

            AddCompositeLabel("Players: ", playersCount.ToString());
        }

        private void DrawBandwidthGUI()
        {
            GUILayout.Box("Bandwidth");
            if (isShowTraffic)
            {
                AddCompositeLabel("In:\t", trafficCounter.IncomingBandwidth.ToString("0.00"), " kbps");
                AddCompositeLabel("Out:\t", trafficCounter.OutgoingBandwidth.ToString("0.00"), " kbps");
            }
        }

        private void DrawMemoryGUI()
        {
            GUILayout.Box("Memory");
            AddCompositeLabel("Total reserved:\t", (Profiler.GetTotalReservedMemoryLong() / 1048576f).ToString("0.0"), " MB");
            AddCompositeLabel("Mono used:\t", (Profiler.GetMonoUsedSizeLong() / 1048576f).ToString("0.0"), " MB");
        }

        private void AddCompositeLabel(string prefix, string value, string sufix = "")
        {
            GUILayout.BeginHorizontal();
            if(prefix!="")
                GUILayout.Label(prefix, labelStyle);

            GUILayout.Label(value, valueStyle);

            if (sufix != "")
                GUILayout.Label(sufix, labelStyle);
            GUILayout.EndHorizontal();
        }
    }
}
