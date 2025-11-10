using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using System.IO;

namespace TacticalEleven.Scripts
{
    public class CreateManager : MonoBehaviour
    {
        private TextField nombre, apellido, dia, mes, anio;
        private DropdownField dpNacionalidad;
        private Button btnSeguir, btnVolver;

        private string dbPath;

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            // Referencias a elementos
            dpNacionalidad = root.Q<DropdownField>("dpNacionalidad");
            nombre = root.Q<TextField>("txtNombre");
            apellido = root.Q<TextField>("txtApellido");
            dia = root.Q<TextField>("txtDia");
            mes = root.Q<TextField>("txtMes");
            anio = root.Q<TextField>("txtAnio");
            btnSeguir = root.Q<Button>("btnSeguir");
            btnVolver = root.Q<Button>("btnVolver");

            // --- Dropdown ---
            var nacionalidades = Constants.ObtenerTodasLasNacionalidades();
            dpNacionalidad.choices = nacionalidades;
            dpNacionalidad.value = nacionalidades.Contains("España") ? "España" : nacionalidades[0];

            // --- Botones ---
            btnSeguir.SetEnabled(false);
            btnVolver.clicked += () => SceneLoader.Instance.LoadScene(Constants.MAIN_MENU_SCENE);
            btnSeguir.clicked += () =>
            {
                GuardarManagerEnDB();
                string currency = PlayerPrefs.GetString("Currency", string.Empty);
                if (currency == string.Empty)
                {
                    PlayerPrefs.SetString("Currency", Constants.EURO_NAME);
                }

                SceneLoader.Instance.LoadScene(Constants.TEAM_SELECTION_SCENE);
            };

            // --- Validaciones ---
            nombre.RegisterValueChangedCallback(evt =>
            {
                nombre.value = Regex.Replace(evt.newValue, @"[^a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]", "");
                ValidarCampos();
            });

            apellido.RegisterValueChangedCallback(evt =>
            {
                apellido.value = Regex.Replace(evt.newValue, @"[^a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]", "");
                ValidarCampos();
            });

            dia.RegisterValueChangedCallback(evt =>
            {
                dia.value = Regex.Replace(evt.newValue, @"[^0-9]", "");
                if (int.TryParse(dia.value, out int d))
                {
                    if (d < 1) dia.value = "1";
                    else if (d > 31) dia.value = "31";
                }
                ValidarCampos();
            });

            mes.RegisterValueChangedCallback(evt =>
            {
                mes.value = Regex.Replace(evt.newValue, @"[^0-9]", "");
                if (int.TryParse(mes.value, out int m))
                {
                    if (m < 1) mes.value = "1";
                    else if (m > 12) mes.value = "12";

                    AjustarDiaSegunMes(); // solo modifica el día si es necesario
                }
                ValidarCampos();
            });

            anio.RegisterValueChangedCallback(evt =>
            {
                anio.value = Regex.Replace(evt.newValue, @"[^0-9]", "");
                if (anio.value.Length == 4)
                {
                    AjustarAnio();
                }
                ValidarCampos();
            });

            dpNacionalidad.RegisterValueChangedCallback(evt => ValidarCampos());

            // Ruta a la base de datos
            dbPath = Path.Combine(Application.streamingAssetsPath, Constants.DATABASE_NAME);
        }

        // --- Corrige el día si el mes no lo admite ---
        private void AjustarDiaSegunMes()
        {
            if (!int.TryParse(dia.value, out int d)) return;
            if (!int.TryParse(mes.value, out int m)) return;

            int a = ParseIntSafe(anio.value);
            if (a == 0) a = DateTime.Now.Year; // por si no hay año aún

            int diasEnMes = DateTime.DaysInMonth(a, m);
            if (d > diasEnMes)
            {
                dia.value = diasEnMes.ToString();
            }
        }

        // --- Ajusta el año cuando tiene 4 cifras ---
        private void AjustarAnio()
        {
            if (!int.TryParse(anio.value, out int a)) return;

            int anioActual = DateTime.Now.Year;
            int anioMax = anioActual - 18;
            int anioMin = 1950;

            if (a < anioMin) a = anioMin;
            if (a > anioMax) a = anioMax;

            anio.value = a.ToString();
        }

        // --- Activa/desactiva el botón SEGUIR según la validez ---
        private void ValidarCampos()
        {
            bool completos =
                !string.IsNullOrEmpty(nombre.value) &&
                !string.IsNullOrEmpty(apellido.value) &&
                !string.IsNullOrEmpty(dia.value) &&
                !string.IsNullOrEmpty(mes.value) &&
                !string.IsNullOrEmpty(anio.value) &&
                !string.IsNullOrEmpty(dpNacionalidad.value);

            bool fechaValida = EsFechaValida();

            btnSeguir.SetEnabled(completos && fechaValida);
        }

        // --- Comprueba si la fecha completa es válida ---
        private bool EsFechaValida()
        {
            if (!int.TryParse(dia.value, out int d)) return false;
            if (!int.TryParse(mes.value, out int m)) return false;
            if (!int.TryParse(anio.value, out int a)) return false;

            if (a < 1950 || a > DateTime.Now.Year - 18) return false;
            if (m < 1 || m > 12) return false;

            try
            {
                var fecha = new DateTime(a, m, d);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // --- Helper seguro ---
        private int ParseIntSafe(string text)
        {
            return int.TryParse(text, out int v) ? v : 0;
        }

        // Método que guarda el manager en la base de datos
        private void GuardarManagerEnDB()
        {
            try
            {
                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"❌ No se encontró la base de datos en {dbPath}");
                    return;
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var connection = new SQLiteConnection(connString))
                {
                    connection.Open();

                    string fechaNacimiento = $"{anio.value.PadLeft(4, '0')}-{mes.value.PadLeft(2, '0')}-{dia.value.PadLeft(2, '0')}";

                    string query = @"INSERT INTO managers (nombre, apellido, nacionalidad, fechaNacimiento)
                                     VALUES (@nombre, @apellido, @nacionalidad, @fechaNacimiento);";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre.value.Trim());
                        cmd.Parameters.AddWithValue("@apellido", apellido.value.Trim());
                        cmd.Parameters.AddWithValue("@nacionalidad", dpNacionalidad.value);
                        cmd.Parameters.AddWithValue("@fechaNacimiento", fechaNacimiento);
                        cmd.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                Debug.Log($"Manager '{nombre.value} {apellido.value}' guardado correctamente en la base de datos.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }
    }
}