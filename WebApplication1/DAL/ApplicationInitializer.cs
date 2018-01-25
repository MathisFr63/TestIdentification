using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.Compte;
using WebApplication1.Models.Papiers;
using WebApplication1.Models.Entite;

namespace WebApplication1.DAL
{
    public class ApplicationInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            var utilisateurs = new List<Utilisateur>
            {
                new Utilisateur("MathisFrizot",   "root",     "Mathis",   "FRIZOT",  "Mathis.FRIZOT@etu.uca.fr",    TypeUtilisateur.Administrateur, "Nom du chat",    "Siboulette"),
                new Utilisateur("FlavioRanchon",  "Flavio",   "Flavio",   "RANCHON", "Flavio.RANCHON@etu.uca.fr",   TypeUtilisateur.Enregistré,     "Sport",          "Football"),
                new Utilisateur("MathieuRavel",   "Mathieu",  "Mathieu",  "RAVEL",   "Mathieu.RAVEL@etu.uca.fr",    TypeUtilisateur.Enregistré,     "Nom de famille", "Ravel"),
                new Utilisateur("AurelienBerger", "Aurelien", "Aurélien", "BERGER",  "Aurelien.BERGER2@etu.uca.fr", TypeUtilisateur.Enregistré,     "Nom de famille", "Berger"),
                new Utilisateur("BernardoPereiraAugusto", "Bernardo", "Bernardo", "PEREIRA AUGUSTO", "Bernardo.PEREIRA_AUGUSTO@etu.uca.fr", TypeUtilisateur.Enregistré, "Lieu d'étude l'an prochain", "IUT")
            };
            utilisateurs.ForEach(u => context.Utilisateurs.Add(u));
            context.SaveChanges();
            /*
            var entreprises = new List<Entreprise>
            {
                new Entreprise{ Type = TypeEntreprise.CLient, NomEntreprise =   "Fnac",   Mail = "fnac@mail.com", UtilisateurID=1},
                new Entreprise{ Type = TypeEntreprise.CLient, NomEntreprise = "Amazon", Mail = "amazon@mail.com", UtilisateurID=1},
                new Entreprise{ Type = TypeEntreprise.CLient, NomEntreprise =  "Test1",  Mail = "Test1@mail.com", UtilisateurID=1},
                new Entreprise{ Type = TypeEntreprise.CLient, NomEntreprise =  "Test2",  Mail = "Test2@mail.com", UtilisateurID=2},
                new Entreprise{ Type = TypeEntreprise.CLient, NomEntreprise =  "Test3",  Mail = "Test3@mail.com", UtilisateurID=3},
                new Entreprise{ Type = TypeEntreprise.CLient, NomEntreprise =  "Test4",  Mail = "Test4@mail.com", UtilisateurID=4}
            };
            entreprises.ForEach(e => context.Entreprises.Add(e));
            context.SaveChanges();
            */
            var produits = new List<Produit>
            {
                new Produit{ Nom = "Joint de culasse", Commentaire = "Petites pièces d'un moteur", PrixHT = 10, Reduction =  0, TVA = 3, Type = TypeService.Bien},
                new Produit{ Nom = "Opium",    Commentaire = "Latex qu'exsude le pavot somnifère", PrixHT = 20, Reduction = 15, TVA = 5, Type = TypeService.Bien},
                new Produit{ Nom = "Boules de cristal", Commentaire = "Drago Ball Z",              PrixHT =  5, Reduction =  5, TVA = 0, Type = TypeService.Bien}
            };
            produits.ForEach(p => context.Produits.Add(p));
            context.SaveChanges();
        }
    }
}