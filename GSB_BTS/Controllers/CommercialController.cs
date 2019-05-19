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

        public string AjaxReceiver(string table, int id)
        {
            string response = "";
            if (table.Equals("rendez_vous"))
            {
                RendezVousDAO rendezVousManager = new RendezVousDAO();
                RendezVous rendezVous = rendezVousManager.Read(id);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Serialize(rendezVous);
                //Debug.WriteLine("=============>" + response);
            }

            return response;
        }

        public ActionResult Etablissement()
        {
            PraticienDAO praticienDAO = new PraticienDAO();
            PersonneDAO etablissementDAO = new PersonneDAO();

            List<Personne> mesEtablissement = etablissementDAO.ReadEtablissement();

            ViewBag.MesEtablissement = mesEtablissement;

            return View("Etablissement");
        }
    }
   
}