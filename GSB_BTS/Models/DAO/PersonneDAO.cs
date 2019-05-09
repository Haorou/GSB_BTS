
using System.Collections.Generic;

namespace GSB.Models.DAO
{
    public class PersonneDAO : DAO_Manager
    {
        public List<Personne> ReadAll()
        {
            List<Personne> liste_personnes = new List<Personne>();
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "SELECT * FROM personne";             


                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    liste_personnes.Add(new Personne((int)dataReader["id_personne"],
                                (string)dataReader["nom"],
                                (string)dataReader["prenom"],
                                (string)dataReader["adresse"],
                                (string)dataReader["email"],
                                (string)dataReader["telephone"],
                                (string)dataReader["etablissement"]));
                }
                dataReader.Close();
                CloseConnection();
            }

            return liste_personnes;
        }

        /*public void Create(string nom, string prenom, string adresse, string email, string etablissement)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "INSERT INTO personne "+
                                      "(nom, prenom, adresse, email, etablissement) "+
                                      "VALUES (@nom, @prenom, @adresse, @email, @etablissement)";
                command.Parameters.AddWithValue("@nom", nom);
                command.Parameters.AddWithValue("@prenom", prenom);
                command.Parameters.AddWithValue("@adresse", adresse);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@etablissement", etablissement);

                CloseConnection();
            }
        }*/

        public void Update(Personne personne)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "UPDATE personne " +
                                      "SET nom=@nom, prenom=@prenom, adresse=@adresse, email=@email, etablissement=@etablissement " +
                                      "WHERE personne.id_personne=@id";
                command.Parameters.AddWithValue("@id", personne.Id);
                command.Parameters.AddWithValue("@nom", personne.Nom);
                command.Parameters.AddWithValue("@prenom", personne.Prenom);
                command.Parameters.AddWithValue("@adresse", personne.Adresse);
                command.Parameters.AddWithValue("@email", personne.Email);
                command.Parameters.AddWithValue("@etablissement", personne.Etablissement);

                CloseConnection();
            }
        }

        public void Delete(Personne personne)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "DELETE FROM personne " +
                                      "WHERE id_personne=@id";
                command.Parameters.AddWithValue("@id", personne.Id);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }
    }
}