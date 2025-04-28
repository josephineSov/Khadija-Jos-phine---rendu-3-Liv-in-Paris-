using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace psi_joséphine
{
    public class Personne
    {
        private static string connexionString = "SERVER=localhost;PORT=3306;DATABASE=psi_LivinParis;UID=root;PASSWORD=Root";

        public int Id { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string StationMetro { get; set; }
        public string MotDePasse { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }

        public string GetNomComplet()
        {
            return Prenom + " " + Nom;
        }

        public string GetInfo()
        {
            return GetNomComplet() + " (" + Role + ")";
        }

        // Méthodes pour les opérations sur les plats
        public static List<Plat> AfficherPlats()
        {
            List<Plat> plats = new List<Plat>();
            using (var con = new MySqlConnection(connexionString))
            {
                con.Open();
                var cmd = new MySqlCommand("SELECT * FROM plat", con);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        plats.Add(new Plat
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Nom = reader["nom"].ToString(),
                            Prix = Convert.ToDecimal(reader["prix"]),
                            NbDePersonne = Convert.ToInt32(reader["nb_de_personne"])
                        });
                    }
                }
            }
            return plats;
        }

        public static void AjouterPlat(Plat plat, int idCuisinier)
        {
            using (var con = new MySqlConnection(connexionString))
            {
                con.Open();
                var cmd = new MySqlCommand(
                    "INSERT INTO plat (nom, prix, nb_de_personne, id_cuisinier) VALUES (@nom, @prix, @nbPersonne, @idCuisinier)",
                    con);
                cmd.Parameters.AddWithValue("@nom", plat.Nom);
                cmd.Parameters.AddWithValue("@prix", plat.Prix);
                cmd.Parameters.AddWithValue("@nbPersonne", plat.NbDePersonne);
                cmd.Parameters.AddWithValue("@idCuisinier", idCuisinier);
                cmd.ExecuteNonQuery();
            }
        }

        // Méthodes pour les opérations sur les commandes
        public static void CreerCommande(int clientId, int cuisinierId)
        {
            using (var con = new MySqlConnection(connexionString))
            {
                con.Open();
                var cmd = new MySqlCommand(
                    "INSERT INTO commande (client_id, cuisinier_id, statut) VALUES (@clientId, @cuisinierId, 'En attente')",
                    con);
                cmd.Parameters.AddWithValue("@clientId", clientId);
                cmd.Parameters.AddWithValue("@cuisinierId", cuisinierId);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Commande> AfficherCommandes(int userId)
        {
            List<Commande> commandes = new List<Commande>();
            using (var con = new MySqlConnection(connexionString))
            {
                con.Open();
                var cmd = new MySqlCommand(
                    "SELECT * FROM commande WHERE client_id = @userId OR cuisinier_id = @userId",
                    con);
                cmd.Parameters.AddWithValue("@userId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        commandes.Add(new Commande
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            ClientId = Convert.ToInt32(reader["client_id"]),
                            CuisinierId = Convert.ToInt32(reader["cuisinier_id"]),
                            Statut = reader["statut"].ToString(),
                            DateCreation = Convert.ToDateTime(reader["date_creation"]),
                            Total = Convert.ToDecimal(reader["total"])
                        });
                    }
                }
            }
            return commandes;
        }
    }

    public class Plat
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public decimal Prix { get; set; }
        public int NbDePersonne { get; set; }
    }

    public class Commande
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CuisinierId { get; set; }
        public string Statut { get; set; }
        public DateTime DateCreation { get; set; }
        public decimal Total { get; set; }
    }
}
