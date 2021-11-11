using System;

namespace PunLoadTest
{
    public interface IPUNConnection
    {
        event Action OnJoined;
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
