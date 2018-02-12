using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Papiers;

namespace WebApplication1.ViewModels
{
    public class DevisProduitViewModel
    {
        private ApplicationContext db = new ApplicationContext();

        public Devis Devis { get; set; }
        public DonneeProduit DonneeProduit { get; set; }
        public List<SelectListItem> Produits { get; set; }
        public List<DonneeProduit> ProduitsSelected { get; set; }

        public DevisProduitViewModel()
        {
            Produits = new List<SelectListItem>();
            db.Produits.ToList().ForEach(a => Produits.Add(new SelectListItem { Text = a.Nom, Value = a.ID.ToString() }));
        }

        public DevisProduitViewModel(List<DonneeProduit> produits) : this()
        {
            ProduitsSelected = produits;
        }
    }
}
