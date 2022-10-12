using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer
{
    //Class related to the User object with its constructors and fields.

    [DataContract]
    public class User
    {
        [DataMember]
        public string name;
        [DataMember]
        public string surname;
        [DataMember]
        public string email;
        [DataMember]
        public string password;
        [DataMember]
        public float wallet;

        public User(string name, string surname, string email, string password, float wallet)
        {
            this.name = name;
            this.surname = surname;
            this.email = email;
            this.password = password;
            this.wallet = wallet;
        }
    }
}
