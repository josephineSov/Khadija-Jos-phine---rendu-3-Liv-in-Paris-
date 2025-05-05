use psi_LivinParis;
-- Peuplement de la table utilisateurs
INSERT INTO `utilisateurs` (`prenom`, `nom`, `adresse`, `station_metro`, `mot_de_passe`, `role`, `email`) VALUES
('Jean', 'Dupont', '123 rue de Paris', 'Châtelet', 'mdp123', 'client', 'jean.dupont@email.com'),
('Marie', 'Martin', '456 avenue des Champs', 'Opéra', 'mdp456', 'cuisinier', 'marie.martin@email.com'),
('Pierre', 'Durand', '789 boulevard Saint-Germain', 'Saint-Michel', 'mdp789', 'client_cuisinier', 'pierre.durand@email.com'),
('Sophie', 'Leroy', '321 rue de Rivoli', 'Louvre', 'mdp321', 'client', 'sophie.leroy@email.com'),
('Thomas', 'Moreau', '654 rue de la Paix', 'Concorde', 'mdp654', 'cuisinier', 'thomas.moreau@email.com'),
('Lucie', 'Bernard', '147 rue du Temple', 'Bastille', 'mdp147', 'client', 'lucie.bernard@email.com'),
('Marc', 'Petit', '258 rue Saint-Antoine', 'Nation', 'mdp258', 'cuisinier', 'marc.petit@email.com'),
('Emma', 'Richard', '369 avenue Daumesnil', 'Gare de Lyon', 'mdp369', 'client_cuisinier', 'emma.richard@email.com'),
('Lucas', 'Simon', '741 rue de la Roquette', 'Bastille', 'mdp741', 'client', 'lucas.simon@email.com'),
('Julie', 'Laurent', '852 boulevard Voltaire', 'Nation', 'mdp852', 'cuisinier', 'julie.laurent@email.com');

-- Voir tous les utilisateurs
SELECT * FROM utilisateurs;

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

-- Voir toutes les stations de métro
SELECT * FROM stations_metro;

-- Peuplement de la table plat
INSERT INTO `plat` (`nom`, `prix`, `nb_de_personne`, `id_cuisinier`) VALUES
('Poulet rôti', 15.99, 2, 2),
('Pâtes carbonara', 12.50, 1, 2),
('Salade César', 8.99, 1, 3),
('Boeuf bourguignon', 18.75, 2, 5),
('Ratatouille', 11.25, 2, 2),
('Quiche lorraine', 9.99, 1, 3),
('Soupe à l\'oignon', 7.50, 1, 5),
('Tarte tatin', 6.99, 1, 2),
('Couscous royal', 22.99, 2, 7),
('Paella valenciana', 24.50, 2, 8),
('Sushi mix', 19.99, 1, 10),
('Curry de poulet', 16.75, 1, 2),
('Pizza margherita', 13.99, 1, 3),
('Moules marinières', 17.50, 2, 5),
('Lasagnes', 15.99, 2, 7);

-- Voir tous les plats
SELECT * FROM plat;

-- Peuplement de la table commande
INSERT INTO `commande` (`client_id`, `cuisinier_id`, `statut`, `date_creation`, `total`) VALUES
(3, 2, 'En cours', '2024-03-21 15:45:00', 15.99),
(4, 5, 'Livré', '2024-03-22 18:20:00', 30.25),
(3, 5, 'En cours', '2024-03-24 14:30:00', 18.75),
(4, 8, 'En cours', '2024-03-26 17:45:00', 33.50),
(3, 3, 'En cours', '2024-03-29 16:30:00', 22.50);

-- Voir toutes les commandes
SELECT * FROM commande;

-- Peuplement de la table livraison
INSERT INTO `livraison` (`adresse_depart`, `adresse_arrivee`, `date_livraison`, `statut_livraison`, `heure_disponible`, `zone_geographique`, `cuisinier_id`) VALUES
('456 avenue des Champs', '123 rue de Paris', '2024-04-25 19:00:00', 'En cours', '19:00:00', 'Paris Centre', 2),
('654 rue de la Paix', '789 boulevard Saint-Germain', '2024-04-25 20:00:00', 'En attente', '20:00:00', 'Paris Sud', 5),
('258 rue Saint-Antoine', '321 rue de Rivoli', '2024-04-25 18:30:00', 'Livrée', '18:30:00', 'Paris Est', 7),
('369 avenue Daumesnil', '147 rue du Temple', '2024-04-26 19:30:00', 'En attente', '19:30:00', 'Paris Est', 8),
('852 boulevard Voltaire', '741 rue de la Roquette', '2024-04-26 20:30:00', 'En cours', '20:30:00', 'Paris Est', 10),
('258 rue Saint-Antoine', '456 avenue des Champs', '2024-03-25 13:30:00', 'Livrée', '13:30:00', 'Paris Centre', 7),
('369 avenue Daumesnil', '321 rue de Rivoli', '2024-03-26 18:15:00', 'En cours', '18:15:00', 'Paris Est', 8),
('852 boulevard Voltaire', '123 rue de Paris', '2024-03-27 20:30:00', 'En attente', '20:30:00', 'Paris Nord', 10),
('456 avenue des Champs', '654 rue de la Paix', '2024-03-28 12:45:00', 'Livrée', '12:45:00', 'Paris Ouest', 2),
('789 boulevard Saint-Germain', '789 boulevard Saint-Germain', '2024-03-29 17:00:00', 'En cours', '17:00:00', 'Paris Sud', 3);

-- Voir toutes les livraisons
SELECT * FROM livraison;

-- Voir les IDs des commandes existantes
SELECT id, date_creation, total FROM commande ORDER BY id;

-- Peuplement de la table Notes
INSERT INTO `Notes` (`commentaire_note`, `date_avis`, `note_retour`, `commande_id`) VALUES
('Livraison rapide, nourriture encore chaude', '2024-04-25 21:00:00', 4, 1),
('Bon rapport qualité-prix', '2024-04-25 19:30:00', 4, 2),
('Plats savoureux mais livraison un peu tardive', '2024-04-25 22:00:00', 3, 3),
('Parfait en tout point', '2024-04-25 21:30:00', 5, 4),
('Très satisfait du service', '2024-03-29 18:00:00', 4, 5);

-- Voir toutes les notes
SELECT * FROM Notes;

-- Peuplement de la table Ingredients_principaux
INSERT INTO `Ingredients_principaux` (`ingredient`, `volume`) VALUES
('Poulet fermier', 1.5),
('Riz basmati', 1.0),
('Tomates fraîches', 0.8),
('Parmesan', 0.3),
('Fruits de mer mix', 1.2),
('Boeuf haché', 1.0),
('Légumes assortis', 2.0),
('Pâtes fraîches', 1.0),
('Épices mélangées', 0.2),
('Sauce tomate', 1.5);

-- Voir tous les ingrédients
SELECT * FROM Ingredients_principaux;

-- Peuplement de la table Nourriture
INSERT INTO `Nourriture` (`type_nourriture`, `nom_nourriture`, `nb_personnes`, `nombre_portions_disponibles`, 
                         `date_fabrication`, `date_peremption`, `prix_par_personne`, `nationalite_cuisine`, 
                         `regime_alimentaire`, `photo_nourriture`) VALUES
('Plat principal', 'Couscous Royal', 2, 5, '2024-04-25 10:00:00', '2024-04-27 10:00:00', 12.50, 'Marocaine', 'Halal', 'couscous.jpg'),
('Entrée', 'Salade Niçoise', 1, 8, '2024-04-25 09:00:00', '2024-04-26 09:00:00', 8.00, 'Française', 'Végétarien', 'salade.jpg'),
('Plat principal', 'Paella', 2, 3, '2024-04-25 11:00:00', '2024-04-27 11:00:00', 14.00, 'Espagnole', 'Fruits de mer', 'paella.jpg'),
('Dessert', 'Tiramisu', 1, 10, '2024-04-25 08:00:00', '2024-04-27 08:00:00', 6.50, 'Italienne', 'Végétarien', 'tiramisu.jpg'),
('Plat principal', 'Curry Vert', 1, 6, '2024-04-25 12:00:00', '2024-04-27 12:00:00', 11.00, 'Thaïlandaise', 'Épicé', 'curry.jpg'),
('Entrée', 'Sushi Mix', 1, 7, '2024-04-25 09:30:00', '2024-04-26 09:30:00', 15.00, 'Japonaise', 'Poisson', 'sushi.jpg'),
('Plat principal', 'Lasagnes', 2, 4, '2024-04-25 11:30:00', '2024-04-27 11:30:00', 10.00, 'Italienne', 'Standard', 'lasagnes.jpg'),
('Dessert', 'Crème Brûlée', 1, 12, '2024-04-25 08:30:00', '2024-04-27 08:30:00', 5.50, 'Française', 'Végétarien', 'creme.jpg');

-- Voir toute la nourriture
SELECT * FROM Nourriture;

-- Peuplement de la table nourriture_ingredients
INSERT INTO `nourriture_ingredients` (`nourriture_id`, `ingredient_id`) VALUES
(1, 1), -- Couscous - Poulet
(1, 7), -- Couscous - Légumes
(2, 3), -- Salade - Tomates
(3, 5), -- Paella - Fruits de mer
(3, 2), -- Paella - Riz
(4, 4), -- Tiramisu - Parmesan
(5, 1), -- Curry - Poulet
(5, 9), -- Curry - Épices
(6, 5), -- Sushi - Fruits de mer
(7, 6), -- Lasagnes - Boeuf
(7, 8), -- Lasagnes - Pâtes
(7, 10); -- Lasagnes - Sauce tomate

-- Voir toutes les associations nourriture-ingrédients
SELECT * FROM nourriture_ingredients;

-- Peuplement de la table commande_nourriture
INSERT INTO `commande_nourriture` (`commande_id`, `nourriture_id`) VALUES
(1, 3),
(2, 2),
(2, 5),
(3, 6),
(3, 8),
(4, 5),
(4, 6),
(5, 1),
(5, 5);

-- Voir toutes les associations commande-nourriture
SELECT * FROM commande_nourriture;

-- Peuplement de la table lignecommande
INSERT INTO `lignecommande` (`commande_id`, `plat_id`, `quantite`, `date_livraison`, `lieu_livraison`) VALUES
(1, 1, 1, '2024-04-25 20:00:00', '789 boulevard Saint-Germain'),
(2, 4, 1, '2024-04-25 18:30:00', '321 rue de Rivoli'),
(2, 5, 1, '2024-04-25 18:30:00', '321 rue de Rivoli'),
(3, 4, 1, '2024-04-25 21:00:00', '789 boulevard Saint-Germain'),
(4, 11, 1, '2024-03-26 18:15:00', '321 rue de Rivoli'),
(4, 12, 2, '2024-03-26 18:15:00', '321 rue de Rivoli'),
(5, 2, 2, '2024-03-29 17:00:00', '789 boulevard Saint-Germain'),
(5, 3, 1, '2024-03-29 17:00:00', '789 boulevard Saint-Germain');

-- Voir toutes les lignes de commande
SELECT * FROM lignecommande;

-- Mise à jour des lignecommande avec les livraisons
UPDATE `lignecommande` SET `livraison_id` = 2 WHERE `commande_id` = 1;
UPDATE `lignecommande` SET `livraison_id` = 3 WHERE `commande_id` = 2;
UPDATE `lignecommande` SET `livraison_id` = 4 WHERE `commande_id` = 3;
UPDATE `lignecommande` SET `livraison_id` = 5 WHERE `commande_id` = 4;
UPDATE `lignecommande` SET `livraison_id` = 7 WHERE `commande_id` = 5;

-- Voir les lignes de commande après mise à jour
SELECT * FROM lignecommande;

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
