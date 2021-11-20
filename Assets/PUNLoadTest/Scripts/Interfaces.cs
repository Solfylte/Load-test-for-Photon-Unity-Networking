using System;
using UnityEngine;

namespace PunLoadTest
{
    public interface IPUNConnection
    {
        event Action OnJoined;
    }

    public interface ILoadTest
    {
        bool IsRun { get; }
        void Run(float testTime, int count, bool isLoopInstantiating, bool isRPCSync);
        void Interrupt();
    }

    public interface ISpawner
    {
        void SpawnObjects(int count, bool isLoopInstantiating, bool isRPCSync);
        void DestroyAll();
    }

    public interface IMovementController
    {
        event Action<PhotonView> OnArrivedToDestination;
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
