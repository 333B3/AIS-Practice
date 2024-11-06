using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace MAG
{
    public partial class products : Window
    {
        public products()
        {
            InitializeComponent();
            CreateButtonsForCategories();
        }

        private void CreateButtonsForCategories()
        {
            List<CategoryItem> categoryItems = GetMatchingCategoriesWithPrices();

            if (categoryItems.Count == 0)
            {
                MessageBox.Show("Категорії не знайдені", "Повідомлення", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            foreach (var item in categoryItems)
            {
                Button categoryButton = new Button
                {
                    Content = $"{item.Name} // {item.Price:C}",
                    Width = 400,
                    Height = 30,
                    Margin = new Thickness(5),
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Tag = item
                };

                categoryButton.Click += CategoryButton_Click;
                CategoriesStackPanel.Children.Add(categoryButton);
            }
        }
        // Підключення до БД
        private List<CategoryItem> GetMatchingCategoriesWithPrices()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            List<CategoryItem> categoryItems = new List<CategoryItem>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                    SELECT DISTINCT c.Name, c.price
                    FROM categories c
                    JOIN search s ON c.Moto = s.type
                    JOIN search2 s2 ON c.Categories = s2.type";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categoryItems.Add(new CategoryItem
                                {
                                    Name = reader["Name"].ToString(),
                                    Price = Convert.ToDecimal(reader["price"])
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при извлечении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return categoryItems;
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton?.Tag is CategoryItem selectedItem)
            {
                string selectedName = selectedItem.Name;
                decimal selectedPrice = selectedItem.Price;

                UpdatePriceInSearch4(selectedPrice, selectedName);

                MessageBoxResult result = MessageBox.Show(
                "Перейти до кошика?",
                "Продовжити?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    end endWindow = new end();
                    endWindow.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Помилка отримання даних", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdatePriceInSearch4(decimal price, string selectedName)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string insertQuerySearch4 = "INSERT INTO search4 (price) VALUES (@price)";
                    using (SQLiteCommand insertCommandSearch4 = new SQLiteCommand(insertQuerySearch4, connection))
                    {
                        insertCommandSearch4.Parameters.AddWithValue("@price", price);
                        insertCommandSearch4.ExecuteNonQuery();
                    }
                    string insertQuerySearch3 = "INSERT INTO search3 (type) VALUES (@type)";
                    using (SQLiteCommand insertCommandSearch3 = new SQLiteCommand(insertQuerySearch3, connection))
                    {
                        insertCommandSearch3.Parameters.AddWithValue("@type", selectedName);
                        insertCommandSearch3.ExecuteNonQuery();
                    }
                    string insertNum = "INSERT INTO num (prod) VALUES (@type)";
                    using (SQLiteCommand insertCommandNum = new SQLiteCommand(insertNum, connection))
                    {
                        insertCommandNum.Parameters.AddWithValue("@type", selectedName);
                        insertCommandNum.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            categories categories = new();
            categories.Show();
            this.Hide();
        }
    }
    public class CategoryItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

