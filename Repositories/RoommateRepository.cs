using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

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
