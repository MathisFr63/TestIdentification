﻿namespace WebApplication1.Models.Entreprise
{
    public class Telephone
    {
        public int ID { get; private set; }

        public TypeTelephone Type { get; set; }

        public string Numéro { get; set; }

        public string Préfixe { get; set; }
    }
}