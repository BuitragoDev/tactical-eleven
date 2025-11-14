using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class HistorialData
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

        // ------------------------------------------------------------------- MÉTODO PARA INSERTAR UN NUEVO HISTORIAL DE MÁNAGER VACÍO
        public static void CrearLineaHistorial(int equipo, string temporada)
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
                using (var connection = new SQLiteConnection(connString))
                {
                    connection.Open();

                    string query = @"INSERT INTO historial_manager (id_equipo, temporada) 
                                     VALUES (@IdEquipo, @Temporada)";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);
                        cmd.Parameters.AddWithValue("@Temporada", temporada);
                        cmd.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }
    }
}