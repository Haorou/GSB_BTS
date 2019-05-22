using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSB.Models
{
    public class TypeFrais
    {
        public enum Frais { Hébergement, Restauration, Transport }

        public Frais typeFraisCommercial { get => typeFraisCommercial; set => typeFraisCommercial = value; }

        public TypeFrais()
        {

        }

        public TypeFrais(Frais type)
        {
            this.typeFraisCommercial = type;
        }
    }
}