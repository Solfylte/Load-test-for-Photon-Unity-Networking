using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
#if IS_PUN2
using Photon.Pun;
using Photon.Realtime;
#endif

namespace PunLoadTest
{
    /// <summary>
    /// Facade for built-in PUN1/PUN2 compatibility, without multiples directives inside other code.
    /// </summary>
    public static class PhotonNetworkFacade
    {
#if IS_PUN1
        public static bool IsMine(PhotonView photonView) => photonView.isMine;
        public static int SendRate { get => PhotonNetwork.sendRate; set => PhotonNetwork.sendRate = value; }
        public static int SerializationRate { get => PhotonNetwork.sendRateOnSerialize; set => PhotonNetwork.sendRateOnSerialize = value; }
        public static bool InLobby => PhotonNetwork.insideLobby;
        public static bool InRoom => PhotonNetwork.inRoom;
        public static bool IsMasterClient => PhotonNetwork.isMasterClient;
        public static int CountOfRooms => PhotonNetwork.countOfRooms;
        public static int PlayersInRoom => PhotonNetwork.room.PlayerCount;
        public static float TotalIncomingBytes => PhotonNetwork.networkingPeer.TrafficStatsIncoming.TotalPacketBytes;
        public static float TotalOutgoingBytes => PhotonNetwork.networkingPeer.TrafficStatsOutgoing.TotalPacketBytes;

        public static void ConnectUsingSettings() => PhotonNetwork.ConnectUsingSettings("1.0.0");
        public static void RPC(PhotonView photonView,
                               string methodName,
                               PunLoadTest.RpcTarget targets,
                               params object[] param) => photonView.RPC(methodName, (PhotonTargets)targets, param);

#elif IS_PUN2
        public static bool IsMine(PhotonView photonView) => photonView.IsMine;
        public static int SendRate { get => PhotonNetwork.SendRate; set => PhotonNetwork.SendRate = value; }
        public static int SerializationRate { get => PhotonNetwork.SerializationRate; set => PhotonNetwork.SerializationRate = value; }
        public static bool InLobby => PhotonNetwork.InLobby;
        public static bool InRoom => PhotonNetwork.InRoom;
        public static bool IsMasterClient => PhotonNetwork.IsMasterClient;
        public static int CountOfRooms => PhotonNetwork.CountOfRooms;
        public static int CountOfPlayers => PhotonNetwork.CurrentRoom.PlayerCount;
        public static float TotalIncomingBytes => PhotonNetwork.NetworkingClient.LoadBalancingPeer.TrafficStatsIncoming.TotalPacketBytes;
        public static float TotalOutgoingBytes => PhotonNetwork.NetworkingClient.LoadBalancingPeer.TrafficStatsOutgoing.TotalPacketBytes;

        public static void ConnectUsingSettings() => PhotonNetwork.ConnectUsingSettings();
        public static void RPC(PhotonView photonView,
                               string methodName,
                               PunLoadTest.RpcTarget targets,
                               object[] param) => photonView.RPC(methodName, (Photon.Pun.RpcTarget)targets, param);
#endif
    }

    public enum RpcTarget
    {
        All,
        Others,
        MasterClient,
        AllBuffered,
        OthersBuffered,
        AllViaServer,
        AllBufferedViaServer
    }
}
