using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_PSI_new
{
    public class Personne
    {
        string prenom;
        string nom;
        string adresse;
        string stationMetro;
        string role;
        string motDePasse;
        int rows;
        int cols;
        public Personne(int exligne)
        {
            string filePath = "personnes_metro_complet.xlsx";
            if (File.Exists(filePath))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    this.prenom = Convert.ToString(worksheet.Cells[exligne, 1].Value);
                    this.nom = Convert.ToString(worksheet.Cells[exligne, 2].Value);
                    this.adresse = Convert.ToString(worksheet.Cells[exligne, 3].Value);
                    this.stationMetro = Convert.ToString(worksheet.Cells[exligne, 4].Value);
                    this.role = Convert.ToString(worksheet.Cells[exligne, 5].Value);
                    this.motDePasse = Convert.ToString(worksheet.Cells[exligne, 6].Value);
                    this.rows = worksheet.Dimension.Rows;
                    this.cols = worksheet.Dimension.Columns;
                }
            }
            else
            {
                Console.WriteLine("Fichier introuvable !");
            }
        }


        public string Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }
        public string Nom {
            get { return nom; }
            set { nom = value; }   
        }
        public string Adresse { 
            get {  return adresse; }    
            set {  adresse = value; }
        }
        public string StationMetro { 
            get { return stationMetro; }
            set {  stationMetro = value; }
        }
        public string Role {
            get {  return role; }
            set { role = value; }
        }
        public string MotDePasse
        { get { return motDePasse; } 
          set { motDePasse = value; } 
        }
    }
}
