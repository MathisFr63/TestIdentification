using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.Compte;
using WebApplication1.Models.Papiers;
using WebApplication1.Models.Entite;

namespace WebApplication1.DAL
{
    public class ApplicationInitializer : System.Data.Entity.DropCreateDatabaseAlways<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            var utilisateurs = new List<Utilisateur>
            {
                new Utilisateur("MathisFrizot", "root", "Mathis", "FRIZOT", "Mathis.FRIZOT@etu.uca.fr", TypeUtilisateur.Administrateur, "Nom du chat", "Siboulette"),
                new Utilisateur("FlavioRanchon", "Flavio", "Flavio", "RANCHON", "Flavio.RANCHON@etu.uca.fr", TypeUtilisateur.Enregistré, "Sport", "Football"),
                new Utilisateur("MathieuRavel", "Mathieu", "Mathieu", "RAVEL", "Mathieu.RAVEL@etu.uca.fr", TypeUtilisateur.Enregistré, "Nom de famille", "Ravel"),
                new Utilisateur("BernardoPereiraAugusto", "Bernardo", "Bernardo", "PEREIRA AUGUSTO", "Bernardo.PEREIRA_AUGUSTO@etu.uca.fr", TypeUtilisateur.Enregistré, "Lieu d'étude l'an prochain", "IUT"),
                new Utilisateur("AurelienBerger", "Aurelien", "Aurélien", "BERGER", "Aurelien.BERGER2@etu.uca.fr", TypeUtilisateur.Enregistré, "Nom de famille", "Berger")
            };
            utilisateurs.ForEach(s => context.Utilisateurs.Add(s));
            context.SaveChanges();

            var clients = new List<Client>
            {
                new Client{ Nom =   "Fnac", SiteWeb = "https://www.fnac.com/",   Commentaire = "La Fnac", UtilisateurID=1},
                new Client{ Nom = "Amazon", SiteWeb = "https://www.amazon.com/", Commentaire = "Le client Amazon", UtilisateurID=1},
                new Client{ Nom =  "Test1", SiteWeb = "https://www.amazon.com/", Commentaire = "", UtilisateurID=1},
                new Client{ Nom =  "Test2", SiteWeb = "https://www.amazon.com/", Commentaire = "", UtilisateurID=2},
                new Client{ Nom =  "Test3", SiteWeb = "https://www.amazon.com/", Commentaire = "", UtilisateurID=3},
                new Client{ Nom =  "Test4", SiteWeb = "https://www.amazon.com/", Commentaire = "", UtilisateurID=4}
            };
            clients.ForEach(c => context.Clients.Add(c));
            context.SaveChanges();

            var articles = new List<Article>
            {
                new Article{ Nom = "Article 1", Commentaire = "Commentaire 1", PrixHT = 10, Reduction =  0, TVA = 3},
                new Article{ Nom = "Article 2", Commentaire = "Commentaire 2", PrixHT = 20, Reduction = 15, TVA = 5},
                new Article{ Nom = "Article 3", Commentaire = "Commentaire 3", PrixHT =  5, Reduction =  5, TVA = 0}
            };
            articles.ForEach(a => context.Articles.Add(a));
            context.SaveChanges();
        }
    }
}