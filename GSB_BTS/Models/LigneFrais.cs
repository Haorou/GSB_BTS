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
        private DateTime date_engagement;
        private string libelle;
        private string justificatif;
        private EtatLigneFrais etatLigne;
        private TypeForfait forfait;
        private TypeFrais frais;

        public int Id { get => id; set => id = value; }
        public FicheFrais FicheFrais { get => ficheFrais; set => ficheFrais = value; }
        public DateTime Date_engagement { get => date_engagement; set => date_engagement = value; }
        public string Libelle { get => libelle; set => libelle = value; }
        public string Justificatif { get => justificatif; set => justificatif = value; }
        public EtatLigneFrais EtatLigne { get => etatLigne; set => etatLigne = value; }
        public TypeForfait Forfait { get => forfait; set => forfait = value; }
        public TypeFrais Frais { get => frais; set => frais = value; }

        public LigneFrais() { }
        public LigneFrais(int id, FicheFrais ficheFrais, DateTime Date_engagement, string Libelle, string Justificatif,
                          EtatLigneFrais etatLigneFrais, TypeForfait typeForfait, TypeFrais frais)
        {
            this.id = id;
            this.ficheFrais = ficheFrais;
            this.Date_engagement = Date_engagement;
            this.Libelle = Libelle;
            this.Justificatif = Justificatif;
            this.EtatLigne = etatLigneFrais;
            this.Forfait = typeForfait;
            this.Frais = frais;
        }
    }
}