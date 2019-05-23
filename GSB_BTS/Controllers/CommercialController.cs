using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GSB.Models;
using GSB.Models.DAO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GSB.Controllers
{
    public class CommercialController : Controller
    {
        public ActionResult ConsultationRDV()
        {
            ViewBag.Employe = (Employe)Session["Employe"];
            
            if(ViewBag.Employe != null)
            {
                RendezVousDAO rendezVousDAO = new RendezVousDAO();
                PraticienDAO praticienDAO = new PraticienDAO();
                List<RendezVous> futursRDV = rendezVousDAO.ReadRDVFuturFromCommercialID(ViewBag.Employe.Id);
                List<RendezVous> passesRDV = rendezVousDAO.ReadRDVHistoFromCommercialID(ViewBag.Employe.Id);
                List<Praticien> mesPraticiens = praticienDAO.ReadAll();

                ViewBag.FutursRDV = futursRDV;
                ViewBag.PassesRDV = passesRDV;
                ViewBag.MesPraticiens = mesPraticiens;

                ViewData["Message"] = "Page de Consultation de vos Rendez-vous.";

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Etablissement()
        {
            ViewBag.Employe = (Employe)Session["Employe"];

            if (ViewBag.Employe != null)
            {
                EtablissementDAO etablissementDAO = new EtablissementDAO();

                List<Etablissement> mesEtablissement = etablissementDAO.ReadAll();

                ViewBag.MesEtablissement = mesEtablissement;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Echantillon()
        {
            Employe employe = (Employe)Session["Employe"];
            EchantillonDonneDAO echantillonDAO = new EchantillonDonneDAO();
            ProduitDAO produitDAO = new ProduitDAO();

            List<Produit> mesProduits = produitDAO.ReadAll();

            ViewBag.Echantillon = mesProduits;
            ViewBag.Employe = (Employe)Session["Employe"];

            return View();
        }

        public ActionResult FicheFrais(int id_rdv)
        {

            Employe employe = (Employe)Session["Employe"];

            PersonneDAO personneDAO = new PersonneDAO();
            LigneFraisDAO ligneFraisDAO = new LigneFraisDAO();
            TypeFraisDAO typeFraisDAO = new TypeFraisDAO();

            List<LigneFrais> mesLignesFrais = ligneFraisDAO.ReadAllFromID(employe.Id, id_rdv);
            List<TypeFrais> mesTypesFrais = typeFraisDAO.ReadAll();

            ViewBag.MesFichesFrais = mesLignesFrais;
            ViewBag.MesTypesFrais = mesTypesFrais;
            ViewBag.Employe = (Employe)Session["Employe"];

            //Debug.WriteLine("==================================="+employe.Id);
            //Debug.WriteLine("==================================="+mesLignesFrais[0].Date_engagement + " === count === " + mesLignesFrais.Count);
            Debug.WriteLine("==================================="+ mesTypesFrais[0].TypeFraisCommercial + " === count === " + mesLignesFrais.Count);

            return View();
        }

        public string AjaxReader(string table, int id, int id_rdv)
        {
            string response = "";
            string response1 = "";
            if (table.Equals("rendez_vous"))
            {
                RendezVousDAO rendezVousManager = new RendezVousDAO();
                RendezVous rendezVous = rendezVousManager.Read(id);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Serialize(rendezVous);
            }
            else if (table.Equals("etablissement"))
            {
                PraticienDAO praticienManager = new PraticienDAO();
                EtablissementDAO etablissementManager = new EtablissementDAO();

                Etablissement etablissement = etablissementManager.Read(id);

                Praticien praticien = new Praticien();
                praticien.Etablissement = etablissement;

                List<Praticien> PraticiensInEtablissement = new List<Praticien>();
                PraticiensInEtablissement.Add(praticien);
                PraticiensInEtablissement.AddRange(praticienManager.ReadAllPraticiensInEtablissement(id));
                
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Serialize(PraticiensInEtablissement);
            }

            else if (table.Equals("ligne_frais"))
            {
                TypeFraisDAO typeFraisDAO = new TypeFraisDAO();
                LigneFraisDAO ligneFraisDAO = new LigneFraisDAO();

                List<TypeFrais> mesTypesFrais = typeFraisDAO.ReadAll();
                List<LigneFrais> mesLigneFrais = ligneFraisDAO.ReadAllFromID(id, id_rdv);

                ViewBag.MesTypesFrais = mesTypesFrais;
                ViewBag.MeslignesFrais = mesLigneFrais;

            }

            return response;
        }

        public void AjaxAddModifyRDV(int? id, string date, string time, string motif, int indice, int id_employe, int id_praticien)
        {
            RendezVousDAO rendezVousManager = new RendezVousDAO();
            PraticienDAO praticienManager = new PraticienDAO();
            EmployeDAO employeManager = new EmployeDAO();

            RendezVous newRDV = id == null ? new RendezVous() : rendezVousManager.Read((int)id);

            Debug.WriteLine("Debug.Time = > " + time);

            newRDV.Date_rdv = new DateTime(Convert.ToInt32(date.Substring(0, 4)),
                               Convert.ToInt32(date.Substring(5, 2)),
                               Convert.ToInt32(date.Substring(8)),
                               Convert.ToInt32(time.Substring(0, 2)),
                               Convert.ToInt32(time.Substring(3)),
                               00);

            newRDV.Date_bilan = newRDV.Date_rdv.AddDays(7);
            newRDV.Indice_confiance = indice;
            newRDV.Motif_rdv = (RendezVous.Rdv)Enum.Parse(typeof(RendezVous.Rdv), motif);
            newRDV.Praticien = praticienManager.Read(id_praticien);
            newRDV.Employe = employeManager.Read(id_employe);

            if (id == null) // ADD
            {
                rendezVousManager.Create(newRDV);
            }
            else // MODIFY
            {
                rendezVousManager.Update(newRDV);
            }
        }

        public void AjaxAddEtablissement(string nom, string adresse)
        {
            Etablissement etablissement = new Etablissement();
            EtablissementDAO etablissementManager = new EtablissementDAO();

            etablissement.Nom = nom;
            etablissement.Adresse = adresse;
            etablissementManager.Create(etablissement);
        }

        public void AjaxDelete(string table, int id)
        {
            if(table.Equals("rendez_vous"))
            {
                RendezVousDAO rendezVousManager = new RendezVousDAO();
                rendezVousManager.Delete(rendezVousManager.Read(id));
            }
        }


    }
   
}