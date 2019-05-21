using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSB.Models
{
    public class FicheFrais
    {
        private int id_fiche_frais;
        private Employe commercial_visiteur;
        private Employe comptable;
        private DateTime date_fiche;
        private DateTime? date_modification;
        private List<LigneFrais> liste_lignes_frais;

        public int Id_fiche_frais { get => id_fiche_frais; set => id_fiche_frais = value; }
        public Employe Commercial_visiteur { get => commercial_visiteur; set => commercial_visiteur = value; }
        public Employe Comptable { get => comptable; set => comptable = value; }
        public DateTime Date_fiche { get => date_fiche; set => date_fiche = value; }
        public DateTime? Date_modification { get => date_modification; set => date_modification = value; } // Le "?" à côté du type signifie que le DateTime peut être "null"
        public List<LigneFrais> Liste_lignes_frais { get => liste_lignes_frais; set => liste_lignes_frais = value; }

        public FicheFrais() { }

        public FicheFrais(int Id_fiche_frais, Employe commercial_visiteur, Employe comptable, 
                          DateTime Date_fiche, DateTime Date_modification, List<LigneFrais> liste_lignes_frais)
        {
            this.Id_fiche_frais = Id_fiche_frais;
            this.Commercial_visiteur = commercial_visiteur;
            this.Comptable = comptable;
            this.Date_fiche = Date_fiche;
            this.Date_modification = Date_modification;
            this.Liste_lignes_frais = liste_lignes_frais;
        }


    }
}