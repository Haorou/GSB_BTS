using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSB.Models
{
    public class Praticien : Personne
    {
        private string fonction;
        private string specialite;
        private DateTime date_derniere_entrevue;

        public string Fonction { get => fonction; set => fonction = value; }
        public string Specialite { get => specialite; set => specialite = value; }
        public DateTime Date_derniere_entrevue { get => date_derniere_entrevue; set => date_derniere_entrevue = value; }

        public Praticien() { }

        public Praticien(string fonction, string specialite, DateTime date_derniere_entrevue,
            int id, string nom, string prenom, string email, string telephone, Etablissement etablissement) :
            base(id, nom, prenom, email, telephone, etablissement)
        {
            this.Fonction = fonction;
            this.Specialite = specialite;
            this.Date_derniere_entrevue = date_derniere_entrevue;
        }

    }
}