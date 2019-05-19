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
                List<RendezVous> mesRDV = rendezVousDAO.ReadAllFromCommercialID(ViewBag.Employe.Id);
                List<Praticien> mesPraticiens = praticienDAO.ReadAll();

                ViewBag.MesRDV = mesRDV;
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
            PraticienDAO praticienDAO = new PraticienDAO();
            PersonneDAO etablissementDAO = new PersonneDAO();

            List<Personne> mesEtablissement = etablissementDAO.ReadEtablissement();

            ViewBag.MesEtablissement = mesEtablissement;

            return View();
        }

        public string AjaxReceiver(string table, int id)
        {
            string response = "";
            if (table.Equals("rendez_vous"))
            {
                RendezVousDAO rendezVousManager = new RendezVousDAO();
                RendezVous rendezVous = rendezVousManager.Read(id);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Serialize(rendezVous);
            }

            return response;
        }

        public string AjaxRDV(int id, string date, string time, string motif, int indice, int id_employe, int id_praticien)
        {
            Debug.WriteLine("id = " + id);
            if (id == 0)
            {
                RendezVousDAO rendezVousManager = new RendezVousDAO();
                PraticienDAO praticienManager = new PraticienDAO();
                EmployeDAO employeManager = new EmployeDAO();

                RendezVous newRDV = new RendezVous();
                newRDV.Date_rdv = new DateTime(Convert.ToInt32(date.Substring(0, 4)),
                                               Convert.ToInt32(date.Substring(5, 2)),
                                               Convert.ToInt32(date.Substring(8)), 
                                               Convert.ToInt32(time.Substring(0,2)),
                                               Convert.ToInt32(time.Substring(3)),
                                               00);
                newRDV.Date_bilan = DateTime.Now;
                newRDV.Indice_confiance = indice;
                newRDV.Motif_rdv = (RendezVous.Rdv)Enum.Parse(typeof(RendezVous.Rdv), motif);
                newRDV.Praticien = praticienManager.Read(id_praticien);
                newRDV.Employe = employeManager.Read(id_employe);

                rendezVousManager.Create(newRDV);
            }
            else
            {

            }

            return "success";
        }


    }
   
}