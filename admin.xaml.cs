using System.Data.SQLite;
using System.Windows;

namespace MAG
{
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
            this.Top = 200;
            this.Left = 200;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            this.Hide();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string enteredCode = code.Password;

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";
            using SQLiteConnection connection = new(connectionString);
            try
            {
                connection.Open();
                string query = "SELECT Code FROM admin WHERE Code = @Code";
                using SQLiteCommand command = new(query, connection);

                command.Parameters.AddWithValue("@Code", enteredCode);

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    // Відображення кнопок
                    add.IsEnabled = true;
                    delete.IsEnabled = true;
                    login.IsEnabled = false;
                    order.IsEnabled = true;


                }
                else
                {
                    MessageBox.Show("Код невірний.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка підключення {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            Delete delete = new();
            delete.Show();
            this.Hide();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            Add addWindow = new();
            addWindow.Show();
            this.Close();
        }

        private void order_Click(object sender, RoutedEventArgs e)
        {
            Order order = new();
            order.Show();
            this.Hide();
        }
    }
}
