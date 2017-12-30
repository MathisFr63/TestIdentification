using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Papiers
{
    public class Facture : Devis
    {
        public int Relances { get; set; }
        public TypeReglement Reglement { get; set; }
    }
}