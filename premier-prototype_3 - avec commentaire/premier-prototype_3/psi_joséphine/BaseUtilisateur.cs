using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace psi_joséphine
{
    public class BaseUtilisateur
    {
        public static string connexionString = "SERVER=127.0.0.1;PORT=3306;DATABASE=psi_LivinParis;UID=root;PASSWORD=Root";

        public static void CreerUtilisateur(Personne utilisateur)
        {
            using (MySqlConnection con = new MySqlConnection(connexionString))
            {
                con.Open();

                // Vérifier si l'email existe déjà
                string checkEmailQuery = "SELECT COUNT(*) FROM utilisateurs WHERE email = @email";
                MySqlCommand checkEmail = new MySqlCommand(checkEmailQuery, con);
                checkEmail.Parameters.AddWithValue("@email", utilisateur.Email);
                
                int count = Convert.ToInt32(checkEmail.ExecuteScalar());
                if (count > 0)
                {
                    throw new Exception("Cet email est déjà utilisé.");
                }

                // Créer l'utilisateur
                string insertQuery = "INSERT INTO utilisateurs (prenom, nom, adresse, station_metro, mot_de_passe, role, email) " +
                                   "VALUES (@prenom, @nom, @adresse, @stationMetro, @motDePasse, @role, @email)";
                
                MySqlCommand cmd = new MySqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@prenom", utilisateur.Prenom);
                cmd.Parameters.AddWithValue("@nom", utilisateur.Nom);
                cmd.Parameters.AddWithValue("@adresse", utilisateur.Adresse);
                cmd.Parameters.AddWithValue("@stationMetro", utilisateur.StationMetro);
                cmd.Parameters.AddWithValue("@motDePasse", utilisateur.MotDePasse);
                cmd.Parameters.AddWithValue("@role", utilisateur.Role);
                cmd.Parameters.AddWithValue("@email", utilisateur.Email);
                
                cmd.ExecuteNonQuery();
            }
        }

        public static Personne AuthentifierUtilisateur(string email, string motDePasse)
        {
            using (MySqlConnection con = new MySqlConnection(connexionString))
            {
                con.Open();
                
                string query = "SELECT * FROM utilisateurs WHERE email = @email AND mot_de_passe = @motDePasse";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@motDePasse", motDePasse);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Personne utilisateur = new Personne();
                        utilisateur.Id = Convert.ToInt32(reader["id"]);
                        utilisateur.Prenom = reader["prenom"].ToString();
                        utilisateur.Nom = reader["nom"].ToString();
                        utilisateur.Adresse = reader["adresse"].ToString();
                        utilisateur.StationMetro = reader["station_metro"].ToString();
                        utilisateur.Role = reader["role"].ToString();
                        utilisateur.Email = reader["email"].ToString();
                        
                        return utilisateur;
                    }
                }
            }
            return null;
        }
    }
}