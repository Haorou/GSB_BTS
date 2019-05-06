using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSB.Models
{
    public class RendezVous
    {
        public enum Rdv { reprise_contact, nouveaux_medicaments, suivi }
            
        private int id_rdv;
        private Employe employe;
        private Praticien praticien;
        private DateTime date_rdv;
        private DateTime date_bilan;
        private int indice_confiance;
        private Rdv motif_rdv;
        private List<EchantillonDonne> liste_echantillons_donnes;

        public int Id_rdv { get => id_rdv; set => id_rdv = value; }
        public Employe Employe { get => employe; set => employe = value; }
        public Praticien Praticien { get => praticien; set => praticien = value; }
        public DateTime Date_rdv { get => date_rdv; set => date_rdv = value; }
        public DateTime Date_bilan { get => date_bilan; set => date_bilan = value; }
        public int Indice_confiance { get => indice_confiance; set => indice_confiance = value; }
        public Rdv Motif_rdv { get => motif_rdv; set => motif_rdv = value; }
        public List<EchantillonDonne> Liste_echantillons_donnes { get => liste_echantillons_donnes; set => liste_echantillons_donnes = value; }

        public RendezVous() { }

        public RendezVous(int id_rdv, Employe employe, Praticien praticien, DateTime date_rdv, DateTime date_bilan,Rdv motif_rdv, int indice_confiance, List<EchantillonDonne> liste_echantillons_donnes)
        {
            this.id_rdv = id_rdv;
            this.Employe = employe;
            this.Praticien = praticien;
            this.Date_rdv = date_rdv;
            this.Date_bilan = date_bilan;
            this.motif_rdv = motif_rdv;
            this.Indice_confiance = indice_confiance;
            this.Liste_echantillons_donnes = liste_echantillons_donnes;
        }





    }
}