using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunLoadTest.UI
{
    public static class Timer
    {
        public static string FormatedTime
        {
            get
            {
                int minutes = ((int)Time.realtimeSinceStartup) / 60;
                int seconds = ((int)Time.realtimeSinceStartup) % 60;
                return string.Format("{0:D2}:{1:D2}", minutes, seconds);
            }
        }
    }
}
