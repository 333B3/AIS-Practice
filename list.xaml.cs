using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MAG
{
public partial class List : Window
    {



        private string selectedCategory;
        public List()
        {
            InitializeComponent();
            CreateButtons();
            this.Top = 200;
            this.Left = 200;
        }
        private void Button_click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            this.Hide();
        }
        private List<string> GetCategoriesFromDatabase()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            dbPath = dbPath.Replace("\\", "\\\\");
            string connectionString = $"Data Source={dbPath};Version=3;";

            List<string> categories = new List<string>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Moto FROM categories";
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            HashSet<string> uniqueCategories = new HashSet<string>();

                            while (reader.Read())
                            {
                                uniqueCategories.Add(reader["Moto"].ToString());
                            }

                            categories = uniqueCategories.ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка виводу даних: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return categories;
        }
        private void CreateButtons()
        {
            List<string> categories = GetCategoriesFromDatabase();

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
            Categories categories = new Categories();
            categories.Show();
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
                    string deleteQuery = "DELETE FROM search";
                    using (SQLiteCommand deleteCommand = new SQLiteCommand(deleteQuery, connection))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }
                    string insertQuery = "INSERT INTO search (type) VALUES (@type)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@type", newCategory);
                        insertCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка оновлення: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
