using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace MAG
{
    public partial class delete : Window
    {

        public delete()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            ButtonContainer.Children.Clear();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT name, price, categories, moto FROM categories";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader["name"].ToString();
                        decimal price = reader.GetDecimal(reader.GetOrdinal("price"));
                        string category = reader["categories"].ToString();
                        string moto = reader["moto"].ToString();

                        string buttonText = $"{name} - {price:C} - {category} - {moto}";

                        Button deleteButton = new Button
                        {
                            Content = buttonText,
                            Tag = new Tuple<string, decimal, string, string>(name, price, category, moto),
                            Margin = new Thickness(5)
                        };

                        deleteButton.Click += DeleteButton_Click;
                        ButtonContainer.Children.Add(deleteButton);
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var recordData = (Tuple<string, decimal, string, string>)button.Tag;

            string name = recordData.Item1;
            decimal price = recordData.Item2;
            string category = recordData.Item3;
            string moto = recordData.Item4;
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string deleteQuery = "DELETE FROM categories WHERE name = @name AND price = @price AND categories = @category AND moto = @moto";

                using (SQLiteCommand cmd = new SQLiteCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@moto", moto);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadData();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            admin admin = new();
            admin.Show();
            this.Hide();
        }
    }
}