using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Org.BouncyCastle.Math.EC;

namespace psi_joséphine
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Personne utilisateurConnecte = null;
            
      
            Metros metro = new Metros();
            metro.CreateStat();
            metro.CreationStation();
            metro.CreationLien();
            
       
            AfficheGraphe afficheGraphe = new AfficheGraphe(metro.stations, metro.liens);
            GrapheUtilisateur grapheUtilisateur = new GrapheUtilisateur();

          
            bool continuer = true; 
            while (continuer)
            {
                
                AfficherMenu(utilisateurConnecte);

                
                string choix = Console.ReadLine();
                
                
                if (string.IsNullOrEmpty(choix))
                {
                    Console.WriteLine("Veuillez faire un choix valide.");
                    continue;
                }

               
                if (utilisateurConnecte == null) 
                {
                    GererMenuNonConnecte(choix, ref utilisateurConnecte, afficheGraphe);
                }
                else
                { 
                    GererMenuConnecte(choix, ref utilisateurConnecte, grapheUtilisateur);
                }
            }
        }

        static void AfficherMenu(Personne utilisateur)
        {
            
            Console.WriteLine();

            /// Menu pour utilisateur non connecté
            if (utilisateur == null)
            {
                Console.WriteLine("- Menu Principal -");
                Console.WriteLine("1. Se connecter");
                Console.WriteLine("2. Créer un compte");
                Console.WriteLine("3. Afficher la carte du métro");
                Console.WriteLine("4. Afficher le graphe des utilisateurs");
                Console.WriteLine("5. Module Statistiques");
                Console.WriteLine("6. Algo WelshPowell");
                Console.WriteLine("7. Quitter");
            }
            /// Menu pour utilisateur connecté
            else
            {
                Console.WriteLine(" - Menu Utilisateur -");
                Console.WriteLine("Bienvenue " + utilisateur.GetInfo());
                
                bool estCuisinier = (utilisateur.Role == "cuisinier" || utilisateur.Role == "client_cuisinier");
                
                if (estCuisinier)
                {
                    
                    Console.WriteLine("1. Afficher les plats disponibles");
                    Console.WriteLine("2. Voir commandes passées");
                    Console.WriteLine("3. Gérer mes commandes");
                    Console.WriteLine("4. Voir mes livraisons");
                    Console.WriteLine("5. Ajouter un plat");
                    Console.WriteLine("6. Se déconnecter");
                }
                else
                {
                    
                    Console.WriteLine("1. Afficher les plats disponibles");
                    Console.WriteLine("2. Commander un plat");
                    Console.WriteLine("3. Voir commandes passées");
                    Console.WriteLine("4. Noter un cuisinier");
                    Console.WriteLine("5. Se déconnecter");
                }
            }
            Console.Write("Choix: ");
        }

        static void AjouterPlat(Personne cuisinier)
        {
            Console.WriteLine("\n - Ajouter un nouveau plat - ");

            
            Plat nouveauPlat = new Plat();

            
            Console.Write("Nom du plat: ");
            nouveauPlat.Nom = Console.ReadLine();

            Console.Write("Prix (en euros): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal prix))
            {
                Console.WriteLine("Prix invalide. Veuillez entrer un nombre valide");
                return;
            }
            nouveauPlat.Prix = prix;

            Console.Write("Nombre de personnes: ");
            if (!int.TryParse(Console.ReadLine(), out int nbPersonnes))
            {
                Console.WriteLine("Nombre de personnes invalide. Veuillez entrer un nombre entier valide");
                return;
            }
            nouveauPlat.NbDePersonne = nbPersonnes;

            
            if (string.IsNullOrEmpty(nouveauPlat.Nom))
            {
                Console.WriteLine("Le nom du plat est requis.");
                return;
            }

            try
            {
                
                Personne.AjouterPlat(nouveauPlat, cuisinier.Id);
                Console.WriteLine("Plat ajouté avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'ajout du plat: " + ex.Message);
            }
        }
        static void GererMenuNonConnecte(string choix, ref Personne utilisateurConnecte, AfficheGraphe afficheGraphe)
        {
            switch (choix) 
            {
                case "1": 
                    ConnecterUtilisateur(ref utilisateurConnecte);
                    break;

                case "2": 
                    CreerNouveauCompte();
                    break;

                case "3": 
                    afficheGraphe.AfficherGrapheDansConsole();
                    break;

                case "4": 
                    GrapheUtilisateur.AfficherGraphe();
                    break;

                case "5": 
                    AfficherMenuStatistiques();
                    break;
                case "6":
                    AlgoWelshPowell();
                    break;
                case "7": 
                    Console.WriteLine("Au revoir !");
                    Environment.Exit(0); 
                    break;

                default:
                    Console.WriteLine(" Choix invalide.");
                    break;
            }
        }

        static void AlgoWelshPowell()
        {

            GrapheUtilisateur.AppliquerWelshPowell();
        }

        static void ConnecterUtilisateur(ref Personne utilisateurConnecte)
        {
            
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Mot de passe: ");
            string motDePasse = Console.ReadLine();

            
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(motDePasse))
            {
                Console.WriteLine(" Email et mot de passe sont requis.");
                return;
            }

            
            try 
            {
                utilisateurConnecte = BaseUtilisateur.AuthentifierUtilisateur(email, motDePasse);
                if (utilisateurConnecte == null)
                {
                    Console.WriteLine(" Email ou mot de passe incorrect.");
                }
                else
                {
                    Console.WriteLine(" Connexion réussie !");
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(" Erreur: " + ex.Message); 
            }
        }

        static void CreerNouveauCompte()
        {
          
            Personne nouvelUtilisateur = new Personne();

          
            Console.Write("Prénom: ");
            nouvelUtilisateur.Prenom = Console.ReadLine();
            Console.Write("Nom: ");
            nouvelUtilisateur.Nom = Console.ReadLine();
            Console.Write("Email: ");
            nouvelUtilisateur.Email = Console.ReadLine();
            Console.Write("Adresse: ");
            nouvelUtilisateur.Adresse = Console.ReadLine();
            Console.Write("Station de métro: ");
            nouvelUtilisateur.StationMetro = Console.ReadLine();
            Console.Write("Mot de passe: ");
            nouvelUtilisateur.MotDePasse = Console.ReadLine();
            Console.Write("Rôle (client/cuisinier/client_cuisinier): ");
            nouvelUtilisateur.Role = Console.ReadLine();

           
            bool champsValides = !string.IsNullOrEmpty(nouvelUtilisateur.Prenom) && !string.IsNullOrEmpty(nouvelUtilisateur.Nom) && !string.IsNullOrEmpty(nouvelUtilisateur.Email) &&
                               !string.IsNullOrEmpty(nouvelUtilisateur.MotDePasse) && !string.IsNullOrEmpty(nouvelUtilisateur.Role);

            if (!champsValides)
            {
                Console.WriteLine("Tous les champs sont requis.");
                return;
            }

            
            try
            {
                BaseUtilisateur.CreerUtilisateur(nouvelUtilisateur);
                Console.WriteLine(" Compte créé avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Erreur: " + ex.Message);
            }
        }

        static void GererMenuConnecte(string choix, ref Personne utilisateurConnecte, GrapheUtilisateur grapheUtilisateur)
        {
            bool estCuisinier = (utilisateurConnecte.Role == "cuisinier" || utilisateurConnecte.Role == "client_cuisinier");
            bool estClient = utilisateurConnecte.Role == "client";


            switch (choix)
            {
                case "1":
                    AfficherPlatsDisponibles();
                    break;

                case "2": 
                    if (estCuisinier)
                    {
                        AfficherCommandesUtilisateur(utilisateurConnecte);
                    }
                    else
                    {
                        CommanderPlat(utilisateurConnecte);
                    }
                    break;

                case "3": 
                    if (estCuisinier)
                    {
                        GererCommandesCuisinier(utilisateurConnecte);
                    }
                    else
                    {
                        AfficherCommandesUtilisateur(utilisateurConnecte);
                    }
                    break;

                case "4": 
                    if (estCuisinier)
                    {
                        VoirLivraisonsCuisinier(utilisateurConnecte);
                    }
                    else
                    {
                        NoterCuisinier(utilisateurConnecte);
                    }
                    break;

                case "5": 
                    if (estClient)
                    {
                        Deconnecter(ref utilisateurConnecte);
                    }
                    else if (estCuisinier)
                    {
                        AjouterPlat(utilisateurConnecte);
                    }
                    else
                    {
                        Console.WriteLine("Choix invalide.");
                    }
                    break;
                case "6": /// Déconnexion pour cuisiniers
                    if (estCuisinier)
                    {
                        Deconnecter(ref utilisateurConnecte);
                    }
                    
                    break;

                default:
                    Console.WriteLine("Choix invalide.");
                    break;
            }
        }

        static void Deconnecter(ref Personne utilisateurConnecte)
        {
            utilisateurConnecte = null;
            Console.WriteLine("Déconnexion réussie.");
        }

        static void AfficherPlatsDisponibles()
        {
           
            List<Plat> plats = Personne.AfficherPlats();

            
            if (plats.Count == 0)
            {
                Console.WriteLine("Aucun plat disponible.");
                return;
            }

            
            Console.WriteLine("\nPlats disponibles:");
            foreach (Plat plat in plats)
            {
                string infoPlat = plat.Id + " - " + plat.Nom;
                infoPlat += " | Prix: " + plat.Prix + "euros";  
                infoPlat += " | Pour " + plat.NbDePersonne + " personne(s)";
                Console.WriteLine(infoPlat);             }
        }

        static void CommanderPlat(Personne utilisateur)
        {
            
            AfficherPlatsDisponibles();

            
            Console.Write("\nID du plat à commander: ");
            bool idValide = int.TryParse(Console.ReadLine(), out int idPlat); 

            if (idValide)
            {
                try
                {
                    using (var con = new MySql.Data.MySqlClient.MySqlConnection(BaseUtilisateur.connexionString))
                    {
                        con.Open();
                        
                        
                        var commandVerif = con.CreateCommand();
                        string requeteVerif = "SELECT prix, id_cuisinier FROM plat WHERE id = @idPlat";
                        commandVerif.CommandText = requeteVerif;  
                        commandVerif.Parameters.AddWithValue("@idPlat", idPlat);
                        
                        using (var reader = commandVerif.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                Console.WriteLine("Ce plat n'existe pas.");
                                return;
                            }
                            
                            decimal prixPlat = Convert.ToDecimal(reader["prix"]);
                            int cuisinierId = Convert.ToInt32(reader["id_cuisinier"]);
                            reader.Close();

                            /// Créer la commande
                            var commandCommande = con.CreateCommand();
                            string requeteCommande = "INSERT INTO commande (client_id, cuisinier_id, statut, date_creation, total) " +
                                                   "VALUES (@clientId, @cuisinierId, 'En attente', NOW(), @total)";
                            commandCommande.CommandText = requeteCommande;
                            commandCommande.Parameters.AddWithValue("@clientId", utilisateur.Id);
                            commandCommande.Parameters.AddWithValue("@cuisinierId", cuisinierId);
                            commandCommande.Parameters.AddWithValue("@total", prixPlat);
                            commandCommande.ExecuteNonQuery();
                            
                           
                            long commandeId = commandCommande.LastInsertedId;

                            
                            var commandLigne = con.CreateCommand();
                            string requeteLigne = "INSERT INTO lignecommande (commande_id, plat_id, quantite, date_livraison, lieu_livraison) " +
                                                "VALUES (@commandeId, @platId, 1, DATE_ADD(NOW(), INTERVAL 1 HOUR), @lieuLivraison)";
                            commandLigne.CommandText = requeteLigne;
                            commandLigne.Parameters.AddWithValue("@commandeId", commandeId);
                            commandLigne.Parameters.AddWithValue("@platId", idPlat);
                            commandLigne.Parameters.AddWithValue("@lieuLivraison", utilisateur.Adresse);
                            commandLigne.ExecuteNonQuery();

                            Console.WriteLine("Commande effectuée avec succès !");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Erreur: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine(" ID de plat invalide.");
            }
        }

        static void AfficherCommandesUtilisateur(Personne utilisateur)
        {
            try
            {
                using (var con = new MySql.Data.MySqlClient.MySqlConnection(BaseUtilisateur.connexionString))
                {
                    con.Open();
                    var command = con.CreateCommand();
                    string requete = "SELECT c.*, p.nom as nom_plat, p.prix as prix_plat, lc.quantite " + "FROM commande c " +"LEFT JOIN lignecommande lc ON c.id = lc.commande_id " +
                                   "LEFT JOIN plat p ON lc.plat_id = p.id " +  "WHERE c.client_id = @userId " + "ORDER BY c.date_creation DESC"; // selectionne toutes les colonnes des commandes 
                    command.CommandText = requete;
                    command.Parameters.AddWithValue("@userId", utilisateur.Id);

                    var reader = command.ExecuteReader();
                    Console.WriteLine("\nVos commandes:");
                    int commandeIdPrecedent = -1; 
                    decimal totalCommande = 0;

                    while (reader.Read())
                    {
                        int commandeId = Convert.ToInt32(reader["id"]);
                        
                        if (commandeId != commandeIdPrecedent) 
                        {
                            if (commandeIdPrecedent != -1)
                            {
                                Console.WriteLine($"Total de la commande: {totalCommande}euros");
                                Console.WriteLine();
                            }
                            
                            Console.WriteLine($"Commande #{commandeId}");
                            Console.WriteLine($"- Statut: {reader["statut"]}");
                            Console.WriteLine($"- Date: {Convert.ToDateTime(reader["date_creation"])}");
                            totalCommande = 0;
                            commandeIdPrecedent = commandeId;
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("nom_plat"))) 
                        {
                            string nomPlat = reader["nom_plat"].ToString();
                            decimal prixPlat = Convert.ToDecimal(reader["prix_plat"]);
                            int quantite = Convert.ToInt32(reader["quantite"]);
                            totalCommande += prixPlat * quantite;
                            Console.WriteLine($"  {nomPlat} x{quantite} ({prixPlat}euros/unité)");
                        }
                    }

                    if (commandeIdPrecedent != -1)
                    {
                        Console.WriteLine($"Total de la commande: {totalCommande}euros");
                        Console.WriteLine();
                    }

                    if (commandeIdPrecedent == -1)
                    {
                        Console.WriteLine("Vous n'avez aucune commande.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la récupération des commandes: " + ex.Message);
            }
        }

        static void AfficherMenuStatistiques()
        {
            bool continuer = true;
            while (continuer)
            {
                Console.WriteLine("\n- Module Statistiques -");
                Console.WriteLine("1. Afficher le nombre de livraisons par cuisinier");
                Console.WriteLine("2. Afficher les commandes par période");
                Console.WriteLine("3. Afficher la moyenne des prix des commandes");
                Console.WriteLine("4. Afficher les commandes par nationalité et par période "); 
                Console.WriteLine("5. Retour au menu principal");
                
                Console.Write("Choix: ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        AfficherLivraisonsParCuisinier();
                        break;
                    case "2":
                        AfficherCommandesParPeriode();
                        break;
                    case "3":
                        AfficherMoyennePrixCommandes();
                        break;
                    case "4":
                        AfficherCommandesParNationaliteEtPeriode();
                        break;
                    case "5":
                        continuer = false;
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }
            }
        }

        static void AfficherLivraisonsParCuisinier()
        {
            try
            {
                using (var con = new MySql.Data.MySqlClient.MySqlConnection(BaseUtilisateur.connexionString))
                {
                    con.Open();
                    var command = con.CreateCommand();
                    string requete = "SELECT u.prenom, u.nom, COUNT(l.id_livraison) as nombre_livraisons " + "FROM utilisateurs u " + "LEFT JOIN livraison l ON u.id = l.cuisinier_id " +
                                   "WHERE u.role IN ('cuisinier', 'client_cuisinier') " +  "GROUP BY u.id, u.prenom, u.nom " + "ORDER BY nombre_livraisons DESC"; 
                    command.CommandText = requete;

                    var reader = command.ExecuteReader();
                    Console.WriteLine("\nNombre de livraisons par cuisinier:");
                    Console.WriteLine("--------------------------------");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["prenom"]} {reader["nom"]}: {reader["nombre_livraisons"]} livraison(s)");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur: " + ex.Message);
            }
        }

        static void AfficherCommandesParPeriode()
        {
            try
            {
                Console.Write("Date de début (YYYYMMDD): ");
                string dateDebut = Console.ReadLine();
                Console.Write("Date de fin (YYYYMMDD): ");
                string dateFin = Console.ReadLine();

                using (var con = new MySql.Data.MySqlClient.MySqlConnection(BaseUtilisateur.connexionString))
                {
                    con.Open();
                    var command = con.CreateCommand();
                    string requete = "SELECT DATE(date_creation) as date, COUNT(*) as nombre_commandes " + "FROM commande " +"WHERE DATE(date_creation) BETWEEN @dateDebut AND @dateFin " +
                                   "GROUP BY DATE(date_creation) " + "ORDER BY date";
                    command.CommandText = requete;
                    command.Parameters.AddWithValue("@dateDebut", dateDebut);
                    command.Parameters.AddWithValue("@dateFin", dateFin);

                    var reader = command.ExecuteReader();
                    Console.WriteLine("\nCommandes par période:");
                    Console.WriteLine("--------------------");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Date: {((DateTime)reader["date"]).ToShortDateString()}, Nombre de commandes: {reader["nombre_commandes"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur: " + ex.Message);
            }
        }

        static void AfficherMoyennePrixCommandes()
        {
            try
            {
                using (var con = new MySql.Data.MySqlClient.MySqlConnection(BaseUtilisateur.connexionString))
                {
                    con.Open();
                    var command = con.CreateCommand();
                    string requete = "SELECT AVG(total) as moyenne_prix " +"FROM commande " + "WHERE statut != 'Annulé'"; 
                    command.CommandText = requete;

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        decimal moyennePrix = reader["moyenne_prix"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["moyenne_prix"]); 
                        Console.WriteLine($"\nMoyenne des prix des commandes: {moyennePrix:F2}euros"); 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur: " + ex.Message);
            }
        }
        static void AfficherCommandesParNationaliteEtPeriode()
        {
            try
            {
                Console.Write("Date de début (YYYYMMDD): ");
                string dateDebut = Console.ReadLine();
                Console.Write("Date de fin (YYYYMMDD): ");
                string dateFin = Console.ReadLine();
                
                using (var con = new MySql.Data.MySqlClient.MySqlConnection(BaseUtilisateur.connexionString))
                {
                    con.Open();
                    var command = con.CreateCommand();
                    string requete = "SELECT n.nationalite_cuisine, COUNT(*) as nombre_commandes " +"FROM commande c " +"JOIN commande_nourriture cn ON c.id = cn.commande_id " +
                                   "JOIN Nourriture n ON cn.nourriture_id = n.id_Nourriture " + "WHERE DATE(c.date_creation) BETWEEN @dateDebut AND @dateFin " +"GROUP BY n.nationalite_cuisine " + "ORDER BY nombre_commandes DESC";
                    command.CommandText = requete;
                    command.Parameters.AddWithValue("@dateDebut", dateDebut);
                    command.Parameters.AddWithValue("@dateFin", dateFin);

                    var reader = command.ExecuteReader();
                    Console.WriteLine("\nCommandes par nationalité de cuisine:");
                    Console.WriteLine("----------------------------------");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Cuisine {reader["nationalite_cuisine"]}: {reader["nombre_commandes"]} commande(s)");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur: " + ex.Message);
            }
        }

        static void NoterCuisinier(Personne utilisateur)
        {
            try
            {
                
                using (var con = new MySql.Data.MySqlClient.MySqlConnection(BaseUtilisateur.connexionString))
                {
                    con.Open();
                    var command = con.CreateCommand();
                    string requete = "SELECT c.id, c.date_creation, u.prenom, u.nom " +"FROM commande c " +"JOIN utilisateurs u ON c.cuisinier_id = u.id " +
                        "WHERE c.client_id = @userId AND c.statut = 'Livré' " + "ORDER BY c.date_creation DESC";
                    command.CommandText = requete;
                    command.Parameters.AddWithValue("@userId", utilisateur.Id);

                    var reader = command.ExecuteReader();
                    Console.WriteLine("\nVos commandes livrées:");
                    bool hasCommands = false; 
                    while (reader.Read())
                    {
                        hasCommands = true;
                        Console.WriteLine($"Commande #{reader["id"]} - {reader["prenom"]} {reader["nom"]} - {Convert.ToDateTime(reader["date_creation"]).ToShortDateString()}");
                    }

                    if (!hasCommands)
                    {
                        Console.WriteLine("Vous n'avez aucune commande livrée à noter.");
                        return; 
                    }

                    reader.Close();

                    
                    Console.Write("\nID de la commande à noter: ");
                    if (!int.TryParse(Console.ReadLine(), out int commandeId))
                    {
                        Console.WriteLine("ID invalide.");
                        return;
                    }

                    
                    command = con.CreateCommand();
                    requete = "SELECT c.id, u.prenom, u.nom " + "FROM commande c " + "JOIN utilisateurs u ON c.cuisinier_id = u.id " +
                             "WHERE c.id = @commandeId AND c.client_id = @userId AND c.statut = 'Livré'";
                    command.CommandText = requete;
                    command.Parameters.AddWithValue("@commandeId", commandeId);
                    command.Parameters.AddWithValue("@userId", utilisateur.Id);

                    reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        Console.WriteLine("Commande non trouvée, non livrée ou ne vous appartient pas.");
                        return;
                    }

                    string cuisinierNom = $"{reader["prenom"]} {reader["nom"]}";
                    reader.Close();

                  
                    command = con.CreateCommand();
                    requete = "SELECT id_note FROM Notes WHERE commande_id = @commandeId";
                    command.CommandText = requete;
                    command.Parameters.AddWithValue("@commandeId", commandeId);
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Console.WriteLine("Une note existe déjà pour cette commande.");
                        return;
                    }
                    reader.Close();

                  
                    Console.Write($"Note pour {cuisinierNom} (1-5): ");
                    if (!int.TryParse(Console.ReadLine(), out int note) || note < 1 || note > 5)
                    {
                        Console.WriteLine("Note invalide. Veuillez entrer un nombre entre 1 et 5.");
                        return;
                    }

                    
                    Console.Write("Commentaire (optionnel): ");
                    string commentaire = Console.ReadLine();

               
                    command = con.CreateCommand();
                    requete = "INSERT INTO Notes (commentaire_note, date_avis, note_retour, commande_id) " + "VALUES (@commentaire, NOW(), @note, @commandeId)"; // La date de l'avis est automatiquement définie avec NOW()
                    command.CommandText = requete;
                    command.Parameters.AddWithValue("@commentaire", commentaire);
                    command.Parameters.AddWithValue("@note", note);
                    command.Parameters.AddWithValue("@commandeId", commandeId);
                    command.ExecuteNonQuery();

                    Console.WriteLine("Note enregistrée avec succès !");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur: " + ex.Message);
            }
        }

        static void GererCommandesCuisinier(Personne cuisinier)
        {
            try
            {
                using (var con = new MySql.Data.MySqlClient.MySqlConnection(BaseUtilisateur.connexionString))
                {
                    con.Open();
                    var command = con.CreateCommand();
                    string requete = "SELECT c.id, c.date_creation, c.statut, u.prenom, u.nom, u.adresse " +"FROM commande c " +
                                   "JOIN utilisateurs u ON c.client_id = u.id " +"WHERE c.cuisinier_id = @cuisinierId " + "ORDER BY c.date_creation DESC";
                    command.CommandText = requete;
                    command.Parameters.AddWithValue("@cuisinierId", cuisinier.Id);

                    var reader = command.ExecuteReader();
                    Console.WriteLine("\nVos commandes:");
                    bool hasCommands = false;
                    while (reader.Read())
                    {
                        hasCommands = true;
                        Console.WriteLine($"Commande #{reader["id"]} - Client: {reader["prenom"]} {reader["nom"]} - Statut: {reader["statut"]} - Date: {Convert.ToDateTime(reader["date_creation"]).ToShortDateString()}");
                    }

                    if (!hasCommands)
                    {
                        Console.WriteLine("Vous n'avez aucune commande.");
                        return;
                    }

                    reader.Close();

                   
                    Console.Write("\nID de la commande à modifier: ");
                    if (!int.TryParse(Console.ReadLine(), out int commandeId))
                    {
                        Console.WriteLine("ID invalide.");
                        return;
                    }

                
                    command = con.CreateCommand();
                    requete = "SELECT c.id, c.statut, u.adresse as adresse_client " +"FROM commande c " +
                             "JOIN utilisateurs u ON c.client_id = u.id " + "WHERE c.id = @commandeId AND c.cuisinier_id = @cuisinierId"; // la commande doit correspondre à l'id de la commande et à l'id du cuisinier 
                    command.CommandText = requete;
                    command.CommandText = requete;
                    command.Parameters.AddWithValue("@commandeId", commandeId);
                    command.Parameters.AddWithValue("@cuisinierId", cuisinier.Id);

                    reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        Console.WriteLine(" Commande non trouvée ou ne vous appartient pas.");
                        return;
                    }

                    string statutActuel = reader["statut"].ToString();
                    string adresseClient = reader["adresse_client"].ToString();
                    reader.Close();

                   
                    Console.WriteLine("\nStatuts possibles:");
                    Console.WriteLine("1. En attente");
                    Console.WriteLine("2. En cours");
                    Console.WriteLine("3. Livré");
                    Console.WriteLine("4. Annulé");

                    
                    Console.Write("Nouveau statut (1-4): ");
                    if (!int.TryParse(Console.ReadLine(), out int choixStatut) || choixStatut < 1 || choixStatut > 4)
                    {
                        Console.WriteLine("Choix invalide.");
                        return;
                    }


                    string nouveauStatut;
                    switch (choixStatut)
                    {
                        case 1:
                            nouveauStatut = "En attente";
                            break;
                        case 2:
                            nouveauStatut = "En cours";
                            break;
                        case 3:
                            nouveauStatut = "Livré";
                            break;
                        case 4:
                            nouveauStatut = "Annulé";
                            break;
                        default:
                            nouveauStatut = statutActuel;
                            break;
                    }


                    command = con.CreateCommand();
                    requete = "UPDATE commande SET statut = @statut WHERE id = @commandeId"; 
                    command.CommandText = requete;
                    command.Parameters.AddWithValue("@statut", nouveauStatut);
                    command.Parameters.AddWithValue("@commandeId", commandeId);
                    command.ExecuteNonQuery();

                    /// Si le statut passe à "En cours", créer une livraison
                    if (nouveauStatut == "En cours" && statutActuel != "En cours")
                    {
                       
                        command = con.CreateCommand();
                        requete = "SELECT adresse FROM utilisateurs WHERE id = @cuisinierId";
                        command.CommandText = requete;
                        command.Parameters.AddWithValue("@cuisinierId", cuisinier.Id);
                        reader = command.ExecuteReader();
                        reader.Read();
                        string adresseCuisinier = reader["adresse"].ToString();
                        reader.Close();

                      
                        command = con.CreateCommand();
                        requete = "INSERT INTO livraison (adresse_depart, adresse_arrivee, date_livraison, statut_livraison, heure_disponible, zone_geographique, cuisinier_id) " +
                                 "VALUES (@adresseDepart, @adresseArrivee, NOW(), 'En attente', TIME(NOW()), 'Paris', @cuisinierId)"; // insert : creer une nouvelle ligne de commande 
                        command.CommandText = requete;
                        command.Parameters.AddWithValue("@adresseDepart", adresseCuisinier);
                        command.Parameters.AddWithValue("@adresseArrivee", adresseClient);
                        command.Parameters.AddWithValue("@cuisinierId", cuisinier.Id);
                        command.ExecuteNonQuery();

                        
                        long livraisonId = command.LastInsertedId;

                      
                        command = con.CreateCommand();
                        requete = "UPDATE lignecommande SET livraison_id = @livraisonId WHERE commande_id = @commandeId";
                        command.CommandText = requete;
                        command.Parameters.AddWithValue("@livraisonId", livraisonId);
                        command.Parameters.AddWithValue("@commandeId", commandeId);
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine($"Statut de la commande #{commandeId} mis à jour: {nouveauStatut}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur: " + ex.Message);
            }
        }

        static void VoirLivraisonsCuisinier(Personne cuisinier)
        {
            try
            {
                using (var con = new MySql.Data.MySqlClient.MySqlConnection(BaseUtilisateur.connexionString))
                {
                    con.Open();
                    var command = con.CreateCommand();
                    string requete = "SELECT l.id_livraison, l.adresse_depart, l.adresse_arrivee, l.date_livraison, " +
                                   "l.statut_livraison, l.zone_geographique, c.id as commande_id " +
                                   "FROM livraison l " +
                                   "JOIN lignecommande lc ON l.id_livraison = lc.livraison_id " +
                                   "JOIN commande c ON lc.commande_id = c.id " +
                                   "WHERE c.cuisinier_id = @cuisinierId " +
                                   "ORDER BY l.date_livraison DESC";
                    command.CommandText = requete;
                    command.Parameters.AddWithValue("@cuisinierId", cuisinier.Id);

                    var reader = command.ExecuteReader();
                    Console.WriteLine("\nVos livraisons:");
                    bool hasLivraisons = false;
                    while (reader.Read())
                    {
                        hasLivraisons = true;
                        Console.WriteLine($"\nLivraison #{reader["id_livraison"]}");
                        Console.WriteLine($"Commande #{reader["commande_id"]}");
                        Console.WriteLine($"De: {reader["adresse_depart"]}");
                        Console.WriteLine($"À: {reader["adresse_arrivee"]}");
                        Console.WriteLine($"Date: {Convert.ToDateTime(reader["date_livraison"]).ToString("dd/MM/yyyy HH:mm")}");
                        Console.WriteLine($"Statut: {reader["statut_livraison"]}");
                        Console.WriteLine($"Zone: {reader["zone_geographique"]}");
                    }

                    if (!hasLivraisons)
                    {
                        Console.WriteLine("Vous n'avez aucune livraison.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Erreur: " + ex.Message);
            }
        }

        static void AfficherMenuCuisinier()
        {
            Console.WriteLine("\nMenu Cuisinier:");
            Console.WriteLine("1. Afficher les plats");
            Console.WriteLine("2. Gérer les commandes");
            Console.WriteLine("3. Voir les livraisons");
            Console.WriteLine("4. Ajouter un plat");
            Console.WriteLine("5. Déconnexion");
        }
    }
}