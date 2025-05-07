
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

    }
}
