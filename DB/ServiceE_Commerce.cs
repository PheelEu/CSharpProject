using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WCFServer
{
    public class ServiceE_Commerce : IServiceE_Commerce
    {
        //Connection Function
        public void Connection()
        {
            return;
        }
        
        //USER CORRELATED FUNCTIONS

        //This is the LoginUser function, which takes two arguments (email, password) checks
        //if there is any corrispondence to an actual registered user inside the database
        //if there is, and both email and password match, a user is returned
        public User LoginUser(string email, string password)
        {
            string query = "SELECT * FROM uuser WHERE email ='" + email + "' and password = '" + password + "'";

            try
            {
                using (SqlDataReader reader = DBConnection.Read(query))
                {
                    if (reader.Read())
                    {
                        return new User(reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), float.Parse(reader[5].ToString()));
                    }
                }
            }
            catch (Exception) { }
            return null;
        }

        //This is the Sign_up function, which takes an user as input and sends a request
        //to the database, to store this new user inside the database, returns true if successful
        //false if not
        public bool Sign_up(User u)
        {
            string query = "INSERT INTO uuser (name, surname, email, password, wallet) VALUES('" + u.name + "','" + u.surname + "','" + u.email + "','" + u.password + "', 0)";
            return DBConnection.Execute(query);
        }

        //This is the GetUsers function, takes no arguments as input but creates a list of all the 
        //registered users, taken from the database with an appropriate query.
        //The function returns a list of users, or an exeption is thrown and shown in the WCF console.
        public List<User> GetUsers()
        {
            List<User> listUsers = new List<User>();
            string query = "SELECT * FROM uuser";

            try
            {
                using (SqlDataReader reader = DBConnection.Read(query))
                {
                    while (reader.Read())
                    {
                        listUsers.Add(new User(reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), float.Parse(reader[5].ToString())));
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return listUsers;
        }
        
        //This is the UpdateUserWallet function, it takes three input arguments
        //The first one is he user, the second one the price/amount and the third is a boolean.
        //If the boolean is true the amount given as input will be added to the user wallet.
        //If the boolean is false the amount will be removed from the user wallet (after a purchase). 
        //Returns the query result. 
        public bool UpdateUserWallet(User u, float price, bool add)
        {
            string query;
            if(add)
            {   
                query = "UPDATE uuser SET wallet ='" + (u.wallet + price) + "' WHERE email ='" + u.email + "'";
            }
            else
            {
                query = "UPDATE uuser SET wallet ='" + (u.wallet - price) + "' WHERE email ='" + u.email + "'";
            }
            return DBConnection.Execute(query);
        }


        //ADDRESS RELATED FUNCTIONS

        //This is the AddAddress function, this function takes two arguments as input, one is the address
        //and the other one is the user corresponding to this specific address.
        //The function sends a query to the database to add the given address.
        public bool AddAddress(Address a,User u)
        {
            string query = "INSERT INTO address (user_email, country, city, post_code, address) VALUES('" + u.email + "','" + a.country + "','" + a.city + "','" + a.postCode + "','" + a.address + "')";
            return DBConnection.Execute(query);
        }
        
        //This is the DeleteAddress function, this function takes two arguments as input, one is the address
        //and the other one is the user corresponding to this specific address.
        //The function sends a query to the database to add the given address to remove it from the database.
        public bool DeleteAddress(Address a, User u)
        {
            string query = "DELETE address WHERE user_email ='" + u.email + "' and address ='" + a.address +"'";
            return DBConnection.Execute(query);
        }

        //This is the UpdateAddress function, this function takes two arguments as input, one is the address
        //and the other one is the user corresponding to this specific address.
        //The function sends a query to the database to add the given address to update it in the database.
        public bool UpdateAddress(Address a, User u)
        {
            string query = "UPDATE address SET country = '" + a.country + "', city = '" + a.city + "', post_code = '" + a.postCode + "', address = '" + a.address + "' WHERE user_email ='" + u.email + "'";
            return DBConnection.Execute(query);
        }

        //This is the GetAddress function, this function takes one argument as input, a user.
        //The function sends a query to the database to get the address
        //of a specific user from the database.
        //Throws an exeption and shows it in the WCF Console if the DB query fails.
        public Address GetAddress(User user)
        {
            string query = "SELECT * FROM address WHERE user_email = '" + user.email + "'";
            try
            {
                using (SqlDataReader reader = DBConnection.Read(query))
                {
                    while (reader.Read())
                    {
                         return new Address(reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), int.Parse(reader[4].ToString()), reader[5].ToString());
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex);}
            return null;
        }

        //ADMIN RELATED FUNCTIONS

        //This is the LoginAdmin function, it takes two arguments as input (email, password) and makes a 
        //query to the DB, if the email and password match a result in the DB, then a new admin gets created
        public Admin LoginAdmin(string email, string password)
        {
            string query = "SELECT * FROM admin WHERE email ='" + email + "' and password = '" + password + "'";

            try
            {
                using (SqlDataReader reader = DBConnection.Read(query))
                {
                    if (reader.Read())
                    {
                        return new Admin(reader[1].ToString(), reader[2].ToString());
                    }
                }
            }
            catch (Exception) { }
            return null;
        }

        //PRODUCT RELATED FUNCTIONS

        //This is the AddProduct function, it takes a product as input argument.
        //A query is made to the DB and if successful all the information regarding the product
        //is saved in the DB in a new row corresponding to the newly added product.
        //True is returned if the query is made, false if something goes wrong and the product is not added
        public bool AddProduct(Product p)
        {
            string query = "INSERT product (name, brand, description, img, price, quantity) VALUES('" + p.name + "','" + p.brand + "','" + p.description + "','" + p.img + "','" + p.price + "','" + p.quantity + "')";
            return DBConnection.Execute(query);
        }

        //This is the GetProducts function, this function takes no arguments as input.
        //The function sends a query to the database to get the all the products registered
        //in the DB.
        //If the DB query fails an exeption its thrown and its shown in the WCF Console .
        public List<Product> GetProducts() {
            List<Product> listProducts = new List<Product>();
            string query = "SELECT * FROM product";

            try
            {
                using (SqlDataReader reader = DBConnection.Read(query))
                {
                    while (reader.Read())
                    {
                        listProducts.Add(new Product(reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), float.Parse(reader[5].ToString()), int.Parse(reader[6].ToString())));
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            return listProducts;
        }

        //This is the delete product function, it takes a product as input argument.
        //This function sends a query to remove a product from the database.
        //If the product gets deleted a boolean true is returned, if not it returns a false one.
        public bool DeleteProduct(Product p)
        {
            string query = "DELETE product WHERE name ='" + p.name + "'";
            return DBConnection.Execute(query);
        }

        
        //This is the UpdateProduct function, it takes two arguments as input (product and quantity)
        //This function is used to update the quantity of a specified product in the DB, after a purchase for example.
        //Returns the result of query.
        public bool UpdateProduct(Product p, int quantity)
        {
            int stock=p.quantity-quantity;
            string query = "UPDATE product SET quantity ='" + stock +"' WHERE name ='" + p.name + "'"; 
            return DBConnection.Execute(query);
        }
        
        //This is the BuyProduct function, it takes four input arguments, one is the product to buy, the second is the
        //user making this purchase, the third is the quantity of product the user is trying to get and the last argument
        //is the address to send the product to.
        //When a user makes a purchase a new order is inserted into the DB, with all the information needed.
        //After adding the order to the DB the user wallet gets updated, the amount needed is removed from it and
        //the product stock gets changed to the new availability of the product.
        //A boolean is returned. True if everthing is updated, false if any of the query fail.
        public bool BuyProduct(Product p, User u, int quantity,Address a)
        {
            float price=p.price * quantity;
            string query = "INSERT INTO oorder (user_email,product_name,quantity,price, country, city, post_code, address,order_date) VALUES('" + u.email +
                "','" + p.name + "','" + quantity + "','" + price + "','" + a.country + "','" + a.city + "','" + a.postCode + "','" + a.address + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "')";

            if (UpdateUserWallet(u, price, false))
            {
                if (UpdateProduct(p, quantity))
                {
                    return DBConnection.Execute(query);
                }
            }
            return false;
        }

        //ORDER RELATED FUNCTIONS

        //This is the GetOrders function, it takes no arguments as input and creates a list of orders.
        //With a specific query it takes all the orders from the database and creates an order with
        //information regarding alse the product of this order (by making a query and creating a product instance)
        //All the orders are added to a list, and this list is returned, if something fails an exception is thrown and
        //the output is shown in the console of the WCF
        public List<Order> GetOrders()
        {
            List<Order> listOrders = new List<Order>();
            string query = "SELECT * FROM oorder";
            try
            {
                using (SqlDataReader reader = DBConnection.Read(query))
                {
                    while (reader.Read())
                    {
                        string query2 = "SELECT * FROM product WHERE name ='" + reader[2].ToString() + "'";
                        try
                        {
                            using (SqlDataReader reader2 = DBConnection.Read(query2))
                            {
                                if (reader2.Read())
                                {
                                    Product p = new Product(reader2[1].ToString(), reader2[2].ToString(), reader2[3].ToString(), reader2[4].ToString(), float.Parse(reader2[5].ToString()), int.Parse(reader2[6].ToString()));
                                    if (p != null)
                                    {
                                        listOrders.Add(new Order(int.Parse(reader[0].ToString()), reader[1].ToString(),
                                                                  int.Parse(reader[3].ToString()), float.Parse(reader[4].ToString()),
                                                                  reader[5].ToString(), reader[6].ToString(), int.Parse(reader[7].ToString()),
                                                                  reader[8].ToString(), DateTime.Parse(reader[9].ToString()), p));
                                    }
                                    else
                                    {
                                        Console.WriteLine("Product is null!");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return listOrders;
        }

    }
}
