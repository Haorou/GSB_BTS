using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSB.Models
{
    public class Employe : Personne
    {
        public enum TypeEmploye { comptable, commercial_visiteur, commercial_delegue, commercial_responsable }

        private string login;
        private string mot_de_passe;
        private TypeEmploye type_employe;
        public string Login { get => login; set => login = value; }
        public string Mot_de_passe { get => mot_de_passe; set => mot_de_passe = value; }
        public TypeEmploye Type { get => type_employe; set => type_employe = value; }

        public Employe() { }

        public Employe(int id, string nom, string prenom, string adresse, string email,
            string telephone, string etablissement, string login, string mot_de_passe, TypeEmploye type) :
            base(id, nom, prenom, adresse, email, telephone, etablissement)
        {
            this.Login = login;
            this.Mot_de_passe = mot_de_passe;
            this.Type = type;


        }
    }
}