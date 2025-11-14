using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class MainScreen : MonoBehaviour
    {
        [Header("Sound Clips")]
        [SerializeField] private AudioClip clickSFX;

        private VisualElement miEquipoEscudo, cabeceraManagerValoracion;
        private VisualElement homeIcon, clubIcon, alineacionIcon, competicionesIcon, calendarioIcon, fichajesIcon,
                              finanzasIcon, estadioIcon, managerIcon, mensajesIcon, ajustesIcon;
        private VisualElement mainContainer;
        private Button btnSeguir;
        private Label miEquipoNombre, miPresupuesto, managerNombre, fecha1, fecha2;
        Manager miManager;
        Equipo miEquipo;

        void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            // Referencias a elementos pantalla principal
            miEquipoEscudo = root.Q<VisualElement>("cabecera-escudo");
            miEquipoNombre = root.Q<Label>("miEquipoNombre");
            miPresupuesto = root.Q<Label>("miPresupuesto");
            managerNombre = root.Q<Label>("managerNombre");
            cabeceraManagerValoracion = root.Q<VisualElement>("cabecera-manager-valoracion");
            fecha1 = root.Q<Label>("fecha1");
            fecha2 = root.Q<Label>("fecha2");
            btnSeguir = root.Q<Button>("btnSeguir");

            homeIcon = root.Q<VisualElement>("home-icon");
            clubIcon = root.Q<VisualElement>("club-icon");
            alineacionIcon = root.Q<VisualElement>("alineacion-icon");
            competicionesIcon = root.Q<VisualElement>("competiciones-icon");
            calendarioIcon = root.Q<VisualElement>("calendario-icon");
            fichajesIcon = root.Q<VisualElement>("fichajes-icon");
            finanzasIcon = root.Q<VisualElement>("finanzas-icon");
            estadioIcon = root.Q<VisualElement>("estadio-icon");
            managerIcon = root.Q<VisualElement>("manager-icon");
            mensajesIcon = root.Q<VisualElement>("mensajes-icon");
            ajustesIcon = root.Q<VisualElement>("ajustes-icon");

            mainContainer = root.Q<VisualElement>("main-container");

            // Consulta a la BD
            miManager = ManagerData.MostrarManager();
            miEquipo = EquipoData.ObtenerDetallesEquipo((int)miManager.IdEquipo);

            // Mostrar mi escudo en la cabecera.
            var sprite = Resources.Load<Sprite>($"EscudosEquipos/120x120/{miManager.IdEquipo}");
            if (sprite != null)
                miEquipoEscudo.style.backgroundImage = new StyleBackground(sprite);

            // Mostrar el nombre de mi equipo en la cabecera
            miEquipoNombre.text = miEquipo.Nombre;

            // Mostrar el presupuesto en la cabecera
            float presupuestoConversion = 0f;
            string symbol = "€";

            string currency = PlayerPrefs.GetString("Currency", string.Empty);
            if (currency != string.Empty)
            {
                switch (currency)
                {
                    case Constants.EURO_NAME:
                        presupuestoConversion = miEquipo.Presupuesto * Constants.EURO_VALUE;
                        symbol = Constants.EURO_SYMBOL;
                        break;
                    case Constants.POUND_NAME:
                        presupuestoConversion = miEquipo.Presupuesto * Constants.POUND_VALUE;
                        symbol = Constants.POUND_SYMBOL;
                        break;
                    case Constants.DOLLAR_NAME:
                        presupuestoConversion = miEquipo.Presupuesto * Constants.DOLLAR_VALUE;
                        symbol = Constants.DOLLAR_SYMBOL;
                        break;
                    default:
                        presupuestoConversion = miEquipo.Presupuesto * Constants.EURO_VALUE;
                        symbol = Constants.EURO_SYMBOL;
                        break;
                }
            }
            miPresupuesto.text = $"{presupuestoConversion.ToString("N0")} {symbol}";

            // Mostrar el nombre del manager en la cabecera
            managerNombre.text = $"{miManager.Nombre} {miManager.Apellido}";

            // Mostrar valoración del mánager en la cabecera
            int reputacionManager = miManager.Reputacion;
            MostrarEstrellas(reputacionManager);

            // Mostrar fecha en la cabecera
            Fecha fechaObjeto = FechaData.ObtenerFechaHoy();
            DateTime hoy = DateTime.Parse(fechaObjeto.Hoy);
            // Formatear la fecha en español
            CultureInfo culturaEspañol = new CultureInfo("es-ES");
            string dia = hoy.ToString("dd", culturaEspañol); // Día
            string mes = hoy.ToString("MMM", culturaEspañol).ToUpper(); // Mes abreviado en español y en mayúsculas
            string año = hoy.ToString("yyyy", culturaEspañol); // Año

            // Combinar el formato
            string fechaFormateada = $"{dia} {mes} {año}";

            // Mostrar la fecha
            fecha1.text = fechaFormateada;

            // Obtener el día de la semana en español
            string diaSemana = hoy.ToString("dddd", culturaEspañol);

            // Capitalizar la primera letra (opcional, si el formato por defecto no es suficiente)
            diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);

            // Mostrar el día de la semana
            fecha2.text = diaSemana;

            CargarPortada();

            // Botón Avanzar de la cabecera
            btnSeguir.clicked += () =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
            };

            // Eventos iconos del Menu Lateral
            homeIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarPortada();
            });

            clubIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
            });

            alineacionIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
            });

            competicionesIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
            });

            calendarioIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
            });

            fichajesIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
            });

            finanzasIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
            });

            estadioIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
            });

            managerIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
            });

            mensajesIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
            });

            ajustesIcon.RegisterCallback<ClickEvent>(evt =>
            {
                SceneLoader.Instance.LoadScene(Constants.MAIN_MENU_SCENE);
            });
        }

        private void MostrarEstrellas(int reputacion)
        {
            // Limpiar el contenedor
            cabeceraManagerValoracion.Clear();

            // Cargar las imágenes desde Resources
            Sprite estrellaON = Resources.Load<Sprite>("Icons/estrellaOn");
            Sprite estrellaOFF = Resources.Load<Sprite>("Icons/estrellaOff");

            if (estrellaON == null || estrellaOFF == null)
            {
                Debug.LogError("No se pudieron cargar las imágenes de las estrellas");
                return;
            }

            // Determinar el número de estrellas según la reputación
            int numeroEstrellas = reputacion switch
            {
                100 => 5,
                >= 90 => 4,
                >= 70 => 3,
                >= 50 => 2,
                >= 25 => 1,
                _ => 0
            };

            // Crear 5 estrellas
            for (int i = 0; i < 5; i++)
            {
                Image estrella = new Image
                {
                    image = i < numeroEstrellas ? estrellaON.texture : estrellaOFF.texture,
                    scaleMode = ScaleMode.ScaleToFit,
                    style =
            {
                width = 32,
                height = 32,
                marginRight = 3 // separación horizontal
            }
                };

                cabeceraManagerValoracion.Add(estrella);
            }

            // Opcional: usar un contenedor horizontal si quieres alinear automáticamente
            cabeceraManagerValoracion.style.flexDirection = FlexDirection.Row;
        }

        private void CargarPortada()
        {
            mainContainer.Clear();

            var tree = Resources.Load<VisualTreeAsset>("UI/PortadaScreen/Portada");
            if (tree == null)
            {
                Debug.LogError("No se encontró el UXML Portada en Resources/UI/PortadaScreen/Portada");
                return;
            }

            var portadaInstance = tree.Instantiate();
            portadaInstance.style.flexGrow = 1; // Asegura que llene el contenedor
            portadaInstance.style.width = Length.Percent(100);
            portadaInstance.style.height = Length.Percent(100);

            mainContainer.Add(portadaInstance);
        }

    }
}