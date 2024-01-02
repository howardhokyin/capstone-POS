using POS.Models;
using POS_System.Database;
using POS_System.Dialog;
using POS_System.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
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

namespace POS_System.Pages
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        private DatabaseHelper db;
        private ObservableCollection<User> users = new ObservableCollection<User>();
        public User user = new User();
        public LoginScreen()
        {
            InitializeComponent();
            id.Focus();
            db = new DatabaseHelper("localhost", "pos_db", "root", "password");
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string enteredUserId = id.Text;
            int UserID = int.Parse(enteredUserId);
            string enteredPassword = password.Password;
            user.Id = UserID;
            

            if (db.AuthenticateUser(enteredUserId, enteredPassword))
            {
                
                string authenticatedUsername = db.GetUsername(enteredUserId);
                loginbtn.Visibility = Visibility.Collapsed;
                Label label = new Label();
                label.Visibility = Visibility.Collapsed;
                TextBox textBox = new TextBox();
                textBox.Visibility = Visibility.Collapsed;
                PasswordBox passwordBox = new PasswordBox();
                passwordBox.Visibility = Visibility.Collapsed;
                WelcomeDialog welcomDialog = new WelcomeDialog(this,authenticatedUsername);
                if (welcomDialog.ShowDialog() == true)
                {

                    int userId = int.Parse(enteredUserId);

                    // Only 100 to 110 admin can go to AdminManagement page
                    if (userId >= 100 & userId <= 199)
                    {
                        AdminManagement windowAdmin = new AdminManagement();
                        windowAdmin.Show();
                    }
                    else if (userId >= 200 & userId <= 299)
                    {
                        ManagerManagement windowManager = new ManagerManagement();
                        windowManager.Show();
                    }

                    else
                    {
                        TablePage window2 = new TablePage();
                        window2.Show();
                    }

                    // Close the current login window if needed
                    this.Close();
                }
                else
                {
                    int userId = int.Parse(enteredUserId);

                    // Only 100 to 110 admin can go to AdminManagement page
                    if (userId >= 100 & userId <= 199)
                    {
                        AdminManagement windowAdmin = new AdminManagement();
                        windowAdmin.Show();
                    }
                    else if (userId >= 200 & userId <= 299)
                    {
                        ManagerManagement windowManager = new ManagerManagement();
                        windowManager.Show();
                    }

                    else
                    {
                        TablePage window2 = new TablePage();
                        window2.Show();
                    }

                    // Close the current login window if needed
                    this.Close();
                }




            }
            else
            {
                MessageBox.Show("Invalid userid or password. Please try again.");
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSubmit_Click(sender, e);
            }
        }


        private void id_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }


    }
}
