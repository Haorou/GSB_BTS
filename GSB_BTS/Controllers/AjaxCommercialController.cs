using System;
using System.Collections.Generic;
using GSB.Models;
using GSB.Models.DAO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GSB.Controllers
{
    public class AjaxCommercialController : Controller
    {

        public string Read(string table, int id)
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

                //Debug.WriteLine("====================================" + mesLigneFrais.Frais);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                
                response = serializer.Serialize(mesLigneFrais);

                //Debug.WriteLine("================================================" + response);
            }

            return response;
        }

        public void AddModifyRDV(int? id, string date, string time, string motif, int indice, int id_employe, int id_praticien)
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

        public string AddModifyED(string nom, int concentration, int id_rdv, int quantite, string addOrModify)
        {
            string response = "";
            EchantillonDonneDAO echantillonDonneManager = new EchantillonDonneDAO();
            RendezVousDAO rendezVousManager = new RendezVousDAO();
            EchantillonDAO echantillonManager = new EchantillonDAO();
            ProduitDAO produitDAO = new ProduitDAO();

            int echantillonLu = echantillonManager.Read_IdEchantillon_FromNomConcentration(nom, concentration);

            EchantillonDonne echantillonDonne = new EchantillonDonne();
            echantillonDonne.RendezVous = rendezVousManager.Read(id_rdv, true);
            echantillonDonne.Echantillon = echantillonManager.Read(echantillonLu, true);
            echantillonDonne.Quantite = quantite;
            echantillonDonne.Produit = produitDAO.ReadFromNom(nom, true);

            if (addOrModify.Equals("add")) // ADD
            {
                if (echantillonDonneManager.Read(echantillonDonne.Echantillon.Id_echantillon,
                                                echantillonDonne.RendezVous.Id_rdv) == null)
                {
                    echantillonDonneManager.Create(echantillonDonne);
                    response = "Add done !";
                }
                else
                {
                    response = "Allready exist !";
                }

            }
            else // MODIFY
            {
                echantillonDonneManager.Update(echantillonDonne);
                response = "Modify done !";
            }
            return response;
        }

        public void ModifyED(int quantite)
        {
            EchantillonDonneDAO echantillonDonneManager = new EchantillonDonneDAO();
            EchantillonDonne echantillonDonne = new EchantillonDonne();
            echantillonDonne.Quantite = quantite;
            echantillonDonneManager.Update(echantillonDonne);
        }

        public string ProduitFromFamille(string famille)
        {
            string response = "";
            ProduitDAO produitManager = new ProduitDAO();
            List<Produit> mesProduits = produitManager.ReadNom(famille);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            response = serializer.Serialize(mesProduits);

            return response;
        }

        public void AddPraticienToEtablissement(string specialite, string fonction, string nom, string prenom, 
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

        public void AddEtablissement(string nom, string adresse)
        {
            Etablissement etablissement = new Etablissement();
            EtablissementDAO etablissementManager = new EtablissementDAO();

            etablissement.Nom = nom;
            etablissement.Adresse = adresse;
            etablissementManager.Create(etablissement);
        }

        public void AddModifyFF(int? id_ligne_frais, int id_fiche_frais, string type_frais, string type_forfait, int montant, string libelle, int id_rdv, int id_employe, DateTime? date_modif)
        {
            LigneFraisDAO ligneFraisManager = new LigneFraisDAO();
            FicheFraisDAO ficheFraisManager = new FicheFraisDAO();

            LigneFrais newLigneFrais = id_ligne_frais == null ? new LigneFrais() : ligneFraisManager.Read((int)id_ligne_frais, true);
            newLigneFrais.FicheFrais = ficheFraisManager.Read(id_fiche_frais, true);
            newLigneFrais.EtatLigne = LigneFrais.EtatLigneFrais.en_cours;
            newLigneFrais.Forfait = (LigneFrais.TypeForfait)Enum.Parse(typeof(LigneFrais.TypeForfait), type_forfait);
            newLigneFrais.Frais = (LigneFrais.TypeFrais)Enum.Parse(typeof(LigneFrais.TypeFrais), type_frais);
            newLigneFrais.Montant = montant;
            newLigneFrais.Libelle = libelle;
            newLigneFrais.Date_modification = date_modif;

            if (id_ligne_frais == null) // ADD
            {
                ligneFraisManager.Create(id_fiche_frais, newLigneFrais);
            }
            else // MODIFY
            {
                ligneFraisManager.Update(newLigneFrais, (DateTime)date_modif);
                //Debug.WriteLine("=============================================" + newLigneFrais.Date_modification);
            }
        }

        public void Delete(string table, int id)
        {
            if (table.Equals("rendez_vous"))
            {
                RendezVousDAO rendezVousManager = new RendezVousDAO();
                rendezVousManager.Delete(rendezVousManager.Read(id, false));
            }
            else if (table.Equals("ligne_frais"))
            {
                LigneFraisDAO ligneFraisDAO = new LigneFraisDAO();
                ligneFraisDAO.Delete(id);
            }
            else if (table.Equals("praticien"))
            {
                PraticienDAO praticienManager = new PraticienDAO();
                praticienManager.Delete(id);
            }
        }

        public string ReadDoubleID(string table, int id1, int id2)
        {
            string response = "";
            if (table.Equals("echantillon_donne"))
            {
                EchantillonDonneDAO echantillonDonneManager = new EchantillonDonneDAO();
                EchantillonDonne echantillonDonne = echantillonDonneManager.Read(id1, id2);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                if (echantillonDonne == null)
                {
                    response = "null";
                }
                else
                {
                    response = serializer.Serialize(echantillonDonne);
                }
            }

            return response;
        }

        public void DeleteDoubleID(string table, int id1, int id2)
        {
            if (table.Equals("echantillon_donne"))
            {
                EchantillonDonneDAO echantillonDonneManager = new EchantillonDonneDAO();
                echantillonDonneManager.Delete(id1, id2);
            }
        }

    }
   
}