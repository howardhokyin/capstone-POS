using MySql.Data.MySqlClient;
using POS.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POS_System.Pages
{
    /// <summary>
    /// Interaction logic for PreviewMenuDialog.xaml
    /// </summary>
    public partial class PreviewMenuDialog : Window
    {
        public string connStr = "SERVER=localhost;DATABASE=pos_db;UID=root;PASSWORD=password;";

        //categories
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        //new order
        private ObservableCollection<Item> items = new ObservableCollection<Item>();
        public PreviewMenuDialog()
        {
            InitializeComponent();
            LoadCategoryData();
            
        }

        private void LoadCategoryData()
        {

            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();
                string sql = "SELECT * FROM category;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Category category = new Category
                    {
                        Id = Convert.ToInt32(rdr["category_id"]),
                        Name = rdr["category_name"].ToString(),
                    };

                    categories.Add(category);

                    Button newCategoryButton = new Button();
                    newCategoryButton.Content = rdr["category_name"].ToString();
                    newCategoryButton.Tag = category;
                    newCategoryButton.Click += (sender, e) => LoadItemsByCategory(newCategoryButton.Content.ToString());
                    newCategoryButton.Width = 120;
                    newCategoryButton.Height = 50;
                    newCategoryButton.FontSize = 15;
                    newCategoryButton.Background = Brushes.DarkOrange;

                    newCategoryButton.Margin = new Thickness(5);
                    SetButtonStyle(newCategoryButton);

                    CategoryButtonPanel.Children.Add(newCategoryButton);
                }

                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }



        private void LoadItemsByCategory(string categoryName)
        {
            ItemButtonPanel.Children.Clear();

            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();
                string sql = "SELECT * FROM item WHERE item_category = @category;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@category", categoryName);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Item item = new Item
                    {
                        Id = Convert.ToInt32(rdr["item_id"]),
                        item_name = rdr["item_name"].ToString(),
                        ItemPrice = Convert.ToDouble(rdr["item_price"]),
                        Description = rdr["item_description"].ToString(),
                        Category = rdr["item_category"].ToString()
                    };

                    Button newItemButton = new Button();
                    // Create a TextBlock for the button content
                    TextBlock textBlock = new TextBlock
                    {
                        Text = rdr["item_name"].ToString(),
                        TextWrapping = TextWrapping.Wrap, // Enable text wrapping
                        TextAlignment = TextAlignment.Center
                    };

                    newItemButton.Content = textBlock;
                    newItemButton.Tag = item;
                    newItemButton.Width = 120;
                    newItemButton.Height = 80;
                    newItemButton.FontSize = 15;
                    newItemButton.Background = Brushes.LightGoldenrodYellow;


                    SetButtonStyle(newItemButton);
                    ItemButtonPanel.Children.Add(newItemButton);
                }

                rdr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }


        private void SetButtonStyle(Button button)
        {
            button.FontFamily = new FontFamily("Verdana");
            button.FontSize = 20;
            button.FontWeight = FontWeights.Bold;
            button.BorderBrush = Brushes.Orange;
            button.Margin = new Thickness(5);
        }
    }
}
