using System;
using System.Collections.Generic;
using GSB.Models;
using GSB.Models.DAO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GSB.Controllers
{
    public class AjaxComptableController : Controller
    {
        public string Read(string table, int id)
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