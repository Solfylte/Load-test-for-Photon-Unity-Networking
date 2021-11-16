using System;
using UnityEngine;

namespace PunLoadTest
{
    public interface IPUNConnection
    {
        event Action OnJoined;
    }

    public interface IMovementController
    {
        event Action<Transform> OnArrivedToDestination;
    }

    namespace UI
    {
        public interface IFpsCounter
        {
            int Average { get; }
        }

        public interface ITrafficCounter
        {
            float IncomingBandwidth { get; }
            float OutgoingBandwidth { get; }
        }
    }
}
