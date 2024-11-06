using System.Data.SQLite;
using System.Windows;

namespace MAG
{
    public partial class Buy : Window
    {
        public Buy()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            MessageBox.Show("Чекайте! Вам скоро зателефонують");
            // Отримання тексту з textbox для подальшого запису в БД
            string categoryText = Categories.Text;
            if (!string.IsNullOrEmpty(categoryText))
            {
                // Підключення до БД


                string query = "INSERT INTO num (number) VALUES (@number)";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@number", categoryText);
                        command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                MessageBox.Show("Введіть дані в поле!");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }
    }
}
