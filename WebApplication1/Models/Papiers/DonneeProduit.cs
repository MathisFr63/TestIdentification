namespace WebApplication1.Models.Papiers
{
    public class DonneeProduit
    {
        public int ID { get; private set; }
        public int DonneeID { get; set; }
        public int ProduitID { get; set; }
        public int Quantite { get; set; }
    }
}