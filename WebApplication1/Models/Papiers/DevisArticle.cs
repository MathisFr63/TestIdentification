using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Papiers
{
    public class DevisArticle
    {
        public int ID { get; private set; }
        public int DevisID { get; set; }
        public int ArticleID { get; set; }
        public int Quantite { get; set; }
    }
}