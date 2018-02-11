namespace WebApplication1.Models.Papiers
{
    public class DonneeProduit
    {
        public int ID { get; private set; }

        public int? DevisID { get; set; }

        public int? FactureID { get; set; }

        public int Quantite { get; set; }

        public string Nom { get; set; }

        public string Commentaire { get; set; }

        public int PrixHT { get; set; }

        public int Reduction { get; set; }

        public int TVA { get; set; }

        public TypeService Type { get; set; }


        public DonneeProduit()
        {
        }

        public DonneeProduit(Produit produit) : this()
        {
            Nom = produit.Nom;
            Commentaire = produit.Commentaire;
            PrixHT = produit.PrixHT;
            Reduction = produit.Reduction;
            TVA = produit.TVA;
            Type = produit.Type;
        }

        public DonneeProduit(Produit produit, int id, int quantite) : this(produit)
        {
            DevisID = id;
            Quantite = quantite;
        }
    }
}