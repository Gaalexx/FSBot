using System.Data.SqlClient;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Program
{
    class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FSBotDB;Integrated Security=True");
        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }
        public async Task<bool> addUser(/*List<string> regMsgTxt*/User user)
        {
            string insertCB = $"insert UserInfo (Name, Mail, Password, ChatId) values (@Name, @Mail, @Password, '{user.ChatId}')";
            List<Object> obj_P_regInfo = new List<Object>();
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
                SqlDataReader sqlReader;
                SqlCommand commandSrch = new SqlCommand(
                    $"SELECT Name FROM dbo.UserInfo WHERE Name = @Name OR Mail = @Mail", sqlConnection);
                commandSrch.Parameters.AddWithValue("@Name", user.Name);
                commandSrch.Parameters.AddWithValue("@Mail", user.Email);
                using(SqlDataReader sqlDtrdr = await commandSrch.ExecuteReaderAsync())
                {
                    if (sqlDtrdr.HasRows)
                    {
                        while (await sqlDtrdr.ReadAsync())
                        {
                            obj_P_regInfo.Add(sqlDtrdr.GetValue(0));
                            obj_P_regInfo.Add(sqlDtrdr.GetValue(1));
                            Console.WriteLine(obj_P_regInfo[0].ToString() + obj_P_regInfo[1].ToString());
                        }
                        obj_P_regInfo.Clear();
                    }
                }
                commandSrch.ExecuteNonQuery();
                sqlReader = commandSrch.ExecuteReader();
                bool isInDB = sqlReader.Read();
                sqlReader.Close();
                if (isInDB == false)
                {
                    SqlCommand command = new SqlCommand(
                    insertCB, sqlConnection);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Mail", user.Email);
                    using (SqlDataReader scndRdr = await commandSrch.ExecuteReaderAsync())
                    {
                        if (scndRdr.HasRows)
                        {
                            while (await scndRdr.ReadAsync())
                            {
                                //object P_Name = sqlDtrdr.GetValue(0);
                                //object P_Email = sqlDtrdr.GetValue(1);
                                obj_P_regInfo.Add(scndRdr.GetValue(0));
                                obj_P_regInfo.Add(scndRdr.GetValue(1));
                                obj_P_regInfo.Add(scndRdr.GetValue(2));
                                Console.WriteLine(obj_P_regInfo[0].ToString() + obj_P_regInfo[1].ToString() + obj_P_regInfo[2].ToString);
                            }
                        }
                    }
                    command.ExecuteNonQuery();

                    sqlConnection.Close();
                    return true;
                }
                
                sqlConnection.Close();
                return false;
            }
            return false;
        }
        static public bool ExitFromDb(long chatId)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FSBotDB;Integrated Security=True");
            if(sqlCon.State == System.Data.ConnectionState.Closed)
            {
                sqlCon.Open();
                SqlCommand sqlCom = new SqlCommand($"UPDATE UserInfo SET IsAuthorized = '{false}' WHERE ChatId = @ChatId", sqlCon);
                sqlCom.Parameters.AddWithValue("@ChatId", chatId);
                sqlCom.ExecuteNonQuery();

                return true;
            }
            return false;
        }
        static public bool ConsostsInDBName(User user)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FSBotDB;Integrated Security=True");
           
            if (sqlCon.State == System.Data.ConnectionState.Closed)
            {
                sqlCon.Open();
                SqlDataReader sqlReader;
                SqlCommand commandSrch = new SqlCommand(
                    $"SELECT Name FROM dbo.UserInfo WHERE Name = @Name", sqlCon);
                commandSrch.Parameters.AddWithValue("@Name", user.Name);
                commandSrch.ExecuteNonQuery();
                sqlReader = commandSrch.ExecuteReader();
                bool isInDB = sqlReader.Read();
                sqlReader.Close();
                if(isInDB == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
                sqlCon.Close();
            }
            sqlCon.Close();
            return false;
        }
        static public bool IsRg(long ChatId)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FSBotDB;Integrated Security=True");

            if (sqlCon.State == System.Data.ConnectionState.Closed)
            {
                sqlCon.Open();
                SqlDataReader sqlReader;
                SqlCommand commandSrch = new SqlCommand(
                    $"SELECT ChatId FROM dbo.UserInfo WHERE ChatId = @chatId", sqlCon);
                commandSrch.Parameters.AddWithValue("@chatId", ChatId);
                commandSrch.ExecuteNonQuery();
                sqlReader = commandSrch.ExecuteReader();
                bool isInDB = sqlReader.Read();
                sqlReader.Close();
                if (isInDB == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
                sqlCon.Close();
            }
            sqlCon.Close();
            return false;
        }
        static public void AddFavouriteteam(long ChatId, string FavouriteTeam)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FSBotDB;Integrated Security=True");

            if(sqlCon.State == System.Data.ConnectionState.Closed)
            {
                sqlCon.Open();
                SqlCommand commandAdd = new SqlCommand($"UPDATE UserInfo SET FavouriteTeam = @FavoriteTeam WHERE ChatId = @ChatId", sqlCon);
                commandAdd.Parameters.AddWithValue("@FavoriteTeam", FavouriteTeam);
                commandAdd.Parameters.AddWithValue("@ChatId", ChatId);
                commandAdd.ExecuteNonQuery();
                sqlCon.Close();
                Console.WriteLine("Команда успешно добавлена");
            }

        }
        static public string TeamSrch(long chatId)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FSBotDB;Integrated Security=True");
            if (sqlCon.State == System.Data.ConnectionState.Closed)
            {
                sqlCon.Open();
                SqlDataReader sqlReader;
                SqlCommand commandSrch = new SqlCommand(
                    $"SELECT FavouriteTeam FROM UserInfo WHERE chatId = @ChatId", sqlCon);
                commandSrch.Parameters.AddWithValue("@ChatId", chatId);
                commandSrch.ExecuteNonQuery();
                sqlReader = commandSrch.ExecuteReader();
                //bool isInDB = sqlReader.Read();

                string Team = "";

                while (sqlReader.Read())
                {
                    
                    //sqlReader.Close(); 
                    if (sqlReader["FavouriteTeam"] != DBNull.Value)
                    {
                        Team = Convert.ToString(sqlReader["FavouriteTeam"]);
                    }
                }
                return Team;
                sqlCon.Close();
            }
            
            sqlCon.Close();
            return "Нет";
        }
        static public bool ConsostsInDBPassword(User user)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FSBotDB;Integrated Security=True");

            if (sqlCon.State == System.Data.ConnectionState.Closed)
            {
                sqlCon.Open();
                SqlDataReader sqlReader;
                SqlCommand commandSrch = new SqlCommand(
                    $"SELECT Password, Name FROM UserInfo WHERE Password = @Password AND Name = @Name", sqlCon);
                commandSrch.Parameters.AddWithValue("@Password", user.Password);
                commandSrch.Parameters.AddWithValue("@Name", user.Name);
                commandSrch.ExecuteNonQuery();
                sqlReader = commandSrch.ExecuteReader();
                bool isInDB = sqlReader.Read();
                
                sqlReader.Close();
                if (isInDB == false)
                {
                    sqlCon.Close();
                    return false;
                }
                else
                {
                    Console.WriteLine("пароль верный");
                    bool forSql = true;
                    Console.WriteLine(sqlCon.ConnectionString.ToString());
                    SqlCommand command = new SqlCommand(
                   $"UPDATE UserInfo SET IsAuthorized = '{forSql}' WHERE Name = @Name", sqlCon);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    
                    command.ExecuteNonQuery();
                    
                    
                    sqlCon.Close();
                    return true;
                }
                sqlCon.Close();
            }
            sqlCon.Close();
            return false;
        }
        public static List<User> WhoIsRegistrated()
        {
            List<User> registratedUsr = new List<User>();
            SqlConnection sqlCon = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FSBotDB;Integrated Security=True");
            sqlCon.Open();
            SqlCommand sqlCommand = new SqlCommand(@"SELECT ChatId, FavouriteTeam, Name FROM UserInfo WHERE IsAuthorized = 'True'", sqlCon);
            
            sqlCommand.ExecuteNonQuery();
            SqlDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                Console.WriteLine(dataReader["ChatId"].ToString());
                User user = new User();
                user.Name = Convert.ToString(dataReader["Name"].ToString());
                user.ChatId = Convert.ToInt64(dataReader["ChatId"].ToString());
                if (dataReader["FavouriteTeam"] != DBNull.Value)
                {
                    user.Team = dataReader["FavouriteTeam"].ToString();
                }
                registratedUsr.Add(user);
            }
            dataReader.Close();
            sqlCon.Close();
            return registratedUsr;
        }
        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection getConnection()
        {
            return sqlConnection;   
        }

        
    }

}
