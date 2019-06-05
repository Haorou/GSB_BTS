using System;
using System.Collections.Generic;
using GSB.Models;
using GSB.Models.DAO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GSB.Controllers
{
    public class AjaxComptableController : Controller
    {

        public void UpdateLigneFrais(int id_comptable, int id_ligne_frais, int id_fiche_frais, string etat_ligne_frais)
        {
            EmployeDAO employeManager = new EmployeDAO();
            FicheFraisDAO ficheFraisManager = new FicheFraisDAO();
            LigneFraisDAO ligneFraisManager = new LigneFraisDAO();

            FicheFrais ficheFrais = ficheFraisManager.Read(id_fiche_frais, true);
            if(ficheFrais.Comptable == null)
            {
                ficheFrais.Comptable = employeManager.Read(id_comptable);
                ficheFraisManager.Update(ficheFrais);
            }

            LigneFrais ligneFrais = ligneFraisManager.Read(id_ligne_frais, true);
            ligneFrais.EtatLigne = (LigneFrais.EtatLigneFrais)Enum.Parse(typeof(LigneFrais.EtatLigneFrais), etat_ligne_frais);
            ligneFrais.Date_modification = DateTime.Now;
            if(ligneFrais.EtatLigne == LigneFrais.EtatLigneFrais.mise_en_paiement)
            {
                ligneFrais.Date_engagement = DateTime.Now;
            }
            else
            {
                ligneFrais.Date_engagement = null;
            }
            ligneFraisManager.Update(ligneFrais);
        }

        public string Read(string table, int id)
        {
            string response = "";
            if(table.Equals("fiche_frais"))
            {
                FicheFraisDAO ficheFraisManager = new FicheFraisDAO();
                FicheFrais ficheFrais = ficheFraisManager.Read(id, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Serialize(ficheFrais);
            }

            return response;
        }
    }
}