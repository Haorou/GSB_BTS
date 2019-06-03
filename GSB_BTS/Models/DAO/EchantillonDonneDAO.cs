using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public EchantillonDonne Read(int id_echantillon, int id_rdv)
        {
            EchantillonDonne echantillonDonne = new EchantillonDonne();
            if (OpenConnection())
            {
                command = manager.CreateCommand();

                EchantillonDAO echantillonManager = new EchantillonDAO();
                RendezVousDAO rendezVousManager = new RendezVousDAO();

                command.CommandText = "SELECT * " +
                                      "FROM echantillon_donne " +
                                      "WHERE id_echantillon = @id_echantillon AND " +
                                      "id_rdv = @id_rdv";
                command.Parameters.AddWithValue("@id_echantillon", id_echantillon);
                command.Parameters.AddWithValue("@id_rdv", id_rdv);

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    echantillonDonne.Echantillon = echantillonManager.Read((int)dataReader["id_echantillon"], true);
                    echantillonDonne.RendezVous = rendezVousManager.Read((int)dataReader["id_rdv"], true);
                    echantillonDonne.Quantite = (int)dataReader["quantite"];
                }
                dataReader.Close();
                CloseConnection();
            }
            return echantillonDonne;
        }

        public void Update(EchantillonDonne echantillonDonne)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "UPDATE echantillon_donne " +
                                      "SET quantite=@quantite " +
                                      "WHERE  id_echantillon=@id " +
                                      "AND id_rdv=@id_rdv";
                command.Parameters.AddWithValue("@id", echantillonDonne.Echantillon.Id_echantillon);
                command.Parameters.AddWithValue("@id_rdv", echantillonDonne.RendezVous.Id_rdv);
                command.Parameters.AddWithValue("@quantite", echantillonDonne.Quantite);

                command.ExecuteNonQuery();

                CloseConnection();
            }
        }

        public void Delete(int id_echantillon, int id_rdv)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "DELETE FROM echantillon_donne " +
                                      "WHERE id_echantillon= @id AND " +
                                      "id_rdv = @id_rdv";
                command.Parameters.AddWithValue("@id", id_echantillon);
                command.Parameters.AddWithValue("@id_rdv", id_rdv);

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
                                                                        rendezVousManager.Read((int)dataReader["id_rdv"], false),
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
                EchantillonDonne echantillonDonne = new EchantillonDonne();
                EchantillonDAO echantillonManager = new EchantillonDAO();
                RendezVousDAO rendezVousManager = new RendezVousDAO();
                ProduitDAO produitManager = new ProduitDAO();

                command = manager.CreateCommand();
                command.CommandText =   "SELECT ed.id_echantillon, ed.id_rdv, ed.quantite AS echantillonQuantite, p.id_produit " +
                                        "FROM produit p " +
                                        "join echantillon e on e.id_produit = p.id_produit " +
                                        "join echantillon_donne ed on ed.id_echantillon = e.id_echantillon " +
                                        "where id_rdv =@id_rdv ";


                command.Parameters.AddWithValue("@id_rdv", id_rdv);

                // Lecture des résultats
                dataReader = command.ExecuteReader();
                Debug.WriteLine("ICI ");
                while (dataReader.Read())
                {
                    echantillonDonne.Echantillon = echantillonManager.Read((int)dataReader["id_echantillon"], true);
                    echantillonDonne.Produit = produitManager.Read((int)dataReader["id_produit"], true);
                    echantillonDonne.Quantite = (int)dataReader["echantillonQuantite"];
                    echantillonDonne.RendezVous = rendezVousManager.Read(id_rdv,true);

                    liste_echantillons_donnes.Add(echantillonDonne);
                }
                dataReader.Close();
                CloseConnection();
            }

            return liste_echantillons_donnes;
        }


    }
}
