#nullable enable

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class FinanzaData
    {
        private static string GetDBPath()
        {
            string path = DatabaseManager.GetActiveDatabasePath();
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("No se ha establecido una base de datos activa en DatabaseManager.");
            }
            return path;
        }

        // ----------------------------------------------------------------------------------- MÉTODO QUE CREA UN GASTO
        public static void CrearGasto(Finanza finanza)
        {
            try
            {
                // Usa la base activa (temporal si existe)
                string dbPath = DatabaseManager.GetActiveDatabasePath();

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                    return;
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    string query = @"INSERT INTO finanzas (id_equipo, temporada, id_concepto, tipo, cantidad, fecha)
                                     VALUES (@IdEquipo, @Temporada, @IdConcepto, @Tipo, @Cantidad, @Fecha)";

                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdEquipo", finanza.IdEquipo);
                        comando.Parameters.AddWithValue("@Temporada", finanza.Temporada);
                        comando.Parameters.AddWithValue("@IdConcepto", finanza.IdConcepto);
                        comando.Parameters.AddWithValue("@Tipo", finanza.Tipo);
                        comando.Parameters.AddWithValue("@Cantidad", finanza.Cantidad);
                        comando.Parameters.AddWithValue("@Fecha", finanza.Fecha.ToString("yyyy-MM-dd"));
                        comando.ExecuteNonQuery();
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }
    }
}