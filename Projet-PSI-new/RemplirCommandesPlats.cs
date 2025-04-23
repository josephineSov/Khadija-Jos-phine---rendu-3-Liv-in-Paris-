using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_PSI_new
{
    public class RemplirCommandesPlats
    {
        private string connectionString = "SERVER=localhost;PORT=3306;DATABASE=LIVRABLE2;UID=root;PASSWORD=Root";

        public int CreerCommande(int clientId, int cuisinierId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Commande (client_id, cuisinier_id, statut, total) VALUES (@clientId, @cuisinierId, 'En attente', 0);";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@clientId", clientId);
                cmd.Parameters.AddWithValue("@cuisinierId", cuisinierId);
                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public int AjouterPlat(string nom, decimal prix, int nbPersonne, int idCuisinier)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Plat (nom, prix, nb_de_personne, id_cuisinier) VALUES (@nom, @prix, @nbPersonne, @idCuisinier);";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prix", prix);
                cmd.Parameters.AddWithValue("@nbPersonne", nbPersonne);
                cmd.Parameters.AddWithValue("@idCuisinier", idCuisinier);
                cmd.ExecuteNonQuery();
                return (int)cmd.LastInsertedId;
            }
        }

        public void AjouterLigneCommande(int commandeId, int platId, int quantite, DateTime dateLivraison, string lieuLivraison)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO LigneCommande (commande_id, plat_id, quantite, date_livraison, lieu_livraison) VALUES (@commandeId, @platId, @quantite, @dateLivraison, @lieuLivraison);";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@commandeId", commandeId);
                cmd.Parameters.AddWithValue("@platId", platId);
                cmd.Parameters.AddWithValue("@quantite", quantite);
                cmd.Parameters.AddWithValue("@dateLivraison", dateLivraison);
                cmd.Parameters.AddWithValue("@lieuLivraison", lieuLivraison);
                cmd.ExecuteNonQuery();
            }
        }

        public void MettreAJourTotalCommande(int commandeId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE Commande c
                             JOIN (SELECT commande_id, SUM(p.prix * lc.quantite) AS total FROM LigneCommande lc
                             JOIN Plat p ON lc.plat_id = p.id
                             WHERE lc.commande_id = @commandeId GROUP BY lc.commande_id) AS subquery
                             ON c.id = subquery.commande_id
                             SET c.total = subquery.total;";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@commandeId", commandeId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
