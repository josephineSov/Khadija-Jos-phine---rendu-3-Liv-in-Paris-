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
        private const string CHEMIN_IMAGE = "carte_metro_PARIS.png";

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

        {"1", Color.FromArgb(255, 255, 200, 0)},      // Orange vif (plus chaud)
        {"2", Color.FromArgb(255, 0, 102, 204)},      // Bleu roi (plus lisible)
        {"3", Color.FromArgb(255, 178, 153, 0)},      // Ocre jaune / Olive foncé
        {"3bis", Color.FromArgb(255, 100, 190, 255)}, // Bleu ciel doux
        {"4", Color.FromArgb(255, 170, 50, 140)},     // Violet soutenu
        {"5", Color.FromArgb(255, 255, 120, 0)},      // Orange moyen
        {"6", Color.FromArgb(255, 140, 200, 100)},    // Vert doux
        {"7", Color.FromArgb(255, 255, 140, 170)},    // Rose clair
        {"7bis", Color.FromArgb(255, 255, 182, 193)}, // Rose pastel
        {"8", Color.FromArgb(255, 145, 70, 155)},     // Violet moyen
        {"9", Color.FromArgb(255, 200, 210, 60)},     // Vert anis
        {"10", Color.FromArgb(255, 240, 190, 0)},     // Or lumineux
        {"11", Color.FromArgb(255, 120, 60, 20)},     // Marron foncé
        {"12", Color.FromArgb(255, 0, 130, 60)},      // Vert forêt
        {"13", Color.FromArgb(255, 170, 255, 170)},   // Vert menthe pâle
        {"14", Color.FromArgb(255, 100, 0, 100)}      // Violet foncé intense
            };
        }

        public void AfficherGrapheDansConsole()
        {
            try
            {
                Console.WriteLine("\n Les couleurs des lignes de métro sur le graphe correspondent aux couleurs des lignes dans la réalité");
               

                DessinerGraphe();
                Console.WriteLine("\n - Carte du Métro de Paris - ");
                Console.WriteLine($"L'image de la carte se trouve dans le fichier bin - Debug - net6.0-windows : {CHEMIN_IMAGE}");
                Console.WriteLine("\nAppuyer pour continuer");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
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

                    using (Brush pinceau = new SolidBrush(Color.Black))
                    using (Pen crayon = new Pen(Color.Black, 1))
                    {
                        g.FillEllipse(pinceau, x - TAILLE_STATION/2, y - TAILLE_STATION/2, 
                            TAILLE_STATION, TAILLE_STATION);
                        g.DrawEllipse(crayon, x - TAILLE_STATION/2, y - TAILLE_STATION/2, 
                            TAILLE_STATION, TAILLE_STATION);
                    }
                }

                // Titre centré en haut
                using (Font fontTitre = new Font("Arial", 24, FontStyle.Bold))
                using (Brush pinceauTitre = new SolidBrush(Color.Black))
                {
                    string titre = "Carte Métro Paris - Joséphine Sauvé et Khadija Saifoudine";
                    SizeF dim = g.MeasureString(titre, fontTitre);
                    float xTitre = (largeur - dim.Width) / 2;
                    g.DrawString(titre, fontTitre, pinceauTitre, xTitre, 10);
                }

                // Sauvegarder l'image
                image.Save(CHEMIN_IMAGE, ImageFormat.Png);
            }
        }
    }
} 