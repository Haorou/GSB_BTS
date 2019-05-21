using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSB.Models
{
    public class Etablissement
    {
        private int id;
        private string nom;
        private string adresse;

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Adresse { get => adresse; set => adresse = value; }

        public Etablissement() { }

        public Etablissement(int id, string nom, string adresse)
        {
            this.id = id;
            this.nom = nom;
            this.adresse = adresse;
        }
    }
}