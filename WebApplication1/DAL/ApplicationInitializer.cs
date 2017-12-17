using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class ApplicationInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            var utilisateurs = new List<Utilisateur>
            {
                new Utilisateur{Identifiant="MathisFrizot", MotDePasse="root", Mail="Mathis.FRIZOT@etu.uca.fr", Nom="FRIZOT", Prénom="Mathis", Type=TypeUtilisateur.Administrateur},
                new Utilisateur{Identifiant="MathieuRavel", MotDePasse="Mathieu", Mail = "Mathieu.RAVEL@etu.uca.fr", Nom = "RAVEL", Prénom = "Mathieu", Type = TypeUtilisateur.Enregistré},
                new Utilisateur{Identifiant="FlavioRanchon", MotDePasse="Flavio", Mail = "Flavio.RANCHON@etu.uca.fr", Nom = "RANCHON", Prénom = "Flavio", Type = TypeUtilisateur.Enregistré},
                new Utilisateur{Identifiant="BernardoPereiraAugusto", MotDePasse="Bernardo", Mail = "Bernardo.PEREIRA_AUGUSTO@etu.uca.fr", Nom = "PEREIRA AUGUSTO", Prénom = "Bernardo", Type = TypeUtilisateur.Enregistré},
                new Utilisateur{Identifiant="AurelienBerger", MotDePasse="Aurelien", Mail = "Aurelien.BERGER@etu.uca.fr", Nom = "BERGER", Prénom = "Aurélien", Type = TypeUtilisateur.Enregistré},
            };

            utilisateurs.ForEach(s => context.Utilisateurs.Add(s));
            context.SaveChanges();

            var clients = new List<Client>
            {
                new Client{ Nom = "Fnac", SiteWeb = "https://www.fnac.com/", Commentaire = "La Fnac", UtilisateurID=1},
                new Client{ Nom = "Amazon", SiteWeb = "https://www.amazon.com/", Commentaire = "Le client Amazon", UtilisateurID=1},
                new Client{ Nom = "Test1", SiteWeb = "https://www.amazon.com/", Commentaire = "", UtilisateurID=1},
                new Client{ Nom = "Test2", SiteWeb = "https://www.amazon.com/", Commentaire = "", UtilisateurID=2},
                new Client{ Nom = "Test3", SiteWeb = "https://www.amazon.com/", Commentaire = "", UtilisateurID=3},
                new Client{ Nom = "Test4", SiteWeb = "https://www.amazon.com/", Commentaire = "", UtilisateurID=4}
            };
            clients.ForEach(c => context.Clients.Add(c));
            context.SaveChanges();
        }
    }
}