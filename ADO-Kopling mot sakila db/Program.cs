using Microsoft.Data.SqlClient;

namespace ADO_Kopling_mot_sakila_db
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Sakila;Integrated Security=True;";

            while (true)
            {
                Console.Write("Ange skådespelarens förnamn (eller skriv 'avsluta' för att avsluta): ");
                string firstName = Console.ReadLine();

                if (firstName.ToLower() == "avsluta")
                {
                    Console.WriteLine("Programmet avslutas. Tack!");
                    break;
                }

                Console.Write("Ange skådespelarens efternamn: ");
                string lastName = Console.ReadLine();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = @"
                        SELECT title 
                        FROM film
                        INNER JOIN film_actor ON film.film_id = film_actor.film_id
                        INNER JOIN actor ON actor.actor_id = film_actor.actor_id
                        WHERE actor.first_name = @FirstName AND actor.last_name = @LastName";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine($"\nFilmer med {firstName} {lastName}:");

                            if (!reader.HasRows)
                            {
                                Console.WriteLine("Inga filmer hittades för denna skådespelare.");
                            }
                            else
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine("- " + reader["title"]);
                                }
                            }
                        }
                    }

                    connection.Close();
                }
            }
        }
    }
}

