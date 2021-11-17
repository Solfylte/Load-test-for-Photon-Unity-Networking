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
        event Action<PhotonView> OnArrivedToDestination;
    }

    public interface ISpawner
    {
        void SpawnObjects(int count, bool isAlwaysInstantiatingNewObjects = false);
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
