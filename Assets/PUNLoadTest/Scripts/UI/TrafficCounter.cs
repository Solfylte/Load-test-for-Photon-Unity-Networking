using UnityEngine;
using UnityEngine.UI;

namespace PunLoadTest.UI
{
    public class TrafficCounter : MonoBehaviour, ITrafficCounter
    {
        public float IncomingBandwidth => incomingBandwidth *0.001f;
        public float OutgoingBandwidth => outgoingBandwidth * 0.001f;

        private float incomingBandwidth;
        private float outgoingBandwidth;

        private float sampleTime;
        private float lastInTotalPacketBytes;
        private float lastOutTotalPacketBytes;

        void Update()
        {
            if (Time.unscaledTime > sampleTime)
            {
                sampleTime = Time.unscaledTime + 1f;
                incomingBandwidth = PhotonNetworkFacade.TotalIncomingBytes - lastInTotalPacketBytes;
                outgoingBandwidth = PhotonNetworkFacade.TotalOutgoingBytes - lastOutTotalPacketBytes;
                
                lastInTotalPacketBytes = PhotonNetworkFacade.TotalIncomingBytes;
                lastOutTotalPacketBytes = PhotonNetworkFacade.TotalOutgoingBytes;
            }
        }
    }
}
