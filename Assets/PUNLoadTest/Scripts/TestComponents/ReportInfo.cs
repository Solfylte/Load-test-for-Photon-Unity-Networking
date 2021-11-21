using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunLoadTest
{
    public class ReportInfo
    {
        public ReportInfo(int fps, int inBytesDelta, int outBytesDelta)
        {
            DeviceName = SystemInfo.deviceName;
            IsMasterClient = PhotonNetworkFacade.IsMasterClient;
            Fps = fps;
            InBytesDelta = inBytesDelta;
            OutBytesDelta = outBytesDelta;
        }

        public ReportInfo(string receivedString)
        {
            UnpackFromString(receivedString);
        }

        public string DeviceName { get; private set; }
        public bool IsMasterClient { get; private set; }
        public int Fps { get; private set; }
        public int InBytesDelta { get; private set; }
        public int OutBytesDelta { get; private set; }

        public string PackToString()    // To avoid castom type serialization
        {
            string result = $"{DeviceName}," +
                            $"{IsMasterClient}," +
                            $"{Fps}," +
                            $"{InBytesDelta}," +
                            $"{OutBytesDelta}";

            return result;
        }

        private void UnpackFromString(string receivedString)
        {
            string[] results = receivedString.Split(',');
            DeviceName = results[0];
            IsMasterClient = Convert.ToBoolean(results[1]);
            Fps = Convert.ToInt32(results[2]);
            InBytesDelta = Convert.ToInt32(results[3]);
            OutBytesDelta = Convert.ToInt32(results[4]);
        }
    }
}
