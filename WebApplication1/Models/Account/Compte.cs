using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Account
{
    public class Compte : AdresseMail
    {
        [Display(Name = "Mot de passe")]
        public int MotDePasse { get; set; }

        public int UtilisateurID { get; set; }

        public Compte(string mail, string motDePasse, int userID) : base(mail)
        {
            this.MotDePasse = motDePasse.GetHashCode();
            this.UtilisateurID = userID;
        }
    }
}