using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class MensajeData
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

        // ----------------------------------------------------------------------------------- MÉTODO QUE CREA UN MENSAJE
        public static void CrearMensaje(Mensaje msg)
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

                    string query = @"INSERT INTO mensajes (fecha, remitente, asunto, contenido, tipo_mensaje, id_equipo, leido, icono)
                                     VALUES (@fecha, @remitente, @asunto, @contenido, @tipo_mensaje, @id_equipo, @leido, @icono)";

                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@fecha", msg.Fecha);
                        comando.Parameters.AddWithValue("@remitente", msg.Remitente);
                        comando.Parameters.AddWithValue("@asunto", msg.Asunto);
                        comando.Parameters.AddWithValue("@contenido", msg.Contenido);
                        comando.Parameters.AddWithValue("@tipo_mensaje", msg.TipoMensaje);
                        comando.Parameters.AddWithValue("@id_equipo", msg.IdEquipo.HasValue ? (object)msg.IdEquipo.Value : DBNull.Value);
                        comando.Parameters.AddWithValue("@leido", msg.Leido ? 1 : 0); // Convertir bool a 0 o 1
                        comando.Parameters.AddWithValue("@icono", msg.Icono);
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