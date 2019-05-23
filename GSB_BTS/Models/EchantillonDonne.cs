using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSB.Models
{
    public class EchantillonDonne
    {
        private int quantite;
        private Echantillon echantillon;
        private RendezVous rendez_vous;

        public int Quantite { get => quantite; set => quantite = value; }
        public Echantillon Echantillon { get => echantillon; set => echantillon = value; }
        public RendezVous RendezVous { get => rendez_vous; set => rendez_vous = value; }

        public EchantillonDonne(int quantite, Echantillon echantillon, RendezVous rendez_vous)
        {
            this.quantite = quantite;
            this.Echantillon = echantillon;
            this.RendezVous = rendez_vous;
        }

        public EchantillonDonne() { }

    }

}