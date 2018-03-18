using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Papiers;

namespace WebApplication1.ViewModels
{
    public class FactureProduitViewModel
    {
        // Contexte de l'application permettant de récupérer les données en base.
        private ApplicationContext db = new ApplicationContext();

        // Devis à créer
        public Facture Facture { get; set; }
        public DonneeProduit DonneeProduit { get; set; }
        // Liste des produits sélectionnés afin de les ajouter au devis
        public List<SelectListItem> Produits { get; set; }
        // Liste des données produits que l'on souhaite ajouter au devis
        public List<DonneeProduit> ProduitsSelected { get; set; }

        /// <summary>
        /// Constructeur par défaut d'un DevisProduitViewModel afin de charger les produits et de pouvoir les sélectionner sur la page de création d'un devis
        /// </summary>
        public FactureProduitViewModel()
        {
            Produits = new List<SelectListItem>();
            db.Produits.ToList().ForEach(a => Produits.Add(new SelectListItem { Text = a.Libelle, Value = a.ID.ToString() }));
        }

        /// <summary>
        /// Constructeur d'une DevisProduitViewModel permettant de construire un devisproduit après avoir sélectionner les produits souhaités
        /// </summary>
        /// <param name="produits"></param>
        public FactureProduitViewModel(List<DonneeProduit> produits) : this()
        {
            ProduitsSelected = produits;
        }
    }
}