using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSB.Models
{
    public class Produit
    {
        private int id_produit;
        private string pathologie;
        private string famille;
        private string nom;
        private string notice;
        private string libelle;
        private List<Echantillon> echantillons;

        public int Id_produit { get => id_produit; set => id_produit = value; }
        public string Pathologie { get => pathologie; set => pathologie = value; }
        public string Famille { get => famille; set => famille = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Notice { get => notice; set => notice = value; }
        public string Libelle { get => libelle; set => libelle = value; }
        public List<Echantillon> Echantillons { get => echantillons; set => echantillons = value; }

        public Produit()
        {

        }
        public Produit(int id_produit, string pathologie, string famille, string nom, string notice, string libelle, List<Echantillon> echantillons)
        {
            this.id_produit = id_produit;
            this.pathologie = pathologie;
            this.famille = famille;
            this.nom = nom;
            this.notice = notice;
            this.libelle = libelle;
            this.echantillons = Echantillons;
        }
    }
}