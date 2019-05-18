using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSB.Models.DAO
{
    public class EchantillonDonneDAO : DAO_Manager
    {
        public void Create(EchantillonDonne echantillon_donne)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "INSERT INTO echantillon_donne " +
                                      "(id_echantillon, id_rdv, quantite) " +
                                      "VALUES (@id_echantillon ,@id_rdv , @quantite)";
                command.Parameters.AddWithValue("@id_echantillon", echantillon_donne.Echantillon.Id_echantillon);
                command.Parameters.AddWithValue("@id_rdv", echantillon_donne.RendezVous.Id_rdv);
                command.Parameters.AddWithValue("@quantite", echantillon_donne.Quantite);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }


        public List<EchantillonDonne> ReadAllFromEchantillon(Echantillon echantillon)
        {
            List<EchantillonDonne> liste_echantillons_donnes = new List<EchantillonDonne>();
            RendezVousDAO rendezVousManager = new RendezVousDAO();
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                      "FROM echantillon_donne " +
                                      "WHERE id_echantillon = @id_echantillon";

                command.Parameters.AddWithValue("@id_echantillon", echantillon.Id_echantillon);

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    liste_echantillons_donnes.Add(new EchantillonDonne((int)dataReader["quantite"],
                                                                        echantillon,
                                                                        rendezVousManager.Read((int)dataReader["id_rdv"])));
                }
                dataReader.Close();
                CloseConnection();
            }

            return liste_echantillons_donnes;
        }

        public List<EchantillonDonne> ReadAllFromRendezVous(RendezVous rendezVous)
        {
            List<EchantillonDonne> liste_echantillons_donnes = new List<EchantillonDonne>();
            if (OpenConnection())
            {
                EchantillonDAO echantillonManager = new EchantillonDAO();

                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                      "FROM echantillon_donne " +
                                      "WHERE id_rdv = @id_rdv";

                command.Parameters.AddWithValue("@id_rdv", rendezVous.Id_rdv);

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    liste_echantillons_donnes.Add(new EchantillonDonne((int)dataReader["quantite"],
                                                                        echantillonManager.Read((int)dataReader["id_echantillon"]),
                                                                        rendezVous));
                }
                dataReader.Close();
                CloseConnection();
            }

            return liste_echantillons_donnes;
        }
    }
}
