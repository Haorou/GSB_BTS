using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GSB.Models.DAO
{
    public class EtablissementDAO : DAO_Manager
    {
        public Etablissement Read(int id_etablissement)
        {
            Etablissement etablissement = null;
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                      "FROM etablissement " +
                                      "WHERE id_etablissement = @id_etablissement";
                command.Parameters.AddWithValue("@id_etablissement", id_etablissement);

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    etablissement = new Etablissement((int)dataReader["id_etablissement"],
                                                      (string)dataReader["nom"],
                                                      (string)dataReader["adresse"]);
                }

                dataReader.Close();
                CloseConnection();
            }
            return etablissement;
        }

        public List<Etablissement> ReadAll()
        {
            List<Etablissement> liste_etablissements = null;

            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                      "FROM etablissement";

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                liste_etablissements = new List<Etablissement>();
                while (dataReader.Read())
                {
                    liste_etablissements.Add(new Etablissement((int)dataReader["id_etablissement"],
                                    (string)dataReader["nom"],
                                    (string)dataReader["adresse"]));
                }

                dataReader.Close();
                CloseConnection();
            }
            return liste_etablissements;
        }

        public void Create(Etablissement etablissement)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "INSERT INTO Etablissement " +
                                      "(id_etablissement , nom, adresse) " +
                                      "VALUES (@id_etablissement, @nom, @adresse)";
                command.Parameters.AddWithValue("@id_etablissement", etablissement.Id);
                command.Parameters.AddWithValue("@nom", etablissement.Nom);
                command.Parameters.AddWithValue("@adresse", etablissement.Adresse);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }
        

        public void Delete(Etablissement etablissement)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "DELETE FROM Etablissement " +
                                      "WHERE id_etablissement= @id_etablissement";
                command.Parameters.AddWithValue("@id_etablissement", etablissement.Id);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }
        
    }
}
