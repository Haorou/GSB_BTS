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

            ViewBag.Employe = (Employe)Session["Employe"];

            List<RendezVous> mesRDV = rendezVousDAO.ReadAllFromCommercialID(ViewBag.Employe.Id);

            Debug.WriteLine(mesRDV);

            ViewBag.MesRDV = mesRDV;

            ViewData["Message"] = "Page de Consultation de vos Rendez-vous.";

            return View();
        }
    }
   
}