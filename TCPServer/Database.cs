using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class Database
    {
        MySqlConnection conn;
        private string server = "217.182.34.233";
        private string user = "gs18987";
        private string password = "8ucADqSHhsCLCDEEBYo8AIKZjkZvFHVf";
        private string db_name = "gs18987";

        private string users_table_name = "users_table";
        private string docs_passport_table_name = "docs_passports_table";
        private string users_applications_table_name = "users_applications_table";

        public void Initialize()
        {
            string database = $"server={server};user={user};password={password};database={db_name};";
            string query = $"SELECT * FROM {users_table_name}";

            conn = new MySqlConnection(database);
        }

        public void Connection(bool state)
        {
            if (state) conn.Open();
            else conn.Close();
        }

        public void CreateUser(User user)
        {
            if (GetUser(user.email).email != "-1") throw new Exception("User is in the database.");

            Connection(true);
            string query = $"INSERT INTO {users_table_name} (first_name, last_name, last_last_name, sex, email, password, birth_date) VALUES('{user.first_name}', '{user.last_name}', '{user.last_last_name}', '{user.sex}', '{user.email}', '{user.password}', '{user.birth_date}')";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            Connection(false);
        }

        public User AuthorizateUser(string email, string p)
        {
            var user = GetUser(email);
            if (user.password != p)
            {
                return user;
            }
            return user;
        }

        public User GetUser(string email)
        {
            Connection(true);

            string query = $"SELECT * FROM {users_table_name} WHERE email = '{email}'";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                User user = new User((string)reader["first_name"], (string)reader["last_name"], (string)reader["last_last_name"], (string)reader["sex"], (string)reader["email"], (string)reader["password"], (string)reader["birth_date"]);
                Connection(false);

                return user;
            }

            Connection(false);
            return new User("-1", "-1", "-1", "-1", "-1", "-1", "-1");
        }

        public Passport GetPassport(string email)
        {
            Connection(true);

            string query = $"SELECT * FROM {docs_passport_table_name} WHERE email = '{email}'";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Passport passport = new Passport(email, (int)reader["series"], (int)reader["number"], (string)reader["date"], (int)reader["division_code"]);
                Connection(false);

                return passport;
            }

            Connection(false);
            return new Passport("-1", -1, -1, "-1", -1);
        }

        public bool SetPassport(Passport passport)
        {
            string query;

            if (GetPassport(passport.email).email != "-1")
                query = $"UPDATE {docs_passport_table_name} SET series = '{passport.series}', number = '{passport.number}', date = '{passport.date}', division_code = '{passport.division_code}' WHERE email = '{passport.email}'";
            else query = $"INSERT INTO {docs_passport_table_name} (email, series, number, date, division_code) VALUES('{passport.email}', '{passport.series}', '{passport.number}', '{passport.date}', '{passport.division_code}')";

            Connection(true);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            Connection(false);

            return true;
        }

        public Application GetUserApplication(string email, string title)
        {
            string query = $"SELECT * FROM {users_applications_table_name} WHERE email = '{email}', title = '{title}'";

            Connection(true);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Application application = new Application((string)reader["from_email"], (string)reader["to_email"], (int)reader["timestamp"], (string)reader["title"], (string)reader["description"]);

                Connection(false);
                return application;
            }

            Connection(false);
            return new Application("-1", "-1", -1, "-1", "-1"); //Wrong email or title
        }

        public Application[] GetUserApplications(string email)
        {
            List<Application> applications = new List<Application>();
            string query = $"SELECT * FROM {users_applications_table_name} WHERE email = '{email}'";

            Connection(true);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Application application = new Application((string)reader["from_email"], (string)reader["to_email"], (int)reader["timestamp"], (string)reader["title"], (string)reader["description"]);
                applications.Add(application);
            }

            Connection(false);

            return applications.ToArray();
        }

        public bool SendApplication(Application application)
        {
            if (GetUserApplication(application.from_email, application.title).from_email != "-1") return false; 

            Connection(true);
            string query = $"INSERT INTO {users_applications_table_name} (from_email, to_email, timestamp, title, description) VALUES('{application.from_email}', '{application.timestamp}', '{application.to_email}', '{application.title}', '{application.description}')";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            Connection(false);

            return true;
        }
    }
}