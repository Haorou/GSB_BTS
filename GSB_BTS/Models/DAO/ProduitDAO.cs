using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace GSB.Models.DAO
{
    public class ProduitDAO : DAO_Manager
    {
        public Produit Read(int id)
        {
            Produit produit = new Produit();
            if (OpenConnection())
            {

                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                        "FROM produit " +
                                        "WHERE id_produit = @id";
                command.Parameters.AddWithValue("@id", id);

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    produit.Id_produit = (int)dataReader["id_produit"];
                    produit.Pathologie = (string)dataReader["pathologie"];
                    produit.Famille = (string)dataReader["famille"];
                    produit.Nom = (string)dataReader["nom"];
                    produit.Notice = (string)dataReader["notice"];
                    produit.Libelle = (string)dataReader["libelle"];
                }
                dataReader.Close();
                CloseConnection();
            }
            return produit;
        }


        public void Create(Produit produit)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "INSERT INTO produit " +
                                      "(pathologie, famille, nom, notice, libelle) " +
                                      "VALUES (@pathologie, @famille, @nom, @notice, @libelle)";
                command.Parameters.AddWithValue("@pathologie", produit.Pathologie);
                command.Parameters.AddWithValue("@famille", produit.Famille);
                command.Parameters.AddWithValue("@nom", produit.Nom);
                command.Parameters.AddWithValue("@notice", produit.Notice);
                command.Parameters.AddWithValue("@libelle", produit.Libelle);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public void Update(Produit produit)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "UPDATE produit " +
                                      "SET pathologie=@pathologie, famille=@famille, nom=@nom, notice=@notice, libelle=@libelle " +
                                      "WHERE produit.id_produit=@id";
                command.Parameters.AddWithValue("@id", produit.Id_produit);
                command.Parameters.AddWithValue("@pathologie", produit.Pathologie);
                command.Parameters.AddWithValue("@famille", produit.Famille);
                command.Parameters.AddWithValue("@nom", produit.Nom);
                command.Parameters.AddWithValue("@notice", produit.Notice);
                command.Parameters.AddWithValue("@libelle", produit.Libelle);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }
        // pas besoin du delete on garde les produits dans la bd (historique)
        //public void Delete(Produit produit)
        //{
        //    if (OpenConnection())
        //    {
        //        command = manager.CreateCommand();
        //        command.CommandText = "DELETE FROM produit " +
        //                              "WHERE id_produit= @id" +
        //// certainement rajouter DELETE echantillon
        //        command.Parameters.AddWithValue("@id", produit.IdProduit);

        //        command.ExecuteNonQuery();
        //        CloseConnection();
        //    }
        //}
        public List<Produit> ReadAll()
        {
            List<Produit> produits = new List<Produit>();
            if (OpenConnection())
            {
                EchantillonDAO echantillonManager = new EchantillonDAO();
                Produit produit;

                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                    "FROM produit";

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    produit = new Produit();
                    produit.Id_produit = (int)dataReader["id_produit"];
                    produit.Pathologie = (string)dataReader["pathologie"];
                    produit.Famille = (string)dataReader["famille"];
                    produit.Nom = (string)dataReader["nom"];
                    produit.Notice = (string)dataReader["notice"];
                    produit.Libelle = (string)dataReader["libelle"];
                    produit.Echantillons = echantillonManager.ReadAllFromProduit(produit);

                    produits.Add(produit);
                }
                dataReader.Close();
                CloseConnection();
            }

            return produits;
        }
    }
}