using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer
{
    //Class related to the Product object with its constructors and fields.

    [DataContract]
    public class Product
    {
        [DataMember]
        public string name;
        [DataMember]
        public string brand;
        [DataMember]
        public string description;
        [DataMember]
        public string img;
        [DataMember]
        public float price;
        [DataMember]
        public int quantity;

        public Product()
        {
        }

        public Product(string name,  string brand, string description, string img, float price, int quantity)
        {
            this.name = name;
            this.brand = brand;
            this.description = description;
            this.img = img;
            this.price = price;
            this.quantity = quantity;
        }
    }
}
