using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;


namespace MAG
{
    public partial class Categories : Window
    {
        private string selectedData; 

        public Categories()
        {
            this.Top = 200;
            this.Left = 200;
            InitializeComponent();
            selectedData = GetDataFromSearch(); 
            if (!string.IsNullOrEmpty(selectedData))
            {
                CreateButtons(); 
            }
            else
            {
                MessageBox.Show("Дані не знайдені", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Підключення до БД
        private string GetDataFromSearch()
        {
            string motoType = null;
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT type FROM search LIMIT 1"; 
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                motoType = reader["type"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка зчитування: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return motoType;
        }
        // Підключення до БД
        private List<string> GetFromDB(string moto)
        {
            List<string> categories = new List<string>();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Categories FROM categories WHERE Moto = @moto";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@moto", moto);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            HashSet<string> uniqueCategories = new HashSet<string>();

                            while (reader.Read())
                            {
                                uniqueCategories.Add(reader["Categories"].ToString());
                            }
                            categories = uniqueCategories.ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка зчитування: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


            return categories;
        }
        // Створення кнопок
        private void CreateButtons()
        {
            List<string> categories = GetFromDB(selectedData);

            foreach (var category in categories)
            {
                Button categoryButton = new Button
                {
                    Content = category,
                    Width = 200,
                    Height = 30,
                    Margin = new Thickness(5),
                    Tag = category
                };
                categoryButton.Click += CategoryButton_Click;
                CategoriesStackPanel.Children.Add(categoryButton);
            }
        }


        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            string selectedCategory = clickedButton.Tag.ToString();

            UpdateSearchTable(selectedCategory);
            Products products = new Products();
            products.Show();
            this.Hide();
        }
        private void UpdateSearchTable(string newCategory)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM search2";
                    using (SQLiteCommand deleteCommand = new SQLiteCommand(deleteQuery, connection))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }
                    string insertQuery = "INSERT INTO search2 (type) VALUES (@type)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@type", newCategory);
                        insertCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при оновлені даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            List list = new List();
            list.Show();
            this.Hide();
        }
    }
}
