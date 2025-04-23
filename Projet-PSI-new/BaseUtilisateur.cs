using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_PSI_new
{
    public class BaseUtilisateur
    {
        public int rows;
        public int cols;
        public List<Personne> cuisiniers;
        public List<Personne> clients;
        public List<Personne> users;
        public BaseUtilisateur()
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
            this.clients = new List<Personne>();
            this.cuisiniers = new List<Personne>();
            this.users = new List<Personne>();
        }

        public void ChargerPersonnes()
        {
            for (int i = 2; i <= rows; i++)
            {
                Personne personne = new Personne(i);
                if (personne.Role.ToLower() == "client")
                { clients.Add(personne); }
                else
                { cuisiniers.Add(personne); }
                users.Add(personne);
            }
        }

        public void ChargePersonneRole()
        {

        }

        public void RemplirSQL()
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=LIVRABLE2;UID=root;PASSWORD=Root";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    foreach (var client in clients)
                    {
                        cmd.CommandText = "INSERT INTO clients (prenom, nom, adresse, station_metro, mot_de_passe) VALUES (@prenom, @nom, @adresse, @stationMetro, @motDePasse)";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@prenom", client.Prenom);
                        cmd.Parameters.AddWithValue("@nom", client.Nom);
                        cmd.Parameters.AddWithValue("@adresse", client.Adresse);
                        cmd.Parameters.AddWithValue("@stationMetro", client.StationMetro);
                        cmd.Parameters.AddWithValue("@motDePasse", client.MotDePasse);
                        cmd.ExecuteNonQuery();
                    }
                    foreach (var cuisinier in cuisiniers)
                    {
                        cmd.CommandText = "INSERT INTO cuisiniers (prenom, nom, adresse, station_metro, mot_de_passe) VALUES (@prenom, @nom, @adresse, @stationMetro, @motDePasse)";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@prenom", cuisinier.Prenom);
                        cmd.Parameters.AddWithValue("@nom", cuisinier.Nom);
                        cmd.Parameters.AddWithValue("@adresse", cuisinier.Adresse);
                        cmd.Parameters.AddWithValue("@stationMetro", cuisinier.StationMetro);
                        cmd.Parameters.AddWithValue("@motDePasse", cuisinier.MotDePasse);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        public void RemplirSQLaddClient(Personne pers)
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=LIVRABLE2;UID=root;PASSWORD=Root";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                        cmd.CommandText = "INSERT INTO clients (prenom, nom, adresse, station_metro, mot_de_passe) VALUES (@prenom, @nom, @adresse, @stationMetro, @motDePasse)";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@prenom", pers.Prenom);
                        cmd.Parameters.AddWithValue("@nom", pers.Nom);
                        cmd.Parameters.AddWithValue("@adresse", pers.Adresse);
                        cmd.Parameters.AddWithValue("@stationMetro", pers.StationMetro);
                        cmd.Parameters.AddWithValue("@motDePasse", pers.MotDePasse);
                        cmd.ExecuteNonQuery();
                }
            }
        }
        public void RemplirSQLaddCuisinier(Personne pers)
        {
            string connectionString = "SERVER=localhost;PORT=3306;DATABASE=LIVRABLE2;UID=root;PASSWORD=Root";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO cuisiniers (prenom, nom, adresse, station_metro, mot_de_passe) VALUES (@prenom, @nom, @adresse, @stationMetro, @motDePasse)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@prenom", pers.Prenom);
                    cmd.Parameters.AddWithValue("@nom", pers.Nom);
                    cmd.Parameters.AddWithValue("@adresse", pers.Adresse);
                    cmd.Parameters.AddWithValue("@stationMetro", pers.StationMetro);
                    cmd.Parameters.AddWithValue("@motDePasse", pers.MotDePasse);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int Rows { 
            get { return rows; } 
            set { rows = value; } 
        }
        public int Cols { 
            get { return cols; }
            set { cols = value; }
        }
        public List<Personne> Cuisiniers {
            get { return cuisiniers; }
            set { cuisiniers = value; }
        }
        public List<Personne> Clients {
            get { return clients; }
            set { clients = value; }
        }
        public List<Personne> Users {
            get { return users; }
            set { users = value; }
        }


    }
}
