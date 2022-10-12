using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer
{
    //Class related to the Admin object with its constructors and fields.
    [DataContract]
    public class Admin
    {
        [DataMember]
        public string email;
        [DataMember]
        public string password;

        public Admin(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }
}
