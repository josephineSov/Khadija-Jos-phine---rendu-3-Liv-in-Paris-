using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using MySql.Data.MySqlClient;
using System.Linq;

namespace psi_joséphine
{
    public class GrapheUtilisateur
    {
        private const int NODE_RADIUS = 15;    //Rayon en pixels des nœuds (cercles) qui seront dessinés.
        private const int NODE_SPACING_X = 150; //Espacement horizontal et vertical entre les nœuds du graphe
        private const int NODE_SPACING_Y = 100; // ""
        private const int LEGEND_HEIGHT = 50; // hauteur pour la légende 
        private const int IMAGE_WIDTH = 1200; //
        private const int IMAGE_HEIGHT = 800; // hauteur image 
        private static string connexionString = "SERVER=127.0.0.1;PORT=3306;DATABASE=psi_LivinParis;UID=root;PASSWORD=Root";

        public static void AfficherGraphe()
        {
            try
            {
                Bitmap bitmap = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT); 
                Graphics graphics = Graphics.FromImage(bitmap); 

         
                graphics.Clear(Color.White);


                DessinerGraphe(graphics, IMAGE_WIDTH, IMAGE_HEIGHT);

                
                bitmap.Save("graphe_clients_cuisiniers.png", ImageFormat.Png);
                Console.WriteLine("Le graphe a été sauvegardé dans bin");

                // Libérer les ressources - Important pour éviter les fuites de mémoire
               graphics.Dispose();
               bitmap.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création du graphe : {ex.Message}");
            }
        }

        private static void DessinerGraphe(Graphics g, int width, int height)
        {
            List<Personne> utilisateurs = GetUtilisateurs(); //appelle votre touche d’accès aux données pour charger tous les utilisateurs
            List<Commande> commandes = GetCommandes(); // charge toutes les commandes passées.


            // Séparer les cuisiniers et les clients
            List<Personne> cuisiniers = new List<Personne>();
            List<Personne> clients = new List<Personne>(); 

            // Trier les utilisateurs entre cuisiniers et clients
            foreach (Personne utilisateur in utilisateurs)
            {
                if (utilisateur.Role == "cuisinier" || utilisateur.Role == "client_cuisinier")
                {
                    cuisiniers.Add(utilisateur);
                }
                if (utilisateur.Role == "client" || utilisateur.Role == "client_cuisinier")
                {
                    clients.Add(utilisateur);
                }
            }

            // Dictionnaire pour stocker les positions des utilisateurs
            Dictionary<int, Point> positions = new Dictionary<int, Point>();

            // Positionner les cuisiniers en haut
            int y = 100;
            for (int i = 0; i < cuisiniers.Count; i++)
            {
                int x = (width / (clients.Count + 1)) * (i + 1);
                if (!positions.ContainsKey(cuisiniers[i].Id))
                {
                    positions.Add(cuisiniers[i].Id, new Point(x, y));
                }
            }
            
            // Positionner les clients en bas
            y = height - 200;
            for (int i = 0; i < clients.Count; i++)
            {
                int x = (width / (clients.Count + 1)) * (i + 1);
                if (!positions.ContainsKey(clients[i].Id))
                {
                    positions.Add(clients[i].Id, new Point(x, y));
                }
            }

            /// Dessiner les arcs (commandes)
            foreach (Commande commande in commandes)
            {
                if (positions.ContainsKey(commande.ClientId) && positions.ContainsKey(commande.CuisinierId)) // vérifie si le dictionnaire contient les positions des c et c 
                {
                    Point startPoint = positions[commande.ClientId];
                    Point endPoint = positions[commande.CuisinierId];

                  
                    Pen pen = new Pen(Color.Black, 2);
                    pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5);
                    g.DrawLine(pen, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    pen.Dispose();
                }
            }

            /// Dessiner les nœuds
            foreach (Personne utilisateur in utilisateurs)
            {
                if (positions.ContainsKey(utilisateur.Id))
                {
                    Point position = positions[utilisateur.Id];
                    Brush brush;
                    
                    
                    if (utilisateur.Role == "cuisinier" || utilisateur.Role == "client_cuisinier")
                    {
                        brush = Brushes.Green;
                    }
                    else
                    {
                        brush = Brushes.Orange;
                    }

                    
                    g.FillEllipse(brush, position.X - NODE_RADIUS, position.Y - NODE_RADIUS, NODE_RADIUS * 2, NODE_RADIUS * 2);
                    g.DrawEllipse(Pens.Black, position.X - NODE_RADIUS, position.Y - NODE_RADIUS, NODE_RADIUS * 2, NODE_RADIUS * 2);

                    // Dessiner le nom avec un fond blanc
                    string nom = utilisateur.GetNomComplet();
                    Font font = new Font("Arial", 12);
                    SizeF size = g.MeasureString(nom, font);
                    g.FillRectangle(Brushes.White, position.X - size.Width/2, position.Y + NODE_RADIUS, size.Width, size.Height);
                   g.DrawString(nom, font, Brushes.Black, position.X - size.Width/2, position.Y + NODE_RADIUS);
                   font.Dispose();
                }
            }

            
            int legendY = height - LEGEND_HEIGHT;
            Font legendFont = new Font("Arial", 15);
            
            g.FillEllipse(Brushes.Green, 10, legendY + 5, 15, 15);
            g.DrawString("Cuisiniers", legendFont, Brushes.Black, 30, legendY + 5);
            
            g.FillEllipse(Brushes.Orange, 150, legendY + 5, 15, 15);
            g.DrawString("Clients", legendFont, Brushes.Black, 170, legendY + 5);
            
            Pen legendPen = new Pen(Color.Black, 2);
            g.DrawLine(legendPen, 300, legendY + 12, 350, legendY + 12);
            g.DrawString("Commandes", legendFont, Brushes.Black, 360, legendY + 5);
            
           
            legendFont.Dispose();
            legendPen.Dispose();
        }

        private static List<Personne> GetUtilisateurs()
        {
            List<Personne> utilisateurs = new List<Personne>(); //Initialisation d’une liste vide de Personne qui contiendra le résultat.
            MySqlConnection con = new MySqlConnection(connexionString); // connexion avec sql
            
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM utilisateurs", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Personne personne = new Personne();
                    personne.Id = Convert.ToInt32(reader["id"]);
                    personne.Prenom = reader["prenom"].ToString();
                    personne.Nom = reader["nom"].ToString();
                    personne.Role = reader["role"].ToString();
                    utilisateurs.Add(personne);
                }

                reader.Close();
                return utilisateurs;
            }
            finally
            {
                con.Close();
            }
        }

        private static List<Commande> GetCommandes()
        {
            List<Commande> commandes = new List<Commande>();
            MySqlConnection con = new MySqlConnection(connexionString);
            
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM commande", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Commande commande = new Commande();
                    commande.Id = Convert.ToInt32(reader["id"]);
                    commande.ClientId = Convert.ToInt32(reader["client_id"]);
                    commande.CuisinierId = Convert.ToInt32(reader["cuisinier_id"]);
                    commande.Statut = reader["statut"].ToString();
                    commande.DateCreation = Convert.ToDateTime(reader["date_creation"]);
                    commande.Total = Convert.ToDecimal(reader["total"]);
                    commandes.Add(commande);
                }

                reader.Close();
                return commandes;
            }
            finally
            {
                con.Close();
            }
        }

        public static void AppliquerWelshPowell()
        {
            List<Personne> utilisateurs = GetUtilisateurs();
            List<Commande> commandes = GetCommandes();


            Dictionary<int, List<int>> voisins = new Dictionary<int, List<int>>(); 

            foreach (Personne p in utilisateurs)
            {
                voisins[p.Id] = new List<int>(); // Pour chaque utilisateur, crée une entrée dans voisins avec son Id et une liste vide.
            }

            foreach (Commande commande in commandes) 
            {
                if (!voisins[commande.ClientId].Contains(commande.CuisinierId)) // crée la liste d'adjacence - récupère l’ID du client impliqué - 
                {
                    voisins[commande.ClientId].Add(commande.CuisinierId);
                }
                if (!voisins[commande.CuisinierId].Contains(commande.ClientId))
                {
                    voisins[commande.CuisinierId].Add(commande.ClientId);
                }
            }


            Dictionary<int, int> degres = new Dictionary<int, int>(); // ca compte le nombre de voisins 
            foreach (Personne p in utilisateurs)
            {
                degres[p.Id] = voisins[p.Id].Count;
            }

            List<int> sommetsTries = new List<int>();
            foreach (Personne p in utilisateurs)
            {
                sommetsTries.Add(p.Id);
            }


            for (int i = 0; i < sommetsTries.Count - 1; i++) //liste sommetsTries est ordonnée de façon à avoir les IDs des utilisateurs du plus grand degré au plus petit.
            {
                for (int j = i + 1; j < sommetsTries.Count; j++)
                {
                    if (degres[sommetsTries[j]] > degres[sommetsTries[i]])
                    {
                        int temp = sommetsTries[i];  // echange de la valeur de i et j 
                        sommetsTries[i] = sommetsTries[j];
                        sommetsTries[j] = temp;
                    }
                }
            }


            Dictionary<int, int> couleurs = new Dictionary<int, int>();
            int couleur = 0;

            for (int i = 0; i < sommetsTries.Count; i++)
            {
                int idCourant = sommetsTries[i];

                if (!couleurs.ContainsKey(idCourant)) 
                {
                    couleurs[idCourant] = couleur;

                    for (int j = i + 1; j < sommetsTries.Count; j++)
                    {
                        int autreId = sommetsTries[j];
                        bool adjacent = false;

                        for (int k = 0; k < voisins[autreId].Count; k++)
                        {
                            if (couleurs.ContainsKey(voisins[autreId][k]) && couleurs[voisins[autreId][k]] == couleur)
                            {
                                adjacent = true;
                                break;
                            }
                        }

                        if (!adjacent)
                        {
                            couleurs[autreId] = couleur;
                        }
                    }

                    couleur++;
                }
            }

            Console.WriteLine("Résultat de la coloration par Welsh-Powell :");
            foreach (Personne p in utilisateurs)
            {
                if (couleurs.ContainsKey(p.Id))
                {
                    Console.WriteLine($" {p.GetNomComplet()} (ID {p.Id}) : Couleur {couleurs[p.Id]}");
                }
            }

        }


    }
} 