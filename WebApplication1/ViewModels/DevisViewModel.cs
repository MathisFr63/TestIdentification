using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Entite;
using WebApplication1.Models.Papiers;

namespace WebApplication1.ViewModels
{
    public class DevisViewModel
    {
        public Devis Devis { get; set; }
        public List<SelectListItem> Produits { get; set; }
        public List<SelectListItem> Entreprises { get; set; }
        public int[] ProduitsID { get; set; }
        public int[] Quantite { get; set; }


        public DevisViewModel() {}

        public DevisViewModel(List<Entreprise> listClients, List<Produit> listArticles)
        {
            Produits = new List<SelectListItem>();
            listArticles.ForEach(a => Produits.Add(new SelectListItem { Text = a.Nom, Value = a.ID.ToString() }));

            Entreprises = new List<SelectListItem>();
            listClients.ForEach(e => Entreprises.Add(new SelectListItem { Text = e.NomEntreprise, Value = e.ID.ToString() }));
        }

        public DevisViewModel(List<Produit> listArticles)
        {
            Produits = new List<SelectListItem>();
            listArticles.ForEach(a => Produits.Add(new SelectListItem { Text = a.Nom, Value = a.ID.ToString() }));
        }
    }
}