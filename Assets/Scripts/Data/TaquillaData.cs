using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class TaquillaData
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

        // ------------------------------------------------------------------- MÉTODO QUE CREA UN REGISTRO PARA LA TAQUILLA DEL EQUIPO
        public static void GenerarTaquilla(int equipo)
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

                    string query = @"INSERT INTO taquilla (id_equipo) VALUES (@IdEquipo)";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);
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