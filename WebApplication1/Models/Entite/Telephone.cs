namespace WebApplication1.Models.Entite
{
    /// <summary>
    /// Classe représentant un téléphone
    /// </summary>
    public class Telephone
    {
        // Identifiant permettant d'identifier le téléphone dans la base de données.
        public int ID { get; private set; }

        public string UtilisateurID { get; set; }

        // Type de téléphone
        public TypeTelephone Type { get; set; }

        // Numéro de téléphone
        private string numéro;

        public string Numéro
        {
            get { return numéro; }
            set
            {
                if (value[0].Equals("0"))
                    numéro = value.Remove(0, 1);
                else
                    numéro = value;
            }
        }

        // Préfixe du téléphone permettant de le joindre à l'étranger
        public string Préfixe { get; set; }


        /// <summary>
        /// Constructeur par défaut d'un Téléphone
        /// </summary>
        public Telephone()
        {
        }

        /// <summary>
        /// Constructeur d'un Téléphone prenant en paramètre un numéro, un préfixe et un type
        /// </summary>
        /// <param name="numero">Numéro de téléphone</param>
        /// <param name="prefixe">Préfixe du numéro de téléphone</param>
        /// <param name="type">Type du numéro de téléphone</param>
        public Telephone(string numero, string prefixe, TypeTelephone type)
        {
            this.Numéro = numero;
            this.Préfixe = prefixe;
            this.Type = type;
        }
    }
}