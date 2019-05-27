using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSB.Models
{
    public class LigneFrais
    {
        public enum EtatLigneFrais { en_cours, refuse, mise_en_paiement }
        public enum TypeForfait { forfait, hors_forfait }
        public enum TypeFrais { hebergement, restauration, deplacement, autre }

        private int id;
        private FicheFrais ficheFrais;
        private DateTime? date_engagement;
        private DateTime? date_modification;
        private string libelle;
        private EtatLigneFrais etatLigne;
        private TypeForfait forfait;
        private TypeFrais frais;
        private int montant;

        public int Id { get => id; set => id = value; }
        public FicheFrais FicheFrais { get => ficheFrais; set => ficheFrais = value; }
        public DateTime? Date_engagement { get => date_engagement; set => date_engagement = value; }
        public DateTime? Date_modification { get => date_modification; set => date_modification = value; }
        public string Libelle { get => libelle; set => libelle = value; }
        public EtatLigneFrais EtatLigne { get => etatLigne; set => etatLigne = value; }
        public TypeForfait Forfait { get => forfait; set => forfait = value; }
        public TypeFrais Frais { get => frais; set => frais = value; }
        public int Montant { get => montant; set => montant = value; }

        public LigneFrais() { }
        public LigneFrais(int id, FicheFrais ficheFrais, DateTime Date_modification, DateTime Date_engagement, string Libelle,
                          EtatLigneFrais etatLigneFrais, TypeForfait typeForfait, TypeFrais frais, int montant)
        {
            this.id = id;
            this.ficheFrais = ficheFrais;
            this.Date_modification = Date_modification;
            this.Date_engagement = Date_engagement;
            this.Libelle = Libelle;
            this.EtatLigne = etatLigneFrais;
            this.Forfait = typeForfait;
            this.Frais = frais;
            this.Montant = montant;
        }
    }
}