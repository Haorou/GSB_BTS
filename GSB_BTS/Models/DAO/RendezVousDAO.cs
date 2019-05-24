using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GSB.Models.DAO
{
    public class RendezVousDAO : DAO_Manager
    {
        public RendezVous Read(int id_rdv, bool isReadFromEchantillonDonne)
        {
            RendezVous rendezVous = null;
            if (OpenConnection())
            {
                rendezVous = new RendezVous();
                EmployeDAO employeManager = new EmployeDAO();
                PraticienDAO praticienManager = new PraticienDAO();
                EchantillonDonneDAO echantillonDonneManager = new EchantillonDonneDAO();

                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                      "FROM rendez_vous " +
                                      "WHERE id_rdv = @id "+
                                      "ORDER BY date_rdv ASC";
                command.Parameters.AddWithValue("@id", id_rdv);

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    rendezVous.Id_rdv = (int)dataReader["id_rdv"];
                    rendezVous.Date_bilan = (DateTime)dataReader["date_bilan"];
                    rendezVous.Date_rdv = (DateTime)dataReader["date_rdv"];
                    rendezVous.Employe = employeManager.Read((int)dataReader["id_commercial"]);
                    rendezVous.Praticien = praticienManager.Read((int)dataReader["id_praticien"]);
                    rendezVous.Indice_confiance = (int)dataReader["indice_confiance"];
                    // Utilisation d'un Enum.Parse pour transformer un string en Enum
                    // Pour ce faire : (Type Enum)Enum.Parse(typeof(Type Enum), (string)variable);
                    rendezVous.Motif_rdv = (RendezVous.Rdv)Enum.Parse(typeof(RendezVous.Rdv), (string)dataReader["motif_rdv"]);

                    if(!isReadFromEchantillonDonne)
                    {
                        rendezVous.Liste_echantillons_donnes = echantillonDonneManager.ReadAllFromRendezVous(rendezVous.Id_rdv);
                    }                    
                }

                dataReader.Close();
                CloseConnection();
            }
            return rendezVous;
        }
        public void Create(RendezVous rendezVous)
        {
            if (OpenConnection())
            {
                EchantillonDonneDAO echantillonDonneDAO = new EchantillonDonneDAO();

                command = manager.CreateCommand();
                command.CommandText = "INSERT INTO rendez_vous " +
                                      "(id_commercial, id_praticien, date_rdv, motif_rdv, indice_confiance, date_bilan) " +
                                      "VALUES (@id_commercial, @id_praticien, @date_rdv, @motif_rdv, @indice_confiance,@date_bilan)";
                command.Parameters.AddWithValue("@id_commercial", rendezVous.Employe.Id);
                command.Parameters.AddWithValue("@id_praticien", rendezVous.Praticien.Id);
                command.Parameters.AddWithValue("@date_rdv", rendezVous.Date_rdv);
                command.Parameters.AddWithValue("@motif_rdv", rendezVous.Motif_rdv.ToString());
                command.Parameters.AddWithValue("@indice_confiance", rendezVous.Indice_confiance);
                command.Parameters.AddWithValue("@date_bilan", rendezVous.Date_bilan);

                if(rendezVous.Liste_echantillons_donnes != null)
                {
                    foreach (EchantillonDonne echantillonDonne in rendezVous.Liste_echantillons_donnes)
                    {
                        echantillonDonneDAO.Create(echantillonDonne);
                    }
                }

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public void Update(RendezVous rendezVous)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "UPDATE rendez_vous " +
                                      "SET date_rdv = @date_rdv, " +
                                      "id_praticien = @id_praticien, " +
                                      "indice_confiance = @indice_confiance, " +
                                      "date_bilan = @date_bilan, " +
                                      "motif_rdv = @motif_rdv " +
                                      "WHERE id_rdv = @id_rdv";

                command.Parameters.AddWithValue("@date_rdv", rendezVous.Date_rdv);
                command.Parameters.AddWithValue("@id_praticien", rendezVous.Praticien.Id);
                command.Parameters.AddWithValue("@indice_confiance", rendezVous.Indice_confiance);
                command.Parameters.AddWithValue("@date_bilan", rendezVous.Date_bilan);
                command.Parameters.AddWithValue("@motif_rdv", rendezVous.Motif_rdv.ToString());
                command.Parameters.AddWithValue("@id_rdv", rendezVous.Id_rdv);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public void Delete(RendezVous rendezVous)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "DELETE FROM rendez_vous " +
                                      "WHERE id_rdv= @id";
                                      
                command.Parameters.AddWithValue("@id", rendezVous.Id_rdv);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public List<RendezVous> ReadAll()
        {
            List<RendezVous> liste_rdv = new List<RendezVous>();
            if (OpenConnection())
            {
                RendezVous rdv;
                EmployeDAO employeManager = new EmployeDAO();
                PraticienDAO praticienManager = new PraticienDAO();
                EchantillonDonneDAO echantillonDonneManager = new EchantillonDonneDAO();

                command = manager.CreateCommand();
                command.CommandText = "SELECT * FROM rendez_vous";

                dataReader = command.ExecuteReader();

                
                while (dataReader.Read())
                {
                    rdv = new RendezVous();
                    rdv.Id_rdv = (int)dataReader["id_rdv"];
                    rdv.Employe = employeManager.Read((int)dataReader["id_commercial"]);
                    rdv.Praticien = praticienManager.Read((int)dataReader["id_praticien"]);
                    rdv.Date_rdv = (DateTime)dataReader["date_rdv"];                    
                    rdv.Motif_rdv = (RendezVous.Rdv)Enum.Parse(typeof(RendezVous.Rdv), (string)dataReader["motif_rdv"]);
                    rdv.Indice_confiance = (int)dataReader["indice_confiance"];
                    rdv.Date_bilan = (DateTime)dataReader["date_bilan"];
                    rdv.Liste_echantillons_donnes = echantillonDonneManager.ReadAllFromRendezVous(rdv.Id_rdv);
                    liste_rdv.Add(rdv);
                }
                dataReader.Close();
                command.ExecuteNonQuery();
                CloseConnection();
            }
            return liste_rdv;

        }

        public List<RendezVous> ReadAllFromCommercialID(int id_commercial)
        {
            List<RendezVous> liste_rdv = new List<RendezVous>();
            if (OpenConnection())
            {
                RendezVous rdv;
                EmployeDAO employeManager = new EmployeDAO();
                PraticienDAO praticienManager = new PraticienDAO();
                EchantillonDonneDAO echantillonDonneManager = new EchantillonDonneDAO();

                command = manager.CreateCommand();
                command.CommandText = "SELECT * FROM rendez_vous WHERE id_commercial = @id_commercial";

                command.Parameters.AddWithValue("@id_commercial", id_commercial);

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    rdv = new RendezVous();
                    rdv.Id_rdv = (int)dataReader["id_rdv"];
                    rdv.Employe = employeManager.Read((int)dataReader["id_commercial"]);
                    rdv.Praticien = praticienManager.Read((int)dataReader["id_praticien"]);
                    rdv.Date_rdv = (DateTime)dataReader["date_rdv"];
                    rdv.Date_bilan = (DateTime)dataReader["date_bilan"];
                    rdv.Motif_rdv = (RendezVous.Rdv)Enum.Parse(typeof(RendezVous.Rdv), (string)dataReader["motif_rdv"]);
                    rdv.Indice_confiance = (int)dataReader["indice_confiance"];
                    rdv.Liste_echantillons_donnes = echantillonDonneManager.ReadAllFromRendezVous(rdv.Id_rdv);
                    liste_rdv.Add(rdv);
                }
                dataReader.Close();
                command.ExecuteNonQuery();
                CloseConnection();
            }
            return liste_rdv;

        }


        public List<RendezVous> ReadRDVFuturFromCommercialID(int id_commercial)
        {
            List<RendezVous> liste_rdv = new List<RendezVous>();
            if (OpenConnection())
            {
                RendezVous rdv;
                EmployeDAO employeManager = new EmployeDAO();
                PraticienDAO praticienManager = new PraticienDAO();
                EchantillonDonneDAO echantillonDonneManager = new EchantillonDonneDAO();

                command = manager.CreateCommand();
                command.CommandText = "SELECT * FROM rendez_vous WHERE id_commercial = @id_commercial AND date_rdv >= NOW()";

                command.Parameters.AddWithValue("@id_commercial", id_commercial);

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    rdv = new RendezVous();
                    rdv.Id_rdv = (int)dataReader["id_rdv"];
                    rdv.Employe = employeManager.Read((int)dataReader["id_commercial"]);
                    rdv.Praticien = praticienManager.Read((int)dataReader["id_praticien"]);
                    rdv.Date_rdv = (DateTime)dataReader["date_rdv"];
                    rdv.Date_bilan = (DateTime)dataReader["date_bilan"];
                    rdv.Motif_rdv = (RendezVous.Rdv)Enum.Parse(typeof(RendezVous.Rdv), (string)dataReader["motif_rdv"]);
                    rdv.Indice_confiance = (int)dataReader["indice_confiance"];
                    rdv.Liste_echantillons_donnes = echantillonDonneManager.ReadAllFromRendezVous(rdv.Id_rdv);
                    liste_rdv.Add(rdv);
                }
                dataReader.Close();
                command.ExecuteNonQuery();
                CloseConnection();
            }
            return liste_rdv;

        }
        public List<RendezVous> ReadRDVHistoFromCommercialID(int id_commercial)
        {
            List<RendezVous> liste_rdv = new List<RendezVous>();
            if (OpenConnection())
            {
                RendezVous rdv;
                EmployeDAO employeManager = new EmployeDAO();
                PraticienDAO praticienManager = new PraticienDAO();
                EchantillonDonneDAO echantillonDonneManager = new EchantillonDonneDAO();

                command = manager.CreateCommand();
                command.CommandText = "SELECT * FROM rendez_vous WHERE id_commercial = @id_commercial AND date_rdv < NOW()";

                command.Parameters.AddWithValue("@id_commercial", id_commercial);

                dataReader = command.ExecuteReader();
                Debug.WriteLine("==== Je suis appellé une fois pour lire les RDV");
                while (dataReader.Read())
                {
                    rdv = new RendezVous();
                    rdv.Id_rdv = (int)dataReader["id_rdv"];
                    rdv.Employe = employeManager.Read((int)dataReader["id_commercial"]);
                    rdv.Praticien = praticienManager.Read((int)dataReader["id_praticien"]);
                    rdv.Date_rdv = (DateTime)dataReader["date_rdv"];
                    rdv.Date_bilan = (DateTime)dataReader["date_bilan"];
                    rdv.Motif_rdv = (RendezVous.Rdv)Enum.Parse(typeof(RendezVous.Rdv), (string)dataReader["motif_rdv"]);
                    rdv.Indice_confiance = (int)dataReader["indice_confiance"];
                    rdv.Liste_echantillons_donnes = echantillonDonneManager.ReadAllFromRendezVous(rdv.Id_rdv);
                    liste_rdv.Add(rdv);
                }
                dataReader.Close();
                command.ExecuteNonQuery();
                CloseConnection();
            }
            return liste_rdv;
        }


    }
}
