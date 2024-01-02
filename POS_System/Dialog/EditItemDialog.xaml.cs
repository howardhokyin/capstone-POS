using MySql.Data.MySqlClient;
using POS_System.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace POS_System
{
    /// <summary>
    /// Interaction logic for EditCategoryDialog.xaml
    /// </summary>
    public partial class EditItemDialog : Window
    {
        string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";
        public int editedId { get; set; } // Change 'private set' to 'set'
        public string editedName { get; set; }
        public double editedPrice { get; set; }
        public string editedDescripion { get; set; }
        public string editedCategory { get; set; }

        public EditItemDialog(int currentId,string currentName, double currentPrice, string currentDescription, string currentCategory)
        {
            InitializeComponent();
            LoadCategories();
            CurrentIdTextBox.Text = currentId.ToString();
            CurrentNameTextBox.Text = currentName;
            CurrentPriceTextBox.Text = currentPrice.ToString();
            CurrentDescriptionTextBox.Text = currentDescription;
            CurrentCategoryComboBox.Text = currentCategory;

            EditedIdTextBox.Text = currentId.ToString();
            EditedNameTextBox.Text = currentName;
            EditedItemDescriptionTextBox.Text = currentDescription;
            EditedItemCategoryComboBox.Text = currentCategory;


        }

        private void SaveItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                editedId = int.Parse(EditedIdTextBox.Text);
                editedName = EditedNameTextBox.Text;
                editedPrice = double.Parse(EditedPriceTextBox.Text);
                editedDescripion = EditedItemDescriptionTextBox.Text;
                editedCategory = EditedItemCategoryComboBox.Text;
                
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void CancelItemButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void LoadCategories()
        {
            ObservableCollection<Category> categories = new ObservableCollection<Category>();

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

            EditedItemCategoryComboBox.ItemsSource = categories;
        }


    }
}
