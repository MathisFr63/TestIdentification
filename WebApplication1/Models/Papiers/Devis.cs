namespace WebApplication1.Models.Papiers
{
    /// <summary>
    /// Classe permettant de représenter les données d'un devis
    /// </summary>
    public class Devis : Document
    {
        // Booléen permettant de désigner si le devis est encore valide selon la durée écoulée
        public bool Valide { get; set; }

        //public int Total { get; set; } Prix calculé
    }
}