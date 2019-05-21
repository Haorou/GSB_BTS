using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSB.Models.DAO
{
    public class LigneFraisDAO : DAO_Manager
    {
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

        public List<LigneFrais> ReadAllFromID(int id_employe)
        {
            List<LigneFrais> list_fiche_frais = new List<LigneFrais>();
            if (OpenConnection())
            {
                LigneFrais ligne_frais;

                command = manager.CreateCommand();
                command.CommandText = "SELECT * " +
                                        "FROM fiche_frais ff " +
                                        "JOIN ligne_frais lf on ff.id_fiche_frais = lf.id_fiche_frais " +
                                        "WHERE ff.id_commercial_visiteur= @id_employe";
                command.Parameters.AddWithValue("@id_employe", id_employe);

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

    }
}
