using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;


namespace MAG
{
    public partial class order : Window
    {
        public order()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(dbPath))
                {
                    connection.Open();
                    string query = "SELECT model, prod, number, price FROM num";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        int rowCount = 0;

                        while (reader.Read())
                        {
                            string model = reader["model"] as string ?? "";
                            string prod = reader["prod"] as string ?? "";
                            string number = reader["number"] as string ?? "";
                            string price = reader["price"] as string ?? "";

                            string buttonText = $"{model}, {prod}, {number}, {price} грн".Trim(new char[] { ',', ' ' });
                            Button orderButton = new Button
                            {
                                Content = buttonText,
                                Tag = number, 
                                Margin = new Thickness(5),
                                Width = 500,
                                Height = 30,
                                FontSize = 14
                            };
                            orderButton.Click += OrderButton_Click;
                            ButtonContainer.Children.Add(orderButton);
                            rowCount++;
                        }

                        if (rowCount == 0)
                        {
                            MessageBox.Show("Дані не знайдені:");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження даних: " + ex.Message);
            }
        }
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string numberToDelete = button.Tag.ToString();

                // Удаляем строку из базы данных
                DeleteOrder(numberToDelete);

                // Удаляем кнопку из интерфейса
                ButtonContainer.Children.Remove(button);

                // Выводим сообщение
                MessageBox.Show("Замовлення оброблено!");
            }
        }
        private void DeleteOrder(string number)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = System.IO.Path.Combine(basePath, "..", "..", "data", "main.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(dbPath))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM num WHERE number = @number";

                    using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@number", number);
                        int rowsAffected = command.ExecuteNonQuery();


                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Не вдалося видалити з БД");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при видаленні: " + ex.Message);
            }
        }
    }
}
