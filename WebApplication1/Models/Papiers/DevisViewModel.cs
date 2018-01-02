using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.Compte;
using WebApplication1.Models.Entite;

namespace WebApplication1.Models.Papiers
{
    public class DevisViewModel
    {
        public Devis Devis { get; set; }
        public List<SelectListItem> Articles { get; set; }
        public int[] ArticlesID { get; set; }
    }
}