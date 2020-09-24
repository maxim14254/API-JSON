using System;
using System.Runtime.Serialization;

namespace WindowsFormsApp1
{
    [DataContract]
    class AllRecords
    {
        [DataMember]
        public int allrecords { get; set; }
    }
}
