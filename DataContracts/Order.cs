using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer
{
    //Class related to the Order object with its constructors and fields.
    [DataContract]
    public class Order
    {
        [DataMember]
        public int orderId;
        [DataMember]
        public string user_email;
        [DataMember]
        public int quantity;
        [DataMember]
        public float price;
        [DataMember]
        public string country;
        [DataMember]
        public string city;
        [DataMember]
        public int post_code;
        [DataMember]
        public string address;
        [DataMember]
        public DateTime orderDate;
        [DataMember]
        public Product product;

        public Order(int orderId, string user_email, int quantity, float price, string country, string city, int post_code, string address, DateTime orderDate, Product product)
        {
            this.orderId = orderId;
            this.user_email = user_email;
            this.quantity = quantity;
            this.price = price;
            this.country = country;
            this.city = city;
            this.post_code = post_code;
            this.address = address;
            this.orderDate = orderDate;
            this.product = product;
        }
    }
}
