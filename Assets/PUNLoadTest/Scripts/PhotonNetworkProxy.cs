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
    public static class PhotonNetworkProxy
    {
#if IS_PUN1
        public static int SendRate { get => PhotonNetwork.sendRate; set => PhotonNetwork.sendRate = value; }
        public static int SerializationRate { get => PhotonNetwork.sendRateOnSerialize; set => PhotonNetwork.sendRateOnSerialize = value; }
        public static bool IsMine(PhotonView photonView) => photonView.isMine;
        public static bool InLobby => PhotonNetwork.insideLobby;
        public static int CountOfRooms => PhotonNetwork.countOfRooms;
        public static void ConnectUsingSettings() => PhotonNetwork.ConnectUsingSettings("1.0.0");

        public static float TotalIncomingBytes => PhotonNetwork.networkingPeer.TrafficStatsIncoming.TotalPacketBytes;
        public static float TotalOutgoingBytes => PhotonNetwork.networkingPeer.TrafficStatsOutgoing.TotalPacketBytes;
#elif IS_PUN2
        public static int SendRate { get => PhotonNetwork.SendRate; set => PhotonNetwork.SendRate = value; }
        public static int SerializationRate { get => PhotonNetwork.SerializationRate; set => PhotonNetwork.SerializationRate = value; }
        public static bool IsMine(PhotonView photonView) => photonView.IsMine;
        public static bool InLobby => PhotonNetwork.InLobby;
        public static int CountOfRooms => PhotonNetwork.CountOfRooms;
        public static void ConnectUsingSettings() => PhotonNetwork.ConnectUsingSettings();

        public static float TotalIncomingBytes => PhotonNetwork.NetworkingClient.LoadBalancingPeer.TrafficStatsIncoming.TotalPacketBytes;
        public static float TotalOutgoingBytes => PhotonNetwork.NetworkingClient.LoadBalancingPeer.TrafficStatsOutgoing.TotalPacketBytes;
#endif
    }
}
