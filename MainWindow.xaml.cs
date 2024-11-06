using System.Data.SQLite;
using System.Windows;


namespace MAG
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            clearData();
        }
      
        private void clearData()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

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
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            list list = new();
            list.Show();
            this.Hide();
        }

        private void Admin_login(object sender, RoutedEventArgs e)
        {
            admin admin = new();
            admin.Show();
            this.Hide();
        }
    }
}