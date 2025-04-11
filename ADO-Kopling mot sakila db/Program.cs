using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace ADO_Kopling_mot_sakila_db
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Sakila;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;";
            while (true)
            {
                Console.Write("Ange skådespelarnamn (eller skriv 'avsluta' för att avsluta): ");
                string actorName = Console.ReadLine();

                if (actorName.ToLower() == "avsluta")
                {
                    Console.WriteLine("Programmet avslutas. Tack!");
                    break;
                }

                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();

                string sql = @"SELECT title from film
            inner join  film_actor  on  film_actor.film_id= film.film_id
            inner join actor on actor.actor_id= film_actor.actor_id
            where actor.first_name = @ActorName";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ActorName", actorName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("\nFilmer med " + actorName + ":");

                        if (!reader.HasRows)
                        {
                            Console.WriteLine("Inga filmer hittades för denna skådespelare.");
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("- " + reader["Title"]);
                            }
                        }
                    }
                }
            }
        }
    }        
 }             
