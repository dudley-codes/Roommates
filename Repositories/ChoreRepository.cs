﻿using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    class ChoreRepository : BaseRepository
    {
        //<summary>
        // When ChoreRepository is instantiated, pass the connection string along to the BaseRepository
        // </summary>
        public ChoreRepository(string connectionString) : base(connectionString) { }

        // Create a new Chore
        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore(Name)
                                        OUTPUT INSERTED.Id
                                        VALUES @name";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                }
            }
        }
        // Get Chore by Id

        // Get a list of all Chores in the database
        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we set up the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    //A list to hold the chores we retreive from the database.
                    List<Chore> chores = new List<Chore>();

                    //Read() will return true if there's more data to read
                    while(reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                        int idColumnPosition = reader.GetOrdinal("Id");

                        //We use the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        // Now let's create a new chore object using the data from the database.
                        Chore chore = new Chore
                        {
                            Id = idValue,
                            Name = nameValue
                        };

                        //Add that chore object to our list.
                        chores.Add(chore);
                    }
                    // Make sure you close the reader
                    reader.Close();

                    // Return the list of chores to whomever called the method.
                    return chores;
                }
            }
        }

    }
}