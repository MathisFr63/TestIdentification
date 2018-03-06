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

        public Parametre() : this(30, 3, Theme.Clear, 15, $"<p>Bonjour,</p><br/><p>Cordialement,</p>", 30, 6)
        {
        }

        public Parametre(int DureeValiditeDevis, int NbRelanceFacture, Theme Theme, int NbElementPage, string DefaultTextFeedback, int NbJourStat, int TailleHistorique)
        {
            this.DureeValiditeDevis = DureeValiditeDevis;
            this.NbRelanceFacture = NbRelanceFacture;
            this.Theme = Theme;
            this.NbElementPage = NbElementPage;
            this.DefaultTextFeedback = DefaultTextFeedback;
            this.NbJourStat = NbJourStat;
            this.TailleHistorique = TailleHistorique;
        }
    }
}