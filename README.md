# Rapport projet PSI - Livrable 2

## Informations Utiles

- **Log In** : `localhost`
- **Password** : 
  - `ESILV1234` (Thibaut Texier)  
  - `Root` (Joséphine Sauvé)
- **GitHub** : [LeTibz2](https://github.com/LeThibz2/Projet-PSI-new)

## Contexte du Projet

Dans le cadre du projet **Liv’In Paris**, nous étudions l'optimisation des trajets dans les graphes pondérés appliquée aux stations de métro de la ville de Paris intra-muros. 

Plusieurs algorithmes permettent de calculer le plus court chemin entre des sommets (ici les stations de métro) :
- **Dijkstra**
- **Bellman-Ford**
- **Floyd-Warshall**

Nous allons comparer ces trois algorithmes en fonction de plusieurs critères :
- Complexité algorithmique
- Rapidité d’exécution
- Pertinence pour notre cas d’application

L’objectif est de déterminer **quel algorithme est le plus approprié** en fonction du contexte du projet **Liv’In Paris**.

## Comparaison des Algorithmes

### 1. Algorithme de Floyd-Warshall

- **Complexité** : $O(n^3)$
- **Utilisation** : Calcul des plus courts chemins entre toutes les paires de sommets.
- **Inconvénients** :
  - Inefficace pour les grands graphes comme le réseau de métro parisien.
  - Temps d’exécution trop important pour un grand nombre de stations.

#### Code :
```python
# Implémentation de Floyd-Warshall
```

---

### 2. Algorithme de Bellman-Ford

- **Complexité** : $O(n^3)$ dans le pire des cas.
- **Utilisation** : Fonctionne avec des poids négatifs et détecte les cycles absorbants.
- **Inconvénients** :
  - Pas pertinent pour notre application car les temps de trajet sont toujours positifs.
  - Plus lent que Dijkstra pour un réseau dense comme celui du métro parisien.

#### Code :
```python
# Implémentation de Bellman-Ford
```

---

### 3. Algorithme de Dijkstra

- **Complexité** : $O(n^2)$
- **Utilisation** : Recherche du plus court chemin entre une station de départ et une station d’arrivée.
- **Avantages** :
  - Rapide et efficace pour les graphes bien structurés comme un réseau de transport en commun.
  - S'applique aux graphes avec des poids positifs, ce qui correspond à notre projet.

#### Code :
```python
# Implémentation de Dijkstra
```

---

## Conclusion

L'algorithme **Dijkstra** est le **plus adapté** au projet **Liv’In Paris**. 

- Il offre la **meilleure rapidité d’exécution**.
- Il est **efficace pour trouver le chemin le plus court** entre une station de départ et une station d’arrivée.

Ainsi, **Dijkstra est le meilleur choix** pour optimiser les trajets dans le réseau de métro parisien. 🚇
