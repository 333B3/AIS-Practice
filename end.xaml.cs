using System.Data.SQLite;
using System.Windows;
using System.Text;

namespace MAG
{
    public partial class end : Window
    {
        public end()
        {
            InitializeComponent();
            DisplayType();
            DisplayProduct();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            products products = new();
            products.Show();
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
                        model.Text = "Не знайдено";
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
                    string insertNum = "INSERT INTO num (price) VALUES (@type)";
                    using (SQLiteCommand insertCommandNum = new SQLiteCommand(insertNum, connection))
                    {
                        insertCommandNum.Parameters.AddWithValue("@type", totalPrice);
                        insertCommandNum.ExecuteNonQuery();
                    }
            }
            poduct.Text = $"{productTypes}";
            price.Text = $"Всього: {totalPrice:C}";
        }

        private void buy_Click(object sender, RoutedEventArgs e)
        {
            Buy Buy = new();
            Buy.Show();
            this.Hide();
        }
    }
}

