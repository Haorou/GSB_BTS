using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GSB.Models;
using GSB.Models.DAO;
using System.Web.Mvc;

namespace GSB.Controllers
{
    public class ComptableController : Controller
    {

        public ActionResult Comptable()
        {
            FicheFraisDAO ficheFraisManager = new FicheFraisDAO();
            List<FicheFrais> fichesFrais = ficheFraisManager.ReadAll();
            ViewBag.FichesFrais = fichesFrais;

            ViewData["Message"] = "Page de Consultation des fiches de frais.";

            return View();
        }
    }
}