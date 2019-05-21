
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
                EtablissementDAO etablissementManager = new EtablissementDAO();
                command = manager.CreateCommand();
                command.CommandText = "SELECT * FROM personne";             


                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    liste_personnes.Add(new Personne((int)dataReader["id_personne"],
                                (string)dataReader["nom"],
                                (string)dataReader["prenom"],
                                (string)dataReader["email"],
                                (string)dataReader["telephone"],
                                etablissementManager.Read((int)dataReader["id_etablissement"])));
                }
                dataReader.Close();
                CloseConnection();
            }

            return liste_personnes;
        }

        public void Update(Personne personne)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "UPDATE personne " +
                                      "SET nom=@nom, prenom=@prenom, adresse=@adresse, email=@email, id_etablissement=@id_etablissement " +
                                      "WHERE personne.id_personne=@id";
                command.Parameters.AddWithValue("@id", personne.Id);
                command.Parameters.AddWithValue("@nom", personne.Nom);
                command.Parameters.AddWithValue("@prenom", personne.Prenom);
                command.Parameters.AddWithValue("@email", personne.Email);
                command.Parameters.AddWithValue("@id_etablissement", personne.Etablissement.Id);

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