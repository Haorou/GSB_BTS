﻿using System;
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
            Employe employe = (Employe)Session["Employe"];

            ProduitDAO produitManager = new ProduitDAO();
            EchantillonDonneDAO echantillonDAO = new EchantillonDonneDAO();
            RendezVousDAO rendezVousManager = new RendezVousDAO();

            RendezVous rendezVous = rendezVousManager.Read(id_rdv, false);
            List<EchantillonDonne> mesEchantillons = echantillonDAO.ReadAllFromRendezVous(id_rdv);
            List<Produit> mesFamilles = produitManager.ReadFamille();


            ViewBag.Famille = mesFamilles;
            ViewBag.Echantillon = mesEchantillons;
            ViewBag.RendezVous = rendezVous;
            ViewBag.Employe = (Employe)Session["Employe"]; 

            return View();
        }

        public ActionResult Produit(string famille)
        {
            Employe employe = (Employe)Session["Employe"];
            ProduitDAO produitManager = new ProduitDAO();
            List<Produit> listeProduit = new List<Produit>();
            List<Produit> mesFamilles = produitManager.ReadFamille();
            List<Produit> mesNoms = produitManager.ReadNom(famille);

            ViewBag.Famille = mesFamilles;
            ViewBag.Nom = mesNoms;
            ViewBag.Employe = (Employe)Session["Employe"];
            Debug.WriteLine("==================================="+mesFamilles);
            return View();
        }

        public ActionResult FicheFrais(int id_rdv)
        {

            Employe employe = (Employe)Session["Employe"];

            PersonneDAO personneDAO = new PersonneDAO();
            LigneFraisDAO ligneFraisDAO = new LigneFraisDAO();

            //Debug.WriteLine("===============> ID RDV : " + id_rdv);

            List<LigneFrais> mesLignesFrais = ligneFraisDAO.ReadAllFromID(employe.Id, id_rdv);
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
            ViewBag.Employe = (Employe)Session["Employe"];
            ViewBag.Id_rdv = id_rdv;

            //Debug.WriteLine("==================================="+employe.Id);
            //Debug.WriteLine("==================================="+mesLignesFrais[0].Date_engagement + " === count === " + mesLignesFrais.Count);
            //Debug.WriteLine("==================================="+ mesTypesFrais[0].GetType() + " === count === " + mesLignesFrais.Count);

            return View();
        }

        public string AjaxReader(string table, int id)
        {
            string response = "";
            if (table.Equals("rendez_vous"))
            {
                RendezVousDAO rendezVousManager = new RendezVousDAO();
                RendezVous rendezVous = rendezVousManager.Read(id, false);
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
                LigneFraisDAO ligneFraisDAO = new LigneFraisDAO();
                LigneFrais mesLigneFrais = ligneFraisDAO.Read(id, true);

                Debug.WriteLine("====================================" + mesLigneFrais.Frais);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                
                response = serializer.Serialize(mesLigneFrais);

                Debug.WriteLine("================================================" + response);
            }

            return response;
        }

        public void AjaxAddModifyRDV(int? id, string date, string time, string motif, int indice, int id_employe, int id_praticien)
        {
            RendezVousDAO rendezVousManager = new RendezVousDAO();
            PraticienDAO praticienManager = new PraticienDAO();
            EmployeDAO employeManager = new EmployeDAO();

            RendezVous newRDV = id == null ? new RendezVous() : rendezVousManager.Read((int)id,false);

            //Debug.WriteLine("Debug.Time = > " + time);

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

        public void AjaxAddModifyED(string nom, int concentration, int id_rdv, int quantite)
        {
            EchantillonDonneDAO echantillonDonneManager = new EchantillonDonneDAO();
            RendezVousDAO rendezVousManager = new RendezVousDAO();
            EchantillonDAO echantillonManager = new EchantillonDAO();
            Echantillon echantillonLu = echantillonManager.ReadNomConcentration(nom, concentration, false);
            EchantillonDonne echantillonDonne = new EchantillonDonne();

            echantillonDonne.Echantillon = echantillonLu;
            echantillonDonne.RendezVous = rendezVousManager.Read(id_rdv, false);
            echantillonDonne.Quantite = quantite;

            if (echantillonDonne == null) // ADD
            {
                echantillonDonneManager.Create(echantillonDonne);
            }
            else // MODIFY
            {
                echantillonDonneManager.Update(echantillonDonne);
            }
        }

        public string AjaxProduitFromFamille(string famille)
        {
            string response = "";
            ProduitDAO produitManager = new ProduitDAO();
            List<Produit> mesProduits = produitManager.ReadNom(famille);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            response = serializer.Serialize(mesProduits);

            return response;
        }

        public void AjaxAddPraticienToEtablissement(string specialite, string fonction, string nom, string prenom, 
                                                    string telephone, string email, int id_etablissement)
        {
            PraticienDAO praticienManager = new PraticienDAO();
            EtablissementDAO etablissementManager = new EtablissementDAO();

            Praticien praticien = new Praticien();
            praticien.Specialite = specialite;
            praticien.Fonction = fonction;
            praticien.Nom = nom;
            praticien.Prenom = prenom;
            praticien.Telephone = telephone;
            praticien.Email = email;
            praticien.Etablissement = etablissementManager.Read(id_etablissement);

            praticienManager.Create(praticien);
        }

        public void AjaxAddEtablissement(string nom, string adresse)
        {
            Etablissement etablissement = new Etablissement();
            EtablissementDAO etablissementManager = new EtablissementDAO();

            etablissement.Nom = nom;
            etablissement.Adresse = adresse;
            etablissementManager.Create(etablissement);
        }

        public void AjaxAddModifyFF(int? id_ligne_frais, int id_fiche_frais,  DateTime date_creation, string type_frais, string type_forfait, int montant, string libelle, int id_rdv, int id_employe)
        {
            LigneFraisDAO ligneFraisManager = new LigneFraisDAO();
            FicheFraisDAO ficheFraisManager = new FicheFraisDAO();

            LigneFrais newLigneFrais = id_ligne_frais == null ? new LigneFrais() : ligneFraisManager.Read((int)id_ligne_frais, false);

            newLigneFrais.EtatLigne = LigneFrais.EtatLigneFrais.en_cours;
            newLigneFrais.Forfait = (LigneFrais.TypeForfait)Enum.Parse(typeof(LigneFrais.TypeForfait), type_forfait);
            newLigneFrais.Frais = (LigneFrais.TypeFrais)Enum.Parse(typeof(LigneFrais.TypeFrais), type_frais);
            newLigneFrais.Montant = montant;
            newLigneFrais.Libelle = libelle;
            date_creation = DateTime.Now;
            newLigneFrais.FicheFrais.Id_fiche_frais = id_fiche_frais;

            

            if (id_ligne_frais == null) // ADD
            {
                ligneFraisManager.Create(id_employe, newLigneFrais, id_rdv);
            }
            else // MODIFY
            {
                ligneFraisManager.Update(newLigneFrais);
            }
        }

        public void AjaxDelete(string table, int id)
        {
            if(table.Equals("rendez_vous"))
            {
                RendezVousDAO rendezVousManager = new RendezVousDAO();
                rendezVousManager.Delete(rendezVousManager.Read(id, false));
            }
            else if(table.Equals("ligne_frais"))
            {
                LigneFraisDAO ligneFraisDAO = new LigneFraisDAO();
                ligneFraisDAO.Delete(id);
            }
            else if(table.Equals("praticien"))
            {
                PraticienDAO praticienManager = new PraticienDAO();
                praticienManager.Delete(id);
            }
        }

        public void AjaxDeleteDoubleID(string table, int id1, int id2)
        {
            if (table.Equals("echantillon_donne"))
            {
                EchantillonDonneDAO echantillonDonneManager = new EchantillonDonneDAO();
                echantillonDonneManager.Delete(id1, id2);
            }
        }


    }
   
}