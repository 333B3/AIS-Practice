using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MAG
{
    public partial class Order : Window
    {
        public Order()
        {
            InitializeComponent();
            DisplayType();
            DisplayProduct();
            this.Top = 200;  
            this.Left = 200; 
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Admin admin = new();
            admin.Show();
            this.Hide();
        }
        // Підключення до БД та вивід
        private void DisplayType()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT type FROM search LIMIT 1";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string modelType = reader["type"].ToString();
                        model.Text = $"{modelType}";
                    }
                    else
                    {
                        model.Text = "";
                    }
                }
                string num = "SELECT number FROM num LIMIT 1";

                using (var command = new SQLiteCommand(num, connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string NUM = reader["number"].ToString();
                        Number.Text = $"{NUM}";
                    }
                    else
                    {
                        Number.Text = "";
                    }

                }
            }
        }
            
        // Вивід назви продукту та повної ціни
        private void DisplayProduct()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            StringBuilder productTypes = new StringBuilder();
            decimal totalPrice = 0;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string productQuery = "SELECT type FROM search3";
                using (var productCommand = new SQLiteCommand(productQuery, connection))
                using (var productReader = productCommand.ExecuteReader())
                {
                    while (productReader.Read())
                    {
                        string productType = productReader["type"].ToString();
                        if (productTypes.Length > 0)
                            productTypes.Append(", ");
                        productTypes.Append(productType);
                    }
                }
                string priceQuery = "SELECT SUM(price) AS TotalPrice FROM search4";
                using (var priceCommand = new SQLiteCommand(priceQuery, connection))
                using (var priceReader = priceCommand.ExecuteReader())
                {
                    if (priceReader.Read() && priceReader["TotalPrice"] != DBNull.Value)
                    {
                        totalPrice = Convert.ToDecimal(priceReader["TotalPrice"]);
                    }
                }

            }
            poduct.Text = $"{productTypes}";
            price.Text = $"Всього: {totalPrice:C}";
        }

        private void buy_Click(object sender, RoutedEventArgs e)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string deleteSearch = "DELETE FROM search";
                    using (SQLiteCommand deleteCommandSearch3 = new SQLiteCommand(deleteSearch, connection))
                    {
                        deleteCommandSearch3.ExecuteNonQuery();
                    }
                    string deletenum = "DELETE FROM num";
                    using (SQLiteCommand deleteCommandSearch3 = new SQLiteCommand(deletenum, connection))
                    {
                        deleteCommandSearch3.ExecuteNonQuery();
                    }
                    string deleteSearch3 = "DELETE FROM search3";
                    using (SQLiteCommand deleteCommandSearch3 = new SQLiteCommand(deleteSearch3, connection))
                    {
                        deleteCommandSearch3.ExecuteNonQuery();
                    }
                    string deleteSearch4 = "DELETE FROM search4";
                    using (SQLiteCommand deleteCommandSearch3 = new SQLiteCommand(deleteSearch4, connection))
                    {
                        deleteCommandSearch3.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка оновлення: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            Order newOrderWindow = new Order();
            newOrderWindow.Show();
            this.Close();
        }
    }
}
