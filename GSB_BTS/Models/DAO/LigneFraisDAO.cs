﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSB.Models.DAO
{
    public class LigneFraisDAO : DAO_Manager
    {
        public LigneFrais Read(int id_ligne_frais)
        {
            LigneFrais ligneFrais = new LigneFrais();
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                      "FROM ligne_frais " +
                                      "WHERE id_ligne_frais = @id_ligne_frais";
                command.Parameters.AddWithValue("@id_ligne_frais", id_ligne_frais);

                // Lecture des résultats
                dataReader = command.ExecuteReader();

                ligneFrais.Id = id_ligne_frais;
                while (dataReader.Read())
                {
                    ligneFrais.FicheFrais.Id_fiche_frais = (int)dataReader["id_fiche_frais"];
                    ligneFrais.Date_engagement = (DateTime)dataReader["date_engagement"];
                    ligneFrais.Frais = (LigneFrais.TypeFrais)Enum.Parse(typeof(LigneFrais.TypeFrais), (string)dataReader["type_frais"]);
                    ligneFrais.Forfait = (LigneFrais.TypeForfait)Enum.Parse(typeof(LigneFrais.TypeForfait), (string)dataReader["type_forfait"]);
                    ligneFrais.Libelle = (string)dataReader["libelle"];
                    ligneFrais.Montant = (int)dataReader["montant"];
                    ligneFrais.EtatLigne = (LigneFrais.EtatLigneFrais)Enum.Parse(typeof(LigneFrais.EtatLigneFrais), (string)dataReader["etat_ligne_frait"]);
                    // Utilisation d'un Enum.Parse pour transformer un string en Enum
                    // Pour ce faire : (Type Enum)Enum.Parse(typeof(Type Enum), (string)variable);

                }

                dataReader.Close();
                CloseConnection();
            }
            return ligneFrais;
        }

        public List<LigneFrais> ReadAllFromFicheFrais(FicheFrais ficheFrais, bool isSerializeRead)
        {
            List<LigneFrais> list_fiche_frais = new List<LigneFrais>();
            if (OpenConnection())
            {
                LigneFrais ligne_frais;

                command = manager.CreateCommand();
                command.CommandText = "SELECT * FROM ligne_frais WHERE id_fiche_frais = @id_fiche_frais";

                command.Parameters.AddWithValue("@id_fiche_frais", ficheFrais.Id_fiche_frais);

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    ligne_frais = new LigneFrais();
                    ligne_frais.Id = (int)dataReader["id_ligne_frais"];
                    if(!isSerializeRead)
                    {
                        ligne_frais.FicheFrais = ficheFrais;
                    }
                    ligne_frais.EtatLigne = (LigneFrais.EtatLigneFrais)Enum.Parse(typeof(LigneFrais.EtatLigneFrais), (string)dataReader["etat_ligne_frais"]);
                    ligne_frais.Forfait = (LigneFrais.TypeForfait)Enum.Parse(typeof(LigneFrais.TypeForfait), (string)dataReader["type_forfait"]);
                    ligne_frais.Frais = (LigneFrais.TypeFrais)Enum.Parse(typeof(LigneFrais.TypeFrais), (string)dataReader["type_frais"]);
                    ligne_frais.Libelle = (string)dataReader["libelle"];
                    ligne_frais.Date_engagement = (DateTime)dataReader["date_engagement"];
                    list_fiche_frais.Add(ligne_frais);
                }
                dataReader.Close();
                command.ExecuteNonQuery();
                CloseConnection();
            }
            return list_fiche_frais;

        }

        public List<LigneFrais> ReadAllFromID(int id_employe, int id_rdv)
        {
            List<LigneFrais> list_fiche_frais = new List<LigneFrais>();
            if (OpenConnection())
            {
                LigneFrais ligne_frais;

                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                        "FROM fiche_frais ff " +
                                        "JOIN ligne_frais lf on ff.id_fiche_frais = lf.id_fiche_frais " +
                                        "WHERE ff.id_commercial_visiteur= @id_employe "+
                                        "AND id_rdv=@id_rdv";
                command.Parameters.AddWithValue("@id_employe", id_employe);
                command.Parameters.AddWithValue("@id_rdv", id_rdv);

                dataReader = command.ExecuteReader();

                FicheFraisDAO ficheFraisManager = new FicheFraisDAO();

                while (dataReader.Read())
                {
                    ligne_frais = new LigneFrais();
                    ligne_frais.FicheFrais = ficheFraisManager.ReadFromIdCommercialVisiteur(id_employe);
                    ligne_frais.Id = (int)dataReader["id_ligne_frais"];
                    ligne_frais.Frais = (LigneFrais.TypeFrais)Enum.Parse(typeof(LigneFrais.TypeFrais), (string)dataReader["type_frais"]);
                    ligne_frais.Forfait = (LigneFrais.TypeForfait)Enum.Parse(typeof(LigneFrais.TypeForfait), (string)dataReader["type_forfait"]);
                    ligne_frais.Libelle = (string)dataReader["libelle"];
                    ligne_frais.Montant = (int)dataReader["montant"];
                    ligne_frais.Date_engagement = (DateTime)dataReader["date_engagement"];

                    list_fiche_frais.Add(ligne_frais);
                    
                }
                dataReader.Close();
                command.ExecuteNonQuery();
                CloseConnection();
            }
            return list_fiche_frais;

        }
        public void Delete(int id)
        {
            if (OpenConnection())
            {
                command = manager.CreateCommand();
                command.CommandText = "DELETE FROM ligne_frais " +
                                      "WHERE id_ligne_frais= @id";
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

    }
}
