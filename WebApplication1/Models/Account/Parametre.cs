using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Account
{
    public class Parametre
    {
        public int ID { get; private set; }

        [Display(Name = "Durée de validité d'un devis")]
        public int DureeValiditeDevis { get; set; }
        //[Display(Name = "Nombre de relances maximales d'une facture")]
        //public int NbRelanceFacture { get; set; }
        [Display(Name = "Thème")]
        public Theme Theme { get; set; }
        [Display(Name = "Couleurs des boutons")]
        public ColorBtn ColorBtn { get; set; }
        [Display(Name = "Nombre d'éléments à afficher par pages")]
        public int NbElementPage { get; set; }
        [Display(Name = "Texte par défaut des feedbacks")]
        public string DefaultTextFeedback { get; set; }
        [Display(Name = "Nombre de jours des statistiques")]
        public int NbJourStat { get; set; }
        [Display(Name = "Taille de l'historique")]
        public int TailleHistorique { get; set; }
        [Display(Name = "Abonnements aux Newsletters")]
        public bool Abonnee { get; set; }
        [Display(Name = "URL de l'image par défaut des produits")]
        public string DefaultUrl { get; set; }

        [Display(Name = "Conditions générales")]
        public string ConditionsGeneralesDevis { get; set; }
        [Display(Name = "Texte de fin")]
        public string TexteDeFinDevis { get; set; }
        [Display(Name = "Pied de page")]
        public string FooterDevis { get; set; }
        [Display(Name = "Texte d'introduction")]
        public string TexteIntroductionDevis { get; set; }

        [Display(Name = "Conditions générales")]
        public string ConditionsGeneralesFacture { get; set; }
        [Display(Name = "Texte de fin")]
        public string TexteDeFinFacture { get; set; }
        [Display(Name = "Pied de page")]
        public string FooterFacture { get; set; }
        [Display(Name = "Texte d'introduction")]
        public string TexteIntroductionFacture { get; set; }

        public Parametre() : this(30, 3, Theme.Clear, ColorBtn.CouleurVive, 15, $"Bonjour,\r\n\r\nCordialement,\r\n", 30, 6, false, "/image/boite.png", "", "", "", "", "", "", "", "")
        {
        }

        public Parametre(int DureeValiditeDevis, int NbRelanceFacture, Theme Theme, ColorBtn colorBtn, int NbElementPage, string DefaultTextFeedback, int NbJourStat,
            int TailleHistorique, bool Abonnee, string DefaultUrl, string ConditionsGeneralesDevis, string TexteDeFinDevis, string FooterDevis,
            string TexteIntroductionDevis, string ConditionsGeneralesFacture, string TexteDeFinFacture, string FooterFacture, string TexteIntroductionFacture)
        {
            this.DureeValiditeDevis = DureeValiditeDevis;
            //this.NbRelanceFacture = NbRelanceFacture;
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