using MySql.Data.MySqlClient;
using POS.Models;
using POS_System.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace POS_System.Pages
{
    public partial class ManageMenu : Window
    {
        //categories
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        //new order
        private ObservableCollection<Item> items = new ObservableCollection<Item>();

        string connectionString = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";

        public ManageMenu()
        {
            InitializeComponent();
            AddCategoryButtonVisibility(false);
            AddItemButtonVisibility(false);

        }

        //Method: Add category button, if true, visible. otherwise, collapsed(ie. hide)
        public void AddCategoryButtonVisibility(bool isVisible)
        {
            AddCategoryButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        }
        public void AddItemButtonVisibility(bool isVisible)
        {
            AddItemButton.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        }



        private void ShowItem_Click(object sender, RoutedEventArgs e)
        {
            itemCategoryDataGrid.ContentTemplate = (DataTemplate)this.Resources["ItemTemplate"];
            itemCategoryDataGrid.Content = GetAllItems();
            
            AddItemButtonVisibility(true);
            AddCategoryButtonVisibility(false);

        }

        private void ShowCategory_Click(object sender, RoutedEventArgs e)
        {
            itemCategoryDataGrid.ContentTemplate = (DataTemplate)this.Resources["CategoryTemplate"];
            itemCategoryDataGrid.Content = GetAllCategories();
            AddCategoryButtonVisibility(true);
            AddItemButtonVisibility(false);

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void DeleteItemButton_Click(Object sender, RoutedEventArgs e)
        {
            MessageBox.Show("click delete");
            if (sender is Button button && button.CommandParameter is Item item)
            {
                int id = item.Id;
                string name = item.item_name;
                double price = item.ItemPrice;
                string description = item.Description;
                string category = item.Category;


                // Confirm user wants to delete with the user's name included
                MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure you want to delete {name}?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    DeleteItemFromDatabase(id,name,price,description,category);
                    MessageBox.Show("Delete successfully");
                    itemCategoryDataGrid.Content = GetAllItems();
                }
                else
                {
                    return;
                }
            }
        }


        private void DeleteButton_Click(Object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Category category)
            {
                int id = category.Id;
                string name = category.Name;
             

                // Confirm user wants to delete with the user's name included
                MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure you want to delete {name}?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    DeleteCategoryFromDatabase(id);
                    MessageBox.Show("Delete successfully");
                    itemCategoryDataGrid.Content = GetAllCategories();
                }
                else
                {
                    return;
                }
            }
        }

        private void EditItemButton_Click(Object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Item item)
            {
                int id = item.Id;
                string name = item.item_name;
                double price = item.ItemPrice;
                string description = item.Description;
                string category = item.Category;





                EditItemDialog editItemDialog = new EditItemDialog(id, name, price, description,category);
                if (editItemDialog.ShowDialog() == true)
                {
                    int editedId = editItemDialog.editedId;
                    string editedName = editItemDialog.editedName;
                    double editedprice = editItemDialog.editedPrice;
                    string editedDescription = editItemDialog.editedDescripion;
                    string editedCategory = editItemDialog.editedCategory;
                    

                    if (!string.IsNullOrWhiteSpace(editedName))
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure want to edit {editedName}?", "Delete Confirmation", MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            if (EditItemFromDatabase(editedId, editedName,editedprice,editedDescription,editedCategory))
                            {
                                MessageBox.Show($"Updated Data!");
                                itemCategoryDataGrid.Content = GetAllItems();
                            }
                            else
                            {
                                MessageBox.Show("Category name cannot be empty.");
                            }
                        }
                        else
                        {
                            return;
                        }

                    }


                }
            }
        }

        private void EditButton_Click(Object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Category category)
            {
                int id = category.Id;
                string name = category.Name;





                var editCategoryDialog = new EditCategoryDialog(id, name);
                if (editCategoryDialog.ShowDialog() == true)
                {
                    // Retrieve the category name from the dialog
                    string categoryName = editCategoryDialog.EditedCategoryName;
                    int categoryId = editCategoryDialog.EditedCategoryId;

                    if (!string.IsNullOrWhiteSpace(categoryName))
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure want to edit {categoryName}?", "Delete Confirmation", MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            if (EditCategoryFromDatabase(categoryName, categoryId))
                            {
                                MessageBox.Show($"Updated data!");
                                itemCategoryDataGrid.Content = GetAllCategories();
                            }
                            else
                            {
                                MessageBox.Show("Category name cannot be empty.");
                            }
                        }
                        else
                        {
                            return;
                        }

                    }


                }
            }
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var addCategoryDialog = new AddCategoryDialog();
            if (addCategoryDialog.ShowDialog() == true)
            {
                // Retrieve the category name from the dialog
                string categoryName = addCategoryDialog.CategoryName;
                int categoryId = addCategoryDialog.CategoryId;

                if (!string.IsNullOrWhiteSpace(categoryName))
                {
                    // Insert the new category into your database
                    if (InsertCategoryIntoDatabase(categoryName, categoryId))
                    {
                        MessageBox.Show("Added to Category!");
                        itemCategoryDataGrid.Content = GetAllCategories();
                    }
                }
                else
                {
                    MessageBox.Show("Category name cannot be empty.");
                }
            }
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            AddItemDialog addItemDialog = new AddItemDialog();
            if (addItemDialog.ShowDialog() == true)
            {
                // Retrieve the category name from the dialog
                int itemId = addItemDialog.id;
                string itemName = addItemDialog.name;
                double itemPrice = addItemDialog.price;
                string itemDesciption = addItemDialog.descripion;
                string itemCategory = addItemDialog.category;

                if (!string.IsNullOrWhiteSpace(itemName))
                {
                    // Insert the new category into your database
                    if (InsertItemIntoDatabase(itemId, itemName, itemPrice, itemDesciption, itemCategory))
                    {
                        MessageBox.Show("Added to Item!");
                        itemCategoryDataGrid.Content = GetAllItems();
                    }
                }
                else
                {
                    MessageBox.Show("Item name cannot be empty.");
                }
            }
        }

        //Method: To get all item from database
        private ObservableCollection<Item> GetAllItems()
        {
            items = new ObservableCollection<Item>();

            using (var connection = new MySqlConnection(connectionString))
            {
                var cmd = new MySqlCommand("SELECT * FROM item ORDER BY 1", connection);
                try
                {
                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new Item
                            {
                                Id = Convert.ToInt32(reader["item_id"]),
                                item_name = reader["item_name"].ToString(),
                                ItemPrice = Convert.ToDouble(reader["item_price"]),
                                Description = reader["item_description"].ToString(),
                                Category = reader["item_category"].ToString()
                                
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                }
            }
            return items;
        }


        private void PreviewMenu_Click(object sender, RoutedEventArgs e)
        {
            PreviewMenuDialog previewMenuDialog = new PreviewMenuDialog();
            previewMenuDialog.ShowDialog();

        }
        private ObservableCollection<Category> GetAllCategories()
        {
            var categorylist = new Category();
            categories = new ObservableCollection<Category>();

            using (var connection = new MySqlConnection(connectionString))
            {
                var cmd = new MySqlCommand("SELECT * FROM category ORDER BY 1", connection);
                try
                {
                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Category category = new Category
                            {
                                Id = Convert.ToInt32(reader["category_id"]),
                                Name = reader["category_name"].ToString(),
                            };
                            categories.Add(category);
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                }
            }
            return categories;
        }

        //Method: add new item to database category
        private bool InsertItemIntoDatabase(int itemId, string itemName, double itemPrice, string itemDescription, string itemCategory)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString)) // Ensure connStr is your connection string
                {
                    conn.Open();

                    string insertQuery = "INSERT INTO item (item_id, item_name, item_price, item_description, item_category) VALUES (@itemId, @itemName, @itemPrice, @itemDescription, @itemCategory);";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@itemId", itemId);
                        cmd.Parameters.AddWithValue("@itemName", itemName);
                        cmd.Parameters.AddWithValue("@itemPrice", itemPrice);
                        cmd.Parameters.AddWithValue("@itemDescription", itemDescription);
                        cmd.Parameters.AddWithValue("@itemCategory", itemCategory);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding the category: " + ex.Message);
                return false;
            }
        }

        //Method: add category id and name to database category
        private bool InsertCategoryIntoDatabase(string categoryName, int categoryId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString)) // Ensure connStr is your connection string
                {
                    conn.Open();

                    string insertQuery = "INSERT INTO category (category_name, category_id) VALUES (@categoryName, @categoryId);";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryName", categoryName);
                        cmd.Parameters.AddWithValue("@categoryId", categoryId);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding the category: " + ex.Message);
                return false;
            }
        }

        //Method: delete item
        private bool DeleteItemFromDatabase(int id, string name, double price, string description, string category)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString)) // Ensure connStr is your connection string
                {
                    conn.Open();

                    string DeleteQuery = "DELETE FROM item WHERE item_id=@itemId;";
                    using (MySqlCommand cmd = new MySqlCommand(DeleteQuery, conn))
                    {

                        cmd.Parameters.AddWithValue("@itemId", id);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while delete the category: " + ex.Message);
                return false;
            }
        }



        //Method: delete category
        private bool DeleteCategoryFromDatabase(int categoryId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString)) // Ensure connStr is your connection string
                {
                    conn.Open();

                    string DeleteQuery = "DELETE FROM category WHERE category_id=@categoryId;";
                    using (MySqlCommand cmd = new MySqlCommand(DeleteQuery, conn))
                    {

                        cmd.Parameters.AddWithValue("@categoryId", categoryId);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while delete the category: " + ex.Message);
                return false;
            }
        }

        //Method: edit category
        private bool EditCategoryFromDatabase(string categoryName,int categoryId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString)) // Ensure connStr is your connection string
                {
                    conn.Open();

                    string EditCategoryQuery = "UPDATE `category` SET category_name = @categoryName WHERE category_id = @categoryId;";
                    using (MySqlCommand cmd = new MySqlCommand(EditCategoryQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@categoryName", categoryName);
                        cmd.Parameters.AddWithValue("@categoryId", categoryId);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while edit the category: " + ex.Message);
                return false;
            }
        }

        private bool EditItemFromDatabase(int itemId,string itemName, double itemPrice, string itemDescription, string itemCategory)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString)) // Ensure connStr is your connection string
                {
                    conn.Open();

                    string EditItemQuery = "UPDATE `item` SET item_name = @itemName, item_price = @itemPrice, item_description = @itemDescription, item_category = @itemCategory WHERE item_id = @itemId;";
                    using (MySqlCommand cmd = new MySqlCommand(EditItemQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@itemId", itemId);
                        cmd.Parameters.AddWithValue("@itemName", itemName);
                        cmd.Parameters.AddWithValue("@itemPrice", itemPrice);
                        cmd.Parameters.AddWithValue("@itemDescription", itemDescription);
                        cmd.Parameters.AddWithValue("@itemCategory", itemCategory);


                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while edit the category: " + ex.Message);
                return false;
            }
        }



       


    }
}