using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Account
{
    public class Compte
    {
        public string ID { get; set; }

        [Display(Name = "Mot de passe")]
        public int MotDePasse { get; set; }

        public int UtilisateurID { get; set; }

        public Compte(string mail, string motDePasse, int userID)
        {
            this.ID = mail;
            this.MotDePasse = motDePasse.GetHashCode();
            this.UtilisateurID = userID;
        }

        public Compte() { }
    }
}