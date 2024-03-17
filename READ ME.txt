MOVIES

Movies est une application Web Blazor qui permet aux utilisateurs de rechercher des films et de les sauvegarder dans une liste personnelle. L'application utilise une API externe TMBd pour la recherche de films et stocke les informations de la liste des films sauvegardés dans le stockage local du navigateur pour une persistance côté client.

FONCTIONNALITES

-Recherche de films sur base de mots via une API externe TMBd.
-Affichage des résultats de recherche (titre du film et résumé).
-Sauvegarde des films dans une liste personnelle.
-Stockage de la liste des films sauvegardés dans le stockage local du navigateur.
-Possibilité d'éditer le titre du film et de supprimer le film de la liste des films sauvegardé.

TECHNOLOGIE

- Blazor WebAssembly : pour une expérience côté client en utilisant C# et Razor.
- Blazored.LocalStorage : pour le stockage local.
- System.Text.Json : pour la sérialisation et la désérialisation des données JSON.

STRUCTURE DU PROJET

- Models : contient les modèles de données utilisés dans l'application.
- Services : contient les services qui gèrent la logique métier comme la recherche de films et la gestion de la liste sauvegardée.
- Pages : contient les composants Razor pour les différentes pages de l'application.
- wwwroot : contient les fichiers statiques tels que les styles CSS et les images.
- Movies.Tests : contient les tests unitaires et d'intégration pour s'assurer de la qualité du code et de la fiabilité des fonctionnalités.

DEMARRAGE RAPIDE

-Clonez le dépôt du projet.
- Ouvrez le projet dans votre IDE préféré (par exemple, Visual Studio).
- Restaurez les packages NuGet.
- Démarrez l'application avec dotnet run ou en utilisant l'IDE.
- Visitez https://localhost:7162 pour voir l'application en action.

