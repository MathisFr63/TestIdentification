using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;

namespace WebApplication1.Models.Papiers
{
    /// <summary>
    /// Classe représentant les données d'un produit
    /// </summary>
    public class Produit
    {
        // Identifiant représentant le produit dans la base de données.
        [Display(Name = "Identifiant")]
        public int ID { get; private set; }

        // Nom du produit
        [Display(Name = "Libellé")]
        public string Libelle { get; set; }

        // Commentaire lié au produit afin d'ajouter une description supplémentaire
        public string Détails { get; set; }

        // Prix hors taxe du produit
        [Display(Name = "Prix HT")]
        public double PrixHT { get; set; }

        // Réduction à ajouter au prix du produit
        [Display(Name = "Réduction")]
        public double Reduction { get; set; }

        // Montant de TVA à ajouter au prix du produit
        [Display(Name = "TVA")]
        public double TVA { get; set; }

        // Type de service lié au produit
        [Display(Name = "Type")]
        public TypeService Type { get; set; }

        // Identifiant de l'utilisateur auquel appartient le produit.
        //public string UtilisateurID { get; set; }

        // Prix final du produit sans la TVA
        //public int Prixfinal { get; set; } Prix calculé

        //Prix toutes taxes comprises du produit
        [Display(Name = "Prix TTC")]
        public double TotalTTC
        {
            get
            {
                var total = PrixHT + (PrixHT * TVA / 100);
                return total - (total * Reduction / 100);
            }
        }

        // Image du produit qui est pour l'instant en dur.
        public WebImage image = new WebImage("~/Images/test.png");


        /// <summary>
        /// Méthode permettant de retranscrire les données du produit sous forme de chaîne de caractères.
        /// </summary>
        /// <returns>string contenant les informations du produit</returns>
        public override string ToString()
        {
            return Libelle;
        }
    }
}