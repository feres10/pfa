# README — Fiche de test (API + Selenium) pour E_sante

Ce document explique ce qui a été mis en place pour tester ton projet .NET (backend + frontend) avec un notebook Python.

## 1) Fichiers ajoutés
- `FICHE_TEST.ipynb` : notebook de tests API + tests UI (Selenium).
- `FICHE_TEST.md` : fiche de test manuelle (checklist simple).

## 2) Objectif des tests
Valider rapidement (sans complexité) :
- l’inscription (register) fonctionne,
- le login/logout fonctionne,
- quelques opérations métier (ex: médecins / ordonnances) répondent,
- le frontend permet aussi de faire ces actions via l’UI.

## 3) Comment lancer (ordre recommandé)
1) Démarrer le backend (.NET) et le frontend.
2) Ouvrir `FICHE_TEST.ipynb`.
3) Exécuter les cellules du haut vers le bas.

> Important : l’UI (Selenium) a besoin de Chrome installé.

## 4) Ce que fait `FICHE_TEST.ipynb`
### A) Tests API (requests)
- `POST /api/auth/register` : inscription avec un email unique.
- `POST /api/auth/login` : connexion avec le nouveau compte, puis logout.
- Login admin ensuite, puis exemples métier :
  - `POST /api/medecins`
  - `GET /api/medecins`
  - `POST /api/ordonnances`
  - `POST /api/auth/logout`

Pourquoi un email unique ?
- Pour éviter l’erreur "Un compte avec cet email existe déjà." si tu relances le notebook.

### B) Tests UI (Selenium)
- 3 tests de navigation via boutons/liens (plus robustes) :
  - Login → Register (via lien "Créer un compte")
  - Register → Login (via lien "Se connecter")
  - Register → Home (via "Retour à l'accueil")
- Inscription UI sur `/register`.
- Login UI avec le nouveau compte, puis logout.
- Scénario admin UI (login admin, ajout médecin, ajout ordonnance, logout).

## 5) Variables de configuration (optionnel)
Tu peux changer les URLs et identifiants sans modifier le code :
- `API_URL` (défaut: `http://localhost:5139/api`)
- `FRONT_URL` (défaut: `http://localhost:7004`)
- `TEST_EMAIL` / `TEST_PASSWORD` : compte admin
- `NEW_EMAIL` / `NEW_PASSWORD` : compte patient à créer
- `HEADLESS=1` : exécuter Selenium en mode headless (sans ouvrir la fenêtre)

Le notebook génère aussi automatiquement :
- `RUN_ID` pour construire `NEW_EMAIL` si `NEW_EMAIL` n’est pas fourni.

## 6) Petites améliorations faites (pour la stabilité)
- Selenium utilise `webdriver-manager` : pas besoin d’installer ChromeDriver à la main.
- Ajout de fonctions de clic “fallback” :
  - clic sur `submit` si possible,
  - sinon clic par texte (ex: "Se connecter", "Créer un compte", "Ajouter").
- Attentes (`WebDriverWait`) pour éviter les tests “flaky”.

## 7) Dépannage rapide
### Erreur 404 sur `/register` ou `/login`
- Vérifie `FRONT_URL` et que le frontend est bien lancé.

### Erreur API (connexion refusée)
- Vérifie `API_URL` et que le backend est bien lancé.

### Selenium ne trouve pas un champ
- Les sélecteurs UI utilisent parfois des `placeholder` (car Blazor InputText n’a pas toujours `name/id`).
- Si tu changes le texte des placeholders ou le design, il faudra ajuster les sélecteurs.

### Inscription échoue (400)
- Le backend attend au minimum : `nom`, `prenom`, `email`, `password`.

---

Si tu veux, je peux :
- ajouter une cellule "Run All + résumé" (API puis UI) qui affiche un rapport final,
- ou adapter les sélecteurs Selenium exactement à ton HTML final (si tu me dis le texte exact des boutons/champs).
