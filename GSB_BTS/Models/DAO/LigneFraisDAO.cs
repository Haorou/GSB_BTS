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
                    ligne_frais.Justificatif = (int)dataReader["justificatif"];
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

    }
}
