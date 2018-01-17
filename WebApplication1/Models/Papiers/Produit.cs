namespace WebApplication1.Models.Papiers
{
    public class Produit
    {
        public int ID { get; private set; }

        public string Nom { get; set; }

        public string Commentaire { get; set; }

        public int PrixHT { get; set; }

        public int Reduction { get; set; }

        public int TVA { get; set; }

        public TypeService Type { get; set; }

        //public int Prixfinal { get; set; } Prix calculé

        //public int TotalTTC { get; set; } Prix calculé

        public override string ToString()
        {
            return Nom;
        }
    }
}