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
                                              (string)dataReader["adresse"],
                                              (string)dataReader["email"],
                                              (string)dataReader["telephone"],
                                              (string)dataReader["etablissement"]);
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
                                (string)dataReader["adresse"],
                                (string)dataReader["email"],
                                (string)dataReader["telephone"],
                                (string)dataReader["etablissement"]));
                }
                dataReader.Close();
                CloseConnection();
            }

            return mesPraticiens;
        }

        public List<Praticien> ReadEtablissements()
        {
            List<Praticien> liste_etablissement = new List<Praticien>();
            Praticien praticien;
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "SELECT DISTINCT etablissement, id_personne, adresse FROM personne p JOIN praticien prat ON prat.id_praticien = p.id_personne ";

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    praticien = new Praticien();
                    praticien.Id = (int)dataReader["id_personne"];
                    praticien.Etablissement = (string)dataReader["etablissement"];
                    praticien.Adresse = (string)dataReader["adresse"];
                    liste_etablissement.Add(praticien);
                }
                dataReader.Close();
                CloseConnection();
            }

            return liste_etablissement;
        }

        public List<Praticien> ReadAllPraticiensInEtablissement(string etablissement)
        {
            List<Praticien> mesPraticiens = new List<Praticien>();
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                    "FROM personne INNER JOIN praticien " +
                    "ON praticien.id_praticien = personne.id_personne " +
                    "WHERE etablissement = @etablissement";

                command.Parameters.AddWithValue("@etablissement", etablissement);

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
                                (string)dataReader["adresse"],
                                (string)dataReader["email"],
                                (string)dataReader["telephone"],
                                (string)dataReader["etablissement"]));
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
                command.CommandText = "INSERT INTO praticien " +
                                      "(fonction, specialite, date_derniere_entrevue) " +
                                      "VALUES (@fonction, @specialite, @date_derniere_entrevue)";
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