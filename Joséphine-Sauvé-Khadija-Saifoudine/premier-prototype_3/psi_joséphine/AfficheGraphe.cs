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
    [SupportedOSPlatform("windows")] // morceau de code est uniquement pris en charge sur windows
    public class AfficheGraphe
    {
        private List<Noeud> noeuds;
        private List<Lien> liens;
        private Dictionary<string, Color> couleursLignes;
        private const int TAILLE_STATION = 6;
        private const int EPAISSEUR_LIGNE = 3;
        private const string CHEMIN_IMAGE = "carte_metro_PARIS.png";


        public AfficheGraphe(List<Noeud> noeuds, List<Lien> liens) // constructeur 
        {
            this.noeuds = noeuds ?? new List<Noeud>(); // ?? renvoie la valeur de gauche si liste pas null sinon celle de droite 
            this.liens = liens ?? new List<Lien>();
            InitialiserCouleurs();
        }

        private void InitialiserCouleurs()
        {
            couleursLignes = new Dictionary<string, Color>
            {

    { "1", Color.FromArgb(255, 255, 205, 0) },
    { "2", Color.FromArgb(255, 0, 60, 166) },
    { "3", Color.FromArgb(255, 131, 121, 2) },
    { "3bis", Color.FromArgb(255, 110, 196, 232) },
    { "4", Color.FromArgb(255, 207, 0, 158) },
    { "5", Color.FromArgb(255, 255, 126, 46) },
    { "6", Color.FromArgb(255, 110, 202, 151) },
    { "7", Color.FromArgb(255, 250, 154, 186) },
    { "7bis", Color.FromArgb(255, 110, 196, 232) },
    { "8", Color.FromArgb(255, 225, 155, 223) },
    { "9", Color.FromArgb(255, 182, 189, 0) },
    { "10", Color.FromArgb(255, 201, 145, 13) },
    { "11", Color.FromArgb(255, 112, 75, 28) },
    { "12", Color.FromArgb(255, 0, 120, 82) },
    { "13", Color.FromArgb(255, 110, 202, 151) },
    { "14", Color.FromArgb(255, 98, 37, 157) }
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
                Console.ReadKey(); // attendre que l'utilisateur appuie sur une touche pour continuer execution du programme 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
                Console.WriteLine($"Détails de l'erreur : {ex.StackTrace}"); // afficher la liste d'erreur jusqu'a ce que l'erreur se produise 
            }
        }

        private void DessinerGraphe()
        {
            int largeur = 1750;
            int hauteur = 1300;

            double minLat = noeuds.Min(n => n.Lat);
            double maxLat = noeuds.Max(n => n.Lat);
            double minLon = noeuds.Min(n => n.Lon);
            double maxLon = noeuds.Max(n => n.Lon);

            using (Bitmap image = new Bitmap(largeur, hauteur))
            using (Graphics g = Graphics.FromImage(image))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                

                /// Dessiner les liens (lignes de métro)
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

                /// Dessiner les stations
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

                
                using (Font fontTitre = new Font("Arial", 24, FontStyle.Bold))
                using (Brush pinceauTitre = new SolidBrush(Color.Black))
                {
                    string titre = "Carte Métro Paris - Joséphine Sauvé et Khadija Saifoudine";
                    SizeF dim = g.MeasureString(titre, fontTitre);
                    float xTitre = (largeur - dim.Width) / 2;
                    g.DrawString(titre, fontTitre, pinceauTitre, xTitre, 10);
                }

                
                image.Save(CHEMIN_IMAGE, ImageFormat.Png);
            }
        }
    }
} 