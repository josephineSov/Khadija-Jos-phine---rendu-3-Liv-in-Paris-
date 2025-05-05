DROP DATABASE IF EXISTS psi_LivinParis;
CREATE DATABASE psi_LivinParis;
USE psi_LivinParis;

-- Suppression des tables existantes
DROP TABLE IF EXISTS `nourriture_ingredients`;
DROP TABLE IF EXISTS `commande_nourriture`;
DROP TABLE IF EXISTS `lignecommande`;
DROP TABLE IF EXISTS `Notes`;
DROP TABLE IF EXISTS `commande`;
DROP TABLE IF EXISTS `plat`;
DROP TABLE IF EXISTS `livraison`;
DROP TABLE IF EXISTS `Ingredients_principaux`;
DROP TABLE IF EXISTS `Nourriture`;
DROP TABLE IF EXISTS `utilisateurs`;
DROP TABLE IF EXISTS `stations_metro`;

-- Création de la table utilisateurs
CREATE TABLE `utilisateurs` (
  `id` int NOT NULL AUTO_INCREMENT,
  `prenom` varchar(50) NOT NULL,
  `nom` varchar(50) NOT NULL,
  `adresse` varchar(255) DEFAULT NULL,
  `station_metro` varchar(100) DEFAULT NULL,
  `mot_de_passe` varchar(255) NOT NULL,
  `role` enum('client','cuisinier','client_cuisinier') NOT NULL,
  `email` varchar(100) NOT NULL UNIQUE,
  PRIMARY KEY (`id`)
);

-- Création de la table stations_metro
CREATE TABLE `stations_metro` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nom` varchar(255) NOT NULL,
  `longitude` decimal(9,6) NOT NULL,
  `latitude` decimal(9,6) NOT NULL,
  `ligne` varchar(50) NOT NULL,
  `liens` varchar(255) DEFAULT NULL,
  `listeligne` varchar(150) NOT NULL,
  PRIMARY KEY (`id`)
);

-- Création de la table livraison
CREATE TABLE `livraison` (
  `id_livraison` int NOT NULL AUTO_INCREMENT,
  `adresse_depart` varchar(255) NOT NULL,
  `adresse_arrivee` varchar(255) NOT NULL,
  `date_livraison` datetime NOT NULL,
  `statut_livraison` enum('En attente', 'En cours', 'Livrée', 'Annulée') NOT NULL,
  `heure_disponible` time NOT NULL,
  `zone_geographique` varchar(100) NOT NULL,
  `cuisinier_id` int DEFAULT NULL,
  PRIMARY KEY (`id_livraison`),
  KEY `cuisinier_id` (`cuisinier_id`),
  FOREIGN KEY (`cuisinier_id`) REFERENCES `utilisateurs` (`id`) ON DELETE SET NULL
);

-- Création de la table plat
CREATE TABLE `plat` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nom` varchar(50) NOT NULL,
  `prix` decimal(6,2) NOT NULL,
  `nb_de_personne` int NOT NULL,
  `id_cuisinier` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id_cuisinier` (`id_cuisinier`),
  FOREIGN KEY (`id_cuisinier`) REFERENCES `utilisateurs` (`id`) ON DELETE SET NULL
);

-- Création de la table commande
CREATE TABLE `commande` (
  `id` int NOT NULL AUTO_INCREMENT,
  `client_id` int DEFAULT NULL,
  `cuisinier_id` int DEFAULT NULL,
  `statut` enum('En attente','En cours','Livré','Annulé') DEFAULT 'En attente',
  `date_creation` datetime DEFAULT CURRENT_TIMESTAMP,
  `total` decimal(8,2) DEFAULT 0.00,
  PRIMARY KEY (`id`),
  KEY `client_id` (`client_id`),
  KEY `cuisinier_id` (`cuisinier_id`),
  FOREIGN KEY (`client_id`) REFERENCES `utilisateurs` (`id`) ON DELETE CASCADE,
  FOREIGN KEY (`cuisinier_id`) REFERENCES `utilisateurs` (`id`) ON DELETE CASCADE
);

-- Création de la table Notes
CREATE TABLE `Notes` (
  `id_note` int NOT NULL AUTO_INCREMENT,
  `commentaire_note` text,
  `date_avis` datetime DEFAULT CURRENT_TIMESTAMP,
  `note_retour` int,
  `commande_id` int NOT NULL,
  PRIMARY KEY (`id_note`),
  KEY `commande_id` (`commande_id`),
  FOREIGN KEY (`commande_id`) REFERENCES `commande` (`id`) ON DELETE CASCADE
);

-- Création de la table lignecommande
CREATE TABLE `lignecommande` (
  `id` int NOT NULL AUTO_INCREMENT,
  `commande_id` int DEFAULT NULL,
  `plat_id` int DEFAULT NULL,
  `quantite` int DEFAULT 1,
  `date_livraison` datetime DEFAULT NULL,
  `lieu_livraison` text,
  `livraison_id` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `commande_id` (`commande_id`),
  KEY `plat_id` (`plat_id`),
  KEY `livraison_id` (`livraison_id`),
  FOREIGN KEY (`commande_id`) REFERENCES `commande` (`id`) ON DELETE CASCADE,
  FOREIGN KEY (`plat_id`) REFERENCES `plat` (`id`) ON DELETE CASCADE,
  FOREIGN KEY (`livraison_id`) REFERENCES `livraison` (`id_livraison`) ON DELETE SET NULL
);

-- Création de la table Ingredients_principaux
CREATE TABLE `Ingredients_principaux` (
  `id_Ingredients_principaux` int NOT NULL AUTO_INCREMENT,
  `ingredient` varchar(100) NOT NULL,
  `volume` decimal(10,2),
  PRIMARY KEY (`id_Ingredients_principaux`)
);

-- Création de la table Nourriture
CREATE TABLE `Nourriture` (
  `id_Nourriture` int NOT NULL AUTO_INCREMENT,
  `type_nourriture` varchar(50) NOT NULL,
  `nom_nourriture` varchar(100) NOT NULL,
  `nb_personnes` int NOT NULL,
  `nombre_portions_disponibles` int NOT NULL,
  `date_fabrication` datetime NOT NULL,
  `date_peremption` datetime NOT NULL,
  `prix_par_personne` decimal(10,2) NOT NULL,
  `nationalite_cuisine` varchar(50) NOT NULL,
  `regime_alimentaire` varchar(50),
  `photo_nourriture` varchar(255),
  PRIMARY KEY (`id_Nourriture`)
);

-- Table de liaison entre Nourriture et Ingredients_principaux
CREATE TABLE `nourriture_ingredients` (
  `nourriture_id` int NOT NULL,
  `ingredient_id` int NOT NULL,
  PRIMARY KEY (`nourriture_id`, `ingredient_id`),
  FOREIGN KEY (`nourriture_id`) REFERENCES `Nourriture` (`id_Nourriture`) ON DELETE CASCADE,
  FOREIGN KEY (`ingredient_id`) REFERENCES `Ingredients_principaux` (`id_Ingredients_principaux`) ON DELETE CASCADE
);

-- Table de liaison entre Commande et Nourriture
CREATE TABLE `commande_nourriture` (
  `commande_id` int NOT NULL,
  `nourriture_id` int NOT NULL,
  PRIMARY KEY (`commande_id`, `nourriture_id`),
  FOREIGN KEY (`commande_id`) REFERENCES `commande` (`id`) ON DELETE CASCADE,
  FOREIGN KEY (`nourriture_id`) REFERENCES `Nourriture` (`id_Nourriture`) ON DELETE CASCADE
);

