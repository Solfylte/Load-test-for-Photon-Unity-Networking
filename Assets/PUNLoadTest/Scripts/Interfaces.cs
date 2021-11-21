using System;
using System.Collections.Generic;
using UnityEngine;
#if IS_PUN2
using Photon.Pun;
#endif

namespace PunLoadTest
{
    public interface IPUNConnection
    {
        event Action OnJoined;
    }

    public interface ILoadTest
    {
        bool IsRun { get; }
        IReadOnlyList<ReportInfo> Reports { get; }
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
            /// <summary>
            /// Average FPS per 1 second sample
            /// </summary>
            int Average { get; }

            /// <summary>
            /// Average FPS since start of measurements
            /// </summary>
            int TotalAverage { get; }
        }

        public interface ITrafficCounter
        {
            float IncomingBandwidth { get; }
            float OutgoingBandwidth { get; }
        }
    }
}
