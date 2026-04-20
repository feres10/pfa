namespace E_santeBackend.Shared.Constants
{
    public static class AppConstants
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Medecin = "Medecin";
            public const string Pharmacien = "Pharmacien";
            public const string Patient = "Patient";
        }

        public static class StatutOrdonnance
        {
            public const string EnAttente = "EnAttente";
            public const string Traitee = "Traitee";
            public const string Annulee = "Annulee";
        }

        public static class StatutPaiement
        {
            public const string EnAttente = "EnAttente";
            public const string Paye = "Paye";
            public const string Annule = "Annule";
        }

        public static class TypeStockMouvement
        {
            public const string Entree = "Entree";
            public const string Sortie = "Sortie";
        }

        public static class JwtClaims
        {
            public const string UserId = "userId";
            public const string Email = "email";
            public const string Role = "role";
        }
    }
}