using System.Collections.Generic;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class Constants
    {
        // -------------------------------- NOMBRE DE LA BASE DE DATOS ---------------------------------
        public const string DATABASE_NAME = "tacticalElevenDB.db";

        // ----------------------------------- VALOR DE LA MONEDA --------------------------------------
        public const string EURO_NAME = "EUR";
        public const float EURO_VALUE = 1f;
        public const string EURO_SYMBOL = "€";
        public const string POUND_NAME = "GBP";
        public const float POUND_VALUE = 0.87f;
        public const string POUND_SYMBOL = "£";
        public const string DOLLAR_NAME = "USD";
        public const float DOLLAR_VALUE = 1.15f;
        public const string DOLLAR_SYMBOL = "$";

        // ------------------------------- PARÁMETROS VOLUMEN AUDIOMIXER -------------------------------
        public const string MASTER_VOLUME_PARAMETER = "MasterVolumeParameter";
        public const string MUSIC_VOLUME_PARAMETER = "MusicVolumeParameter";
        public const string SFX_VOLUME_PARAMETER = "SFXVolumeParameter";

        // ----------------------------------- NOMBRE DE LOS BOTONES -----------------------------------
        public const string MANAGER_MODE_BUTTON = "manager-mode";
        public const string CAREER_MODE_BUTTON = "career-mode";
        public const string SETTINGS_BUTTON = "settings";
        public const string EDITOR_BUTTON = "editor";
        public const string CARGAR_BUTTON = "cargar-partida";
        public const string CREDITS_BUTTON = "credits-icon";
        public const string EXIT_BUTTON = "exit-icon";
        public const string WEB_BUTTON = "web-icon";
        public const string BACK_BUTTON = "btnVolver";
        public const string CONTINUE_BUTTON = "btnSeguir";

        // -------------------------------- NOMBRE DE ESCENAS DEL JUEGO --------------------------------
        public const string INTRO_SCENE = "Intro";
        public const string MAIN_MENU_SCENE = "MainMenu";
        public const string CREDITS_SCENE = "Credits";
        public const string CREATE_MANAGER_SCENE = "CreateManager";
        public const string TEAM_SELECTION_SCENE = "TeamSelection";
        public const string PRE_SEASON_SCENE = "PreSeason";
        public const string TEAM_OBJECTIVES_SCENE = "TeamObjectives";
        public const string MAIN_SCREEN_SCENE = "MainScreen";
        public const string LOAD_SCREEN_SCENE = "CargarPartida";
        public const string SETTINGS_SCREEN_SCENE = "Settings";

        // -------------------------------------- NACIONALIDADES ---------------------------------------
        public static List<string> ObtenerTodasLasNacionalidades()
        {
            return new List<string>
            {
                "España", "Albania", "Alemania", "Andorra", "Angola", "Antigua y Barbuda", "Arabia Saudita",
                "Argelia", "Argentina", "Armenia", "Australia", "Austria", "Azerbaiyán", "Bahamas", "Bangladesh",
                "Barbados", "Baréin", "Bélgica", "Belice", "Benín", "Bielorrusia", "Birmania", "Bolivia",
                "Bosnia y Herzegovina", "Botsuana", "Brasil", "Brunéi", "Bulgaria", "Burkina Faso", "Burundi",
                "Bután", "Cabo Verde", "Camboya", "Camerún", "Canadá", "Chad", "Chile", "China", "Chipre",
                "Colombia", "Comoras", "Corea del Norte", "Corea del Sur", "Costa Rica", "Costa de Marfil", "Croacia",
                "Cuba", "Curazao", "Dinamarca", "Dominica", "Ecuador", "Egipto", "El Salvador", "Emiratos Árabes Unidos",
                "Eritrea", "Escocia", "Eslovaquia", "Eslovenia", "Estados Unidos", "Estonia", "Eswatini",
                "Etiopía", "Fiji", "Filipinas", "Finlandia", "Francia", "Gabón", "Gales", "Gambia", "Georgia", "Ghana",
                "Granada", "Grecia", "Guatemala", "Guinea", "Guinea-Bisáu", "Guyana", "Haití", "Honduras", "Hungría",
                "India", "Indonesia", "Inglaterra", "Irak", "Irán", "Irlanda", "Islandia", "Islas Feroe",
                "Islas Marshall", "Islas Salomón", "Israel", "Italia", "Jamaica", "Japón", "Jordania", "Kazajistán",
                "Kenia", "Kirguistán", "Kiribati", "Kosovo", "Kuwait", "Laos", "Lesoto", "Letonia", "Líbano", "Liberia",
                "Libia", "Liechtenstein", "Lituania", "Luxemburgo", "Macedonia del Norte", "Madagascar", "Malasia",
                "Malawi", "Maldivas", "Mali", "Malta", "Moldavia", "Marruecos", "Martinica", "Mauricio", "Mauritania",
                "México", "Micronesia", "Mónaco", "Mongolia", "Montenegro", "Mozambique", "Namibia", "Nauru", "Nepal",
                "Nicaragua", "Níger", "Nigeria", "Noruega", "Nueva Zelanda", "Omán", "Países Bajos", "Pakistán",
                "Palaos", "Panamá", "Papúa Nueva Guinea", "Paraguay", "Perú", "Polonia", "Portugal", "Reino Unido",
                "República Checa", "República del Congo", "República Dominicana", "Ruanda", "Rumanía", "Rusia",
                "Samoa", "San Cristóbal y Nieves", "San Marino", "Santo Tomé y Príncipe", "Senegal", "Serbia",
                "Seychelles", "Sierra Leona", "Singapur", "Siria", "Somalia", "Sri Lanka", "Sudáfrica", "Sudán",
                "Sudán del Sur", "Suecia", "Suiza", "Surinam", "Sáhara Occidental", "Tailandia", "Tayikistán",
                "Tanzania", "Togo", "Tonga", "Trinidad y Tobago", "Túnez", "Turkmenistán", "Turquía", "Tuvalu",
                "Ucrania", "Uganda", "Uruguay", "Uzbekistán", "Vanuatu", "Vaticano", "Venezuela", "Vietnam", "Yemen",
                "Yibuti", "Zambia", "Zimbabue"
            };
        }

        public static readonly Dictionary<string, string> PaisesPorColor = new Dictionary<string, string>
        {
            { "#6B1100", "TUR" },
            { "#DECD31", "ESP" },
            { "#1C4910", "POR" },
            { "#008404", "ITA" },
            { "#023FE6", "FRA" },
            { "#F7BBBB", "ING" },
            { "#0028BB", "ESC" },
            { "#C3A720", "BEL" },
            { "#DB5E0D", "HOL" },
            { "#EF8F84", "SUI" },
            { "#D92E2E", "AUT" },
            { "#FFF100", "UCR" },
            { "#4F7CD9", "GRE" },
            { "#0097BD", "CRO" },
            { "#C23E15", "MON" },
            { "#9C2C2D", "NOR" },
            { "#ADA210", "SUE" },
            { "#00D4D9", "FIN" },
            { "#D32C00", "DIN" },
            { "#00445A", "CZE" },
            { "#CB7805", "RUM" },
            { "#EB0000", "POL" },
            { "#00003C", "ISR" },
            { "#8337CB", "HUN" },
            { "#E6E5E6", "GEO" },
            { "#8C101C", "ALE" },
        };
    }
}

