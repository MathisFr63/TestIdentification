using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Entite;
using WebApplication1.Models.Papiers;

namespace WebApplication1.ViewModels
{
    /// <summary>
    /// ViewModel permettant de créer un devis.
    /// </summary>
    public class DevisViewModel
    {
        // Devis à créer
        public Devis Devis { get; set; }
        // Liste des produits sélectionnés pour les ajouter au devis
        public List<SelectListItem> Produits { get; set; }

        // Liste des entreprises sélectionnées
        public List<SelectListItem> Entreprises { get; set; }

        // Liste des données produits liées au devis
        public IEnumerable<DonneeProduit> DP { get; set; }
        // Tableau dynamique contenant les identifiants des produits que l'on souhaite ajouter au devis
        public int[] ProduitsID { get; set; }
        // Tableau dynamique contenant la quantité de chaque produit à ajouter
        public int[] Quantite { get; set; }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public DevisViewModel() {}

        /// <summary>
        /// Constructeur prenant en paramètre une liste d'entreprises et une liste d'articles sous forme de données produit
        /// </summary>
        /// <param name="listClients">Liste des clients auquels on souhaite envoyer le devis</param>
        /// <param name="listArticles">Liste des articles sous forme de données produit que l'on souhaite ajouter au devis</param>
        public DevisViewModel(List<Entreprise> listClients, List<DonneeProduit> listArticles)
        {
            Produits = new List<SelectListItem>();
            listArticles.ForEach(a => Produits.Add(new SelectListItem { Text = a.Nom, Value = a.ID.ToString() }));

            Entreprises = new List<SelectListItem>();
            listClients.ForEach(e => Entreprises.Add(new SelectListItem { Text = e.NomEntreprise, Value = e.ID.ToString() }));
        }

        /// <summary>
        /// Constructeur prenant en paramètre une liste d'articles sous forme de données produit
        /// </summary>
        /// <param name="listArticles">Liste des articles sous forme de données produit que l'on souhaite ajouter au devis</param>
        public DevisViewModel(List<DonneeProduit> listArticles)
        {
            Produits = new List<SelectListItem>();
            listArticles.ForEach(a => Produits.Add(new SelectListItem { Text = a.Nom, Value = a.ID.ToString() }));
        }

        /// <summary>
        /// Constructeur prenant en paramètre une liste d'articles sous forme de produits
        /// </summary>
        /// <param name="listArticles">Liste des articles sous forme de produits que l'on souhaite ajouter au devis</param>

        public DevisViewModel(List<Produit> listArticles)
        {
            Produits = new List<SelectListItem>();
            listArticles.ForEach(a => Produits.Add(new SelectListItem { Text = a.Nom, Value = a.ID.ToString() }));
        }
    }
}