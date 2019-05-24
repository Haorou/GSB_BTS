using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GSB.Models.DAO
{
    public class EchantillonDAO : DAO_Manager
    {
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
                        Debug.WriteLine("   JE NE SUIS PAS LU ET C BIEN");
                        echantillon.Liste_echantillons_donnes = enchantillonDonneManager.ReadAllFromEchantillon(echantillon);
                    }
                }
                dataReader.Close();
                CloseConnection();
            }

            return echantillon;
        }

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
    }
}
