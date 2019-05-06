
namespace GSB.Models
{
    public class Personne
    {
        private int id;
        private string nom;
        private string email;
        private string telephone;
        private string etablissement;

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Email { get => email; set => email = value; }
        public string Telephone { get => telephone; set => telephone = value; }
        public string Etablissement { get => etablissement; set => etablissement = value; }

        public Personne() { }

        public Personne(int id, string nom, string prenom, string adresse, string email, string telephone, string etablissement)
        {
            this.Id = id;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Adresse = adresse;
            this.Email = email;
            this.Telephone = telephone;
            this.Etablissement = etablissement;
        }
    }




}