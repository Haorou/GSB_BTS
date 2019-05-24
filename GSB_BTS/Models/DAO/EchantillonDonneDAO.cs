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
            Produit produit = new Produit();
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
                                                                        rendezVousManager.Read((int)dataReader["id_rdv"]),
                                                                        produit));
                }
                dataReader.Close();
                CloseConnection();
            }

            return liste_echantillons_donnes;
        }

        public List<EchantillonDonne> ReadAllFromRendezVous(int id_rdv)
        {
            List<EchantillonDonne> liste_echantillons_donnes = new List<EchantillonDonne>();
            if (OpenConnection())
            {
                EchantillonDonne echantillonManager = new EchantillonDonne();
                Produit produit = new Produit();
                Echantillon echantillon = new Echantillon();


                command = manager.CreateCommand();
                command.CommandText =   "SELECT distinct * " +
                                        "FROM produit p " +
                                        "join echantillon e on e.id_produit = p.id_produit " +
                                        "join echantillon_donne ed on ed.id_echantillon = e.id_echantillon " +
                                        "where id_rdv =@id_rdv ";


                command.Parameters.AddWithValue("@id_rdv", id_rdv);

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    echantillonManager.Echantillon.Id_echantillon = (int)dataReader["id_echantillon"];
                    echantillonManager.Produit.Famille = (string)dataReader["famille"];
                    echantillonManager.Produit.Nom = (string)dataReader["nom"];
                    echantillonManager.Quantite = (int)dataReader["quantite"];
                    echantillonManager.Echantillon.Concentration = (int)dataReader["concentration"];
                   

                    liste_echantillons_donnes.Add(echantillonManager);
                }
                dataReader.Close();
                CloseConnection();
            }

            return liste_echantillons_donnes;
        }
    }
}
