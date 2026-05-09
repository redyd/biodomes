# Bio Domes

Projet d'application web pour l'UE19. Le projet prend la forme d'une solution .NET découpée en cinq projets :

- un projet d'application web Razor Page
- un projet de bibliothèque modélisant le domaine.
- un projet d'infrastructures, ce dernier reprenant notamment les éléments propres à EntityFrameworkCore.
- un projet de test permettant de tester le projet de bibliothèque modélisant le domaine.
- un projet de test permettant de tester le projet d'infrastructures.

La suite du document sera à compléter par vos soins.

## Membres de l'équipe

- Mattéo GEORGES Q240062
- Timëo SOEUR Q240078

Mattéo est responsable du déploiement, disponible https://ue19.cg.helmo.be/q240062.

## Construction de la solution

### Via dotnet 8

`dotnet run --project BioDomes.Web`

### Via docker

`docker compose up --build`

L'application démarra sur le port `5298`.

## Fonctionnalités implémentées

- US1: totalement achevée
- US2: totalement achevée
- US3: totalement achevée
- US4: totalement achevée
- US5: totalement achevée
- US6: totalement achevée
- US7: totalement achevée
- US8: totalement achevée
- US9: totalement achevée
- US10: totalement achevée (la page d'accueil affiche les 3 meilleurs biômes en équilibre)
- US11: totalement achevée
- US12: totalement achevée
- US13: totalement achevée
- US14: partiellement achevée (pas de propositions d'équipements)
- US15: totalement achevée
- US16: totalement achevée

## Données de connexion

- [ADMIN] email: admin@biodomes.com | pwd: Password1234!
- [USER] email: john@biodomes.com | pwd: Password1234!
- [USER] email: bob@biodomes.com | pwd: Password1234!

## Éléments techniques notables

- Utilisation de Cytoscape.js, Chart.js et Hammer.js pour les graphiques.
- Modélisation d'une chaine alimentaire globale utilisée pour créer des sous-chaine au sein des biomes.
