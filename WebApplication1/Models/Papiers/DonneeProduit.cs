using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Papiers
{
    /// <summary>
    /// Classe permettant de représenter les données entre un document et les produits qui lui sont liés.
    /// </summary>
    public class DonneeProduit
    {
        // Identifiant de la donnée afin de l'identifier dans la base de données.
        public int ID { get; private set; }

        // Identifiant du document pour lequel on stocke les données
        //public int DocumentID { get; set; }

        public int? DevisID { get; set; }

        public int? FactureID { get; set; }

        // Quantité du produit
        [Display(Name = "Quantité")]
        public int Quantite { get; set; }
        
        // Nom du produit
        public string Nom { get; set; }

        // Commentaire lié au produit
        public string Commentaire { get; set; }

        // Prix hors taxe du produit
        public double PrixHT { get; set; }

        public double TotalHT
        {
            get
            {
                return PrixHT*Quantite;
            }
        }

        //Prix toutes taxes comprises du produit
        public double TotalTTC
        {
            get
            {
                var total = TotalHT + (TotalHT * TVA / 100);
                return total - (total * Reduction / 100);
            }
        }

        // Réduction ajoutée au prix du produit
        public double Reduction { get; set; }

        // Montant de TVA a ajouté au prix du produit.
        public double TVA { get; set; }

        // Type de service du produit
        public TypeService Type { get; set; }

        /// <summary>
        /// Constructeur par défaut d'une donnée produit.
        /// </summary>
        public DonneeProduit()
        {
        }

        /// <summary>
        /// Constructeur d'une donnée produit prenant en paramètre un produit
        /// </summary>
        /// <param name="produit">Produit dont on veut stocker les données au moment T</param>
        public DonneeProduit(Produit produit) : this()
        {
            Nom = produit.Libelle;
            Commentaire = produit.Détails;
            PrixHT = produit.PrixHT;
            Reduction = produit.Reduction;
            TVA = produit.TVA;
            Type = produit.Type;
        }

        /// <summary>
        /// Constructeur d'une donnée produit prenant en paramètre un produit, un identifiant et une quantité
        /// </summary>
        /// <param name="produit">Produit dont on veut stocker les données au moment T</param>
        /// <param name="id">Identifiant du document auquel on souhaite lié les données du produit au moment T</param>
        /// <param name="quantite">Quantité du produit ajouté sur le document</param>
        public DonneeProduit(Produit produit, int quantite) : this(produit)
        {
            //DocumentID = id;
            Quantite = quantite;
        }

        /// <summary>
        /// Constructeur d'une donéè produit penant en paramètre une autre donnée produit et l'identifiant du document auquel on souhaite lier les données du produit
        /// </summary>
        /// <param name="dp">donnée produit dont on veut copier les données</param>
        /// <param name="id">Identifiant du document auquel on souhaite lier les données du produit</param>
        public DonneeProduit(DonneeProduit dp) : this()
        {
            Nom = dp.Nom;
            Commentaire = dp.Commentaire;
            PrixHT = dp.PrixHT;
            Reduction = dp.Reduction;
            TVA = dp.TVA;
            Type = dp.Type;
            Quantite = dp.Quantite;
            //DocumentID = id;
        }
    }
}