use psi_LivinParis;
-- Peuplement de la table utilisateurs
INSERT INTO `utilisateurs` (`prenom`, `nom`, `adresse`, `station_metro`, `mot_de_passe`, `role`, `email`) VALUES
('Jean', 'Dupont', '123 rue de Paris', 'Châtelet', 'mdp123', 'client', 'jean.dupont@email.com'),
('Marie', 'Martin', '456 avenue des Champs', 'Opéra', 'mdp456', 'cuisinier', 'marie.martin@email.com'),
('Pierre', 'Durand', '789 boulevard Saint-Germain', 'Saint-Michel', 'mdp789', 'client_cuisinier', 'pierre.durand@email.com'),
('Sophie', 'Leroy', '321 rue de Rivoli', 'Louvre', 'mdp321', 'client', 'sophie.leroy@email.com'),
('Thomas', 'Moreau', '654 rue de la Paix', 'Concorde', 'mdp654', 'cuisinier', 'thomas.moreau@email.com');

-- Peuplement de la table stations_metro
INSERT INTO `stations_metro` (`nom`, `longitude`, `latitude`, `ligne`, `liens`, `listeligne`) VALUES
('Châtelet', 2.346900, 48.858400, '1', '2,4,7,11,14', '1,2,4,7,11,14'),
('Opéra', 2.331600, 48.870600, '3', '7,8', '3,7,8'),
('Saint-Michel', 2.343800, 48.853000, '4', '10', '4,10'),
('Louvre', 2.336400, 48.861200, '1', '7', '1,7'),
('Concorde', 2.321500, 48.865600, '1', '8,12', '1,8,12'),
('Bastille', 2.369100, 48.853200, '1', '5,8', '1,5,8'),
('Gare de Lyon', 2.373600, 48.844800, '1', '14', '1,14'),
('Nation', 2.395700, 48.848100, '1', '2,6,9', '1,2,6,9');

-- Peuplement de la table plat
INSERT INTO `plat` (`nom`, `prix`, `nb_de_personne`, `id_cuisinier`) VALUES
('Poulet rôti', 15.99, 2, 2),
('Pâtes carbonara', 12.50, 1, 2),
('Salade César', 8.99, 1, 3),
('Boeuf bourguignon', 18.75, 2, 5),
('Ratatouille', 11.25, 2, 2),
('Quiche lorraine', 9.99, 1, 3),
('Soupe à l\'oignon', 7.50, 1, 5),
('Tarte tatin', 6.99, 1, 2);

-- Peuplement de la table commande
INSERT INTO `commande` (`client_id`, `cuisinier_id`, `statut`, `total`) VALUES
(1, 2, 'En attente', 28.49),
(3, 2, 'En cours', 15.99),
(4, 5, 'Livré', 30.25),
(1, 3, 'En attente', 20.98),
(3, 5, 'En cours', 18.75);

-- Peuplement de la table lignecommande
INSERT INTO `lignecommande` (`commande_id`, `plat_id`, `quantite`, `date_livraison`, `lieu_livraison`) VALUES
(1, 1, 1, '2024-04-25 19:00:00', '123 rue de Paris'),
(1, 2, 1, '2024-04-25 19:00:00', '123 rue de Paris'),
(2, 1, 1, '2024-04-25 20:00:00', '789 boulevard Saint-Germain'),
(3, 4, 1, '2024-04-25 18:30:00', '321 rue de Rivoli'),
(3, 5, 1, '2024-04-25 18:30:00', '321 rue de Rivoli'),
(4, 3, 1, '2024-04-25 20:30:00', '123 rue de Paris'),
(4, 6, 1, '2024-04-25 20:30:00', '123 rue de Paris'),
(5, 4, 1, '2024-04-25 21:00:00', '789 boulevard Saint-Germain');

-- Voir tous les utilisateurs
SELECT * FROM utilisateurs;

-- Voir les commandes en cours
SELECT * FROM commande WHERE statut = 'En cours';

-- Voir les plats d'un cuisinier
SELECT * FROM plat WHERE id_cuisinier = 3;

-- Voir les détails d'une commande avec les plats
SELECT c.*, lc.quantite, p.nom, p.prix 
FROM commande c
JOIN lignecommande lc ON c.id = lc.commande_id
JOIN plat p ON lc.plat_id = p.id
WHERE c.id = 1;