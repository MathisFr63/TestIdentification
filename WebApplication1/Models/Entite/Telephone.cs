namespace WebApplication1.Models.Entite
{
    /// <summary>
    /// Classe représentant un téléphone
    /// </summary>
    public class Telephone
    {
        // Identifiant permettant d'identifier le téléphone dans la base de données.
        public int ID { get; private set; }

        // Type de téléphone
        public TypeTelephone Type { get; set; }

        // Numéro de téléphone
        public string Numéro { get; set; }

        // Préfixe du téléphone permettant de le joindre à l'étranger
        public string Préfixe { get; set; }
    }
}