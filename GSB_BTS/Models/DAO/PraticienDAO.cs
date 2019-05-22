using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace GSB.Models.DAO
{
    public class PraticienDAO : DAO_Manager
    {
        public Praticien Read(int id)
        {
            Praticien praticien = new Praticien();
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                    "FROM personne INNER JOIN praticien " +
                    "ON praticien.id_praticien = personne.id_personne " +
                    "WHERE personne.id_personne = @id";
                command.Parameters.AddWithValue("@id", id);

                EtablissementDAO etablissementManager = new EtablissementDAO();
                // Lecture des résultats
                dataReader = command.ExecuteReader();
                
                while (dataReader.Read())
                {
                    praticien = new Praticien((string)dataReader["fonction"],
                                              (string)dataReader["specialite"],
                                              (DateTime)dataReader["date_derniere_entrevue"],
                                              (int)dataReader["id_praticien"],
                                              (string)dataReader["nom"],
                                              (string)dataReader["prenom"],
                                              (string)dataReader["email"],
                                              (string)dataReader["telephone"],
                                               etablissementManager.Read((int)dataReader["id_etablissement"]));
                    Debug.WriteLine(praticien);
                }
                dataReader.Close();
                CloseConnection();
            }
            return praticien;
        }

        public List<Praticien> ReadAll()
        {
            List<Praticien> mesPraticiens = new List<Praticien>();
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                    "FROM personne INNER JOIN praticien " +
                    "ON praticien.id_praticien = personne.id_personne ";

                EtablissementDAO etablissementManager = new EtablissementDAO();
                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    mesPraticiens.Add(new Praticien((string)dataReader["fonction"],
                                (string)dataReader["specialite"],
                                (DateTime)dataReader["date_derniere_entrevue"],
                                (int)dataReader["id_praticien"],
                                (string)dataReader["nom"],
                                (string)dataReader["prenom"],
                                (string)dataReader["email"],
                                (string)dataReader["telephone"],
                                etablissementManager.Read((int)dataReader["id_etablissement"])));
                }
                dataReader.Close();
                CloseConnection();
            }

            return mesPraticiens;
        }

        public List<Praticien> ReadAllPraticiensInEtablissement(int id_etablissement)
        {
            List<Praticien> mesPraticiens = new List<Praticien>();
            if (OpenConnection())
            {
                Praticien praticien;
                EtablissementDAO etablissementManager = new EtablissementDAO();
                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                    "FROM personne INNER JOIN praticien " +
                    "ON praticien.id_praticien = personne.id_personne " +
                    "WHERE id_etablissement = @id_etablissement";

                command.Parameters.AddWithValue("@id_etablissement", id_etablissement);

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    praticien = new Praticien();
                    praticien.Fonction = (string)dataReader["fonction"];
                    praticien.Specialite = (string)dataReader["specialite"];
                    praticien.Date_derniere_entrevue = (DateTime)dataReader["date_derniere_entrevue"];
                    praticien.Id = (int)dataReader["id_praticien"];
                    praticien.Nom = (string)dataReader["nom"];
                    praticien.Prenom = (string)dataReader["prenom"];
                    praticien.Email = (string)dataReader["email"];
                    praticien.Telephone = (string)dataReader["telephone"];
                    mesPraticiens.Add(praticien); 
                }
                dataReader.Close();
                CloseConnection();
            }

            return mesPraticiens;
        }

        public void Create(Praticien praticien)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();

                command.CommandText = "INSERT INTO personne " +
                      "(nom, prenom, email, telephone, id_etablissement) " +
                      "VALUES (@nom, @prenom, @email, @telephone, @id_etablissement)";
                command.Parameters.AddWithValue("@nom", praticien.Nom);
                command.Parameters.AddWithValue("@prenom", praticien.Prenom);
                command.Parameters.AddWithValue("@email", praticien.Email);
                command.Parameters.AddWithValue("@telephone", praticien.Telephone);
                command.Parameters.AddWithValue("@id_etablissement", praticien.Etablissement.Id);

                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO praticien " +
                                      "(id_praticien, fonction, specialite, date_derniere_entrevue) " +
                                      "VALUES (@id_praticien, @fonction, @specialite, @date_derniere_entrevue)";
                command.Parameters.AddWithValue("@id_praticien", command.LastInsertedId);
                command.Parameters.AddWithValue("@fonction", praticien.Fonction);
                command.Parameters.AddWithValue("@specialite", praticien.Specialite);
                command.Parameters.AddWithValue("@date_derniere_entrevue", praticien.Date_derniere_entrevue);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public void Update(Praticien praticien)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "UPDATE personne " +
                                      "SET fonction=@fonction, specialite=@specialite, date_dernier_entrevue=@dernier_entrevue " +
                                      "WHERE praticien.id_praticien=@id";
                command.Parameters.AddWithValue("@id", praticien.Id);
                command.Parameters.AddWithValue("@fonction", praticien.Fonction);
                command.Parameters.AddWithValue("@specialite", praticien.Specialite);
                command.Parameters.AddWithValue("@derniere_entrevue", praticien.Date_derniere_entrevue);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public void Delete(Praticien praticien)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "DELETE FROM praticien " +
                                      "WHERE id_praticien= @id" +
                                      "DELETE FROM personne " +
                                      "WHERE personne.id_personne= @id";
                command.Parameters.AddWithValue("@id", praticien.Id);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }
    }
}