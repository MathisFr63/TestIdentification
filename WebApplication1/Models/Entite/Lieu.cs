using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Account
{
    public class Lieu
    {
        public int ID { get; private set; }
   
        public string Adresse { get; set; }

        public string Complement { get; set; }

        public int CodePostal { get; set; }

        public string Ville { get; set; }

        public string Pays { get; set; }
    }
}