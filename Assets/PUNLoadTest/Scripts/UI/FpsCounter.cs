using UnityEngine;
using UnityEngine.UI;

namespace PunLoadTest.UI
{
    public class FpsCounter : MonoBehaviour, IFpsCounter
    {
        private float sampleTime;
        private int frameCount;
        private int averageFps;

        public int Average => averageFps;

        private void Update()
        {
            frameCount++;
            if (Time.unscaledTime > sampleTime)
            {
                sampleTime = Time.unscaledTime + 1f;
                averageFps = frameCount;
                frameCount = 0;
            }
        }
    }
}
