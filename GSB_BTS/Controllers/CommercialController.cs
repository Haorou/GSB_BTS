using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GSB.Models;
using GSB.Models.DAO;
using System.Web.Mvc;

namespace GSB.Controllers
{
    public class CommercialController : Controller
    {
        public ActionResult ConsultationRDV()
        {
            RendezVousDAO rendezVousDAO = new RendezVousDAO();
            PraticienDAO praticienDAO = new PraticienDAO();

            ViewBag.Employe = (Employe)Session["Employe"];
            ViewBag.Praticien = (Praticien)Session["Praticien"];

            List<RendezVous> mesRDV = rendezVousDAO.ReadAllFromCommercialID(ViewBag.Employe.Id);
            List<Praticien> mesPraticiens = praticienDAO.ReadAll();

            Debug.WriteLine("***************************************************");
            Debug.WriteLine(mesRDV);
            Debug.WriteLine(mesPraticiens);

            ViewBag.MesRDV = mesRDV;
            ViewBag.MesPraticiens = mesPraticiens;

            ViewData["Message"] = "Page de Consultation de vos Rendez-vous.";

            return View();
        }
    }
   
}