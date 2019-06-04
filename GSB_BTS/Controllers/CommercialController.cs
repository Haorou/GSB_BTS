using System;
using System.Collections.Generic;
using System.Diagnostics;
using GSB.Models;
using GSB.Models.DAO;
using System.Web.Mvc;

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

                Debug.WriteLine("Debug Bug");
                List<RendezVous> passesRDV = rendezVousDAO.ReadRDVHistoFromCommercialID(ViewBag.Employe.Id);
                Debug.WriteLine("Fin Bug");

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

        public ActionResult Echantillon(int id_rdv)
        {
            ViewBag.Employe = (Employe)Session["Employe"];

            if (ViewBag.Employe != null)
            {
                ProduitDAO produitManager = new ProduitDAO();
                EchantillonDonneDAO echantillonDonneManager = new EchantillonDonneDAO();
                RendezVousDAO rendezVousManager = new RendezVousDAO();

                RendezVous rendezVous = rendezVousManager.Read(id_rdv, false);
                List<EchantillonDonne> mesEchantillonsDonnes = echantillonDonneManager.ReadAllFromRendezVous(id_rdv);
                List<Produit> mesFamilles = produitManager.ReadFamille();

                ViewBag.Famille = mesFamilles;
                ViewBag.EchantillonDonne = mesEchantillonsDonnes;
                ViewBag.RendezVous = rendezVous;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Produit(string famille)
        {
            ViewBag.Employe = (Employe)Session["Employe"];

            if (ViewBag.Employe != null)
            {
                ProduitDAO produitManager = new ProduitDAO();
                List<Produit> listeProduit = new List<Produit>();
                List<Produit> mesFamilles = produitManager.ReadFamille();
                List<Produit> mesNoms = produitManager.ReadNom(famille);

                ViewBag.Famille = mesFamilles;
                ViewBag.Nom = mesNoms;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult FicheFrais(int id_rdv)
        {
            ViewBag.Employe = (Employe)Session["Employe"];

            if (ViewBag.Employe != null)
            {
                PersonneDAO personneDAO = new PersonneDAO();
                LigneFraisDAO ligneFraisDAO = new LigneFraisDAO();
                FicheFraisDAO ficheFraisDAO = new FicheFraisDAO();

                int id_fiche_frais = ficheFraisDAO.GetIdFicheFrais(id_rdv);

                List<LigneFrais> mesLignesFrais = ligneFraisDAO.ReadAllFromID(ViewBag.Employe.Id, id_rdv);
                List<LigneFrais.TypeFrais> mesTypesFrais = new List<LigneFrais.TypeFrais>();
                List<LigneFrais.TypeForfait> mesTypesForfaits = new List<LigneFrais.TypeForfait>(); 
                foreach (LigneFrais.TypeFrais typeFrais in (LigneFrais.TypeFrais[])Enum.GetValues(typeof(LigneFrais.TypeFrais)))
                {
                    mesTypesFrais.Add(typeFrais);
                }
                foreach (LigneFrais.TypeForfait typeForfait in (LigneFrais.TypeForfait[])Enum.GetValues(typeof(LigneFrais.TypeForfait)))
                {
                    mesTypesForfaits.Add(typeForfait);
                }
            

                ViewBag.MesLignesFrais = mesLignesFrais;
                ViewBag.MesTypesFrais = mesTypesFrais;
                ViewBag.MesTypesForfaits = mesTypesForfaits;
                ViewBag.Id_rdv = id_rdv;
                ViewBag.Id_Fiche_frais = id_fiche_frais;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
   
}