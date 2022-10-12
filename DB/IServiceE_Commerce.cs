using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFServer
{
    [ServiceContract]
    public interface IServiceE_Commerce
    {
        [OperationContract]
        void Connection();
       
        [OperationContract]
        User LoginUser(string email, string password);

        [OperationContract]
        bool Sign_up(User u);

        [OperationContract]
        bool AddAddress(Address a, User u);

        [OperationContract]
        bool DeleteAddress(Address a, User u);

        [OperationContract]
        bool UpdateAddress(Address a, User u);

        [OperationContract]
        Address GetAddress(User u);

        [OperationContract]
        Admin LoginAdmin(string email, string password);

        [OperationContract]
        List<User> GetUsers();

        [OperationContract]
        bool AddProduct(Product p);

        [OperationContract]
        List <Product> GetProducts();

        [OperationContract]
        bool DeleteProduct(Product p);

        [OperationContract]
        bool UpdateUserWallet(User u, float price, bool add);

        [OperationContract]
        bool UpdateProduct(Product p, int quantity);

        [OperationContract]
        bool BuyProduct(Product p, User u, int quantity, Address a);

        [OperationContract]
        List<Order> GetOrders();
    }
}
