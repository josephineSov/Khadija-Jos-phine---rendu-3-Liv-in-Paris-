using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Globalization;
using OfficeOpenXml;
using System.Runtime.Versioning;

namespace psi_joséphine
{
    [SupportedOSPlatform("windows")]
    public class AfficheGraphe
    {
        private List<Noeud> noeuds;
        private List<Lien> liens;
        private Dictionary<string, Color> couleursLignes;
        private const int TAILLE_STATION = 4;
        private const int EPAISSEUR_LIGNE = 2;
        private const string CHEMIN_IMAGE = "carte_metro.png";

        static AfficheGraphe()
        {
            // Définir la licence EPPlus avec la nouvelle syntaxe pour EPPlus 8
            ExcelPackage.License.SetNonCommercialPersonal("Joséphine");
        }

        public AfficheGraphe(List<Noeud> noeuds, List<Lien> liens)
        {
            this.noeuds = noeuds ?? new List<Noeud>();
            this.liens = liens ?? new List<Lien>();
            InitialiserCouleurs();
        }

        private void InitialiserCouleurs()
        {
            couleursLignes = new Dictionary<string, Color>
            {
                {"1", Color.FromArgb(255, 255, 168, 0)},    // Orange vif
                {"2", Color.FromArgb(255, 0, 90, 255)},     // Bleu roi
                {"3", Color.FromArgb(255, 149, 179, 64)},   // Vert olive
                {"3bis", Color.FromArgb(255, 137, 207, 240)}, // Bleu ciel
                {"4", Color.FromArgb(255, 187, 76, 158)},   // Violet
                {"5", Color.FromArgb(255, 255, 140, 0)},    // Orange
                {"6", Color.FromArgb(255, 118, 184, 86)},   // Vert clair
                {"7", Color.FromArgb(255, 255, 155, 185)},  // Rose
                {"7bis", Color.FromArgb(255, 255, 192, 203)}, // Rose clair
                {"8", Color.FromArgb(255, 155, 50, 155)},   // Violet foncé
                {"9", Color.FromArgb(255, 177, 199, 55)},   // Vert citron
                {"10", Color.FromArgb(255, 225, 177, 0)},   // Or
                {"11", Color.FromArgb(255, 139, 69, 19)},   // Marron
                {"12", Color.FromArgb(255, 0, 154, 73)},    // Vert foncé
                {"13", Color.FromArgb(255, 152, 251, 152)}, // Vert pâle
                {"14", Color.FromArgb(255, 110, 20, 110)}   // Violet foncé
            };
        }

        public void AfficherGrapheDansConsole()
        {
            try
            {
                if (noeuds.Count == 0)
                {
                    Console.WriteLine("Erreur : Aucune station n'a été chargée.");
                    return;
                }

                if (liens.Count == 0)
                {
                    Console.WriteLine("Erreur : Aucune connexion entre les stations n'a été chargée.");
                    return;
                }

                Console.WriteLine($"Nombre de stations chargées : {noeuds.Count}");
                Console.WriteLine($"Nombre de connexions chargées : {liens.Count}");

                DessinerGraphe();
                Console.WriteLine("\n=== Carte du Métro de Paris ===");
                Console.WriteLine($"L'image de la carte a été créée avec succès : {CHEMIN_IMAGE}");
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création de l'image : {ex.Message}");
                Console.WriteLine($"Détails de l'erreur : {ex.StackTrace}");
            }
        }

        private void DessinerGraphe()
        {
            int largeur = 1500;
            int hauteur = 1000;

            double minLat = noeuds.Min(n => n.Lat);
            double maxLat = noeuds.Max(n => n.Lat);
            double minLon = noeuds.Min(n => n.Lon);
            double maxLon = noeuds.Max(n => n.Lon);

            using (Bitmap image = new Bitmap(largeur, hauteur))
            using (Graphics g = Graphics.FromImage(image))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Dessiner les liens (lignes de métro)
                foreach (var lien in liens)
                {
                    int x1 = (int)((lien.source.Lon - minLon) / (maxLon - minLon) * (largeur - 200) + 100);
                    int y1 = (int)((maxLat - lien.source.Lat) / (maxLat - minLat) * (hauteur - 200) + 100);
                    int x2 = (int)((lien.destination.Lon - minLon) / (maxLon - minLon) * (largeur - 200) + 100);
                    int y2 = (int)((maxLat - lien.destination.Lat) / (maxLat - minLat) * (hauteur - 200) + 100);

                    string ligne = lien.source.ListeLigne.Intersect(lien.destination.ListeLigne).FirstOrDefault() ?? "1";
                    Color couleurLigne = couleursLignes.ContainsKey(ligne) ? couleursLignes[ligne] : Color.Gray;

                    using (Pen crayon = new Pen(couleurLigne, EPAISSEUR_LIGNE))
                    {
                        g.DrawLine(crayon, x1, y1, x2, y2);
                    }
                }

                // Dessiner les stations
                foreach (var noeud in noeuds)
                {
                    int x = (int)((noeud.Lon - minLon) / (maxLon - minLon) * (largeur - 200) + 100);
                    int y = (int)((maxLat - noeud.Lat) / (maxLat - minLat) * (hauteur - 200) + 100);

                    using (Brush pinceau = new SolidBrush(Color.White))
                    using (Pen crayon = new Pen(Color.Black, 1))
                    {
                        g.FillEllipse(pinceau, x - TAILLE_STATION/2, y - TAILLE_STATION/2, 
                            TAILLE_STATION, TAILLE_STATION);
                        g.DrawEllipse(crayon, x - TAILLE_STATION/2, y - TAILLE_STATION/2, 
                            TAILLE_STATION, TAILLE_STATION);
                    }
                }

                // Dessiner la légende
                DessinerLegende(g, largeur, hauteur);

                // Sauvegarder l'image
                image.Save(CHEMIN_IMAGE, ImageFormat.Png);
            }
        }

        private void DessinerLegende(Graphics g, int largeur, int hauteur)
        {
            int positionX = largeur - 150;
            int positionY = 20;
            int hauteurLigne = 20;
            
            using (Font police = new Font("Arial", 8))
            using (Brush pinceauTexte = new SolidBrush(Color.Black))
            {
                g.FillRectangle(new SolidBrush(Color.White), 
                    positionX - 10, positionY - 10, 140, (couleursLignes.Count * hauteurLigne) + 20);
                g.DrawRectangle(new Pen(Color.Black, 1), 
                    positionX - 10, positionY - 10, 140, (couleursLignes.Count * hauteurLigne) + 20);

                foreach (var ligne in couleursLignes.OrderBy(l => l.Key.Length > 1 ? 
                    int.Parse(l.Key.Replace("bis", "")) + 0.5 : int.Parse(l.Key)))
                {
                    using (Pen crayon = new Pen(ligne.Value, EPAISSEUR_LIGNE))
                    {
                        g.DrawLine(crayon, positionX, positionY + 5, positionX + 25, positionY + 5);
                        g.DrawString($"Ligne {ligne.Key}", police, pinceauTexte, positionX + 30, positionY);
                    }
                    positionY += hauteurLigne;
                }
            }
        }
    }
} 