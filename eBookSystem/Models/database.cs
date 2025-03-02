using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace eBookSystem.Models
{
    public class Database
    {
        private readonly string _connectionString = "Data Source=HP-15S\\SQLEXPRESS;Initial Catalog=eBookSystemDB;Integrated Security=True;TrustServerCertificate=True";

        public string InsertBook(Book book)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Book (Title, Author, ISBN, Publisher, PublishedYear, Edition, Price, Quantity, Condition, Description, CoverImage) VALUES (@Title, @Author, @ISBN, @Publisher, @PublishedYear, @Edition, @Price, @Quantity, @Condition, @Description, @CoverImage)", con);

                    cmd.Parameters.AddWithValue("@Title", (object)book.Title ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Author", (object)book.Author ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ISBN", (object)book.ISBN ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Publisher", (object)book.Publisher ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PublishedYear", (object)book.PublishedYear ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Edition", (object)book.Edition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", (object)book.Price ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Quantity", (object)book.Quantity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Condition", (object)book.Condition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description", (object)book.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CoverImage", (object)book.CoverImage ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                    return "OK";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return ex.Message;
                }
            }
        }

        public List<Book> GetAllBooks()
        {
            var books = new List<Book>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Title,Author,ISBN,Publisher,PublishedYear, Price, Quantity, CoverImage, AverageRating FROM Book";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(new Book
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Author = reader.GetString(2),  // Ensure data is being read correctly
                                ISBN = reader.GetString(3),    // Ensure data is being read correctly
                                Publisher = reader.GetString(4),
                                PublishedYear = reader.GetInt32(5),
                                Price = reader.IsDBNull(6) ? (decimal?)null : reader.GetDecimal(6),
                                Quantity = reader.GetInt32(7),
                                CoverImage = reader["CoverImage"] as byte[],
                                AverageRating = reader["AverageRating"] as double? // Ensure it is read as nullable double
                            });
                        }
                    }
                }
            }
            return books;
        }

        public Book GetBookById(int id)
        {
            Book book = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Book WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        book = new Book
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.GetString(2),
                            ISBN = reader.GetString(3),
                            Publisher = reader.GetString(4),
                            PublishedYear = reader.GetInt32(5),
                            Edition = reader.GetString(6),
                            Price = reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7),
                            Quantity = reader.GetInt32(8),
                            Condition = reader.GetString(9),
                            Description = reader.GetString(10),
                            CoverImage = reader.IsDBNull(11) ? null : (byte[])reader[11],
                             AverageRating = reader["AverageRating"] as double?
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return book;
        }


        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Orders", con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var order = new Order
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetString(1),
                            FullName = reader.GetString(2),
                            Address = reader.GetString(3),
                            City = reader.GetString(4),
                            State = reader.GetString(5),
                            ZipCode = reader.GetString(6),
                            Country = reader.GetString(7),
                            TotalAmount = reader.GetDecimal(8),
                            OrderDate = reader.GetDateTime(9),
                            Status = reader.GetString(10),
                            OrderItems = GetOrderItems(reader.GetInt32(0))
                        };
                        orders.Add(order);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return orders;
        }

        public void DeleteOrder(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Orders WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
        public string UpdateBook(Book book)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Book SET Title = @Title, Author = @Author, ISBN = @ISBN, Publisher = @Publisher, PublishedYear = @PublishedYear, Edition = @Edition, Price = @Price, Quantity = @Quantity, Condition = @Condition, Description = @Description, CoverImage = @CoverImage WHERE Id = @Id", con);

                    cmd.Parameters.AddWithValue("@Title", (object)book.Title ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Author", (object)book.Author ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ISBN", (object)book.ISBN ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Publisher", (object)book.Publisher ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PublishedYear", (object)book.PublishedYear ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Edition", (object)book.Edition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", (object)book.Price ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Quantity", (object)book.Quantity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Condition", (object)book.Condition ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description", (object)book.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CoverImage", (object)book.CoverImage ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Id", book.Id);

                    cmd.ExecuteNonQuery();
                    return "OK";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return ex.Message;
                }
            }
        }

        public string DeleteBook(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Book WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();
                    return "OK";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return ex.Message;
                }
            }

        }

        public void CreateCart(string userEmail)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                // Retrieve the UserId (GUID) from AspNetUsers using email
                string getUserIdQuery = "SELECT Id FROM AspNetUsers WHERE NormalizedEmail = @NormalizedEmail";
                string userId;
                using (SqlCommand cmd = new SqlCommand(getUserIdQuery, con))
                {
                    cmd.Parameters.AddWithValue("@NormalizedEmail", userEmail.ToUpper());
                    userId = cmd.ExecuteScalar()?.ToString();

                    if (string.IsNullOrEmpty(userId))
                    {
                        throw new Exception($"User with email '{userEmail}' does not exist.");
                    }
                }

                // Insert into Cart if the UserId is valid
                string insertCartQuery = "INSERT INTO Cart (UserId) VALUES (@UserId)";
                using (SqlCommand cmd = new SqlCommand(insertCartQuery, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }



        public Cart GetCartByUserEmail(string email)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                // Query to fetch cart by UserId (retrieved via email)
                string query = @"
            SELECT Cart.Id, Cart.UserId
            FROM Cart
            INNER JOIN AspNetUsers ON AspNetUsers.Id = Cart.UserId
            WHERE AspNetUsers.NormalizedEmail = @NormalizedEmail";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@NormalizedEmail", email.ToUpper());

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cart
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetString(1)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public Cart GetCartByUserEmail_Single(string email)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                // Query to fetch cart and cart items with book details
                string query = @"
            SELECT Cart.Id AS CartId, Cart.UserId, 
                   CartItems.Id AS CartItemsId, CartItems.BookId, CartItems.Quantity, 
                   Book.Id AS BookId, Book.Title, Book.ISBN, Book.CoverImage, Book.Price, Book.Quantity AS BookStock
            FROM Cart
            INNER JOIN AspNetUsers ON AspNetUsers.Id = Cart.UserId
            LEFT JOIN CartItems ON Cart.Id = CartItems.CartId
            LEFT JOIN Book ON CartItems.BookId = Book.Id
            WHERE AspNetUsers.NormalizedEmail = @NormalizedEmail";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@NormalizedEmail", email.ToUpper());

                    using (var reader = cmd.ExecuteReader())
                    {
                        Cart cart = null;
                        while (reader.Read())
                        {
                            if (cart == null)
                            {
                                cart = new Cart
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CartId")),
                                    UserId = reader.GetString(reader.GetOrdinal("UserId")),
                                    CartItems = new List<CartItem>()
                                };
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("CartItemsId"))) // If CartItem exists
                            {
                                var cartItem = new CartItem
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CartItemsId")),
                                    CartId = reader.GetInt32(reader.GetOrdinal("CartId")),
                                    BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    Book = !reader.IsDBNull(reader.GetOrdinal("BookId")) ? new Book
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("BookId")),
                                        Title = reader.GetString(reader.GetOrdinal("Title")),
                                        ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                                        CoverImage = reader.IsDBNull(reader.GetOrdinal("CoverImage")) ? null : (byte[])reader["CoverImage"],
                                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                        Quantity = reader.GetInt32(reader.GetOrdinal("BookStock"))
                                    } : null
                                };

                                cart.CartItems.Add(cartItem);
                            }
                        }

                        return cart;
                    }
                }
            }
        }



        public List<Order> GetOrdersByUserId(string userId)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Orders WHERE UserId = @UserId", con);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var order = new Order
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetString(1),
                            FullName = reader.GetString(2),
                            Address = reader.GetString(3),
                            City = reader.GetString(4),
                            State = reader.GetString(5),
                            ZipCode = reader.GetString(6),
                            Country = reader.GetString(7),
                            TotalAmount = reader.GetDecimal(8),
                            OrderDate = reader.GetDateTime(9),
                            Status = reader.GetString(10),
                            OrderItems = GetOrderItems(reader.GetInt32(0))
                        };
                        orders.Add(order);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return orders;
        }
        public void InsertFeedback(int bookId, int rating)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Feedbacks (BookId, Rating) VALUES (@BookId, @Rating)", con);
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    cmd.ExecuteNonQuery();

                    UpdateBookAverageRating(bookId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
        public void AddFeedback(int orderId, int bookId, int rating)
    {
        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Feedbacks (OrderId, BookId, Rating) VALUES (@OrderId, @BookId, @Rating)", con);
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@BookId", bookId);
                cmd.Parameters.AddWithValue("@Rating", rating);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding feedback: " + ex.Message);
                throw;
            }
        }
    }

    public List<Feedback> GetFeedbacksByBookId(int bookId)
    {
        List<Feedback> feedbacks = new List<Feedback>();

        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Feedbacks WHERE BookId = @BookId", con);
                cmd.Parameters.AddWithValue("@BookId", bookId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    feedbacks.Add(new Feedback
                    {
                        Id = reader.GetInt32(0),
                        BookId = reader.GetInt32(1),
                        Rating = reader.GetInt32(2)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while getting feedbacks: " + ex.Message);
                throw;
            }
        }

        return feedbacks;
    }

    public void UpdateBookAverageRating(int bookId, double? averageRating)
    {
        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Books SET AverageRating = @AverageRating WHERE Id = @BookId", con);
                cmd.Parameters.AddWithValue("@AverageRating", (object)averageRating ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@BookId", bookId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the book's average rating: " + ex.Message);
                throw;
            }
        }
    }
        private void UpdateBookAverageRating(int bookId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT AVG(Rating) FROM Feedbacks WHERE BookId = @BookId", con);
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    var averageRating = cmd.ExecuteScalar();

                    cmd = new SqlCommand("UPDATE Books SET AverageRating = @AverageRating WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@AverageRating", averageRating);
                    cmd.Parameters.AddWithValue("@Id", bookId);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

        public Cart GetCartByUserId(string userId)
        {
            Cart cart = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Cart WHERE UserId = @UserId", con);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cart = new Cart
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetString(1),
                            CartItems = GetCartItems(reader.GetInt32(0))
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while getting the cart: " + ex.Message);
                }
            }
            return cart;
        }

        public List<CartItem> GetCartItems(int cartId)
        {
            List<CartItem> cartItems = new List<CartItem>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM CartItems WHERE CartId = @CartId", con);
                    cmd.Parameters.AddWithValue("@CartId", cartId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cartItems.Add(new CartItem
                        {
                            Id = reader.GetInt32(0),
                            CartId = reader.GetInt32(1),
                            BookId = reader.GetInt32(2),
                            Quantity = reader.GetInt32(3),
                            Book = GetBookById(reader.GetInt32(2))
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while getting cart items: " + ex.Message);
                }
            }
            return cartItems;
        }

        public void AddToCart(int cartId, int bookId, int quantity)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO CartItems (CartId, BookId, Quantity) VALUES (@CartId, @BookId, @Quantity)", con);
                cmd.Parameters.AddWithValue("@CartId", cartId);
                cmd.Parameters.AddWithValue("@BookId", bookId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE CartItems SET Quantity = @Quantity WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@Id", cartItemId);
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveFromCart(int cartItemId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM CartItems WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", cartItemId);
                cmd.ExecuteNonQuery();
            }
        }


        public void InsertOrder(Order order)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                string query = @"
            INSERT INTO [Orders] 
            (UserId, FullName, Address, City, State, ZipCode, Country, TotalAmount, OrderDate, Status)
            VALUES 
            (@UserId, @FullName, @Address, @City, @State, @ZipCode, @Country, @TotalAmount, @OrderDate, @Status);
            SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@UserId", order.UserId);
                cmd.Parameters.AddWithValue("@FullName", order.FullName);
                cmd.Parameters.AddWithValue("@Address", order.Address);
                cmd.Parameters.AddWithValue("@City", order.City);
                cmd.Parameters.AddWithValue("@State", order.State);
                cmd.Parameters.AddWithValue("@ZipCode", order.ZipCode);
                cmd.Parameters.AddWithValue("@Country", order.Country);
                cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@Status", order.Status);

                order.Id = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }



        public void InsertOrderItem(int orderId, int bookId, int quantity)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                string query = @"
            INSERT INTO [OrderItems] (OrderId, BookId, Quantity)
            VALUES (@OrderId, @BookId, @Quantity);";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@BookId", bookId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                cmd.ExecuteNonQuery();
            }
        }


        public Order GetLatestOrderByUserId(string userId)
        {
            Order order = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM [Order] WHERE UserId = @UserId ORDER BY Id DESC", con);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        order = new Order
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetString(1),
                            FullName = reader.GetString(2),
                            Address = reader.GetString(3),
                            City = reader.GetString(4),
                            State = reader.GetString(5),
                            ZipCode = reader.GetString(6),
                            Country = reader.GetString(7),
                            TotalAmount = reader.GetDecimal(8),
                            OrderDate = reader.GetDateTime(9),
                            Status = reader.GetString(10)
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while retrieving the latest order: " + ex.Message);
                    throw;
                }
            }

            return order;
        }

        public Order GetOrderById(int id)
        {
            Order order = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM [Orders] WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        order = new Order
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetString(1),
                            FullName = reader.GetString(2),
                            Address = reader.GetString(3),
                            City = reader.GetString(4),
                            State = reader.GetString(5),
                            ZipCode = reader.GetString(6),
                            Country = reader.GetString(7),
                            TotalAmount = reader.GetDecimal(8),
                            OrderDate = reader.GetDateTime(9),
                            Status = reader.GetString(10)
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return order;
        }

        public List<OrderItem> GetOrderItems(int orderId)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM OrderItems WHERE OrderId = @OrderId", con);
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        orderItems.Add(new OrderItem
                        {
                            Id = reader.GetInt32(0),
                            OrderId = reader.GetInt32(1),
                            BookId = reader.GetInt32(2),
                            Quantity = reader.GetInt32(3),
                            Book = GetBookById(reader.GetInt32(2))
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return orderItems;
        }

        public void ClearCart(int cartId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM CartItems WHERE CartId = @CartId", con);
                    cmd.Parameters.AddWithValue("@CartId", cartId);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while clearing the cart: " + ex.Message);
                }
            }
        }
        public List<SalesReport> GetOverallSalesReport(DateTime fromDate, DateTime toDate)
        {
            List<SalesReport> salesReports = new List<SalesReport>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT OrderDate, SUM(TotalAmount) AS TotalSales, SUM(OrderItems.Quantity) AS NumberOfBooksSold
                FROM Orders
                INNER JOIN OrderItems ON Orders.Id = OrderItems.OrderId
                WHERE OrderDate BETWEEN @FromDate AND @ToDate
                GROUP BY OrderDate
            ", con);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        salesReports.Add(new SalesReport
                        {
                            Date = reader.GetDateTime(0),
                            TotalSales = reader.GetDecimal(1),
                            NumberOfBooksSold = reader.GetInt32(2)
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return salesReports;
        }

        // Method to get sales by book report
        public List<SalesByBookReport> GetSalesByBookReport(DateTime fromDate, DateTime toDate)
        {
            List<SalesByBookReport> reports = new List<SalesByBookReport>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT Books.Title, SUM(OrderItems.Quantity) AS UnitsSold, SUM(OrderItems.Quantity * Books.Price) AS RevenueGenerated
                FROM OrderItem
                INNER JOIN Orders ON OrderItems.OrderId = Orders.Id
                INNER JOIN Books ON OrderItems.BookId = Books.Id
                WHERE Orders.OrderDate BETWEEN @FromDate AND @ToDate
                GROUP BY Books.Title
            ", con);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        reports.Add(new SalesByBookReport
                        {
                            BookTitle = reader.GetString(0),
                            UnitsSold = reader.GetInt32(1),
                            RevenueGenerated = reader.GetDecimal(2)
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return reports;
        }

        // Method to get sales by author report
        public List<SalesByAuthorReport> GetSalesByAuthorReport(DateTime fromDate, DateTime toDate)
        {
            List<SalesByAuthorReport> reports = new List<SalesByAuthorReport>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT Books.Author, SUM(OrderItems.Quantity) AS UnitsSold, SUM(OrderItems.Quantity * Books.Price) AS RevenueGenerated
                FROM OrderItems
                INNER JOIN Orders ON OrderItems.OrderId = Orders.Id
                INNER JOIN Books ON OrderItems.BookId = Books.Id
                WHERE Orders.OrderDate BETWEEN @FromDate AND @ToDate
                GROUP BY Books.Author
            ", con);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        reports.Add(new SalesByAuthorReport
                        {
                            AuthorName = reader.GetString(0),
                            UnitsSold = reader.GetInt32(1),
                            RevenueGenerated = reader.GetDecimal(2)
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return reports;
        }

        // Method to get sales by category report
        public List<SalesByCategoryReport> GetSalesByCategoryReport(DateTime fromDate, DateTime toDate)
        {
            List<SalesByCategoryReport> reports = new List<SalesByCategoryReport>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT Books.Category, SUM(OrderItems.Quantity) AS UnitsSold, SUM(OrderItems.Quantity * Books.Price) AS RevenueGenerated
                FROM OrderItems
                INNER JOIN Orders ON OrderItems.OrderId = Orders.Id
                INNER JOIN Books ON OrderItems.BookId = Books.Id
                WHERE Orders.OrderDate BETWEEN @FromDate AND @ToDate
                GROUP BY Books.Category
            ", con);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        reports.Add(new SalesByCategoryReport
                        {
                            CategoryName = reader.GetString(0),
                            UnitsSold = reader.GetInt32(1),
                            RevenueGenerated = reader.GetDecimal(2)
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return reports;
        }

        // Method to get sales by customer report
        public List<SalesByCustomerReport> GetSalesByCustomerReport(DateTime fromDate, DateTime toDate)
        {
            List<SalesByCustomerReport> reports = new List<SalesByCustomerReport>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT Users.FullName, SUM(OrderItems.Quantity) AS UnitsBought, SUM(OrderItems.Quantity * Books.Price) AS TotalSpent
                FROM OrderItems
                INNER JOIN Orders ON OrderItems.OrderId = Orders.Id
                INNER JOIN Books ON OrderItems.BookId = Books.Id
                INNER JOIN Users ON Orders.UserId = Users.Id
                WHERE Orders.OrderDate BETWEEN @FromDate AND @ToDate
                GROUP BY Users.FullName
            ", con);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        reports.Add(new SalesByCustomerReport
                        {
                            CustomerName = reader.GetString(0),
                            UnitsBought = reader.GetInt32(1),
                            TotalSpent = reader.GetDecimal(2)
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return reports;
        }

        // Method to get inventory status report
        public List<InventoryStatusReport> GetInventoryStatusReport()
        {
            List<InventoryStatusReport> reports = new List<InventoryStatusReport>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT Title, Quantity - Reserved - Sold AS Available, Reserved, Sold
                FROM Books
            ", con);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        reports.Add(new InventoryStatusReport
                        {
                            BookTitle = reader.GetString(0),
                            Available = reader.GetInt32(1),
                            Reserved = reader.GetInt32(2),
                            Sold = reader.GetInt32(3)
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return reports;
        }

        // Method to get out of stock report
        public List<OutOfStockReport> GetOutOfStockReport()
        {
            List<OutOfStockReport> reports = new List<OutOfStockReport>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT Title, Quantity
                FROM Books
                WHERE Quantity <= 0
            ", con);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        reports.Add(new OutOfStockReport
                        {
                            BookTitle = reader.GetString(0),
                            Available = reader.GetInt32(1)
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return reports;
        }

        public decimal GetTotalSales()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT SUM(TotalAmount) FROM Orders", con);
                var result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        public int GetTotalSalesCount()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Orders", con);
                return (int)cmd.ExecuteScalar();
            }
        }

        public int GetTotalUsers()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM AspNetUsers", con);
                return (int)cmd.ExecuteScalar();
            }
        }
        public List<Book> GetMostPopularBooks(int count)
        {
            List<Book> books = new List<Book>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT TOP(@Count) Books.*, COALESCE(OrderCount, 0) AS OrderCount
                FROM Books
                LEFT JOIN (
                    SELECT BookId, COUNT(BookId) AS OrderCount
                    FROM OrderItems
                    GROUP BY BookId
                ) AS OrderSummary ON Books.Id = OrderSummary.BookId
                ORDER BY OrderCount DESC", con);
                    cmd.Parameters.AddWithValue("@Count", count);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.GetString(2),
                            ISBN = reader.GetString(3),
                            Publisher = reader.GetString(4),
                            PublishedYear = reader.GetInt32(5),
                            Edition = reader.GetString(6),
                            Price = reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7),
                            Quantity = reader.GetInt32(8),
                            Condition = reader.GetString(9),
                            Description = reader.GetString(10),
                            CoverImage = reader.IsDBNull(11) ? null : (byte[])reader[11]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            // If no books were found, add some default books
            if (books.Count == 0)
            {
                books.AddRange(GetDefaultBooks(count));
            }

            return books;
        }

        private List<Book> GetDefaultBooks(int count)
        {
            // Logic to get some default books, for example, the latest added books
            List<Book> defaultBooks = new List<Book>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT TOP(@Count) *
                FROM Books
                ORDER BY PublishedYear DESC", con);
                    cmd.Parameters.AddWithValue("@Count", count);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        defaultBooks.Add(new Book
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.GetString(2),
                            ISBN = reader.GetString(3),
                            Publisher = reader.GetString(4),
                            PublishedYear = reader.GetInt32(5),
                            Edition = reader.GetString(6),
                            Price = reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7),
                            Quantity = reader.GetInt32(8),
                            Condition = reader.GetString(9),
                            Description = reader.GetString(10),
                            CoverImage = reader.IsDBNull(11) ? null : (byte[])reader[11]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return defaultBooks;
        }

        public List<Order> GetRecentOrders(int count)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT TOP(@Count) * FROM Orders ORDER BY OrderDate DESC", con);
                cmd.Parameters.AddWithValue("@Count", count);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetString(1),
                        FullName = reader.GetString(2),
                        Address = reader.GetString(3),
                        City = reader.GetString(4),
                        State = reader.GetString(5),
                        ZipCode = reader.GetString(6),
                        Country = reader.GetString(7),
                        TotalAmount = reader.GetDecimal(8),
                        OrderDate = reader.GetDateTime(9),
                        Status = reader.GetString(10)
                    });
                }
            }

            return orders;
        }

        //public void AddOrUpdateReview(string userId, int orderId, int bookId, double rating)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        var query = @"
        //            UPDATE OrderItems
        //            SET Rating = @Rating
        //            WHERE OrderId = @OrderId AND BookId = @BookId;

        //            IF NOT EXISTS (SELECT 1 FROM Feedback WHERE OrderId = @OrderId AND BookId = @BookId)
        //            BEGIN
        //                INSERT INTO Feedback (OrderId, BookId, Rating)
        //                VALUES (@OrderId, @BookId, @Rating)
        //            END
        //            ELSE
        //            BEGIN
        //                UPDATE Feedback
        //                SET Rating = @Rating
        //                WHERE OrderId = @OrderId AND BookId = @BookId
        //            END;

        //            UPDATE Books
        //            SET AverageRating = (SELECT AVG(Rating) FROM Feedback WHERE BookId = @BookId)
        //            WHERE Id = @BookId;";

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@OrderId", orderId);
        //            command.Parameters.AddWithValue("@BookId", bookId);
        //            command.Parameters.AddWithValue("@Rating", rating);
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}


        public double GetUserRatingForBook(string userId, int orderId, int bookId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT Rating FROM OrderItems WHERE OrderId = @OrderId AND BookId = @BookId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.Parameters.AddWithValue("@BookId", bookId);

                    var result = command.ExecuteScalar();
                    if (result == DBNull.Value)
                    {
                        return 0;
                    }
                    return Convert.ToDouble(result);
                }
            }
        }

        public List<Review> GetReviewsByBookId(int bookId)
        {
            var reviews = new List<Review>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = "SELECT * FROM Review WHERE BookId = @BookId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", bookId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reviews.Add(new Review
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                BookId = Convert.ToInt32(reader["BookId"]),
                                UserId = reader["UserId"].ToString(),
                                ReviewText = reader["ReviewText"].ToString(),
                                Timestamp = Convert.ToDateTime(reader["Timestamp"])
                            });
                        }
                    }
                }
            }

            return reviews;
        }



        public List<Book> GetFeaturedBooks()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT TOP 8 * FROM Books ORDER BY NEWID()", con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.GetString(2),
                            ISBN = reader.GetString(3),
                            Publisher = reader.GetString(4),
                            PublishedYear = reader.GetInt32(5),
                            Edition = reader.GetString(6),
                            Price = reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7),
                            Quantity = reader.GetInt32(8),
                            Condition = reader.GetString(9),
                            Description = reader.GetString(10),
                            CoverImage = reader.IsDBNull(11) ? null : (byte[])reader[11]
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return books;
        }


            // Method to get low stock alert report
            public List<LowStockAlertReport> GetLowStockAlertReport()
        {
            List<LowStockAlertReport> reports = new List<LowStockAlertReport>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(@"
                SELECT Title, Quantity
                FROM Books
                WHERE Quantity < ReorderLevel
            ", con);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        reports.Add(new LowStockAlertReport
                        {
                            BookTitle = reader.GetString(0),
                            Available = reader.GetInt32(1)
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return reports;
        }

        public void UpdateOrderStatus(Order order)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Orders SET Status = @Status WHERE Id = @Id", con);

                    cmd.Parameters.AddWithValue("@Status", order.Status);
                    cmd.Parameters.AddWithValue("@Id", order.Id);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

 
        public void UpdateBookQuantity(int bookId, int quantityChange)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Books SET Quantity = Quantity + @QuantityChange WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@QuantityChange", quantityChange);
                    cmd.Parameters.AddWithValue("@Id", bookId);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }


        public void AddOrUpdateReview(string userId, int bookId, string reviewText, double rating)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"
                    -- Check if a review already exists for this user and book
                    IF NOT EXISTS (SELECT 1 FROM Review WHERE BookId = @BookId AND UserId = @UserId)
                    BEGIN
                        -- Insert a new review
                        INSERT INTO Review (BookId, UserId, ReviewText, Timestamp, Rating)
                        VALUES (@BookId, @UserId, @ReviewText, GETDATE(), @Rating)
                    END
                    ELSE
                    BEGIN
                        -- Update the existing review
                        UPDATE Review
                        SET ReviewText = @ReviewText, Rating = @Rating, Timestamp = GETDATE()
                        WHERE BookId = @BookId AND UserId = @UserId
                    END;

                    -- Update the average rating in the Books table
                    UPDATE Book
                    SET AverageRating = (SELECT AVG(Rating) FROM Review WHERE BookId = @BookId)
                    WHERE Id = @BookId;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", bookId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@ReviewText", reviewText);
                    command.Parameters.AddWithValue("@Rating", rating);
                    command.ExecuteNonQuery();
                }
            }
        }


        public byte[] GetImageBytes(string url)
        {
            using (var webClient = new System.Net.WebClient())
            {
                return webClient.DownloadData(url);
            }
        }

        public string RegisterAdmin(Admin admin)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var query = @"
                INSERT INTO Admin (FirstName, LastName, MobileNumber, Address, UserName, Email, PasswordHash)
                VALUES (@FirstName, @LastName, @MobileNumber, @Address, @UserName, @Email, @PasswordHash)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Bind parameters
                        command.Parameters.AddWithValue("@FirstName", admin.FirstName);
                        command.Parameters.AddWithValue("@LastName", admin.LastName);
                        command.Parameters.AddWithValue("@MobileNumber", admin.MobileNumber);
                        command.Parameters.AddWithValue("@Address", admin.Address);
                        command.Parameters.AddWithValue("@UserName", admin.UserName);
                        command.Parameters.AddWithValue("@Email", admin.Email);
                        command.Parameters.AddWithValue("@PasswordHash", admin.PasswordHash);

                        // Execute the query
                        command.ExecuteNonQuery();
                    }

                    return "Registration successful!";
                }
                catch (SqlException ex)
                {
                    // Handle exceptions (e.g., unique constraint violation for UserName or Email)
                    return $"Error: {ex.Message}";
                }
            }
        }


        public Admin AdminLogin(string username, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Ensure the parameters are not null or empty
                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    {
                        throw new ArgumentException("Username and password cannot be empty.");
                    }

                        var query = @"
                    SELECT * 
                    FROM Admin
                    WHERE UserName = @UserName AND PasswordHash = @PasswordHash";

                    using (var command = new SqlCommand(query, connection))
                    {
                        // Bind parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@UserName", username);
                        command.Parameters.AddWithValue("@PasswordHash", password);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Admin
                                {
                                    ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash"))
                                };
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception($"An error occurred while logging in: {ex.Message}");
                }
            }

            // Return null if login fails
            return null;
        }


    }
}
