using System;
using System.Runtime.Serialization;

namespace WindowsFormsApp1
{
    [DataContract]
    public class DataJson
    {
        [DataMember]
        public DateTime firstDate { get; set; }

        [DataMember]
        public DateTime secondDate { get; set; }

        [DataMember]
        public decimal price { get; set; }
    }
}
