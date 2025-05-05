-- Insertion des cuisiniers
INSERT INTO personne (prenom, nom, email, adresse, station_metro, mot_de_passe, role)
VALUES 
('Jean', 'Dupont', 'jean.dupont@email.com', '1 rue de la Paix', 'Châtelet', 'motdepasse1', 'cuisinier'),
('Marie', 'Martin', 'marie.martin@email.com', '2 avenue des Champs', 'République', 'motdepasse2', 'cuisinier'),
('Pierre', 'Bernard', 'pierre.bernard@email.com', '3 boulevard Saint-Germain', 'Bastille', 'motdepasse3', 'cuisinier'),
('Sophie', 'Petit', 'sophie.petit@email.com', '4 rue de Rivoli', 'Opéra', 'motdepasse4', 'cuisinier');

-- Insertion des clients
INSERT INTO personne (prenom, nom, email, adresse, station_metro, mot_de_passe, role)
VALUES 
('Alice', 'Durand', 'alice.durand@email.com', '5 rue de la Pompe', 'Trocadéro', 'motdepasse5', 'client'),
('Thomas', 'Leroy', 'thomas.leroy@email.com', '6 avenue Mozart', 'Saint-Lazare', 'motdepasse6', 'client'),
('Emma', 'Moreau', 'emma.moreau@email.com', '7 rue de Rome', 'Montparnasse', 'motdepasse7', 'client'),
('Lucas', 'Simon', 'lucas.simon@email.com', '8 boulevard Haussmann', 'Madeleine', 'motdepasse8', 'client');

-- Insertion des plats
INSERT INTO plat (nom, prix, nb_de_personne, cuisinier_id)
VALUES 
('Poulet rôti', 15.99, 2, 1),
('Bœuf bourguignon', 18.50, 4, 1),
('Lasagnes', 12.99, 3, 2),
('Pizza margherita', 10.50, 2, 2),
('Salade niçoise', 8.99, 1, 3),
('Ratatouille', 9.50, 2, 3),
('Tarte aux pommes', 7.99, 4, 4),
('Crème brûlée', 6.50, 1, 4);

-- Insertion des commandes
INSERT INTO commande (client_id, cuisinier_id, statut, total)
VALUES 
(5, 1, 'en cours', 15.99),  -- Alice commande un poulet rôti
(5, 2, 'en cours', 12.99),  -- Alice commande des lasagnes
(6, 1, 'en cours', 18.50),  -- Thomas commande un bœuf bourguignon
(6, 3, 'en cours', 8.99),   -- Thomas commande une salade niçoise
(7, 2, 'en cours', 10.50),  -- Emma commande une pizza
(7, 4, 'en cours', 7.99),   -- Emma commande une tarte aux pommes
(8, 3, 'en cours', 9.50),   -- Lucas commande une ratatouille
(8, 4, 'en cours', 6.50);   -- Lucas commande une crème brûlée

-- Insertion des détails de commande
INSERT INTO detail_commande (commande_id, plat_id, quantite)
VALUES 
(1, 1, 1),  -- Poulet rôti pour Alice
(2, 3, 1),  -- Lasagnes pour Alice
(3, 2, 1),  -- Bœuf bourguignon pour Thomas
(4, 5, 1),  -- Salade niçoise pour Thomas
(5, 4, 1),  -- Pizza pour Emma
(6, 7, 1),  -- Tarte aux pommes pour Emma
(7, 6, 1),  -- Ratatouille pour Lucas
(8, 8, 1);  -- Crème brûlée pour Lucas 