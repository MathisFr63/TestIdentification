using System.Collections.Generic;
using System.Data.Entity;
using WebApplication1.Models.Account;
using WebApplication1.Models.Entite;
using WebApplication1.Models.Papiers;

namespace WebApplication1.DAL
{
    /// <summary>
    /// Classe permettant l'initialisation de la base de données avec des données en dures lorsque le model change afin de faire des tests sur l'application sans recréer les données à chaque fois.
    /// </summary>
    //public class ApplicationInitializer : DropCreateDatabaseAlways<ApplicationContext>
    public class ApplicationInitializer : DropCreateDatabaseAlways<ApplicationContext>
    {
        /// <summary>
        /// Méthode permettant d'initialiser la base de données.
        /// </summary>
        /// <param name="context">Contexte de l'application dans lequel on veut sauvegarder les données</param>
        protected override void Seed(ApplicationContext context)
        {
            // Ajout d'adresses mails abonnées aux newsletters.
            var adressesMail = new List<AdresseMail>
            {
                new AdresseMail("Mathis.FRIZOT@etu.uca.fr"),
                new AdresseMail("Flavio.RANCHON@etu.uca.fr"),
                new AdresseMail("Mathieu.RAVEL@etu.uca.fr"),
                new AdresseMail("Aurelien.BERGER2@etu.uca.fr"),
                new AdresseMail("Bernardo.PEREIRA_AUGUSTO@etu.uca.fr"),
            };
            adressesMail.ForEach(am => context.AdressesMail.Add(am));

            // Ajout des paramètres des utilisateurs.
            var parametres = new List<Parametre>
            {
                new Parametre(),
                new Parametre(),
                new Parametre(),
                new Parametre(),
                new Parametre(),
                new Parametre()
            };
            parametres.ForEach(p => context.Parametres.Add(p));
            context.SaveChanges();

            // Construction de plusieurs téléphones
            Telephone tel1 = new Telephone("0602393655", "+33", TypeTelephone.Portable);
            Telephone tel2 = new Telephone("0473836373", "+33", TypeTelephone.Fixe);
            Telephone tel3 = new Telephone("0602393652", "+33", TypeTelephone.Portable);
            Telephone tel4 = new Telephone("0473836377", "+33", TypeTelephone.Fixe);

            // Construction de plusieurs lieux
            Lieu lieu1 = new Lieu("10 Rue de la Maugagnade", null, "63370",            "Lempdes", "France");
            Lieu lieu2 = new Lieu("8 Rue des Granges",       null, "63370",            "Lempdes", "France");
            Lieu lieu3 = new Lieu("10 Rue de la Paix",       null, "63800", "Cournon d'Auvergne", "France");
            Lieu lieu4 = new Lieu("7 Avenue Blaise Pascal",  null, "63170",            "Aubière", "France");
            Lieu lieu5 = new Lieu("59 Avenue du Radiateur",  null, "63170",            "Aubière", "France");

            // Ajout d'utilisateurs.
            var utilisateurs = new List<Utilisateur>
            {
                new Utilisateur("admin", "admin", "Admin", "Admin", TypeUtilisateur.SA, new List<Telephone>{ tel1, tel4 }, lieu4, Civilite.Homme, parametres[0], null),
                new Utilisateur("Mathis.FRIZOT@etu.uca.fr",                "root",          "FRIZOT",   "Mathis", TypeUtilisateur.Administrateur, new List<Telephone>{tel1, tel2 }, lieu1, Civilite.Femme, parametres[1], null),
                new Utilisateur("Flavio.RANCHON@etu.uca.fr",             "Flavio",         "RANCHON",   "Flavio",      TypeUtilisateur.EnAttente, new List<Telephone>{tel3, tel4 }, lieu2, Civilite.Homme, parametres[2], null),
                new Utilisateur("Mathieu.RAVEL@etu.uca.fr",             "Mathieu",           "RAVEL",  "Mathieu",      TypeUtilisateur.EnAttente, new List<Telephone>{tel1, tel3 }, lieu3, Civilite.Homme, parametres[3], null),
                new Utilisateur("Aurelien.BERGER2@etu.uca.fr",         "Aurélien",          "BERGER", "Aurélien",      TypeUtilisateur.EnAttente, new List<Telephone>{tel4, tel2 }, lieu4, Civilite.Homme, parametres[4], null),
                new Utilisateur("Bernardo.PEREIRA_AUGUSTO@etu.uca.fr", "Bernardo", "PEREIRA AUGUSTO", "Bernardo",      TypeUtilisateur.EnAttente, new List<Telephone>{tel1, tel4 }, lieu5, Civilite.Homme, parametres[5], null)
            };
            utilisateurs.ForEach(u => context.Utilisateurs.Add(u));

            // Ajout de produits.
            var produit1 = new Produit { PrixHT = 90, Reduction = 0, TVA = 2, Type = TypeService.Bien, Libelle = "Radiateur", Commentaire = "Radiateur milieu de gamme", UtilisateurID = "Mathieu.RAVEL@etu.uca.fr" };
            var produits = new List<Produit>
            {
                produit1,
                new Produit { PrixHT =        10, Reduction =  0, TVA =  3, Type = TypeService.Bien, Libelle = "Joint de culasse", Commentaire = "Petites pièces d'un moteur",         UtilisateurID = "Mathis.FRIZOT@etu.uca.fr" },
                new Produit { PrixHT =        20, Reduction = 15, TVA =  5, Type = TypeService.Bien, Libelle = "Opium",            Commentaire = "Latex qu'exsude le pavot somnifère", UtilisateurID = "Mathis.FRIZOT@etu.uca.fr" },
                new Produit { PrixHT =         7, Reduction =  0, TVA =  1, Type = TypeService.Bien, Libelle = "Boule de cristal", Commentaire = "Shenron exaucera vos voeux",         UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT =  35000000, Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Ocre",       Commentaire = "Obtenez 1 PA",                       UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT = 325000000, Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Vulbis",     Commentaire = "Obtenez 1 PM",                       UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT =   7000000, Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Pourpre",    Commentaire = "Obtenez 25% de dommage",             UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT =    675000, Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Cawotte",    Commentaire = "Obtenez 50 sagesse",                 UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT =   2500000, Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Emeraude",   Commentaire = "Obtenez 100 PV",                     UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT =   7000000, Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Turquoise",  Commentaire = "Obtenez 20% de CC",                  UtilisateurID = "Flavio.RANCHON@etu.uca.fr" }
            };
            produits.ForEach(p => context.Produits.Add(p));

            //Ajout de devis.
            var devis = new List<Devis>
            {
                new Devis("Installation radiateur", "Installation d'un radiateur neuf à la place d'un radiateur défectueux", TypeMonnaie.Euro, new List<DonneeProduit>{ new DonneeProduit(produit1, 1)}, "Mathis.FRIZOT@etu.uca.fr")
            };
            devis.ForEach(d => context.Devis.Add(d));

            // Ajout de feedbacks.
            var feedbacks = new List<Feedback>
            {
                new Feedback{Comment = "<strong>Problème</strong> lors de l'affichage des <b>feedbacks</b>, je ne les vois pas <i>apparaître</i> !", UtilisateurID = "Mathis.FRIZOT@etu.uca.fr", userName = "FRIZOT Mathis", Subject = "Problèmes Feedbacks", IsResolved = false}
            };
            feedbacks.ForEach(f => context.Feedbacks.Add(f));

            // Sauvegarde des données dans la base.
            context.SaveChanges();
        }
    }
}