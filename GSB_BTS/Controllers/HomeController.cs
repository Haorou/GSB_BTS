using System.Diagnostics;
using GSB.Models;
using GSB.Models.DAO;
using System.Web.Mvc;

namespace GSB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Veuillez vous authentifier";

            return View();
        }

        public ActionResult Connexion(string login, string password)
        {
            EmployeDAO employeManager = new EmployeDAO();

            Employe employe = employeManager.Connexion(login, password);
            Session["Employe"] = employe;
            ViewBag.Employe = employe;

            if (employe != null)
            {
                if (employe.Type == Employe.TypeEmploye.comptable)
                {
                    return RedirectToAction("Comptable", "Comptable");
                }
                else
                {
                    return RedirectToAction("ConsultationRDV", "Commercial");
                }
            }
            else
            {
                ViewBag.Message = "Accès refusé, veuillez vous authentifier";
                return View("Index");
            }
        }

    }
}
