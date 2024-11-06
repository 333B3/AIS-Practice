using System.Data.SQLite;
using System.Windows;

namespace MAG
{
    public partial class add : Window
    {
        public add()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            this.Hide();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string category = Categories.Text;
            string motorcycleName = Name.Text;
            string productName = Product.Text;
            string productPrice = Price.Text;
            if (!decimal.TryParse(productPrice, out decimal priceValue))
            {
                MessageBox.Show("Число некоректне", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //підключення до бази даних
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // таблиця categories
                    string insertCategory = "INSERT INTO categories (price, categories, name, Moto) VALUES (@productPrice, @category, @productName, @motorcycleName)";
                    using (SQLiteCommand command = new SQLiteCommand(insertCategory, connection))
                    {
                        command.Parameters.AddWithValue("@productPrice", productPrice);
                        command.Parameters.AddWithValue("@category", category);
                        command.Parameters.AddWithValue("@productName", productName);
                        command.Parameters.AddWithValue("@motorcycleName", motorcycleName);
                        command.ExecuteNonQuery();
                    }

                    // таблиця list
                    string insertNameQuery = "INSERT INTO list (name) VALUES (@name)";
                    using (SQLiteCommand nameCommand = new SQLiteCommand(insertNameQuery, connection))
                    {
                        nameCommand.Parameters.AddWithValue("@name", motorcycleName);
                        nameCommand.ExecuteNonQuery();
                    }

                    // таблиця prod
                    string insertProductName = "INSERT INTO prod (name) VALUES (@productName)";
                    using (SQLiteCommand command = new SQLiteCommand(insertProductName, connection))
                    {
                        command.Parameters.AddWithValue("@productName", productName);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Данные успешно добавлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
