using DocumentFormat.OpenXml.EMMA;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using DocumentFormat.OpenXml.EMMA;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Projet_PSI_new
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
        public int[,] mat;
        public double[,] matFloyd;
        public double[] matBellman;
        public AlgoRecherches()
        {
            string filePath = "MetroParis.xlsx";
            if (File.Exists(filePath))
            {
                OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
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

            
            this.stations.AddRange(nomsUniques.Values);
        }
        public int[,] CreerMatriceAdjacence()
        {
            var graph = Graph; 
            var noeuds = graph.Keys.ToList(); 
            int n = noeuds.Count;
            int[,] matriceAdjacence = new int[n, n]; 

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j && graph[noeuds[i]].Any(lien => lien.Destination == noeuds[j]))
                    {
                        matriceAdjacence[i, j] = 1;
                    }
                }
            }

            return matriceAdjacence;
        }

        public void Matrice()
        {
            this.mat = CreerMatriceAdjacence();
        }

        public void CreationLien()
        {
            Dictionary<string, List<Noeud>> stationsParLigne = new Dictionary<string, List<Noeud>>();

            foreach (var station in stations)
            {
                if (!stationsParLigne.ContainsKey(station.Ligne))
                {
                    stationsParLigne[station.Ligne] = new List<Noeud>();
                }
            }

            foreach (var station in stationsExcel)
            {
                foreach (var Ligne in stationsParLigne.Keys)
                {
                    if (station.Ligne == Ligne)
                    {
                        foreach (var stat in stations)
                        {
                            if (stat.Nom == station.Nom)
                            {
                                stationsParLigne[Ligne].Add(stat);
                            }
                        }
                    }

                }
            }

            
            foreach (var ligne in stationsParLigne.Keys)
            {
                List<Noeud> stationsSurLigne = stationsParLigne[ligne];

                for (int i = 0; i < stationsSurLigne.Count - 1; i++)
                {
                    Noeud stationActuelle = stationsSurLigne[i];
                    Noeud stationSuivante = stationsSurLigne[i + 1];

                    
                    Lien nvlien = new Lien(stationActuelle, stationSuivante);
                    liens.Add(nvlien);
                    
                    if (stationActuelle.Dsens || stationSuivante.Dsens)
                    {
                        Lien nvlienR = new Lien(stationSuivante, stationActuelle);
                        liens.Add(nvlienR);
                    }
                }
            }
        }

        public void RemplirGraph()
        {
            foreach (var lien in liens)
            {
                if (lien.Source == null)
                {
                    throw new Exception("Un lien contient une source null !");
                }

                if (!Graph.ContainsKey(lien.Source))
                {
                    Graph[lien.Source] = new List<Lien>();
                }
                Graph[lien.Source].Add(lien);
            }
        }




        public void AfficherMatrice(int[,] matrice)
        {
            int n = matrice.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(matrice[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        public void AfficherMatrice2(double[,] matrice)
        {
            int n = matrice.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(matrice[i, j] + " ");
                }
                Console.WriteLine();
            }
        }


        public double[,] CreerMatricePondereeTemps()
        {
            var graph = Graph; 
            var noeuds = graph.Keys.ToList(); 
            int n = noeuds.Count;
            double[,] matricePonderee = new double[n, n]; 

            
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        var lien = graph[noeuds[i]].FirstOrDefault(l => l.Destination == noeuds[j]);
                        if (lien != null)
                        {
                            matricePonderee[i, j] = lien.Temps; 
                        }
                        else
                        {
                            matricePonderee[i, j] = double.MaxValue; 
                        }
                    }
                }
            }

            return matricePonderee;
        }

        public void Matrice2()
        {
            this.matPonderee = CreerMatricePondereeTemps();
        }

        


        

        public int Transfert(string nomstation)
        {
            int a = 0;
            for (int i = 0; i < stations.Count; i++)
            {
                if (nomstation == stations[i].Nom)
                {
                    a = i;
                }
            }
            return a;
        }



        public void AfficherTableau(double[] tableau)
        {
            
            Console.WriteLine("Contenu du tableau :");
            for (int i = 0; i < tableau.Length; i++)
            {
                Console.Write(tableau[i] + " ");
            }
            Console.WriteLine(); 
        }

        private int MinDistance(double[] distances, bool[] visited)
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
                Console.WriteLine(" -> " + stations[station].Nom + "   Vous êtes sur la ligne : " + stations[station].Ligne);
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


        public void FloydWarshall(int source, int destination)
        {
            double[,] distances = new double[matPonderee.GetLength(0), matPonderee.GetLength(0)];
            int[,] parent = new int[matPonderee.GetLength(0), matPonderee.GetLength(0)];

            
            for (int i = 0; i < matPonderee.GetLength(0); i++)
            {
                for (int j = 0; j < matPonderee.GetLength(0); j++)
                {
                    if (i == j)
                        distances[i, j] = 0; 
                    else if (matPonderee[i, j] != 0)
                        distances[i, j] = matPonderee[i, j]; 
                    else
                        distances[i, j] = double.MaxValue; 

                    parent[i, j] = (matPonderee[i, j] != 0) ? i : -1; 
                }
            }

            
            for (int k = 0; k < matPonderee.GetLength(0); k++)
            {
                for (int i = 0; i < matPonderee.GetLength(0); i++)
                {
                    for (int j = 0; j < matPonderee.GetLength(0); j++)
                    {
                        if (distances[i, k] != double.MaxValue && distances[k, j] != double.MaxValue &&
                            distances[i, k] + distances[k, j] < distances[i, j])
                        {
                            distances[i, j] = distances[i, k] + distances[k, j];
                            parent[i, j] = parent[k, j]; 
                        }
                    }
                }
            }

            
            for (int i = 0; i < matPonderee.GetLength(0); i++)
            {
                if (distances[i, i] < 0)
                {
                    Console.WriteLine("Le graphe contient un cycle négatif !");
                    return;
                }
            }

           
            AfficherChemin(source, destination, distances, parent);
        }
        private void AfficherChemin(int source, int destination, double[,] distances, int[,] parent)
        {
            if (distances[source, destination] == double.MaxValue)
            {
                Console.WriteLine($"Pas de chemin entre {source} et {destination}.");
                return;
            }

            List<int> chemin = new List<int>();
            int at = destination;

            while (at != -1)
            {
                chemin.Add(at);
                at = parent[source, at];
                if (at == source) 
                {
                    chemin.Add(source);
                    break;
                }
            }

            chemin.Reverse();

            Console.WriteLine($"Chemin le plus court de {stations[source].Nom} à {stations[destination].Nom}");
            foreach (var chem in chemin)
            {
                Console.WriteLine(" -> " + stations[chem].Nom + "   Vous êtes sur la ligne : " + stations[chem].Ligne);
            }
            Console.WriteLine();
            Console.WriteLine($"Temps total : {distances[source, destination]}" + " minutes");
            Console.WriteLine();
        }

        public List<Noeud> Stations
        {
            get { return stations; }
            set { stations = value; }
        }
        public int[,] Mat
        {
            get { return mat; }
            set { mat = value; }
        }
        public double[,] Mat2
        {
            get { return matPonderee; }
            set { matPonderee = value; }
        }
        public double[,] MatFloyd
        {
            get { return matFloyd; }
            set { matFloyd = value; }
        }
        public double[] TaBellman
        {
            get { return matBellman; }
            set { matBellman = value; }
        }
    }

}

