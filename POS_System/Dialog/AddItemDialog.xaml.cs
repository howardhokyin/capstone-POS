using MySql.Data.MySqlClient;
using POS.Models;
using POS_System.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace POS_System
{
    /// <summary>
    /// Interaction logic for AddCategoryDialog.xaml
    /// </summary>
    public partial class AddItemDialog : Window
    {
        string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";
        public int id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string descripion { get; set; }
        public string category { get; set; }

        public AddItemDialog()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            id = int.Parse(IdTextBox.Text);
            name = NameTextBox.Text;
            price = double.Parse(PriceTextBox.Text);
            descripion = DescriptionTextBox.Text;
            category = CategoryComboBox.Text;
            DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void LoadCategories()
        {
            ObservableCollection <Category> categories = new ObservableCollection<Category>();

            using (var connection = new MySqlConnection(connectionString))
            {
                var cmd = new MySqlCommand("SELECT category_id, category_name FROM category ORDER BY category_name", connection);

                try
                {
                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Category category = new Category()
                            {
                                Id = reader.GetInt32("category_id"),
                                Name = reader.GetString("category_name")
                            };
                            categories.Add(category);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading categories: {ex.Message}");
                }
            }

            CategoryComboBox.ItemsSource = categories;
        }



    }
}
