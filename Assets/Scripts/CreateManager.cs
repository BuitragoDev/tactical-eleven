using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class CreateManager : MonoBehaviour
    {
        private void OnEnable()
        {
            // Obtenemos el componente UIDocument asociado a este GameObject
            var uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            // Buscamos el DropdownField por su clase USS o su nombre UXML
            var dropdown = root.Q<DropdownField>(className: "dpNacionalidad");

            if (dropdown == null)
            {
                Debug.LogWarning("No se encontró ningún Dropdown con la clase 'dpNacionalidad'.");
                return;
            }

            // Cargamos las nacionalidades
            var nacionalidades = ObtenerTodasLasNacionalidades();

            // Asignamos las opciones al Dropdown
            dropdown.choices = nacionalidades;

            // (Opcional) Valor inicial vacío
            dropdown.value = "";
        }

        private List<string> ObtenerTodasLasNacionalidades()
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
    }
}