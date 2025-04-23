using DocumentFormat.OpenXml.Drawing;
using MySql.Data.MySqlClient;
using Projet_PSI_new;

internal class Program
{
    public static void Main(string[] args)
    {

        BaseUtilisateur baseUtilisateur = new BaseUtilisateur();
        Metros metros = new Metros();
        AlgoRecherches algos = new AlgoRecherches();
        algos.CreationStation();
        algos.CreateStat();
        algos.CreationLien();
        algos.RemplirGraph();
        algos.Matrice();
        algos.Matrice2();
        Console.WriteLine("Souhaitez-vous calculer un itinéraire (1), accéder à la base utilisateur/créer un utilisateur (2), ajouter un plat (3) ou afficher le graphe (4) ? Entrez le numéro correspondant.");
        bool check = false;
        string num = string.Empty;
        num = Console.ReadLine();
        int numero = 0; 
        while (!check)
        {
            if(num == "1" || num == "2" || num =="3" || num == "4")
            {
                check = true;
                numero = Convert.ToInt32(num);
            }
            if(check)
            {
                break;
            }
            Console.WriteLine("Numero incorrect");
            num = Console.ReadLine();
        }
        if (numero == 1)
        {
            string source = string.Empty;
            Console.WriteLine("Quelle station source ?");
            source = Console.ReadLine();
            bool estdanslabdd = false;
            while (!estdanslabdd)
            {
                foreach (var station in algos.Stations)
                {
                    if (source == station.Nom)
                    {
                        estdanslabdd = true;
                    }
                }
                if (estdanslabdd) { break; }
                Console.WriteLine("station non existante");
                source = Console.ReadLine();
            }
            string destination = string.Empty;
            Console.WriteLine("Quelle station destination?");
            destination = Console.ReadLine();
            bool bdd2 = false;
            while (!bdd2)
            {
                foreach (var station in algos.Stations)
                {
                    if (destination == station.Nom)
                    {
                        bdd2 = true;
                    }
                }
                if (bdd2) { break; }
                Console.WriteLine("station non existante");
                destination = Console.ReadLine();
            }

            int transfert1 = algos.Transfert(source);
            int transfert2 = algos.Transfert(destination);
            Console.WriteLine("Dijkstra : ");
            Console.WriteLine();
            algos.Dijkstra(transfert1, transfert2);
            Console.WriteLine("Bellman Ford : ");
            Console.WriteLine();
            algos.BellmanFord(transfert1, transfert2);
            Console.WriteLine("Floyd Warshall : ");
            Console.WriteLine();
            algos.FloydWarshall(transfert1, transfert2);

            Console.ReadLine();

        }

       

        
        metros.CreationStation();
        metros.CreateStat();
        metros.CreationLien();
        metros.RemplirGraph();
        metros.PopulateDatabase();


        baseUtilisateur.ChargerPersonnes();
        baseUtilisateur.RemplirSQL();

        if (numero == 2)
        {
            Console.WriteLine("Veuillez donner votre prénom");
            string a = Console.ReadLine();
            bool b = false;
            while (!b)
            {

                foreach (var personne in baseUtilisateur.Users)
                {
                    if (personne.Prenom == a)
                    {
                        b = true;
                    }
                }
                if (b == true)
                {
                    break;
                }
                if (b == false)
                {
                    Console.WriteLine("Saisissez un prénom dans la base de données ou créez un nouvel utilisateur, si vous souhaitez créer -> tapez : 1");
                }
                a = Console.ReadLine();
                if (a == "1")
                {

                    Console.WriteLine("Pour créer un nouvel utilisateur -> entrez prénom");
                    string c = Console.ReadLine();
                    while (c == string.Empty)
                    {
                        Console.WriteLine("Entrez un prénom valide");
                        c = Console.ReadLine();
                    }
                    Console.WriteLine("Pour créer un  nouvel utilisateur -> entrez un nom");
                    string d = Console.ReadLine();
                    while (d == string.Empty)
                    {
                        Console.WriteLine("Entrez un nom valide");
                        d = Console.ReadLine();
                    }
                    Console.WriteLine("Entrez une adresse");
                    string e = Console.ReadLine();
                    while (e == string.Empty)
                    {
                        Console.WriteLine("Entrez une adresse valide");
                        e = Console.ReadLine();
                    }
                    Console.WriteLine("Entrez votre station de métro");
                    string g = Console.ReadLine();
                    bool k = false;

                    while (!k)
                    {
                        foreach (var stations in metros.Stations)
                        {
                            if (g == stations.Nom)
                            {
                                k = true;
                            }
                        }
                        if (k == true)
                        {
                            break;
                        }
                        Console.WriteLine("métro non valide");
                        g = Console.ReadLine();
                    }
                    Console.WriteLine("Entrez un mdp");
                    string h = Console.ReadLine();
                    while (h == string.Empty)
                    {
                        Console.WriteLine("mdp non valide");
                        h = Console.ReadLine();
                    }
                    Console.WriteLine("Etes-vous Cuisnier ou Client?");
                    string u = Console.ReadLine();
                    while (u != "Client" && u != "Cuisinier")
                    {
                        Console.WriteLine("Role non valide");
                        u = Console.ReadLine();
                    }
                    int creat = 0;
                    if (u == "Client")
                    {
                        creat = baseUtilisateur.Clients.Count;
                    }
                    else
                    {
                        creat = baseUtilisateur.Cuisiniers.Count;
                    }
                    Personne nvpers = new Personne(creat);
                    nvpers.Prenom = c;
                    nvpers.Nom = d;
                    nvpers.Adresse = e;
                    nvpers.StationMetro = g;
                    nvpers.MotDePasse = h;
                    nvpers.Role = u;
                    if (u == "Client")
                    {
                        baseUtilisateur.Clients.Add(nvpers);
                        baseUtilisateur.RemplirSQLaddClient(nvpers);
                    }
                    else
                    {
                        baseUtilisateur.Cuisiniers.Add(nvpers);
                        baseUtilisateur.RemplirSQLaddCuisinier(nvpers);
                    }
                    baseUtilisateur.Users.Add(nvpers);

                    Console.WriteLine("Utilisateur créé !");
                }
            }

            string p = string.Empty;
            Console.WriteLine("Quel est votre nom ?");
            p = Console.ReadLine();
            bool z = false;
            List<Personne> list = new List<Personne>();
            foreach(var personne in baseUtilisateur.Users)
            {
                if (personne.Prenom == a)
                {
                    list.Add(personne);
                }
            }

            while (!z)
            {
                foreach (var personne in baseUtilisateur.Users)
                {
                    if (personne.Nom == p)
                    {
                        z = true;
                    }
                }
                if(z)
                {
                    break;
                }
                Console.WriteLine("nom incorrect");
                p = Console.ReadLine();
            }
            string MDP = string.Empty;
            foreach (var personne in baseUtilisateur.Users)
            {
                if (personne.Nom == p && personne.Prenom == a)
                {
                    MDP = personne.MotDePasse;
                }
            }
            Console.WriteLine("Quel est votre mot de passe ?");
            string kl = string.Empty;
            kl = Console.ReadLine();
            while (MDP != kl)
            {
                Console.WriteLine("Mot de passe incorrect ! ");
                kl = Console.ReadLine();
            }
            Console.WriteLine("Vous êtes connecté");
        }
        RemplirCommandesPlats gestionCommande = new RemplirCommandesPlats();

        if (num == "3")
        {
            Console.WriteLine("Bienvenue dans le système de commande de plats !");
            Console.Write("Entrez votre ID client : ");
            int clientId = int.Parse(Console.ReadLine());

            Console.Write("Entrez l'ID du cuisinier assigné : ");
            int cuisinierId = int.Parse(Console.ReadLine());

            int commandeId = gestionCommande.CreerCommande(clientId, cuisinierId);
            Console.WriteLine($"Commande créée avec l'ID : {commandeId}");

            while (true)
            {
                Console.Write("Entrez le nom du plat (ou 'exit' pour terminer) : ");
                string nomPlat = Console.ReadLine();
                if (nomPlat.ToLower() == "exit") break;

                Console.Write("Entrez le prix du plat : ");
                decimal prixPlat = decimal.Parse(Console.ReadLine());

                Console.Write("Saisissez le nombre de portions (personnes) que ce plat peut servir : ");
                int nbPersonne = int.Parse(Console.ReadLine());

                int platId = gestionCommande.AjouterPlat(nomPlat, prixPlat, nbPersonne, cuisinierId);

                Console.Write("Entrez la quantité : ");
                int quantite = int.Parse(Console.ReadLine());

                Console.Write("Entrez la date de livraison (----(année)/--(mois)/--(jour) H:min:sec) : ");
                DateTime dateLivraison = DateTime.Parse(Console.ReadLine());

                Console.Write("Entrez le lieu de livraison : ");
                string lieuLivraison = Console.ReadLine();

                gestionCommande.AjouterLigneCommande(commandeId, platId, quantite, dateLivraison, lieuLivraison);
                Console.WriteLine("Plat ajouté à la commande !");
            }

            gestionCommande.MettreAJourTotalCommande(commandeId);
            Console.WriteLine("Commande finalisée et total mis à jour.");
        }

        if (numero == 4)
        {
            Application.EnableVisualStyles();
            Application.Run(new GraphViewer(metros));
        }
        

        Console.WriteLine();
        Console.ReadLine();




    }
}
