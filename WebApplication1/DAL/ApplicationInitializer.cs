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
    public class ApplicationInitializer : DropCreateDatabaseIfModelChanges<ApplicationContext>
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
                new AdresseMail("Jean.DUPONT@etu.uca.fr"),
                new AdresseMail("Jacques.DUTREUIL@etu.uca.fr"),
                new AdresseMail("Marc.MARTIN@etu.uca.fr"),
                new AdresseMail("Pierre.DUTHON@etu.uca.fr"),
                new AdresseMail("Martine.CORE@etu.uca.fr"),
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
                new Parametre(),
                new Parametre(),
                new Parametre(),
                new Parametre(),
                new Parametre(),
                new Parametre()
            };
            parametres.ForEach(p => context.Parametres.Add(p));

            // Construction de plusieurs lieux
            var lieux = new List<Lieu>
            {
                new Lieu( "5 Avenue Blaise Pascal",    null, "63170", "Aubière", "France"), /* admin */
                new Lieu("10 Rue de la Maugagnade",    null, "63370", "Lempdes", "France"), /* Mathis.FRIZOT@etu.uca.fr */
                new Lieu("14 Route de la Sauvetat",    null, "63730", "Plauzat", "France"), /* Flavio.RANCHON@etu.uca.fr */
                new Lieu(      "10 Rue de la Paix",    null, "63800", "Cournon", "France"), /* Mathieu.RAVEL@etu.uca.fr */
                new Lieu( "5 Avenue Blaise Pascal",    null, "63170", "Aubière", "France"), /* Aurelien.BERGER2@etu.uca.fr */
                new Lieu( "59 Avenue du Bourg",    null, "63141", "Durtol", "France"),  /* Bernardo.PEREIRA_AUGUSTO@etu.uca.fr */
                new Lieu( "20 Rue Hector Berlioz",    null, "63170", "Aubière", "France"),  /* Jean.DUPONT@etu.uca.fr */
                new Lieu( "146 Rue de la Pradelle",    null, "63000", "Clermont-Ferrand", "France"),  /* Jacques.DUTREUIL@etu.uca.fr */
                new Lieu( "59 Rue Saint-Alyre",    null, "63000", "Clermont-Ferrand", "France"),  /* Marc.MARTIN@etu.uca.fr */
                new Lieu( "78 Route de Nohanent",    null, "63170", "Aubière", "France"),  /* Pierre.DUTHON@etu.uca.fr */
                new Lieu( "3 Rue Côte Blatin",    null, "63540", "Romagnat", "France")  /* Martine.CORE@etu.uca.fr */
            };
            lieux.ForEach(l => context.Lieux.Add(l));
            context.SaveChanges();

            // Construction de plusieurs téléphones
            var telephones = new List<List<Telephone>>
            {
                new List<Telephone>{ new Telephone("0656993625", "+33", TypeTelephone.Portable) { UtilisateurID = "admin" } },
                new List<Telephone>{ new Telephone("0125236654", "+33", TypeTelephone.Fixe) { UtilisateurID = "admin" } },
                new List<Telephone>{ new Telephone("0473836373", "+33", TypeTelephone.Fixe) { UtilisateurID = "Mathis.FRIZOT@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0602393655", "+33", TypeTelephone.Portable) { UtilisateurID = "Mathis.FRIZOT@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0602393652", "+33", TypeTelephone.Portable) { UtilisateurID = "Flavio.RANCHON@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0473836377", "+33", TypeTelephone.Fixe) { UtilisateurID = "Mathieu.RAVEL@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0771271298", "+33", TypeTelephone.Portable) { UtilisateurID = "Aurelien.BERGER2@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0473836377", "+33", TypeTelephone.Fixe) { UtilisateurID = "Bernardo.PEREIRA_AUGUSTO@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0705063254", "+33", TypeTelephone.Portable) { UtilisateurID = "Jean.DUPONT@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0496582365", "+33", TypeTelephone.Fixe) { UtilisateurID = "Jacques.DUTREUIL@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0452698712", "+33", TypeTelephone.Fixe) { UtilisateurID = "Marc.MARTIN@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0635689541", "+33", TypeTelephone.Portable) { UtilisateurID = "Pierre.DUTHON@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0755984511", "+33", TypeTelephone.Portable) { UtilisateurID = "Martine.CORE@etu.uca.fr" } },
            };
            telephones.ForEach(t => context.Telephones.Add(t[0]));

            // Ajout d'utilisateurs.
            var utilisateurs = new List<Utilisateur>
            {
                new Utilisateur("admin",                                  "admin",           "Admin",    "Admin", telephones[0],             TypeUtilisateur.SA, lieux[0], Civilite.Homme, parametres[0], null, false),
                new Utilisateur("Mathis.FRIZOT@etu.uca.fr",                "root",          "FRIZOT",   "Mathis", telephones[1], TypeUtilisateur.Administrateur, lieux[1], Civilite.Femme, parametres[1], null, false),
                new Utilisateur("Flavio.RANCHON@etu.uca.fr",             "Flavio",         "RANCHON",   "Flavio", telephones[2],         TypeUtilisateur.Client, lieux[2], Civilite.Homme, parametres[2], null, false),
                new Utilisateur("Mathieu.RAVEL@etu.uca.fr",             "Mathieu",           "RAVEL",  "Mathieu", telephones[3],      TypeUtilisateur.EnAttente, lieux[3], Civilite.Homme, parametres[3], null, false),
                new Utilisateur("Aurelien.BERGER2@etu.uca.fr",         "Aurélien",          "BERGER", "Aurélien", telephones[4],      TypeUtilisateur.EnAttente, lieux[4], Civilite.Homme, parametres[4], null, true),
                new Utilisateur("Bernardo.PEREIRA_AUGUSTO@etu.uca.fr", "Bernardo", "PEREIRA AUGUSTO", "Bernardo", telephones[5],      TypeUtilisateur.EnAttente, lieux[5], Civilite.Homme, parametres[5], null, false),
                new Utilisateur("Jean.DUPONT@etu.uca.fr",         "Jean",          "DUPONT", "Jean", telephones[6],      TypeUtilisateur.EnAttente, lieux[6], Civilite.Homme, parametres[6], null, false),
                new Utilisateur("Jacques.DUTREUIL@etu.uca.fr",         "Jacques",          "DUTREUIL", "Jacques", telephones[7],      TypeUtilisateur.Prospect, lieux[7], Civilite.Homme, parametres[7], null, false),
                new Utilisateur("Marc.MARTIN@etu.uca.fr",         "Marc",          "MARTIN", "Marc", telephones[8],      TypeUtilisateur.EnAttente, lieux[8], Civilite.Homme, parametres[8], null, false),
                new Utilisateur("Pierre.DUTHON@etu.uca.fr",         "Pierre",          "DUTHON", "Pierre", telephones[9],      TypeUtilisateur.EnAttente, lieux[9], Civilite.Homme, parametres[9], null, false),
                new Utilisateur("Martine.CORE@etu.uca.fr",         "Martine",          "CORE", "Martine", telephones[10],      TypeUtilisateur.Prospect, lieux[10], Civilite.Femme, parametres[10], null, false)
            };
            utilisateurs.ForEach(u => context.Utilisateurs.Add(u));

            // Ajout de produits.
            var produit1 = new Produit { PrixHT = 90, Reduction = 0, TVA = 20, Type = TypeService.Bien, Libelle = "Radiateur", Commentaire = "Radiateur milieu de gamme" };
            var produit2 = new Produit { PrixHT = 10, Reduction = 0, TVA = 20, Type = TypeService.Bien, Libelle = "Joint de culasse", Commentaire = "Petites pièces d'un moteur" };
            var produit3 = new Produit { PrixHT = 20, Reduction = 15, TVA = 20, Type = TypeService.Bien, Libelle = "Robinet", Commentaire = "Robinet milieu de gamme" };
            var produit4 = new Produit { PrixHT = 52, Reduction = 10, TVA = 20, Type = TypeService.Bien, Libelle = "Pelle", Commentaire = "Outils de chantier de haute qualité" };
            var produit5 = new Produit { PrixHT = 159, Reduction = 0, TVA = 20, Type = TypeService.Bien, Libelle = "AirPods", Commentaire = "Écouteurs bluetooth de la marque Apple" };
            var produit6 = new Produit { PrixHT = 1329, Reduction = 0, TVA = 20, Type = TypeService.Bien, Libelle = "iPhone X", Commentaire = "Dernier smartphone de la marque à la pomme" };
            var produit7 = new Produit { PrixHT = 959, Reduction = 0, TVA = 20, Type = TypeService.Bien, Libelle = "Samsung Galaxy S9+", Commentaire = "Dernier smartphone de la marque Samsung" };
            var produit8 = new Produit { PrixHT = 5999, Reduction = 10, TVA = 20, Type = TypeService.Bien, Libelle = "Q9 2018 75(190cm)", Commentaire = "TV QLED Samsung 2018 | Q Contraste | HDR 2000 | Mode Ambiant | Connexion invisible" };
            var produit9 = new Produit { PrixHT = 55.90, Reduction = 0, TVA = 20, Type = TypeService.Bien, Libelle = "Wiko Goa", Commentaire = "4 Go Double SIM blanc" };
            var produit10 = new Produit{ PrixHT = 5518.98, Reduction = 0, TVA = 20, Type = TypeService.Bien, Libelle = "MacBook Pro 15 pouces - Gris sidéral - 2017", Commentaire = "Touch Bar et Touch ID | Processeur Intel Core i7 quadricœur de 7e génération à 2,9 GHz(Turbo Boost jusqu’à 3, 9 GHz) | 16 Go de mémoire LPDDR3 à 2 133 MHz | SSD de 512 Go | Radeon Pro 560 avec 4 Go de mémoire | Quatre ports Thunderbolt 3 | Clavier rétroéclairé - Français" };
            var produit11 = new Produit{ PrixHT = 15926.99, Reduction = 0, TVA = 20, Type = TypeService.Bien, Libelle = "iMac Pro", Commentaire = "Intel Xeon W 18 cœurs à 2,3 GHz, Turbo Boost jusqu’à 4,3 GHz | 128 Go de mémoire ECC DDR4 à 2 666 MHz | SSD de 4 To | Radeon Pro Vega 64 avec 16 Go de mémoire HBM2 | Magic Mouse 2 + Magic Trackpad 2 - Gris sidéral | Kit de montage VESA pour iMac Pro - Gris sidéral | Magic Keyboard avec pavé numérique - Français - Gris sidéral | Final Cut Pro X" };
            var produit12 = new Produit { PrixHT = 29.89, Reduction = 0, TVA = 20, Type = TypeService.Bien, Libelle = "Pampers - Active Fit Premium Protection", Commentaire = "Couches, taille 5 : 11-23 kg (paquet de 68)" };

            var produits = new List<Produit>
            {
                produit1,
                produit2,
                produit3,
                produit4,
                produit5,
                produit6,
                produit7,
                produit8,
                produit9,
                produit10,
                produit11,
                produit12,
            };
            produits.ForEach(p => context.Produits.Add(p));

            //Ajout de devis.
            var devis = new List<Devis>
            {
                new Devis("Installation radiateur", "Installation d'un radiateur neuf à la place d'un radiateur défectueux", TypeMonnaie.Euro, new List<DonneeProduit>{ new DonneeProduit(produit1, 1)}, "Mathis.FRIZOT@etu.uca.fr"),
                new Devis("Achat de matériel informatique", "Achat de matériel informatique et installation dans les nouveaux locaux", TypeMonnaie.Euro, new List<DonneeProduit>{ new DonneeProduit(produit10, 1), new DonneeProduit(produit5, 1), new DonneeProduit(produit11, 1), new DonneeProduit(produit6, 1)}, "Mathis.FRIZOT@etu.uca.fr"),
                new Devis("Devis pour 3 Wiko Goa et 86 packs de couches pampers", "", TypeMonnaie.Euro, new List<DonneeProduit>{ new DonneeProduit(produit9, 3), new DonneeProduit(produit12, 86)}, "Bernardo.PEREIRA_AUGUSTO@etu.uca.fr"),
                new Devis("Devis pour un joint de culasse", "Réparation du moteur défectueux et changement du joint de culasse", TypeMonnaie.Euro, new List<DonneeProduit>{ new DonneeProduit(produit2, 1)}, "Mathis.FRIZOT@etu.uca.fr"),
                new Devis("Devis pour un robient", "Installation d'un robinet neuf", TypeMonnaie.Euro, new List<DonneeProduit>{ new DonneeProduit(produit3, 1)}, "Mathis.FRIZOT@etu.uca.fr"),
                new Devis("Devis pour un joint de culasse et un robinet", "Réparation du moteur défectueux et changement du joint de culasse +  installation d'un robinet neuf", TypeMonnaie.Euro, new List<DonneeProduit>{ new DonneeProduit(produit2, 1), new DonneeProduit(produit3, 1)}, "Mathis.FRIZOT@etu.uca.fr"),
                new Devis("45 packs de couches", "Devis pour 45 packs de couches", TypeMonnaie.Euro, new List<DonneeProduit>{ new DonneeProduit(produit12, 45)}, "Mathis.FRIZOT@etu.uca.fr"),
                new Devis("12 Wiko Goa", "Achat de téléphones portables pour les employés de l'entreprise", TypeMonnaie.Euro, new List<DonneeProduit>{ new DonneeProduit(produit9, 12)}, "Mathis.FRIZOT@etu.uca.fr"),
                new Devis("4 Pelles", "Achats de 4 pelles pour remplacer les anciennes", TypeMonnaie.Euro, new List<DonneeProduit>{ new DonneeProduit(produit4, 4)}, "Mathis.FRIZOT@etu.uca.fr"),
                new Devis("12 TV QLED 2018", "Achat de télévisions pour les employés", TypeMonnaie.Euro, new List<DonneeProduit>{ new DonneeProduit(produit8, 12)}, "Mathis.FRIZOT@etu.uca.fr"),
            };
            devis.ForEach(d => context.Devis.Add(d));


            // Ajout des factures
            var facture = new List<Facture>
            {
                new Facture(devis[0], TypeReglement.Carte),
                new Facture(devis[1], TypeReglement.Cheque),
                new Facture(devis[2], TypeReglement.CryptoMonnaie),
                new Facture(devis[3], TypeReglement.Liquide),
                new Facture(devis[4], TypeReglement.Paypal),
                new Facture(devis[6], TypeReglement.Carte),
                new Facture(devis[8], TypeReglement.Cheque),
                new Facture(devis[9], TypeReglement.Paypal),
            };
            facture.ForEach(f => context.Factures.Add(f));

            
            // Ajout de feedbacks.
            var feedbacks = new List<Feedback>
            {
                new Feedback{Comment = "<strong>Problème</strong> lors de l'affichage des <b>feedbacks</b>, je ne les vois pas <i>apparaître</i> !", UtilisateurID = "Mathis.FRIZOT@etu.uca.fr", userName = "FRIZOT Mathis", Subject = "Problème Feedbacks", IsResolved = false},
                new Feedback{Comment = "La modification d'un produit ne prend pas en compte le changement du taux de TVA.", UtilisateurID = "Mathieu.RAVEL@etu.uca.fr", userName = "RAVEL Mathieu", Subject = "Problème produits", IsResolved = false},
                new Feedback{Comment = "<strong>Problème</strong> lors de la création d'un utilisateur, une exception est levée !", UtilisateurID = "Mathis.FRIZOT@etu.uca.fr", userName = "FRIZOT Mathis", Subject = "Problème création utilisateur", IsResolved = true},
                new Feedback{Comment = "Lors de la modification d'un devis, la quantité des produits n'est pas modifiée.", UtilisateurID = "Flavio.RANCHON@etu.uca.fr", userName = "RANCHON Flavio", Subject = "Problème modification devis", IsResolved = false}
            };
            feedbacks.ForEach(f => context.Feedbacks.Add(f));

            // Sauvegarde des données dans la base.
            context.SaveChanges();
        }
    }
}