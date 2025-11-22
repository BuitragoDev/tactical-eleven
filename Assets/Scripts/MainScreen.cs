using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class MainScreen : MonoBehaviour
    {
        [Header("Sound Clips")]
        [SerializeField] private AudioClip clickSFX;

        // UI Elements
        private VisualElement miEquipoEscudo, cabeceraManagerValoracion;
        private VisualElement homeIcon, clubIcon, alineacionIcon, competicionesIcon, calendarioIcon,
                              fichajesIcon, finanzasIcon, estadioIcon, managerIcon, mensajesIcon, ajustesIcon;
        private VisualElement clubMenu, alineacionMenu, competicionMenu, calendarioMenu, fichajesMenu, finanzasMenu,
                              estadioMenu, managerMenu, mensajesMenu;
        private VisualElement mainContainer;
        private Button btnSeguir;
        private Label miEquipoNombre, managerNombre, fecha1, fecha2;
        public Label miPresupuesto;
        private Manager miManager;
        private Equipo miEquipo;

        // Elementos Top Menu
        private Label lblInformacion, lblPlantilla, lblEmpleados, lblLesionados, lblManagerFicha, lblManagerPalmares;

        void OnEnable()
        {
            SceneLoader.setSettingsParameter(1);

            var uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            // --- Contenedores y elementos UI ---
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

            // Top menu elements
            lblInformacion = root.Q<Label>("lblInformacion");
            lblPlantilla = root.Q<Label>("lblPlantilla");
            lblEmpleados = root.Q<Label>("lblEmpleados");
            lblLesionados = root.Q<Label>("lblLesionados");
            lblManagerFicha = root.Q<Label>("lblFicha");
            lblManagerPalmares = root.Q<Label>("lblPalmaresManager");

            mainContainer = root.Q<VisualElement>("main-container");

            // CLUB
            clubMenu = root.Q<VisualElement>("clubMenu");
            alineacionMenu = root.Q<VisualElement>("entrenadorMenu");
            competicionMenu = root.Q<VisualElement>("competicionMenu");
            calendarioMenu = root.Q<VisualElement>("calendarioMenu");
            fichajesMenu = root.Q<VisualElement>("fichajesMenu");
            finanzasMenu = root.Q<VisualElement>("finanzasMenu");
            estadioMenu = root.Q<VisualElement>("estadioMenu");
            managerMenu = root.Q<VisualElement>("managerMenu");
            mensajesMenu = root.Q<VisualElement>("mensajesMenu");

            // Listas por sección
            List<Label> clubList = new List<Label> { lblInformacion, lblPlantilla, lblEmpleados, lblLesionados };

            // --- UIManager ---
            if (UIManager.Instance == null)
            {
                var go = new GameObject("UI Manager");
                go.AddComponent<UIManager>();
                Debug.Log("UIManager creado dinámicamente");
            }

            mainContainer = root.Q<VisualElement>("main-container");
            UIManager.Instance.SetMainContainer(mainContainer);

            // --- Cargar datos del manager y equipo ---
            miManager = ManagerData.MostrarManager();
            miEquipo = EquipoData.ObtenerDetallesEquipo((int)miManager.IdEquipo);

            // Escudo
            var sprite = Resources.Load<Sprite>($"EscudosEquipos/120x120/{miManager.IdEquipo}");
            if (sprite != null)
                miEquipoEscudo.style.backgroundImage = new StyleBackground(sprite);

            // Nombre del equipo
            miEquipoNombre.text = miEquipo.Nombre;

            // Presupuesto
            float presupuestoConversion = miEquipo.Presupuesto * Constants.EURO_VALUE;
            string symbol = Constants.EURO_SYMBOL;

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

            // Nombre del manager
            managerNombre.text = $"{miManager.Nombre} {miManager.Apellido}";

            // Valoración del manager
            MostrarEstrellas(miManager.Reputacion);

            // Fecha
            Fecha fechaObjeto = FechaData.ObtenerFechaHoy();
            DateTime hoy = DateTime.Parse(fechaObjeto.Hoy);
            CultureInfo culturaEspañol = new CultureInfo("es-ES");
            string dia = hoy.ToString("dd", culturaEspañol);
            string mes = hoy.ToString("MMM", culturaEspañol).ToUpper();
            string año = hoy.ToString("yyyy", culturaEspañol);
            fecha1.text = $"{dia} {mes} {año}";

            string diaSemana = hoy.ToString("dddd", culturaEspañol);
            diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);
            fecha2.text = diaSemana;

            // --- Cargar portada al iniciar ---
            CargarPortada();

            // --- Botón seguir ---
            btnSeguir.clicked += () => AudioManager.Instance.PlaySFX(clickSFX);

            // --- Eventos iconos menú lateral ---
            List<VisualElement> menuList = new List<VisualElement> { clubMenu, alineacionMenu, competicionMenu,
                                                                     calendarioMenu, fichajesMenu, finanzasMenu,
                                                                     estadioMenu, managerMenu, mensajesMenu
                                                                   };

            // ---------------------------------------------------- Evento HOME ICON
            homeIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, null);
                CargarPortada();
            });

            // ---------------------------------------------------- Eventos CLUB
            clubIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, clubMenu);
                CargarClubInformacion(clubList);
            });
            lblInformacion.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarClubInformacion(clubList);
            });
            lblPlantilla.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarClubPlantilla(clubList);
            });
            lblEmpleados.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarClubEmpleados(clubList);
            });
            lblLesionados.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarClubLesionados(clubList);
            });

            // ---------------------------------------------------- Eventos ALINEACION
            alineacionIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, alineacionMenu);
            });
            competicionesIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, competicionMenu);
            });

            // ---------------------------------------------------- Evento CALENDARIO ICON
            calendarioIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, calendarioMenu);
                CargarCalendario();
            });
            fichajesIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, fichajesMenu);
            });
            finanzasIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, finanzasMenu);
            });
            estadioIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, estadioMenu);
            });

            // ---------------------------------------------------- Eventos MÁNAGER
            managerIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, managerMenu);
                CargarManagerFicha(clubList);
            });
            lblManagerFicha.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarManagerFicha(clubList);
            });
            lblManagerPalmares.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                CargarManagerPalmares(clubList);
            });

            // ---------------------------------------------------- Evento MENSAJES ICON
            mensajesIcon.RegisterCallback<ClickEvent>(evt =>
            {
                AudioManager.Instance.PlaySFX(clickSFX);
                MenuVisibility(menuList, mensajesMenu);
                CargarMensajes();
            });

            // ---------------------------------------------------- Evento AJUSTES ICON
            ajustesIcon.RegisterCallback<ClickEvent>(evt =>
            {
                SceneLoader.Instance.LoadScene(Constants.SETTINGS_SCREEN_SCENE);
            });
        }

        private void MenuVisibility(List<VisualElement> menus, VisualElement visibleMenu)
        {
            foreach (var menu in menus)
            {
                if (menu == visibleMenu && visibleMenu != null)
                {
                    menu.style.display = DisplayStyle.Flex;
                }
                else
                {
                    menu.style.display = DisplayStyle.None;
                }
            }
        }

        private void MostrarEstrellas(int reputacion)
        {
            cabeceraManagerValoracion.Clear();

            Sprite estrellaON = Resources.Load<Sprite>("Icons/estrellaOn");
            Sprite estrellaOFF = Resources.Load<Sprite>("Icons/estrellaOff");

            if (estrellaON == null || estrellaOFF == null)
            {
                Debug.LogError("No se pudieron cargar las imágenes de las estrellas");
                return;
            }

            int numeroEstrellas = reputacion switch
            {
                100 => 5,
                >= 90 => 4,
                >= 70 => 3,
                >= 50 => 2,
                >= 25 => 1,
                _ => 0
            };

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
                        marginRight = 3
                    }
                };
                cabeceraManagerValoracion.Add(estrella);
            }

            cabeceraManagerValoracion.style.flexDirection = FlexDirection.Row;
        }

        private void CargarPortada()
        {
            UIManager.Instance.CargarPantalla("UI/PortadaScreen/Portada", instancia =>
            {
                new PortadaManager(instancia, miEquipo, miManager);
            });
        }

        private void CargarClubInformacion(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblInformacion);
            UIManager.Instance.CargarPantalla("UI/Club/Informacion/ClubInformacion", instancia =>
            {
                new ClubInformacion(instancia, miEquipo, miManager);
            });
        }

        private void CargarClubPlantilla(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblPlantilla);
            UIManager.Instance.CargarPantalla("UI/Club/Plantilla/ClubPlantilla", instancia =>
            {
                new ClubPlantilla(instancia, miEquipo, miManager);
            });
        }

        private void CargarClubEmpleados(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblEmpleados);
            UIManager.Instance.CargarPantalla("UI/Club/Empleados/ClubEmpleados", instancia =>
            {
                new ClubEmpleados(instancia, miEquipo, miManager, this);
            });
        }

        private void CargarClubLesionados(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblLesionados);
            UIManager.Instance.CargarPantalla("UI/Club/Lesionados/ClubLesionados", instancia =>
            {
                new ClubLesionados(instancia, miEquipo, miManager);
            });
        }

        private void CargarCalendario()
        {
            UIManager.Instance.CargarPantalla("UI/CalendarioScreen/Calendario", instancia =>
            {
                new Calendario(instancia, miEquipo, miManager);
            });
        }

        private void CargarManagerFicha(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblManagerFicha);
            UIManager.Instance.CargarPantalla("UI/Manager/Ficha/ManagerFicha", instancia =>
            {
                new ManagerFicha(instancia, miEquipo, miManager);
            });
        }

        private void CargarManagerPalmares(List<Label> clubList)
        {
            CambiarColorTextoClub(clubList, lblManagerPalmares);
            UIManager.Instance.CargarPantalla("UI/Manager/Palmares/ManagerPalmares", instancia =>
            {
                new ManagerPalmares(instancia, miEquipo, miManager);
            });
        }

        private void CargarMensajes()
        {
            UIManager.Instance.CargarPantalla("UI/MensajesScreen/Mensajes", instancia =>
            {
                new Mensajes(instancia, miEquipo, miManager);
            });
        }

        private void CambiarColorTextoClub(List<Label> clubList, Label label)
        {
            foreach (var item in clubList)
            {
                if (item == label)
                {
                    item.style.color = (Color)new Color32(0x1E, 0x72, 0x3C, 0xFF);
                }
                else
                {
                    item.style.color = Color.white;
                }
            }
        }
    }
}