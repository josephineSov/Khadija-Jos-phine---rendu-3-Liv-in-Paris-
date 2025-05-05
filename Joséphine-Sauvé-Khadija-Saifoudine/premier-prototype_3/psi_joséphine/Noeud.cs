using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace psi_joséphine
{
    public class Noeud
    {
        string nom;
        string ligne;
        double lon;
        double lat;
        int identifiant;
        bool doublesens;
        List<string> listeligne; // liste des lignes passant par cette station
        int couleur;

        static Noeud()
        {
            // Définir la licence EPPlus avec la nouvelle syntaxe pour EPPlus 8
            ExcelPackage.License.SetNonCommercialPersonal("Joséphine"); 


        }

        public Noeud(int exligne) // Crée un objet Noeud à partir d'une ligne donnée dans le fichier Excel "MetroParis.xlsx".


        {
            string filePath = "MetroParis.xlsx";
            if (File.Exists(filePath))
            {
                using (var package = new ExcelPackage(new FileInfo(filePath))) // Ouvre le fichier Excel en lecture.
                {
                    var worksheet = package.Workbook.Worksheets[0];// accede a la premiere feuille du fichier 
                    int rows = worksheet.Dimension.Rows;
                    int cols = worksheet.Dimension.Columns;
                    this.nom = Convert.ToString(worksheet.Cells[exligne, 3].Value);
                    this.ligne = Convert.ToString(worksheet.Cells[exligne, 2].Value);
                    this.lon = Convert.ToDouble(worksheet.Cells[exligne, 4].Value, CultureInfo.InvariantCulture);
                    this.lat = Convert.ToDouble(worksheet.Cells[exligne, 5].Value, CultureInfo.InvariantCulture);
                    this.identifiant = Convert.ToInt32(worksheet.Cells[exligne, 1].Value);
                    if (Convert.ToInt32(worksheet.Cells[exligne, 8].Value) == 1) // Teste si la station est à double sens (colonne 8 = 1).
                    {
                        this.doublesens = true;
                    }
                    else
                    {
                        this.doublesens = false;
                    }
                }
            }
            else
            {
                Console.WriteLine("Fichier introuvable !");
            }
            this.listeligne = new List<string> { this.ligne }; // Crée une liste contenant la ligne principale lue dans le fichier.
        }

        public List<string> ListeLigne
        {
            get { return listeligne; }
            set { ListeLigne = value; }
        }
        public int Id
        {
            get { return this.identifiant; }
            set { this.identifiant = value; }
        }

        public string Ligne
        {
            get { return this.ligne; }
            set { this.ligne = value; }
        }

        public double Lat
        {
            get { return lat; }
            set { lat = value; }
        }

        public string Nom
        {
            get { return this.nom; }
            set { nom = value; }
        }

        public bool Dsens
        {
            get { return doublesens; }
            set { doublesens = value; }
        }
        public double Lon
        {
            get { return lon; }
            set { lon = value; }
        }

        public int Couleur
        {
            get { return couleur; }
            set { couleur = value; }
        }

        public void afficher_noeud()
        {
            Console.WriteLine("Noeud: " + this.nom + ", " + this.ligne + ", " + this.lon + ", " + this.lat + ", " + this.identifiant);
        }
    }
}
