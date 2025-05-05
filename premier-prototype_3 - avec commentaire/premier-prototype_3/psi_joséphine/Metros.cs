using DocumentFormat.OpenXml.Bibliography;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.CRUD;
using OfficeOpenXml;
using psi_joséphine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace psi_joséphine
{
    public class Metros
    {
        int rows;
        int cols;
        public List<Noeud> stations;
        public List<Lien> liens;
        public List<Noeud> stationsExcel;
        public int[,] mat;
        public Dictionary<Noeud, List<Lien>> Graph;

        static Metros()
        {
            // Définir la licence EPPlus avec la nouvelle syntaxe pour EPPlus 8
            ExcelPackage.License.SetNonCommercialPersonal("Joséphine");
        }

        public Metros()
        {
            string filePath = "MetroParis.xlsx";
            if (File.Exists(filePath))
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
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

            if (nomsUniques.Count > 0)
            {
                foreach (var noeud in nomsUniques.Values)
                {
                    this.stations.Add(noeud);
                }
            }
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
                    if (i != j)
                    {
                        foreach (var lien in graph[noeuds[i]])
                        {
                            if (lien.Destination == noeuds[j])
                            {
                                matriceAdjacence[i, j] = 1;
                                break;
                            }
                        }
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
                    Console.WriteLine("Un lien contient une source null !");
                }

                if (!Graph.ContainsKey(lien.Source))
                {
                    Graph[lien.Source] = new List<Lien>();
                }
                Graph[lien.Source].Add(lien);
            }
        }

        public void PopulateDatabase()
        {
            string connectionString = "SERVER=127.0.0.1;PORT=3306;" + "DATABASE=psi_LivinParis;" + "UID=root;PASSWORD=Root";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();


                foreach (var station in stations)
                {
                    string lignesString = string.Join(",", station.ListeLigne);

                    string query = "INSERT INTO stations_metro (id, nom, longitude, latitude, ligne, listeligne) VALUES (@id, @nom, @longitude, @latitude, @ligne, @listeligne) " + "ON DUPLICATE KEY UPDATE nom=@nom, longitude=@longitude, latitude=@latitude, ligne=@ligne, listeligne=@listeligne;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", station.Id);
                        cmd.Parameters.AddWithValue("@nom", station.Nom);
                        cmd.Parameters.AddWithValue("@longitude", station.Lon);
                        cmd.Parameters.AddWithValue("@latitude", station.Lat);
                        cmd.Parameters.AddWithValue("@ligne", station.Ligne);
                        cmd.Parameters.AddWithValue("@listeligne", lignesString);
                        cmd.ExecuteNonQuery();
                    }
                }


                foreach (var station in stations)
                {
                    List<string> linkedStations = new List<string>();
                    foreach (var lien in liens)
                    {
                        if (lien.Source.Id == station.Id)
                        {
                            linkedStations.Add(lien.Destination.Nom);
                        }
                    }
                    string linksString = string.Join(",", linkedStations);

                    string updateQuery = "UPDATE stations_metro SET liens = @liens WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@liens", linksString);
                        cmd.Parameters.AddWithValue("@id", station.Id);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine($"Mise à jour liens pour {station.Nom}: {rowsAffected} ligne(s) affectée(s)");
                    }
                }
            }
        }

        public List<Noeud> Stations
        {
            get { return stations; }
            set { stations = value; }
        }

        public int Rows
        {
            get { return rows; }
            set { rows = value; }
        }

    }
}
