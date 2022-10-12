using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer
{
    //Class related to the Address object with its constructors and fields.

    [DataContract]
    public class Address
    {
        [DataMember]
        public string user_email;
        [DataMember]
        public string country;
        [DataMember]
        public string city;
        [DataMember]
        public int postCode;
        [DataMember]
        public string address ;

        public Address(string user_email, string country, string city, int postCode, string address)
        {
            this.user_email = user_email;
            this.country = country;
            this.city = city;
            this.postCode = postCode;
            this.address = address;
        }
    }
}
