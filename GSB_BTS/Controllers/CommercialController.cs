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
            PraticienDAO praticienManager = new PraticienDAO();

            List<Praticien> mesEtablissement = praticienManager.ReadEtablissements();

            ViewBag.MesEtablissement = mesEtablissement;

            return View();
        }

        public string AjaxReader(string table, int id)
        {
            string response = "";
            if (table.Equals("rendez_vous"))
            {
                RendezVousDAO rendezVousManager = new RendezVousDAO();
                RendezVous rendezVous = rendezVousManager.Read(id);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Serialize(rendezVous);
            }
            else if(table.Equals("etablissement"))
            {
                PraticienDAO praticienManager = new PraticienDAO();
                Praticien Etablissement = praticienManager.Read(id);
                List<Praticien> PraticiensInEtablissement = praticienManager.ReadAllPraticiensInEtablissement(Etablissement.Etablissement);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Serialize(PraticiensInEtablissement);
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