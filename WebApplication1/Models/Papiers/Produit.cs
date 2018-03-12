namespace WebApplication1.Models.Papiers
{
    /// <summary>
    /// Classe représentant les données d'un produit
    /// </summary>
    public class Produit
    {
        // Identifiant représentant le produit dans la base de données.
        public int ID { get; private set; }

        // Nom du produit
        public string Libelle { get; set; }

        // Commentaire lié au produit afin d'ajouter une description supplémentaire
        public string Commentaire { get; set; }

        // Prix hors taxe du produit
        public double PrixHT { get; set; }

        // Réduction à ajouter au prix du produit
        public double Reduction { get; set; }

        // Montant de TVA à ajouter au prix du produit
        public double TVA { get; set; }

        // Type de service lié au produit
        public TypeService Type { get; set; }

        // Identifiant de l'utilisateur auquel appartient le produit.
        //public string UtilisateurID { get; set; }

        // Prix final du produit sans la TVA
        //public int Prixfinal { get; set; } Prix calculé

        //Prix toutes taxes comprises du produit
        //public int TotalTTC { get; set; } Prix calculé

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