using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we set up the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, FirstName FROM Roommate";
                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Create an empty list to hold all roommates
                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        int idValue = reader.GetInt32(reader.GetOrdinal("Id"));
                        string firstName = reader.GetString(reader.GetOrdinal("FirstName"));

                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            FirstName = firstName
                        };

                        roommates.Add(roommate);
                    }
                    reader.Close();
                    return roommates;
                }
            }
        }
        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT r.FirstName, r.RentPortion, rm.Name as RoomName
                                        FROM Roommate r
                                        LEFT JOIN Room rm ON r.RoomId = rm.Id
                                        WHERE r.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            Room = new Room()
                            {
                                Name = reader.GetString(reader.GetOrdinal("RoomName"))
                            }

                        };
                    }

                    reader.Close();

                    return roommate;
                }
            }
        }
    }
}
