using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFFinderCoreApp.Models;

namespace BFFinderCoreApp.Repository
{
    internal class DataRepository<T> : IDataRepository<T>
    {
     
        public User FindUserByUsername(String username)
        {
            DatabaseConnectionRepository databaseCon = new();
            String userSearched = "'"+username+"'";
            User user = null;

            databaseCon.OpenConnection();
            string queryString = "SELECT * FROM dbo.users WHERE username = "+userSearched;

            SqlCommand com = new SqlCommand(queryString,databaseCon.GetConnection());
            SqlDataReader searchUser = com.ExecuteReader();

            if (searchUser.Read())
            {
                user = new User();
                user.Id = Convert.ToInt32(searchUser["id_user"]);
                user.Varsta = Convert.ToInt32(searchUser["varsta"]);
                user.FirstName = Convert.ToString(searchUser["nume"]);
                user.SecondName = Convert.ToString(searchUser["prenume"]);
                
            }

        return user;
        }
        public User FindUserByEmail(String email)
        {
            DatabaseConnectionRepository databaseCon = new();

            databaseCon.OpenConnection();
            SqlCommand searchUser = databaseCon.GetConnection().CreateCommand();
            searchUser.CommandText = "SELECT * FROM user WHERE email = " + email;
            databaseCon.CloseConnection();

           
            return new User();
        }
        public bool CheckPassowrd(String password)
        {
            return true;
        }
        public void Insert(T data)
        {

        }
        public void Update(T data)
        {

        }
        public void Delete(T data)
        {

        }
        public void Save()
        {

        }
    }
}
