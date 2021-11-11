using ExitGames.Client.Photon;
using System;
using UnityEngine;

namespace PunLoadTest.UI
{
    public class ShortStatsGUI : MonoBehaviour
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
            if (PhotonNetwork.networkingPeer.TrafficStatsEnabled != isStatsOn)
                PhotonNetwork.networkingPeer.TrafficStatsEnabled = isStatsOn;

            if (isStatsWindowOn)
                statsRect = GUILayout.Window(WindowId, this.statsRect, ShortStatsWindow, "Short stats");
        }

        private void ShortStatsWindow(int windowID)
        {
            AddCompositeLabel("Time: ", GetCurrentTime());

            if (isShowFps)
                AddCompositeLabel("FPS: ", fpsCounter.Average.ToString());

            GUILayout.Box("Bandwidth");
            if (isShowTraffic)
            {
                AddCompositeLabel("In: ", trafficCounter.IncomingBandwidth.ToString("0.00"), " kBytes/sec");
                AddCompositeLabel("Out: ", trafficCounter.OutgoingBandwidth.ToString("0.00"), " kBytes/sec");
            }

            GUI.DragWindow();
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

        private string GetCurrentTime()
        {
            int minutes = ((int)Time.realtimeSinceStartup) / 60;
            int seconds = ((int)Time.realtimeSinceStartup) % 60;
            return string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
    }
}
