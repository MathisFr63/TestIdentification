using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.Account;
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
                new Utilisateur("Mathis.FRIZOT@etu.uca.fr", "root",     "Mathis",   "FRIZOT",  TypeUtilisateur.Administrateur, "Nom du chat",    "Siboulette"),
                new Utilisateur("Flavio.RANCHON@etu.uca.fr",   "Flavio",   "Flavio",   "RANCHON", TypeUtilisateur.Enregistré,     "Sport",          "Football"),
                new Utilisateur("Mathieu.RAVEL@etu.uca.fr",    "Mathieu",  "Mathieu",  "RAVEL",   TypeUtilisateur.Enregistré,     "Nom de famille", "Ravel"),
                new Utilisateur("Aurelien.BERGER2@etu.uca.fr", "Aurelien", "Aurélien", "BERGER",  TypeUtilisateur.Enregistré,     "Nom de famille", "Berger"),
                new Utilisateur("Bernardo.PEREIRA_AUGUSTO@etu.uca.fr", "Bernardo", "Bernardo", "PEREIRA AUGUSTO", TypeUtilisateur.Enregistré, "Lieu d'étude l'an prochain", "IUT")
            };
            utilisateurs.ForEach(u => context.Utilisateurs.Add(u));

            /*var entreprises = new List<Entreprise>
            {
                new Entreprise{ Type = TypeEntreprise.CLient, NomEntreprise =   "Fnac",   Mail = "fnac@mail.com", UtilisateurID=1},
                new Entreprise{ Type = TypeEntreprise.CLient, NomEntreprise = "Amazon", Mail = "amazon@mail.com", UtilisateurID=1},
                new Entreprise{ Type = TypeEntreprise.CLient, NomEntreprise =  "Test1",  Mail = "Test1@mail.com", UtilisateurID=1},
                new Entreprise{ Type = TypeEntreprise.CLient, NomEntreprise =  "Test2",  Mail = "Test2@mail.com", UtilisateurID=2},
                new Entreprise{ Type = TypeEntreprise.CLient, NomEntreprise =  "Test3",  Mail = "Test3@mail.com", UtilisateurID=3},
                new Entreprise{ Type = TypeEntreprise.CLient, NomEntreprise =  "Test4",  Mail = "Test4@mail.com", UtilisateurID=4}
            };
            entreprises.ForEach(e => context.Entreprises.Add(e));*/

            var produits = new List<Produit>
            {
                new Produit{ Nom = "Joint de culasse", Commentaire = "Petites pièces d'un moteur", PrixHT = 10, Reduction =  0, TVA = 3, Type = TypeService.Bien},
                new Produit{ Nom = "Opium",    Commentaire = "Latex qu'exsude le pavot somnifère", PrixHT = 20, Reduction = 15, TVA = 5, Type = TypeService.Bien},
                new Produit{ Nom = "Boules de cristal", Commentaire = "Drago Ball Z",              PrixHT =  5, Reduction =  5, TVA = 0, Type = TypeService.Bien}
            };
            produits.ForEach(p => context.Produits.Add(p));

            var feedbacks = new List<Feedback>
            {
                new Feedback{Email = "Mathis.FRIZOT@etu.uca.fr", Comment = "Problème lors de l'affichage des feedbacks, je ne les vois pas apparaître !", UtilisateurID = 1, Name = "Problèmes Feedbacks", Etat = "En cours"}
            };
            feedbacks.ForEach(f => context.Feedbacks.Add(f));

            context.SaveChanges();
        }
    }
}