using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Account
{
    public class Parametre
    {
        public int ID { get; private set; }

        public int DureeValiditeDevis { get; set; }
        public int NbRelanceFacture { get; set; }
        public Theme Theme { get; set; }
        public int NbElementPage { get; set; }
        public string DefaultTextFeedback { get; set; }
        public int NbJourStat { get; set; }
        public int TailleHistorique { get; set; }
        public bool Abonnee { get; set; }
        public string DefaultUrl { get; set; }

        public string ConditionsGeneralesDevis { get; set; }
        public string TexteDeFinDevis { get; set; }
        public string FooterDevis { get; set; }
        public string TexteIntroductionDevis { get; set; }

        public string ConditionsGeneralesFacture { get; set; }
        public string TexteDeFinFacture { get; set; }
        public string FooterFacture { get; set; }
        public string TexteIntroductionFacture { get; set; }

        public Parametre() : this(30, 3, Theme.Clear, 15, $"Bonjour,\r\n\r\nCordialement,\r\n", 30, 6, false, "/image/boite.png", "", "", "", "", "", "", "", "")
        {
        }

        public Parametre(int DureeValiditeDevis, int NbRelanceFacture, Theme Theme, int NbElementPage, string DefaultTextFeedback, int NbJourStat, 
            int TailleHistorique, bool Abonnee, string DefaultUrl, string ConditionsGeneralesDevis, string TexteDeFinDevis, string FooterDevis, 
            string TexteIntroductionDevis, string ConditionsGeneralesFacture, string TexteDeFinFacture, string FooterFacture, string TexteIntroductionFacture)
        {
            this.DureeValiditeDevis = DureeValiditeDevis;
            this.NbRelanceFacture = NbRelanceFacture;
            this.Theme = Theme;
            this.NbElementPage = NbElementPage;
            this.DefaultTextFeedback = DefaultTextFeedback;
            this.NbJourStat = NbJourStat;
            this.TailleHistorique = TailleHistorique;
            this.Abonnee = Abonnee;
            this.DefaultUrl = DefaultUrl;

            this.ConditionsGeneralesDevis = ConditionsGeneralesDevis;
            this.TexteDeFinDevis = TexteDeFinDevis;
            this.FooterDevis = FooterDevis;
            this.TexteIntroductionDevis = TexteIntroductionDevis;

            this.ConditionsGeneralesFacture = ConditionsGeneralesFacture;
            this.TexteDeFinFacture = TexteDeFinFacture;
            this.FooterFacture = FooterFacture;
            this.TexteIntroductionFacture = TexteIntroductionFacture;
        }
    }
}