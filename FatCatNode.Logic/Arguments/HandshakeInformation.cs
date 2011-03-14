using System.Net;
using System.Runtime.Serialization;

namespace FatCatNode.Logic.Arguments
{
    [DataContract]
    public class HandshakeInformation
    {
        public static HandshakeInformation Empty = new HandshakeInformation();

        [DataMember]
        public string RemoteNodeId { get; set; }

        [DataMember]
        public IPAddress RemoteNodeAddress { get; set; }

        public override bool Equals(object obj)
        {
            var otherObj = obj as HandshakeInformation;

            if (obj == null || GetType() != obj.GetType() || otherObj == null)
            {
                return false;
            }

            return RemoteNodeId == otherObj.RemoteNodeId && RemoteNodeAddress == otherObj.RemoteNodeAddress;
        }

        public override int GetHashCode()
        {
            string hash = string.Format("{0}{1}", RemoteNodeId, RemoteNodeAddress);

            return hash.GetHashCode();
        }

        public static bool operator ==(HandshakeInformation rightHandSide, HandshakeInformation leftHandSide)
        {
            return OneSideOrTheOtherNull(rightHandSide, leftHandSide) ? BothSidesNull(rightHandSide, leftHandSide) : rightHandSide.Equals(leftHandSide);
        }

        private static bool BothSidesNull(HandshakeInformation rightHandSide, HandshakeInformation leftHandSide)
        {
            return ReferenceEquals(rightHandSide, null) && ReferenceEquals(leftHandSide, null);
        }

        private static bool OneSideOrTheOtherNull(HandshakeInformation rightHandSide, HandshakeInformation leftHandSide)
        {
            return ReferenceEquals(rightHandSide, null) || ReferenceEquals(leftHandSide, null);
        }

        public static bool operator !=(HandshakeInformation rightHandSide, HandshakeInformation leftHandSide)
        {
            return OneSideOrTheOtherNull(rightHandSide, leftHandSide) ? OnlyOneSideNull(rightHandSide, leftHandSide) : !rightHandSide.Equals(leftHandSide);
        }

        private static bool OnlyOneSideNull(HandshakeInformation rightHandSide, HandshakeInformation leftHandSide)
        {
            return !ReferenceEquals(rightHandSide, null) || !ReferenceEquals(leftHandSide, null);
        }
    }
}