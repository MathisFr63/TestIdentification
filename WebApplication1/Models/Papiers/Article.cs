using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Papiers
{
    public class Article
    {
        public int ID { get; private set; }
        public string Nom { get; set; }
        public string Commentaire { get; set; }
        public int PrixHT { get; set; }
        public int Reduction { get; set; }
        public int TVA { get; set; }
        //public int Prixfinal { get; set; } Prix calculé
        //public int TotalTTC { get; set; } Prix calculé
    }
}