using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApplication1.Models.Papiers
{
    public class DevisViewModel
    {
        public Devis Devis { get; set; }
        public int[] ArticlesID { get; set; }
        public List<SelectListItem> Articles { get; set; }
        public List<SelectListItem> Entreprises { get; set; }
        public int[] Quantite { get; set; }
    }
}