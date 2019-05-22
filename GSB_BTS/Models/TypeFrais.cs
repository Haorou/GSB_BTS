using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSB.Models
{
    public class TypeFrais
    {
        public enum Frais { Hébergement, Restauration, Transport }

        private Frais typeFraisCommercial;

        public Frais TypeFraisCommercial { get => typeFraisCommercial; set => typeFraisCommercial = value; }

        public TypeFrais()
        {

        }

        public TypeFrais(Frais typeFraisCommercial)
        {
            this.typeFraisCommercial = typeFraisCommercial;
        }
    }
}