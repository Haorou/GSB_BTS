using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GSB.Models.DAO
{
    public class EchantillonDAO : DAO_Manager
    {

        //public void Create(Echantillon echantillon)
        //{
        //    if (OpenConnection())
        //    {
        //        command = manager.CreateCommand();
        //        command.CommandText = "INSERT INTO echantillon " +
        //                              "(id_produit, quantite, concentration, libelle) " +
        //                              "VALUES (@id_produit, @quantite, @concentration, @libelle)";

        //        command.Parameters.AddWithValue("@id_produit", echantillon.Produit.Id_produit);
        //        command.Parameters.AddWithValue("@quantite", echantillon.Quantite);
        //        command.Parameters.AddWithValue("@concentration", echantillon.Concentration);
        //        command.Parameters.AddWithValue("@libelle", echantillon.Libelle);
                

        //        command.ExecuteNonQuery();

        //        CloseConnection();
        //    }
        //}

        public Echantillon Read(int id_echantillon, bool isReadFromEchantillonDonnes)
        {
            Echantillon echantillon = new Echantillon();

            if (OpenConnection())
            {
                ProduitDAO produitManager = new ProduitDAO();
                EchantillonDonneDAO enchantillonDonneManager = new EchantillonDonneDAO();

                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                      "FROM echantillon " +
                                      "WHERE id_echantillon = @id_echantillon";

                command.Parameters.AddWithValue("@id_echantillon", id_echantillon);

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    echantillon.Id_echantillon = (int)dataReader["id_echantillon"];
                    echantillon.Quantite = (int)dataReader["quantite"];
                    echantillon.Libelle = (string)dataReader["libelle"];
                    echantillon.Concentration = (int)dataReader["concentration"];
                    echantillon.Produit = produitManager.Read((int)dataReader["id_produit"], isReadFromEchantillonDonnes);
                    if(!isReadFromEchantillonDonnes)
                    {
                        //Debug.WriteLine("   JE NE SUIS PAS LU ET C BIEN");
                        echantillon.Liste_echantillons_donnes = enchantillonDonneManager.ReadAllFromEchantillon(echantillon);
                    }
                }
                dataReader.Close();
                CloseConnection();
            }

            return echantillon;
        }

        public Echantillon ReadByNomConcentration(string nom, int concentration, bool isReadFromEchantillonDonnes)
        {
            Echantillon echantillon = new Echantillon();
            Produit produit = new Produit();

            if (OpenConnection())
            {
                ProduitDAO produitManager = new ProduitDAO();
                EchantillonDonneDAO enchantillonDonneManager = new EchantillonDonneDAO();

                command = manager.CreateCommand();
                command.CommandText = "SELECT id_echantillon " +
                                      "FROM echantillon " +
                                      "JOIN produit on produit.id_produit = echantillon.id_produit " +
                                      "WHERE produit.nom = @nom AND echantillon.concentration = @concentration";

                command.Parameters.AddWithValue("@nom", produit.Nom );
                command.Parameters.AddWithValue("@concentration", echantillon.Concentration);

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    echantillon.Id_echantillon = (int)dataReader["id_echantillon"];
                    echantillon.Quantite = (int)dataReader["quantite"];
                    echantillon.Libelle = (string)dataReader["libelle"];
                    echantillon.Concentration = (int)dataReader["concentration"];
                    echantillon.Produit = produitManager.Read((int)dataReader["id_produit"], isReadFromEchantillonDonnes);
                    if (!isReadFromEchantillonDonnes)
                    {
                        //Debug.WriteLine("   JE NE SUIS PAS LU ET C BIEN");
                        echantillon.Liste_echantillons_donnes = enchantillonDonneManager.ReadAllFromEchantillon(echantillon);
                    }
                }
                dataReader.Close();
                CloseConnection();
            }

            return echantillon;
        }

        //public void Update(Echantillon echantillon)
        //{
        //    if (OpenConnection())
        //    {
        //        command = manager.CreateCommand();
        //        command.CommandText = "UPDATE echantillon " +
        //                              "SET id_echantillon=@id_echantillon, id_produit=@id_produit, quantite=@quantite,  concentration=@concentration, libelle=@libelle " +
        //                              "WHERE echantillon.id_echantillon = @id_echantillon";

        //        command.Parameters.AddWithValue("@id_echantillon", echantillon.Id_echantillon);
        //        command.Parameters.AddWithValue("@id_produit", echantillon.Produit.Id_produit);
        //        command.Parameters.AddWithValue("@quantite", echantillon.Quantite);
        //        command.Parameters.AddWithValue("@concentration", echantillon.Concentration);
        //        command.Parameters.AddWithValue("@libelle", echantillon.Libelle);

        //        // Update ligne frais ?

        //        command.ExecuteNonQuery();
        //        CloseConnection();
        //    }
        //}

        //public void Delete(Echantillon echantillon)
        //{
        //    if (OpenConnection())
        //    {
        //        command = manager.CreateCommand();
        //        command.CommandText = "DELETE FROM echantillon " +
        //                              "WHERE id_echantillon = @id_echantillon";
        //        command.Parameters.AddWithValue("@id_echantillon", echantillon.Id_echantillon);

        //        // Delete ligne frais
        //        command.ExecuteNonQuery();
        //        CloseConnection();
        //    }
        //}

        public List<Echantillon> ReadAllFromProduit(Produit produit)
        {
            List<Echantillon> liste_echantillons = new List<Echantillon>();

            if (OpenConnection())
            {
                EchantillonDonneDAO echantillonDonneManager = new EchantillonDonneDAO();
                Echantillon echantillon = new Echantillon();

                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                      "FROM echantillon " +
                                      "WHERE id_produit = @id_produit";

                command.Parameters.AddWithValue("@id_produit", produit.Id_produit);
                
                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    echantillon.Produit = produit;
                    echantillon.Id_echantillon = (int)dataReader["id_echantillon"];
                    echantillon.Quantite = (int)dataReader["quantite"];
                    echantillon.Libelle = (string)dataReader["libelle"];
                    echantillon.Concentration = (int)dataReader["concentration"];
                    echantillon.Liste_echantillons_donnes = echantillonDonneManager.ReadAllFromEchantillon(echantillon);

                    liste_echantillons.Add(echantillon);
                }
                dataReader.Close();
                CloseConnection();
            }

            return liste_echantillons;
        }

        //public List<Echantillon> ReadConcentrationFromNom()
    }
}
