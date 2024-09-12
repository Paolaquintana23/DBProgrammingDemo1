using System.Data.SqlClient;
using System.Data;

namespace DBProgrammingDemo1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=(local);database=Northwind;Integrated Security=SSPI";

            // Call the ConnectionDemo1 method
            ConnectionDemo1(connectionString);

            // Call the ConnectionDemo1 method
            // Managing the unmanaged objects with the using statement
            ConnectionDemo2(connectionString);

        }

        private static void ConnectionDemo2(string connectionString)
        {
            string sql = "SELECT * FROM Products ORDER BY ProductName DESC";

            using (SqlConnection conn = new(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new(sql, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Console.WriteLine($"ProductId: {dr["ProductId"]} ProductName: {dr["productName"]} UnitPrice: {dr["UnitPrice"]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No records found");
                        }
                    } // close and dispose of data reader
                } // dispose of command
            } // close and dispose of connection
        }

        private static void ConnectionDemo1(string connectionString)
        {

            // Unmanaged Objects
            // Connection
            // Comand
            // DataReader

            // We need to manage the unmanaged objects

            //SqlConection Object
            SqlConnection conn = new(connectionString); //SlConnection conn = new SqlConnection(connectionString);

            //Sql Command
            SqlCommand cmd = new();

            //SqlReader
            SqlDataReader dr = null;

            try
            {
                // set the connection of the command
                cmd.Connection = conn;

                string sql = "SELECT * FROM Shippers ORDER BY CompanyName;";

                // set the command text to execute
                cmd.CommandText = sql;

                // zst the command type of my command type
                cmd.CommandType = CommandType.Text;

                //Open the connections
                conn.Open();

                //Create the DR
                dr = cmd.ExecuteReader();

                int shipperId;
                string? companyName;
                string? phone;

                // Check if the data reader has rows
                if (dr.HasRows)
                {
                    Console.WriteLine("We have records");
                    Console.WriteLine($"There are {dr.FieldCount} columns in our rows");
                    // Count the number of rows in our stream
                    int rowCount = 0;

                    // loop the data reader
                    // dr.Reader() returns true if there are still records and moves to the next row in the stream
                    while (dr.Read())
                    {
                        // track row count
                        rowCount++;
                        shipperId = Convert.ToInt32(dr["shipperId"]);
                        companyName = dr["companyName"].ToString();
                        phone = dr["phone"].ToString(); // column names are case insensitive

                        Console.WriteLine($"ShipperId; {shipperId} CompanyName: {companyName} Phone: {phone}");
                    }

                    Console.WriteLine($"We have {rowCount} shippers");
                }

                else
                {
                    Console.WriteLine("No rows found");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Clean up the unmanaged resources
                if (dr != null)
                {
                    dr.Close();// close the reader stream
                    dr.Dispose();// fress up memory
                }
                cmd.Dispose();// fress up memory
                conn.Close();// close the database connection
                conn.Dispose();// fress up memory
            }
        }
    }
}