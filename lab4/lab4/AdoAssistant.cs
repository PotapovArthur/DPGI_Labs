using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

// Клас доступу до БД
public class AdoAssistant
{
    // Отримуємо рядок з'єднання з файлу App.config
    String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[" connectionStringName "].ConnectionString;
    //************************************************ *********
    // Метод читання даних з DataTable
    //************************************************ *********
    DataTable dt = null; // Посилання на об'єкт DataTable
    public DataTable TableLoad()
    {
        if (dt != null) return dt; // Завантажимо таблицю лише один раз
                                   // Заповнюємо об'єкт таблиці даними з БД
        dt = new DataTable();
        // Створюємо об'єкт підключення
        using (SqlConnection сonnection = new SqlConnection(connectionString))
        {
            SqlCommand command = сonnection.CreateCommand(); // Створюємо об'єкт команди
            SqlDataAdapter adapter = new SqlDataAdapter(command); // Створюємо об'єкт читання
                                                                  //Завантажує дані 
            command.CommandText = "SELECT [Record Book Number] AS ID, " +
                                  "[Full Name] AS FullName, " +
                                  "[Group], " +
                                  "[Address] " +
                                  "FROM Students";
            try
            {

                // Метод сам відкриває БД і сам її закриває
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка підключення до БД: " + ex.Message);
            }
        }
        return dt;
    }
    public bool AddRecord(string recordBookNumber, string fullName, string group, string address)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = connection.CreateCommand();

            command.CommandText = "INSERT INTO Students ([Record Book Number], [Full Name], [Group], [Address]) " +
                                  "VALUES (@RecordBookNumber, @FullName, @Group, @Address)";

            command.Parameters.AddWithValue("@RecordBookNumber", recordBookNumber);
            command.Parameters.AddWithValue("@FullName", fullName);
            command.Parameters.AddWithValue("@Group", group);
            command.Parameters.AddWithValue("@Address", address);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при додаванні запису: " + ex.Message);
                return false;
            }
        }
    }

    public bool UpdateRecord(string recordBookNumber, string fullName, string group, string address)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = connection.CreateCommand();

            command.CommandText = "UPDATE Students " +
                                  "SET [Full Name] = @FullName, [Group] = @Group, [Address] = @Address " +
                                  "WHERE [Record Book Number] = @RecordBookNumber";

            command.Parameters.AddWithValue("@FullName", fullName);
            command.Parameters.AddWithValue("@Group", group);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@RecordBookNumber", recordBookNumber);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при оновленні запису: " + ex.Message);
                return false;
            }
        }
    }

    public bool DeleteRecord(string recordBookNumber)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = connection.CreateCommand();

            command.CommandText = "DELETE FROM Students WHERE [Record Book Number] = @RecordBookNumber";
            command.Parameters.AddWithValue("@RecordBookNumber", recordBookNumber);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при видаленні запису: " + ex.Message);
                return false;
            }
        }
    }
}
