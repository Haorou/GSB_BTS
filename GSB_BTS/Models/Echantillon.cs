using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSB.Models
{
    public class Echantillon
    {
        private Produit produit;
        private List<EchantillonDonne> liste_echantillons_donnes;
        private int id_echantillon;
        private int quantite;
        private int concentration;
        private string libelle;

        public Produit Produit { get=> produit; set => produit = value; }
        public List<EchantillonDonne> Liste_echantillons_donnes { get => liste_echantillons_donnes; set => liste_echantillons_donnes = value; }
        public int Id_echantillon { get => id_echantillon; set => id_echantillon = value; }
        public int Quantite { get => quantite; set => quantite = value; }
        public int Concentration { get => concentration; set => concentration = value; }
        public string Libelle { get => libelle; set => libelle = value; }

        public Echantillon() { }

        public Echantillon(Produit produit, List<EchantillonDonne> liste_echantillons_donnes, int Id_echantillon, int quantite, int concentration, string libelle)
        {
            this.produit = produit;
            this.Liste_echantillons_donnes = liste_echantillons_donnes;
            this.id_echantillon = Id_echantillon;
            this.quantite = quantite;
            this.concentration = concentration;
            this.libelle = libelle;
        }
    }
}