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
            /*PraticienDAO PersonneManager = new PraticienDAO();
            string personneText = PersonneManager.Read(1);
            Debug.WriteLine("dzeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee  => " + personneText);
            ViewBag.text = personneText;*/

            /*PraticienDAO monDoc = new PraticienDAO();

            Praticien praticien = new Praticien();
            praticien.Fonction = "Generaliste";
            praticien.Specialite = "Cancer";
            praticien.Date_derniere_entrevue = DateTime.Now;

            monDoc.Create(praticien);*/

            /*PersonneDAO personneDAO = new PersonneDAO();

            List<Personne> mesPersonnes = personneDAO.ReadAll();
           
            ViewBag.MesPersonnes = mesPersonnes;*/

            ViewData["Message"] = "Test de push";

            Debug.WriteLine("=========================================================================");
            Debug.WriteLine("=========================================================================");

            return View();
        }

        public ActionResult Connexion(string login, string password)
        {
            EmployeDAO employeManager = new EmployeDAO();

            Employe employe = employeManager.Connexion(login, password);

            if(employe != null)
            {
                Session["Employe"] =  employe;
                

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
                ViewBag.Message = "Accès refusé, veuillez vous connecter";
                return View("Index");
            }
        }

    }
}
