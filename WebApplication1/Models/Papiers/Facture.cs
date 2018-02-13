namespace WebApplication1.Models.Papiers
{
    public class Facture : Donnee
    {
        public int Relances { get; set; }

        public TypeReglement Reglement { get; set; }

        public Facture(){}
        public Facture(Devis devis)
        {
            Objet = devis.Objet;
            Date = devis.Date;
            Commentaire = devis.Commentaire;
            Monnaie = devis.Monnaie;
            Produits = devis.Produits;
            UtilisateurID = devis.UtilisateurID;

            Relances = 0;
        }
        public Facture(Devis devis, TypeReglement type) : this(devis)
        {
            Reglement = type;
        }
    }
}