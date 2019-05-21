using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GSB.Models.DAO
{
    public class EmployeDAO : DAO_Manager
    {
        public Employe Read(int id_employe)
        {
            Employe employe = null;
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                      "FROM employe e JOIN personne p " +
                                      "ON p.id_personne = e.id_employe " +
                                      "WHERE p.id_personne = @id";
                command.Parameters.AddWithValue("@id", id_employe);

                // Lecture des résultats
                dataReader = command.ExecuteReader();
                EtablissementDAO etablissementManager = new EtablissementDAO();
                while (dataReader.Read())
                {
                    employe = new Employe((int)dataReader["id_personne"],
                                            (string)dataReader["nom"],
                                            (string)dataReader["prenom"],
                                            (string)dataReader["email"],
                                            (string)dataReader["telephone"],
                                            etablissementManager.Read((int)dataReader["id_etablissement"]),
                                            (string)dataReader["login"],
                                            (string)dataReader["mot_de_passe"],
                                            (Employe.TypeEmploye)Enum.Parse(typeof(Employe.TypeEmploye), 
                                                                           (string)dataReader["type_employe"]));
                }

                dataReader.Close();
                CloseConnection();
            }
            return employe;
        }
        public List<Employe> ReadAll()
        {
            List<Employe> liste_employes = null;

            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                      "FROM employe p JOIN personne e " +
                                      "ON p.id_personne = e.id_employe";
                // Lecture des résultats
                dataReader = command.ExecuteReader();
                EtablissementDAO etablissementManager = new EtablissementDAO();
                liste_employes = new List<Employe>();
                while (dataReader.Read())
                {
                    liste_employes.Add(new Employe((int)dataReader["id_personne"],
                                    (string)dataReader["nom"],
                                    (string)dataReader["prenom"],
                                    (string)dataReader["email"],
                                    (string)dataReader["telephone"],
                                    etablissementManager.Read((int)dataReader["id_etablissement"]),
                                    (string)dataReader["login"],
                                    (string)dataReader["mot_de_passe"],
                                    (Employe.TypeEmploye)Enum.Parse(typeof(Employe.TypeEmploye),
                                                                    (string)dataReader["type_employe"])));

                    Debug.WriteLine(liste_employes);
                }

                dataReader.Close();
                CloseConnection();
            }
            return liste_employes;
        }

            public void Create(Employe employe)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "INSERT INTO Employe " +
                                      "(id_employe,login, mot_de_passe, type_employe) " +
                                      "VALUES (@id_employe, @login, @mot_de_passe,@type_employe)";
                command.Parameters.AddWithValue("@id_employe", employe.Id);
                command.Parameters.AddWithValue("@login", employe.Login);
                command.Parameters.AddWithValue("@mot_de_passe", employe.Mot_de_passe);
                command.Parameters.AddWithValue("@type_employe", employe.Type);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public void Update(Employe employe)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "UPDATE personne " +
                                      "SET mot_de_passe=@mot_de_passe" +
                                      "WHERE id_employe=@id AND login=@login";

                command.Parameters.AddWithValue("@id", employe.Id);
                command.Parameters.AddWithValue("@login", employe.Login);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public void Delete(Employe employe)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "DELETE FROM employe " +
                                      "WHERE id_employe= @id" +
                                      "DELETE FROM personne " +
                                      "WHERE personne.id_personne= @id";
                command.Parameters.AddWithValue("@id", employe.Id);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public Employe Connexion(string login, string password)
        {
            Employe employe = null;
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                      "FROM employe e JOIN personne p " +
                                      "ON p.id_personne = e.id_employe " +
                                      "WHERE e.login= @login " +
                                      "AND e.mot_de_passe= @mot_de_passe";
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@mot_de_passe", password);

                EtablissementDAO etablissementManager = new EtablissementDAO();
                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    employe = new Employe((int)dataReader["id_personne"],
                                            (string)dataReader["nom"],
                                            (string)dataReader["prenom"],
                                            (string)dataReader["email"],
                                            (string)dataReader["telephone"],
                                            etablissementManager.Read((int)dataReader["id_etablissement"]),
                                            (string)dataReader["login"],
                                            (string)dataReader["mot_de_passe"],
                                            (Employe.TypeEmploye)Enum.Parse(typeof(Employe.TypeEmploye),
                                                                           (string)dataReader["type_employe"]));
                }

                dataReader.Close();
                CloseConnection();
            }
            return employe;
        }



    }
}
