using System.Runtime.Serialization;

namespace WindowsFormsApp1
{
    [DataContract]
    class MinMax
    {
        [DataMember]
        public decimal min { get; set; }

        [DataMember]
        public decimal max { get; set; }
    }
}
