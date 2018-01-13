using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Entite;

namespace WebApplication1.Models.Papiers
{
    public class DevisViewModel
    {
        public Devis Devis { get; set; }

        public int[] ArticlesID { get; set; }

        public List<SelectListItem> Articles { get; set; }

        public List<SelectListItem> Entreprises { get; set; }

        public int[] Quantite { get; set; }

        public DevisViewModel() { }
        public DevisViewModel(List<Client> listClients, List<Article> listArticles)
        {
            Articles = new List<SelectListItem>();
            listArticles.ForEach(a => Articles.Add(new SelectListItem { Text = a.Nom, Value = a.ID.ToString() }));

            Entreprises = new List<SelectListItem>();
            listClients.ForEach(e => Entreprises.Add(new SelectListItem { Text = e.Nom, Value = e.ID.ToString() }));
        }
        public DevisViewModel(List<Article> listArticles)
        {
            Articles = new List<SelectListItem>();
            listArticles.ForEach(a => Articles.Add(new SelectListItem { Text = a.Nom, Value = a.ID.ToString() }));
        }
    }
}