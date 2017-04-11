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
    public class UsersManager
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

        public IList<Users> GetAllUsers()
        {
            IList<Users> userList = new List<Users>();
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [userID]
                                          ,[UTitle]
                                          ,[firstName]                                          
                                          ,[lastName]
                                          ,[phone]                                          
                                          ,[eMail]
                                          ,[username]
                                          ,[uPassword]
                                      FROM [dbo].[Users]";
                    conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            //Users e = new Users()
                            //{
                            //    userID = rdr.GetInt32(0),
                            //    UTitle = rdr.GetString(1),
                            //    firstName = rdr.GetString(2),
                            //    lastName = rdr.GetString(3),
                            //    phone = rdr.GetString(4),
                            //    eMail = rdr.GetString(5),
                            //    username = rdr.GetString(6),
                            //    uPassword = rdr.GetString(7)
                            //};
                            //userList.Add(e);
                            Users e = new Users();
                            e.userID = rdr.GetInt32(0);
                            e.UTitle = rdr.GetString(1);
                            e.firstName = rdr.GetString(2);
                            e.lastName = rdr.GetString(3);
                            e.phone = rdr.GetString(4);
                            e.eMail = rdr.GetString(5);
                            e.username = rdr.GetString(6);
                            e.uPassword = rdr.GetString(7);
                            userList.Add(e);
                        }
                    }
                }
            }
            return userList;

        }

        public void CreateUser(Users user)
        {
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Users(
                                           [UTitle]
                                          ,[firstName]                                          
                                          ,[lastName]
                                          ,[phone]                                          
                                          ,[eMail]
                                          ,[username]
                                          ,[uPassword]) VALUES (
                            @uTitle,
                            @FirstName,
                            @LastName,
                            @Phone,
                            @EMail,
                            @Userame,
                            @UPassword
                            )"; /*add missing fields yourself!*/

                    DbParameter uT = cmd.CreateParameter();
                    uT.DbType = System.Data.DbType.String;
                    uT.ParameterName = "@uTitle";
                    uT.Value = user.UTitle;
                    cmd.Parameters.Add(uT);

                    DbParameter uFN = cmd.CreateParameter();
                    uFN.DbType = System.Data.DbType.String;
                    uFN.ParameterName = "@FirstName";
                    uFN.Value = user.firstName;
                    cmd.Parameters.Add(uFN);

                    DbParameter uLN = cmd.CreateParameter();
                    uLN.DbType = System.Data.DbType.String;
                    uLN.ParameterName = "@LastName";
                    uLN.Value = user.lastName;
                    cmd.Parameters.Add(uLN);

                    DbParameter uE = cmd.CreateParameter();
                    uE.DbType = System.Data.DbType.String;
                    uE.ParameterName = "@EMail";
                    uE.Value = user.eMail;
                    cmd.Parameters.Add(uE);

                    DbParameter uP = cmd.CreateParameter();
                    uP.DbType = System.Data.DbType.String;
                    uP.ParameterName = "@Phone";
                    uP.Value = user.phone;
                    cmd.Parameters.Add(uP);

                    DbParameter uN = cmd.CreateParameter();
                    uN.DbType = System.Data.DbType.String;
                    uN.ParameterName = "@Userame";
                    uN.Value = user.username;
                    cmd.Parameters.Add(uN);

                    DbParameter uPass = cmd.CreateParameter();
                    uPass.DbType = System.Data.DbType.String;
                    uPass.ParameterName = "@UPassword";
                    uPass.Value = user.uPassword;
                    cmd.Parameters.Add(uPass);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Users GetUserById(int userId)
        {
            //Users currentUser = null;
            Users currentUser = new Users();
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT [userID]
                                        ,[UTitle]
                                        ,[firstName]
                                        ,[lastName]
                                        ,[phone]
                                        ,[eMail]
                                        ,[username]
                                        ,[uPassword]
                                        FROM [dbo].[Users]
	                                    WHERE [userID] = @UserID"; //change text here
                    cmd.AddParameter("@UserID", DbType.Int32, userId);
                    conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read()) //notice if
                        {
                            currentUser.userID = rdr.GetInt32(0);
                            currentUser.UTitle = rdr.GetString(1);
                            currentUser.firstName = rdr.GetString(2);
                            currentUser.lastName = rdr.GetString(3);
                            currentUser.phone = rdr.GetString(4);
                            currentUser.eMail = rdr.GetString(5);
                            currentUser.username = rdr.GetString(6);
                            currentUser.uPassword = rdr.GetString(7);

                        }
                    }
                }
            }
            return currentUser;
        }

        public void UpdateUser(Users user)
        {
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    //Set command text
                    cmd.CommandText = @"UPDATE [dbo].[Users]
                                       SET [UTitle] = @uTitle
                                       ,[firstName] = @FirstName
                                       ,[lastName] = @LastName
                                       ,[phone] = @Phone
                                       ,[eMail] = @EMail
                                       ,[username] = @Username
                                       ,[uPassword] = @Password
                                       WHERE userID = @UserID";

                    cmd.AddParameter("@UserID", DbType.Int32, user.userID);
                    cmd.AddParameter("@uTitle", DbType.String, user.UTitle);
                    cmd.AddParameter("@FirstName", DbType.String, user.firstName);
                    cmd.AddParameter("@LastName", DbType.String, user.lastName);
                    cmd.AddParameter("@EMail", DbType.String, user.eMail);
                    cmd.AddParameter("@Phone", DbType.String, user.phone);
                    cmd.AddParameter("@Username", DbType.String, user.username);
                    cmd.AddParameter("@Password", DbType.String, user.uPassword);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUserStoredProc(Users user)
        {
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"udpUpdateUser";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.AddParameter("@UserID", System.Data.DbType.Int32, user.userID);
                    cmd.AddParameter("@UTitle", System.Data.DbType.String, user.UTitle);
                    cmd.AddParameter("@FirstName", System.Data.DbType.String, user.firstName);
                    cmd.AddParameter("@LastName", System.Data.DbType.String, user.lastName);
                    cmd.AddParameter("@Phone", System.Data.DbType.String, user.phone);
                    cmd.AddParameter("@Email", System.Data.DbType.String, user.eMail);
                    cmd.AddParameter("@Username", System.Data.DbType.String, user.username);
                    cmd.AddParameter("@UPassword", System.Data.DbType.String, user.uPassword);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(Users user)
        {
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    //Set command text
                    cmd.CommandText = @"Delete From Users
                         WHERE userID = @UserID";

                    cmd.AddParameter("@UserID", DbType.Int32, user.userID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CreateUserStoredProc(Users user)
        {
            int userID = 0;
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"udpCreateUser";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.AddParameter("@UTitle", System.Data.DbType.String, user.UTitle);
                    cmd.AddParameter("@firstName", System.Data.DbType.String, user.firstName);
                    cmd.AddParameter("@lastName", System.Data.DbType.String, user.lastName);
                    cmd.AddParameter("@phone", System.Data.DbType.String, user.phone);
                    cmd.AddParameter("@eMail", System.Data.DbType.String, user.eMail);
                    cmd.AddParameter("@username", System.Data.DbType.String, user.username);
                    cmd.AddParameter("@uPassword", System.Data.DbType.String, user.uPassword);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = @"SELECT [userID]
                                        FROM [dbo].[Users]
	                                    WHERE [username] = @username";

                    using (DbDataReader rdr = (DbDataReader)cmd.ExecuteReader())
                    {
                        if (rdr.Read()) //notice if
                        {
                            userID = rdr.GetInt32(0);
                        }
                    }

                    cmd.CommandText = @"INSERT INTO Balance(
                                           [userID]) VALUES (
                            @UserID
                            )";
                    cmd.AddParameter("@UserID", System.Data.DbType.String, userID);
                    cmd.ExecuteNonQuery();

                }

            }
        }

        public bool Authenticate(string userName, string password)
        {
            bool authenticated = false;
            using (DbConnection conn = new SqlConnection(ConnectionStr))
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select dbo.udfLogin(@userName, @password)";
                    cmd.AddParameter("@userName", DbType.String, userName);
                    cmd.AddParameter("@password", DbType.String, password);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            authenticated = reader.GetBoolean(0);
                        }
                    }
                }
            }

            return authenticated;
        }

        public Users GetCurrentUser()
        {
            int currentUserId = 0;
            string name = HttpContext.Current.User.Identity.Name;
            //Users currentUser = null;
            Users currentUser = new Users();
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
                            currentUserId = rdr.GetInt32(0);
                        }
                    }

                    cmd.CommandText = @"SELECT [userID]
                                        ,[UTitle]
                                        ,[firstName]
                                        ,[lastName]
                                        ,[phone]
                                        ,[eMail]
                                        ,[username]
                                        ,[uPassword]
                                        FROM [dbo].[Users]
	                                    WHERE [userID] = @UserID"; //change text here
                    cmd.AddParameter("@UserID", DbType.Int32, currentUserId);
                    //conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read()) //notice if
                        {
                            currentUser.userID = rdr.GetInt32(0);
                            currentUser.UTitle = rdr.GetString(1);
                            currentUser.firstName = rdr.GetString(2);
                            currentUser.lastName = rdr.GetString(3);
                            currentUser.phone = rdr.GetString(4);
                            currentUser.eMail = rdr.GetString(5);
                            currentUser.username = rdr.GetString(6);
                            currentUser.uPassword = rdr.GetString(7);

                        }
                    }

                    cmd.CommandText = @"SELECT [currentAmount]
                                        FROM [dbo].[Balance]
	                                    WHERE [userID] = @UserID"; //change text here
                    //cmd.AddParameter("@UserID", DbType.Int32, currentUserId);
                    //conn.Open();
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read()) //notice if
                        {
                            currentUser.balance = rdr.GetDouble(0);
                        }
                    }
                }
            }
            return currentUser;
        }

        public void MakePayment(Users currentUser)
        {
            int currentUserId = 0;
            string name = HttpContext.Current.User.Identity.Name;
            //Users currentUser = null;
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
                            currentUserId = rdr.GetInt32(0);
                        }
                    }

                    cmd.CommandText = @"INSERT INTO Payment(
                                           [amount]
                                          ,[userID]) VALUES (
                            @amount,
                            @UserID)"; //change text here
                    cmd.AddParameter("@amount", DbType.Double, currentUser.balance);
                    cmd.AddParameter("@UserID", DbType.Int32, currentUserId);
                    //conn.Open();

                    //!!!!!!!!!!!!!!!
                }

                using (DbCommand cmd = conn.CreateCommand())
                {
                    double currentBalance = 0;

                    cmd.CommandText = @"SELECT [currentAmount]
                                        FROM [dbo].[Balance]
	                                    WHERE [userID] = @UserID";
                    cmd.AddParameter("@UserID", DbType.Int32, currentUserId);
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

                    cmd.AddParameter("@newAmount", System.Data.DbType.Double, currentBalance + currentUser.balance);

                    cmd.ExecuteNonQuery();

                    //!!!!!!!!!!!!!!!
                }
            }
        }

    }
}