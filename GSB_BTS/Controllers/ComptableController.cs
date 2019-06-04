using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GSB.Models;
using GSB.Models.DAO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GSB.Controllers
{
    public class ComptableController : Controller
    {

        public ActionResult Comptable()
        {
            ViewBag.Employe = (Employe)Session["Employe"];

            if (ViewBag.Employe != null)
            {
                FicheFraisDAO ficheFraisManager = new FicheFraisDAO();
                LigneFraisDAO ligneFraisDAO = new LigneFraisDAO();
                List<FicheFrais> fichesFrais = ficheFraisManager.ReadAll();

                ViewBag.FichesFrais = fichesFrais;

                ViewData["Message"] = "Page de Consultation des fiches de frais.";

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
            if(table.Equals("fiche_frais"))
            {
                FicheFraisDAO ficheFraisManager = new FicheFraisDAO();
                FicheFrais ficheFrais = ficheFraisManager.Read(id, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                response = serializer.Serialize(ficheFrais);
            }

            return response;
        }
    }
}