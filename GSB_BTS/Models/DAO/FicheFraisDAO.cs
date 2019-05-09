using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GSB.Models.DAO
{
    public class FicheFraisDAO : DAO_Manager
    {
        public FicheFrais Read(int id, bool isSerializeRead)
        {
            FicheFrais ficheFrais = null;
            if (OpenConnection())
            {
                ficheFrais = new FicheFrais();
                LigneFraisDAO ligneFraisManager = new LigneFraisDAO();
                EmployeDAO employeManager = new EmployeDAO();
                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                      "FROM fiche_frais " +
                                      "WHERE id_fiche_frais = @id_fiche_frais";
                command.Parameters.AddWithValue("@id_fiche_frais", id);

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    ficheFrais.Id_fiche_frais = (int)dataReader["id_fiche_frais"];
                    ficheFrais.Comptable = dataReader["id_comptable"].ToString() == "" ? null : employeManager.Read((int)dataReader["id_comptable"]);
                    ficheFrais.Commercial_visiteur = employeManager.Read((int)dataReader["id_commercial_visiteur"]);
                    ficheFrais.Date_fiche = (DateTime)dataReader["date_fiche"];
                    ficheFrais.Date_modification = dataReader["date_modification"].ToString() == "" ? null : (DateTime?)dataReader["date_modification"];
                    ficheFrais.Liste_lignes_frais = ligneFraisManager.ReadAllFromFicheFrais(ficheFrais, isSerializeRead);
                    Debug.WriteLine(ficheFrais);
                }

                dataReader.Close();
                CloseConnection();
            }
            return ficheFrais;
        }


        public void Create(FicheFrais ficheFrais)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "INSERT INTO fiche_frais " +
                                      "(id_comptable, id_commercial_visiteur, date_fiche, date_modification) " +
                                      "VALUES (@id_comptable, @id_commercial_visiteur,  @date_fiche, @date_modification)";

                command.Parameters.AddWithValue("@id_comptable", ficheFrais.Comptable);
                command.Parameters.AddWithValue("@id_commercial_visiteur", ficheFrais.Commercial_visiteur);
                command.Parameters.AddWithValue("@date_fiche", ficheFrais.Date_fiche);
                command.Parameters.AddWithValue("@date_modification", ficheFrais.Date_modification);

                command.ExecuteNonQuery();
                // Add each ligne frais

                CloseConnection();
            }
        }

        public List<FicheFrais> ReadAll()
        {
            List<FicheFrais> liste_ficheFrais = new List<FicheFrais>();
            if (OpenConnection())
            {
                FicheFrais ficheFrais;
                EmployeDAO employeManager = new EmployeDAO();
                LigneFraisDAO ligneFraisManager = new LigneFraisDAO();
                command = manager.CreateCommand();
                command.CommandText = "SELECT * FROM fiche_frais";

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    ficheFrais = new FicheFrais();
                    ficheFrais.Id_fiche_frais = (int)dataReader["id_fiche_frais"];
                    ficheFrais.Commercial_visiteur = employeManager.Read((int)dataReader["id_commercial_visiteur"]);
                    ficheFrais.Comptable = dataReader["id_comptable"].ToString() == "" ? null : employeManager.Read((int)dataReader["id_comptable"]);
                    ficheFrais.Date_fiche = (DateTime)dataReader["date_fiche"];
                    ficheFrais.Date_modification = dataReader["date_modification"].ToString() == "" ? null : (DateTime?)dataReader["date_modification"];
                    ficheFrais.Liste_lignes_frais = ligneFraisManager.ReadAllFromFicheFrais(ficheFrais, false);

                    liste_ficheFrais.Add(ficheFrais);
                }

                dataReader.Close();
                CloseConnection();
            }
            return liste_ficheFrais;
        }

        public List<FicheFrais> ReadAllFromEmploye(Employe employe)
        {
            List<FicheFrais> liste_ficheFrais = new List<FicheFrais>();
            if (OpenConnection())
            {
                FicheFrais ficheFrais;
                EmployeDAO employeManager = new EmployeDAO();
                LigneFraisDAO ligneFraisManager = new LigneFraisDAO();

                string SQL_Request = "SELECT * FROM fiche_frais ";

                if (employe.Type == Employe.TypeEmploye.comptable)
                {
                    SQL_Request += "WHERE id_comptable = @id_employe";
                }
                else
                {
                    SQL_Request += "WHERE id_commercial_visiteur = @id_employe";
                }

                command = manager.CreateCommand();
                command.CommandText = SQL_Request;

                command.Parameters.AddWithValue("@id_employe", employe.Id);

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                if (employe.Type == Employe.TypeEmploye.comptable)
                {
                    while (dataReader.Read())
                    {
                        ficheFrais = new FicheFrais();
                        ficheFrais.Id_fiche_frais = (int)dataReader["id_fiche_frais"];
                        ficheFrais.Commercial_visiteur = employeManager.Read((int)dataReader["id_commercial_visiteur"]);
                        ficheFrais.Comptable = employe;
                        ficheFrais.Date_fiche = (DateTime)dataReader["date_fiche"];
                        ficheFrais.Date_modification = dataReader["date_modification"].ToString() == "" ? null : (DateTime?)dataReader["date_modification"];
                        ficheFrais.Liste_lignes_frais = ligneFraisManager.ReadAllFromFicheFrais(ficheFrais, false);

                        liste_ficheFrais.Add(ficheFrais);
                    }
                }
                else
                {
                    while (dataReader.Read())
                    {
                        ficheFrais = new FicheFrais();
                        ficheFrais.Id_fiche_frais = (int)dataReader["id_fiche_frais"];
                        ficheFrais.Commercial_visiteur = employe;
                        ficheFrais.Comptable = dataReader["id_comptable"].ToString() == "" ? null : employeManager.Read((int)dataReader["id_comptable"]);
                        ficheFrais.Date_fiche = (DateTime)dataReader["date_fiche"];
                        ficheFrais.Date_modification = dataReader["date_modification"].ToString() == "" ? null : (DateTime?)dataReader["date_modification"];
                        ficheFrais.Liste_lignes_frais = ligneFraisManager.ReadAllFromFicheFrais(ficheFrais, false);

                        liste_ficheFrais.Add(ficheFrais);
                    }
                }

                dataReader.Close();
                CloseConnection();
            }
            return liste_ficheFrais;
        }

        public void Update(FicheFrais ficheFrais)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "UPDATE fiche_frais " +
                                      "SET id_comptable=@id_comptable, id_commercial_visiteur=@id_commercial_visiteur, date_fiche=@date_fiche, date_modification=@date_modification " +
                                      "WHERE fiche_frais.id_fiche_frais = @id_fiche_frais";

                command.Parameters.AddWithValue("@id_fiche_frais", ficheFrais.Id_fiche_frais);
                command.Parameters.AddWithValue("@id_comptable", ficheFrais.Comptable);
                command.Parameters.AddWithValue("@id_commercial_visiteur", ficheFrais.Commercial_visiteur);
                command.Parameters.AddWithValue("@date_fiche", ficheFrais.Date_fiche);
                command.Parameters.AddWithValue("@date_modification", ficheFrais.Date_modification);
                
                // Update ligne frais ?

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public void Delete(FicheFrais ficheFrais)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "DELETE FROM fiche_frais " +
                                      "WHERE id_fiche_frais = @id_fiche_frais";
                command.Parameters.AddWithValue("@id_fiche_frais", ficheFrais.Id_fiche_frais);

                // Delete ligne frais
                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

    }
}
