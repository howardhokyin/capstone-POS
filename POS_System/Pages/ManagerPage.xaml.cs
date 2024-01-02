using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using POS_System.Database;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;


namespace POS_System.Pages
{
    /// <summary>
    /// Interaction logic for ManagerPage.xaml
    /// </summary>
    public partial class ManagerPage : Window

    {
        List<String> managerList = new List<string> { "Waiter" };

        private DatabaseHelper db;
        public ManagerPage()
        {
            InitializeComponent();

            if (POS.Models.User.id >= 200)
            {
                GetOnlyWaiter();
                UserCombobox.ItemsSource = managerList;
            }
        }

        private void getAllUser()
        {
            //Tutorial used https://www.youtube.com/watch?v=OPDPI5exPp8

            //db = new DatabaseHelper("localhost", "pos_db", "root", "password");

            //String to make connection to database
            string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";

            //Create a connection object
            MySqlConnection connection = new MySqlConnection(connectionString);

            //SQL query
            MySqlCommand cmd = new MySqlCommand("select * from user order by 1", connection);

            //Open up connection with the user table
            connection.Open();

            //create a datatable object to capture the database table
            DataTable dt = new DataTable();

            //Execute the command and the load the result of reader inside the datatable
            dt.Load(cmd.ExecuteReader());

            //Close connection to user table
            connection.Close();

            userGrid.DataContext = dt;
        }

        private void GetOnlyWaiter()
        {
            //Tutorial used https://www.youtube.com/watch?v=OPDPI5exPp8

            //db = new DatabaseHelper("localhost", "pos_db", "root", "password");

            //String to make connection to database
            string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";

            //Create a connection object
            MySqlConnection connection = new MySqlConnection(connectionString);

            //SQL query
            MySqlCommand cmd = new MySqlCommand("select * from user where user_id >= 300 order by 1", connection);

            //Open up connection with the user table
            connection.Open();

            //create a datatable object to capture the database table
            DataTable dt = new DataTable();

            //Execute the command and the load the result of reader inside the datatable
            dt.Load(cmd.ExecuteReader());

            //Close connection to user table
            connection.Close();

            userGrid.DataContext = dt;


        }

        private void MakeComboBoxReadOnly()
        {
            ComboBox comboBox = new ComboBox();
            comboBox.IsReadOnly = true;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addUser_Click(object sender, RoutedEventArgs e)
        {

            //Tutorial used https://www.includehelp.com/dot-net/insert-records-into-mysql-database-in-csharp.aspx

            String username = adduser_usernameBox.Text;
            String password = adduser_passwordBox.Password;
            String id = adduser_idBox.Text;

            try
            {

                //String to make connection to database
                string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";

                //Create a connection object
                MySqlConnection connection = new MySqlConnection(connectionString);

                //SQL query
                MySqlCommand cmd = new MySqlCommand("insert into user (user_id, user_name, user_password) values (" + id + ",'" + username + "','" + password + "')", connection);

                //Open up connection with the user table
                connection.Open();

                //Execute the command
                cmd.ExecuteNonQuery();

                //Close connection to user table
                connection.Close();
                GetOnlyWaiter();

                //Clear textboxes after method sucessfully executes
                adduser_usernameBox.Clear();
                adduser_passwordBox.Clear();
                adduser_idBox.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show("ID " + id + " is already in use!");
            }
        }

        private void adduser_idBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void editUser_Click(object sender, RoutedEventArgs e)
        {
            String username = edituser_usernameBox.Text;
            String password = edituser_passwordBox.Password;
            String id = edituser_idBox.Text;

            //String to make connection to database
            string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";

            //Create a connection object
            MySqlConnection connection = new MySqlConnection(connectionString);

            //SQL query
            MySqlCommand cmd = new MySqlCommand("update user set user_name = '" + username + "', user_password = '" + password + "' where user_id = " + id, connection);

            //Open up connection with the user table
            connection.Open();

            //Execute the command
            cmd.ExecuteNonQuery();

            //Close connection to user table
            connection.Close();
            GetOnlyWaiter();

            //Clear textboxes after method sucessfully executes
            edituser_idBox.Clear();
            edituser_passwordBox.Clear();
            edituser_usernameBox.Clear();
        }

        private void edituser_idBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            //Tutorial used https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/how-to-detect-when-text-in-a-textbox-has-changed?view=netframeworkdesktop-4.8

            String id = edituser_idBox.Text;

            try
            {
                MySqlDataReader dr;

                //String to make connection to database
                string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";

                //Create a connection object
                MySqlConnection connection = new MySqlConnection(connectionString);

                //SQL query
                MySqlCommand cmd = new MySqlCommand("select * from user where user_id = " + id, connection);

                //Open up connection with the user table
                connection.Open();

                //Execute the command
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    edituser_usernameBox.Text = dr.GetValue(1).ToString();
                    edituser_passwordBox.Password = dr.GetValue(2).ToString();
                }

                //Close connection to user table
                connection.Close();

            }
            catch (Exception ex) //This exception is thrown when the edituser_idBox is empty. If the edituser_idBox is empty, then the other edituser boxes should be empty too.
            {
                edituser_usernameBox.Clear();
                edituser_passwordBox.Clear();
            }
        }

        // Inside dataGrid Edit button
        private void AdminEditButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)userGrid.SelectedItem;
            if (selectedRow != null)
            {
                edituser_idBox.Text = selectedRow["user_id"].ToString();
                edituser_usernameBox.Text = selectedRow["user_name"].ToString();
            }
        }



        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Ensure that the button clicked is actually in the DataGrid
            if (sender is Button button && button.DataContext is DataRowView dataRow)
            {
                var id = dataRow["user_id"];
                var username = dataRow["user_name"].ToString();

                // Confirm user wants to delete with the user's name included
                MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure you want to delete {username}?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    // If yes, delete row
                    // String to make connection to database
                    string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";
                    try
                    {
                        // Create a connection object
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            using (MySqlCommand cmd = new MySqlCommand("DELETE FROM user WHERE user_id = @Id", connection))
                            {
                                cmd.Parameters.AddWithValue("@Id", id);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        GetOnlyWaiter();
                    }
                    catch (Exception ex)
                    {
                        // Log or display the error message
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("An error occurred while deleting the user. Please try again.");
                    }
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //AdminManagement adminManagement = new AdminManagement();
            //adminManagement.Show();
            this.Close();
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selectedRole = UserCombobox.SelectedItem.ToString();
            int newID = GetNextIDForRole(selectedRole);
            adduser_idBox.Text = newID.ToString();
        }

        private int GetNextIDForRole(string role)
        {
            int hightestID = 0;
            int startingNumber = 0;

            // Define starting numbers for each role
            switch (role)
            {
                case "Admin":
                    startingNumber = 100;
                    break;
                case "Manager":
                    startingNumber = 200;
                    break;
                case "Waiter":
                    startingNumber = 300;
                    break;
            }

            using (DatabaseHelper db = new DatabaseHelper("localhost", "pos_db", "root", "password"))
            {
                if (db.OpenConnection())
                {
                    try
                    {
                        string query = $"SELECT MAX(user_id) FROM user WHERE user_id >= {startingNumber} AND user_id < {startingNumber + 100}";

                        // Use reflection to access the private sqlConn field in DatabaseHelper
                        FieldInfo fieldInfo = db.GetType().GetField("sqlConn", BindingFlags.NonPublic | BindingFlags.Instance);
                        MySqlConnection sqlConn = null;
                        if (fieldInfo != null)
                        {
                            sqlConn = (MySqlConnection)fieldInfo.GetValue(db);
                        }

                        MySqlCommand command = new MySqlCommand(query, sqlConn);
                        object result = command.ExecuteScalar();


                        if (result != null && int.TryParse(result.ToString(), out hightestID))
                        {
                            hightestID += 1;
                        }
                        else
                        {
                            // If there's no ID for the role yet, start with the startingNumber
                            hightestID = startingNumber;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error:   " + ex.Message);
                    }
                    finally
                    {
                        db.CloseConnection();
                    }
                }
            }
            return hightestID;
        }
    }
}