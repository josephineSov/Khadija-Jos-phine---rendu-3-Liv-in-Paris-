DROP DATABASE IF EXISTS psi_LivinParis;
CREATE DATABASE psi_LivinParis;
USE psi_LivinParis;
-- Suppression des tables existantes
DROP TABLE IF EXISTS `lignecommande`;
DROP TABLE IF EXISTS `commande`;
DROP TABLE IF EXISTS `plat`;
DROP TABLE IF EXISTS `clients`;
DROP TABLE IF EXISTS `cuisiniers`;
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

-- Création de la table lignecommande
CREATE TABLE `lignecommande` (
  `id` int NOT NULL AUTO_INCREMENT,
  `commande_id` int DEFAULT NULL,
  `plat_id` int DEFAULT NULL,
  `quantite` int DEFAULT 1,
  `date_livraison` datetime DEFAULT NULL,
  `lieu_livraison` text,
  PRIMARY KEY (`id`),
  KEY `commande_id` (`commande_id`),
  KEY `plat_id` (`plat_id`),
  FOREIGN KEY (`commande_id`) REFERENCES `commande` (`id`) ON DELETE CASCADE,
  FOREIGN KEY (`plat_id`) REFERENCES `plat` (`id`) ON DELETE CASCADE
);


