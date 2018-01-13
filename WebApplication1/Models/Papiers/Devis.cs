namespace WebApplication1.Models.Papiers
{
    public class Devis : Donnee
    {
        public int ID { get; private set; }

        public bool Valide { get; set; }

        // public int Total { get; set; } Prix calculé
    }
}