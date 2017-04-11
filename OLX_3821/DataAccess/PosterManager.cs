using OLX_3821.Controllers;
using OLX_3821.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace OLX_3821.DataAccess
{
    public class PosterManager
    {
        public static string ConnectionStr
        {
            get
            {
                return WebConfigurationManager
                    .ConnectionStrings["ConnStr"]
                    .ConnectionString;
            }
        }

        public IList<Posters> GetAllPosters()
        {
            IList<Posters> posterList = new List<Posters>();
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [posterID]
                                          ,[title]
                                          ,[category]                                          
                                          ,[subcategory]
                                          ,[price]                                          
                                          ,[pDescription]
                                          ,[PAddress]
                                          ,[startDate]
                                          ,[cancelDate]
                                          ,[quantity]
                                          ,[userID]
                                      FROM [dbo].[Poster]";
                    conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Posters p = new Posters();
                            p.posterID = rdr.GetInt32(0);
                            p.title = rdr.GetString(1);
                            p.category = rdr.GetString(2);
                            p.subcategory = rdr.GetString(3);
                            p.price = rdr.GetDouble(4);
                            p.description = rdr.GetString(5);
                            p.address = rdr.GetString(6);
                            p.startDate = rdr.GetDateTime(7);
                            p.cancelDate = rdr.GetDateTime(8);
                            p.quantity = rdr.GetInt32(9);
                            p.userID = rdr.GetInt32(10);

                            posterList.Add(p);
                        }
                    }
                }
            }
            return posterList;

        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // POSTER HAS TO BE ADDED FOR THE LOOGGED IN USER
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public void CreatePoster(Posters poster)
        {
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    //Users currentUser = new Users();
                    int currentUser = 0;
                    string name = HttpContext.Current.User.Identity.Name;
                    //cmd.CommandText = @"SELECT [userID]
                    //                    ,[UTitle]
                    //                    ,[firstName]
                    //                    ,[lastName]
                    //                    ,[phone]
                    //                    ,[eMail]
                    //                    ,[username]
                    //                    ,[uPassword]
                    //                    FROM [dbo].[Users]
                    //                 WHERE [username] = @username";
                    //cmd.AddParameter("@username", DbType.String, name);
                    //conn.Open();
                    //using (DbDataReader rdr = cmd.ExecuteReader())
                    //{
                    //    if (rdr.Read()) //notice if
                    //    {
                    //        currentUser.userID = rdr.GetInt32(0);
                    //        currentUser.UTitle = rdr.GetString(1);
                    //        currentUser.firstName = rdr.GetString(2);
                    //        currentUser.lastName = rdr.GetString(3);
                    //        currentUser.phone = rdr.GetString(4);
                    //        currentUser.eMail = rdr.GetString(5);
                    //        currentUser.username = rdr.GetString(6);
                    //        currentUser.uPassword = rdr.GetString(7);

                    //    }
                    //}
                    cmd.CommandText = @"SELECT [userID]
                                        FROM [dbo].[Users]
	                                    WHERE [username] = @username";
                    cmd.AddParameter("@username", DbType.String, name);
                    conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read()) //notice if
                        {
                            currentUser = rdr.GetInt32(0);
                        }
                    }

                    cmd.CommandText = @"INSERT INTO Poster(
                                           [title]
                                          ,[category]                                          
                                          ,[subcategory]
                                          ,[price]                                          
                                          ,[pDescription]
                                          ,[PAddress]
                                          ,[quantity]
                                          ,[userID]) VALUES (
                            @title,
                            @category,
                            @subcategory,
                            @price,
                            @pDescription,
                            @PAddress,
                            @quantity,
                            @userID
                            )";

                    //HttpContext.Current.User.Identity.GetUserId();

                    cmd.AddParameter("@title", DbType.String, poster.title);
                    cmd.AddParameter("@category", DbType.String, poster.category);
                    cmd.AddParameter("@subcategory", DbType.String, poster.subcategory);
                    cmd.AddParameter("@price", DbType.Double, poster.price);
                    cmd.AddParameter("@pDescription", DbType.String, poster.description);
                    cmd.AddParameter("@PAddress", DbType.String, poster.address);
                    cmd.AddParameter("@quantity", DbType.Int32, poster.quantity);
                    cmd.AddParameter("@userID", DbType.Int32, currentUser);

                    //conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Posters GetPosterById(int posterID)
        {
            Posters currentPoster = new Posters();
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [posterID]
                                          ,[title]
                                          ,[category]                                          
                                          ,[subcategory]
                                          ,[price]                                          
                                          ,[pDescription]
                                          ,[PAddress]
                                          ,[startDate]
                                          ,[cancelDate]
                                          ,[quantity]
                                          ,[userID]
                                      FROM [dbo].[Poster]
                                      WHERE [posterID] = @posterID"; //change text here
                    cmd.AddParameter("@posterID", DbType.Int32, posterID);
                    conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read()) //notice if
                        {
                            currentPoster.posterID = rdr.GetInt32(0);
                            currentPoster.title = rdr.GetString(1);
                            currentPoster.category = rdr.GetString(2);
                            currentPoster.subcategory = rdr.GetString(3);
                            currentPoster.price = rdr.GetDouble(4);
                            currentPoster.description = rdr.GetString(5);
                            currentPoster.address = rdr.GetString(6);
                            currentPoster.startDate = rdr.GetDateTime(7);
                            currentPoster.cancelDate = rdr.GetDateTime(8);
                            currentPoster.quantity = rdr.GetInt32(9);
                            currentPoster.userID = rdr.GetInt32(10);
                        }
                    }
                }
            }
            return currentPoster;
        }

        //public void UpdateUser(Users user)
        //{
        //    using (DbConnection conn = new SqlConnection(ConnectionStr))
        //    {
        //        using (DbCommand cmd = conn.CreateCommand())
        //        {
        //            //Set command text
        //            cmd.CommandText = @"UPDATE [dbo].[Poster]
        //                               SET [UTitle] = @uTitle
        //                               ,[firstName] = @FirstName
        //                               ,[lastName] = @LastName
        //                               ,[phone] = @Phone
        //                               ,[eMail] = @EMail
        //                               ,[username] = @Username
        //                               ,[uPassword] = @Password
        //                               WHERE userID = @UserID";

        //            cmd.AddParameter("@UserID", DbType.Int32, user.userID);
        //            cmd.AddParameter("@uTitle", DbType.String, user.UTitle);
        //            cmd.AddParameter("@FirstName", DbType.String, user.firstName);
        //            cmd.AddParameter("@LastName", DbType.String, user.lastName);
        //            cmd.AddParameter("@EMail", DbType.String, user.eMail);
        //            cmd.AddParameter("@Phone", DbType.String, user.phone);
        //            cmd.AddParameter("@Username", DbType.String, user.username);
        //            cmd.AddParameter("@Password", DbType.String, user.uPassword);

        //            conn.Open();
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}


        public void UpdatePosterStoredProc(Posters poster)
        {
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"udpUpdatePoster";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.AddParameter("@title", System.Data.DbType.String, poster.title);
                    cmd.AddParameter("@price", System.Data.DbType.Double, poster.price);
                    cmd.AddParameter("@description", System.Data.DbType.String, poster.description);
                    cmd.AddParameter("@pAddress", System.Data.DbType.String, poster.address);
                    cmd.AddParameter("@quantity", System.Data.DbType.Int32, poster.quantity);
                    cmd.AddParameter("@PosterID", System.Data.DbType.Int32, poster.posterID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeletePoster(Posters poster)
        {
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    //Set command text
                    cmd.CommandText = @"Delete From Poster
                         WHERE posterID = @posterID";

                    cmd.AddParameter("@posterID", DbType.Int32, poster.posterID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IList<Posters> GetAvaliablePosters()
        {
            IList<Posters> posterList = new List<Posters>();
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [posterID]
                                          ,[title]
                                          ,[category]                                          
                                          ,[subcategory]
                                          ,[price]                                          
                                          ,[pDescription]
                                          ,[PAddress]
                                          ,[startDate]
                                          ,[cancelDate]
                                          ,[quantity]
                                          ,[userID]
                                      FROM [dbo].[Poster]
                                      WHERE [quantity] > 0";
                    conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Posters p = new Posters();
                            p.posterID = rdr.GetInt32(0);
                            p.title = rdr.GetString(1);
                            p.category = rdr.GetString(2);
                            p.subcategory = rdr.GetString(3);
                            p.price = rdr.GetDouble(4);
                            p.description = rdr.GetString(5);
                            p.address = rdr.GetString(6);
                            p.startDate = rdr.GetDateTime(7);
                            p.cancelDate = rdr.GetDateTime(8);
                            p.quantity = rdr.GetInt32(9);
                            p.userID = rdr.GetInt32(10);

                            posterList.Add(p);
                        }
                    }
                }
            }
            return posterList;

        }

        public IList<Posters> FilterPosters(string title, string category, double? min, double? max)
        {
            //validation of search strings
            if (title == null)
                title = "";
            if (category == null)
                category = "";
            if (min == null)
                min = 0;
            if (max == null)
                max = float.MaxValue;
            IList<Posters> posterList = new List<Posters>();
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    //SQL Select statement includes where clause
                    //string sql = @"SELECT [EmployeeId]
                    //                      ,[LastName]
                    //                      ,[FirstName]                                          
                    //                      ,[ReportsTo]
                    //                      ,[BirthDate]                                          
                    //                      ,[Email]
                    //                  FROM [dbo].[Poster]
                    //                  WHERE FirstName LIKE '%' + @FirstName + '%' AND LastName LIKE  '%' + @LastName + '%' AND Email LIKE  '%' + @Email + '%' ";

                    string sql = @"SELECT *
                                      FROM [dbo].[Poster]
                                      WHERE title LIKE '%' + @title + '%' AND category LIKE  '%' + @category + '%' AND price BETWEEN @min AND @max";

                    cmd.CommandText = sql;

                    //add search strings coming from GUI as parameter
                    cmd.AddParameter("@title", DbType.String, title);
                    cmd.AddParameter("@category", DbType.String, category);
                    cmd.AddParameter("@min", DbType.Double, min);
                    cmd.AddParameter("@max", DbType.Double, max);

                    conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Posters p = new Posters();
                            p.posterID = rdr.GetInt32(0);
                            p.title = rdr.GetString(1);
                            p.category = rdr.GetString(2);
                            p.subcategory = rdr.GetString(3);
                            p.price = rdr.GetDouble(4);
                            p.description = rdr.GetString(5);
                            p.address = rdr.GetString(6);
                            p.startDate = rdr.GetDateTime(7);
                            p.cancelDate = rdr.GetDateTime(8);
                            p.quantity = rdr.GetInt32(9);
                            p.userID = rdr.GetInt32(10);

                            posterList.Add(p);
                        }
                    }
                }
            }
            return posterList;

        }

        public IList<Posters> GetUserPosters()
        {
            IList<Posters> posterList = new List<Posters>();
            int currentUser = 0;
            string name = HttpContext.Current.User.Identity.Name;
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [userID]
                                        FROM [dbo].[Users]
	                                    WHERE [username] = @username";
                    cmd.AddParameter("@username", DbType.String, name);
                    conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read()) //notice if
                        {
                            currentUser = rdr.GetInt32(0);
                        }
                    }


                    cmd.CommandText = @"SELECT [posterID]
                                          ,[title]
                                          ,[category]                                          
                                          ,[subcategory]
                                          ,[price]                                          
                                          ,[pDescription]
                                          ,[PAddress]
                                          ,[startDate]
                                          ,[cancelDate]
                                          ,[quantity]
                                          ,[userID]
                                      FROM [dbo].[Poster]
                                      WHERE userID = @userID";
                    cmd.AddParameter("@userID", DbType.Int32, currentUser);

                    //conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Posters p = new Posters();
                            p.posterID = rdr.GetInt32(0);
                            p.title = rdr.GetString(1);
                            p.category = rdr.GetString(2);
                            p.subcategory = rdr.GetString(3);
                            p.price = rdr.GetDouble(4);
                            p.description = rdr.GetString(5);
                            p.address = rdr.GetString(6);
                            p.startDate = rdr.GetDateTime(7);
                            p.cancelDate = rdr.GetDateTime(8);
                            p.quantity = rdr.GetInt32(9);
                            p.userID = rdr.GetInt32(10);

                            posterList.Add(p);
                        }
                    }
                }
            }
            return posterList;
        }



        public void Purchase(Posters poster)
        {
            Posters currentPoster = new Posters();
            int currentUser = 0;
            double purchaseAmount = 0;
            string name = HttpContext.Current.User.Identity.Name;

            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [posterID]
                                          ,[title]
                                          ,[category]                                          
                                          ,[subcategory]
                                          ,[price]                                          
                                          ,[pDescription]
                                          ,[PAddress]
                                          ,[startDate]
                                          ,[cancelDate]
                                          ,[quantity]
                                          ,[userID]
                                      FROM [dbo].[Poster]
                                      WHERE [posterID] = @posterID"; //change text here
                    cmd.AddParameter("@posterID", DbType.Int32, poster.posterID);
                    conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read()) //notice if
                        {
                            currentPoster.posterID = rdr.GetInt32(0);
                            currentPoster.title = rdr.GetString(1);
                            currentPoster.category = rdr.GetString(2);
                            currentPoster.subcategory = rdr.GetString(3);
                            currentPoster.price = rdr.GetDouble(4);
                            currentPoster.description = rdr.GetString(5);
                            currentPoster.address = rdr.GetString(6);
                            currentPoster.startDate = rdr.GetDateTime(7);
                            currentPoster.cancelDate = rdr.GetDateTime(8);
                            currentPoster.quantity = rdr.GetInt32(9);
                            currentPoster.userID = rdr.GetInt32(10);
                        }
                    }
                }

                if (currentPoster.quantity < poster.quantity)
                {
                    return;
                }

                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [userID]
                                        FROM [dbo].[Users]
	                                    WHERE [username] = @username";
                    cmd.AddParameter("@username", DbType.String, name);
                    //conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read()) //notice if
                        {
                            currentUser = rdr.GetInt32(0);
                        }
                    }

                    cmd.CommandText = @"INSERT INTO Purchases(
                                           [userID]
                                          ,[posterID]
                                          ,[quantity]) VALUES (
                            @userID,
                            @posterID,
                            @quantity
                            )"; /*add missing fields yourself!*/

                    cmd.AddParameter("@quantity", DbType.Int32, poster.quantity);
                    cmd.AddParameter("@userID", DbType.Int32, currentUser);
                    cmd.AddParameter("@posterID", DbType.Int32, poster.posterID);
                    

                    //conn.Open();
                    cmd.ExecuteNonQuery();

                    purchaseAmount = poster.quantity * currentPoster.price;
                }

                using (DbCommand cmd = conn.CreateCommand())
                {
                    double currentBalance = 0;

                    cmd.CommandText = @"SELECT [currentAmount]
                                        FROM [dbo].[Balance]
	                                    WHERE [userID] = @UserID";
                    cmd.AddParameter("@UserID", DbType.Int32, currentUser);
                    //conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read()) //notice if
                        {
                            currentBalance = rdr.GetDouble(0);
                        }
                    }

                    cmd.CommandText = @"udpUpdateBalance";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.AddParameter("@newAmount", System.Data.DbType.Double, currentBalance - purchaseAmount);

                    cmd.ExecuteNonQuery();

                    //!!!!!!!!!!!!!!!
                }

                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"udpUpdatePoster";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.AddParameter("@title", System.Data.DbType.String, currentPoster.title);
                    cmd.AddParameter("@price", System.Data.DbType.Double, currentPoster.price);
                    cmd.AddParameter("@description", System.Data.DbType.String, currentPoster.description);
                    cmd.AddParameter("@pAddress", System.Data.DbType.String, currentPoster.address);
                    cmd.AddParameter("@quantity", System.Data.DbType.Int32, currentPoster.quantity - poster.quantity);
                    cmd.AddParameter("@PosterID", System.Data.DbType.Int32, currentPoster.posterID);

                    //conn.Open();
                    cmd.ExecuteNonQuery();
                }

            }
        }

        public IList<Report> TrackSalesReport(int id, double min, double max)
        {
            IList<Report> rep = new List<Report>();
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select * from udfInvoicesReport(@userID, @min, @max)";

                    UsersManager manager = new UsersManager();
                    Users currentUser = manager.GetCurrentUser();
                    //Posters poster = new Posters();

                    //cmd.AddParameter("@userID", DbType.Int32, currentUser.userID);
                    //cmd.AddParameter("@min", DbType.Double, poster.price);
                    //cmd.AddParameter("@max", DbType.Double, poster.price);
                    cmd.AddParameter("@userID", DbType.Int32, id);
                    cmd.AddParameter("@min", DbType.Double, min);
                    cmd.AddParameter("@max", DbType.Double, max);

                    conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Report r = new Report();
                            r.userID = rdr.GetInt32(0);
                            r.UTitle = rdr.GetString(1);
                            r.firstName = rdr.GetString(2);
                            r.lastName = rdr.GetString(3);
                            r.category = rdr.GetString(4);
                            r.totalSpent = rdr.GetDouble(5);

                            rep.Add(r);
                        }
                    }
                }
            }
            return rep;
        }


    }
}