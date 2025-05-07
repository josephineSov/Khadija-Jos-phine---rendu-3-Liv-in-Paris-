using DocumentFormat.OpenXml.EMMA;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using psi_joséphine;
using System.IO;

namespace psi_joséphine

{
    internal class AlgoRecherches
    {
        int rows; 
        int cols;
        public List<Noeud> stations; 
        public List<Lien> liens;
        public List<Noeud> stationsExcel;
        public Dictionary<Noeud, List<Lien>> Graph;
        public double[,] matPonderee;

        public AlgoRecherches()
        {
            string filePath = "MetroParis.xlsx";
            if (File.Exists(filePath))
            {
                using (var package = new ExcelPackage(new FileInfo("MetroParis.xlsx")))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    this.rows = worksheet.Dimension.Rows;
                    this.cols = worksheet.Dimension.Columns;
                }
            }
            else
            {
                Console.WriteLine("Fichier introuvable !");
            }
            this.stations = new List<Noeud>();
            this.liens = new List<Lien>();
            this.stationsExcel = new List<Noeud>();
            this.Graph = new Dictionary<Noeud, List<Lien>>();

        }

        public void CreateStat()
        {
            for (int i = 2; i < this.rows; i++)
            {
                Noeud nvnoeud = new Noeud(i);
                this.stationsExcel.Add(nvnoeud);
            }
        }

        public void CreationStation()
        {
            Dictionary<string, Noeud> nomsUniques = new Dictionary<string, Noeud>();

            for (int i = 2; i < this.rows; i++)
            {
                Noeud nvnoeud = new Noeud(i);
                if (!nomsUniques.ContainsKey(nvnoeud.Nom))
                {
                    nomsUniques[nvnoeud.Nom] = nvnoeud;  
                }
                else
                {
                    nomsUniques[nvnoeud.Nom].ListeLigne.Add(nvnoeud.Ligne); 
                }
            }
        }

        public int MinDistance(double[] distances, bool[] visited)
        {
            int n = matPonderee.GetLength(0);  
            double min = double.MaxValue;
            int minIndex = -1;

            for (int v = 0; v < n; v++)
            {
                if (!visited[v] && distances[v] <= min)
                {
                    min = distances[v];
                    minIndex = v;
                }
            }
            return minIndex;
        }

        public void Dijkstra(int source, int destination)
        {
            int n = matPonderee.GetLength(0);
            double[] distances = new double[n];
            bool[] estvisite = new bool[n];
            int[] predecesseur = new int[n];

            for (int i = 0; i < n; i++)
            {
                distances[i] = double.MaxValue;
                estvisite[i] = false;
                predecesseur[i] = -1;
            }

            distances[source] = 0;

            for (int i = 0; i < n - 1; i++)
            {
                int u = MinDistance(distances, estvisite);
                if (u == destination) break;
                estvisite[u] = true;

                for (int v = 0; v < n; v++)
                {
                    if (!estvisite[v] && matPonderee[u, v] != 0 && distances[u] != double.MaxValue && distances[u] + matPonderee[u, v] < distances[v])
                    {
                        distances[v] = distances[u] + matPonderee[u, v];
                        predecesseur[v] = u;
                    }
                }
            }

            AfficherChemin(source, destination, distances, predecesseur);
        }


        private void AfficherChemin(int depart, int arrivee, double[] tempsTrajet, int[] predecesseur)
        {
            if (tempsTrajet[arrivee] == double.MaxValue)
            {
                Console.WriteLine($"Pas de chemin entre {depart} et {arrivee}.");
                return;
            }

            List<int> trajet = new List<int>();
            for (int stationActuelle = arrivee; stationActuelle != -1; stationActuelle = predecesseur[stationActuelle])
            {
                trajet.Add(stationActuelle);
            }
            trajet.Reverse();

            Console.WriteLine($"Chemin le plus court de {stations[depart].Nom} à {stations[arrivee].Nom}");
            foreach (var station in trajet)
            {
                Console.WriteLine(" - " + stations[station].Nom + "   La station se trouve sur la ligne : " + stations[station].Ligne);
            }
            Console.WriteLine();
            Console.WriteLine($"Temps total : {tempsTrajet[arrivee]} minutes");
            Console.WriteLine();
        }



        public void BellmanFord(int depart, int arrivee)
        {
            double[] tempsTrajet = new double[matPonderee.GetLength(0)];
            int[] predecesseur = new int[matPonderee.GetLength(0)];

            for (int i = 0; i < matPonderee.GetLength(0); i++)
            {
                tempsTrajet[i] = double.MaxValue;
                predecesseur[i] = -1;
            }

            tempsTrajet[depart] = 0;


            for (int i = 0; i < matPonderee.GetLength(0) - 1; i++)
            {
                for (int stationActuelle = 0; stationActuelle < matPonderee.GetLength(0); stationActuelle++)
                {
                    for (int stationSuivante = 0; stationSuivante < matPonderee.GetLength(0); stationSuivante++)
                    {
                        if (matPonderee[stationActuelle, stationSuivante] != 0 && tempsTrajet[stationActuelle] != double.MaxValue &&
                            tempsTrajet[stationActuelle] + matPonderee[stationActuelle, stationSuivante] < tempsTrajet[stationSuivante])
                        {
                            tempsTrajet[stationSuivante] = tempsTrajet[stationActuelle] + matPonderee[stationActuelle, stationSuivante];
                            predecesseur[stationSuivante] = stationActuelle;
                        }
                    }
                }
            }


            for (int stationActuelle = 0; stationActuelle < matPonderee.GetLength(0); stationActuelle++)
            {
                for (int stationSuivante = 0; stationSuivante < matPonderee.GetLength(0); stationSuivante++)
                {
                    if (matPonderee[stationActuelle, stationSuivante] != 0 && tempsTrajet[stationActuelle] != double.MaxValue &&
                        tempsTrajet[stationActuelle] + matPonderee[stationActuelle, stationSuivante] < tempsTrajet[stationSuivante])
                    {
                        Console.WriteLine("Le graphe contient un cycle négatif !");
                        return;
                    }
                }
            }

            AfficherChemin(depart, arrivee, tempsTrajet, predecesseur);
        } 
    }
}

