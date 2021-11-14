using System;
using UnityEngine;
#if IS_PUN1
using Photon;
#elif IS_PUN2
using Photon.Pun;
using Photon.Realtime;
#endif

namespace PunLoadTest
{
#if IS_PUN1 || IS_PUN2
    /// <summary>
    /// Works on the principle - who first joined the server, he creates a room. Compatible with PUN 1 and 2.
    /// </summary>
#if IS_PUN1
    public class SimplePUNConnection : PunBehaviour, IPUNConnection
#elif IS_PUN2
    public class SimplePUNConnection : MonoBehaviourPunCallbacks, IPUNConnection
#endif
    {
        public event Action OnJoined;

        private const string ROOM_NAME = "TestRoom";

        [Header("Times per second PhotonNetwork should send a package")]
        [SerializeField] private int sendRate = 20;
        [Space]
        [Header("Times per second OnPhotonSerialize should be called on PhotonViews")]
        [SerializeField] private int serializationRate = 10;

        private void Awake()
        {
            SetupPhotonNetwork();
        }

        private void SetupPhotonNetwork()
        {
            PhotonNetworkFacade.SendRate = sendRate;
            PhotonNetworkFacade.SerializationRate = serializationRate;
        }

        void Start()
        {
            Debug.Log("> Connecting to Photon...");

            // PhotonNetwork.playerName = "Player " + UnityEngine.Random.Range(1000, 10000);
            PhotonNetworkFacade.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("> Connected.");

            if (!PhotonNetworkFacade.InLobby)
            {
                Debug.Log("> Joining to lobby...");
                PhotonNetwork.JoinLobby();
            }
            else
                JoinRoom();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("> Joined to lobby.");

            if (RoomHasBeenCreated())
                JoinRoom();
            else
                CreateRoom();
        }

        private bool RoomHasBeenCreated() => PhotonNetworkFacade.CountOfRooms > 0;

        private void JoinRoom()
        {
            Debug.Log("> Joining to room...");
            PhotonNetwork.JoinRoom(ROOM_NAME);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("> Joined to room.");
            OnJoined?.Invoke();
        }

        private void CreateRoom()
        {
            Debug.Log("> Room creating...");
            PhotonNetwork.CreateRoom(ROOM_NAME, new RoomOptions(), TypedLobby.Default);
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("> New room created.");
        }
    }
#endif
}
