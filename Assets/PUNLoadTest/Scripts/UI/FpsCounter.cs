using UnityEngine;
using UnityEngine.UI;

namespace PunLoadTest.UI
{
    public class FpsCounter : MonoBehaviour, IFpsCounter
    {
        private float sampleTime;
        private int frameCount;
        private int averageFps;
        private int totalAverageFps;

        public int Average => averageFps;
        public int TotalAverage => totalAverageFps;

        private void Update()
        {
            frameCount++;
            if (Time.unscaledTime > sampleTime)
            {
                sampleTime = Time.unscaledTime + 1f;
                averageFps = frameCount;
                totalAverageFps = (totalAverageFps + averageFps) / 2;
                frameCount = 0;
            }
        }
    }
}
