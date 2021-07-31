using System;
using Microsoft.Data.SqlClient;

public class ExemploADONET
{
    string connectionString;
    public ExemploADONET(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void ExecuteADONET()
    {
        #region Abrindo conexão com o banco
        using (var connection = new SqlConnection(connectionString))
        {
            Console.WriteLine("Conectado...");
            connection.Open();

            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "Select Id, Title from Category";

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"{reader.GetGuid(0)} - {reader.GetString(1)}, ");
                }

            }

        }
        #endregion

        Console.WriteLine("Fim da Execulção!");
    }
}