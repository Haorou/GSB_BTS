
namespace GSB.Models
{
    public class Personne
    {
        private int id;
        private string nom;
        private string email;
        private string telephone;
        private Etablissement etablissement;

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get; set; }
        public string Email { get => email; set => email = value; }
        public string Telephone { get => telephone; set => telephone = value; }
        public Etablissement Etablissement { get => etablissement; set => etablissement = value; }

        public Personne() { }

        public Personne(int id, string nom, string prenom, string email, string telephone, Etablissement etablissement)
        {
            this.Id = id;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Email = email;
            this.Telephone = telephone;
            this.Etablissement = etablissement;
        }
    }




}