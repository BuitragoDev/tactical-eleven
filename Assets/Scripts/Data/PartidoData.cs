using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class PartidoData
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

        // --------------------------------------------------------------------------------------------- MÉTODO QUE CREA UN PARTIDO DE LIGA
        public static int CrearPartido(int local, int visitante, string fecha, int competicion, int jornada)
        {
            var dbPath = GetDBPath();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return -1;
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                using var comando = conexion.CreateCommand();
                comando.CommandText = @"INSERT INTO partidos (fecha, id_equipo_local, id_equipo_visitante, id_competicion, jornada) 
                                        VALUES (@Fecha, @IdEquipoLocal, @IdEquipoVisitante, @Competicion, @Jornada)";
                comando.Parameters.AddWithValue("@Fecha", fecha);
                comando.Parameters.AddWithValue("@IdEquipoLocal", local);
                comando.Parameters.AddWithValue("@IdEquipoVisitante", visitante);
                comando.Parameters.AddWithValue("@Competicion", competicion);
                comando.Parameters.AddWithValue("@Jornada", jornada);

                comando.ExecuteNonQuery();

                // Obtener el ID recién insertado
                comando.CommandText = "SELECT last_insert_rowid();";
                long idPartido = (long)comando.ExecuteScalar();
                return (int)idPartido;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al crear el partido: {ex.Message}");
                return -1;
            }
        }

        // --------------------------------------------------------------------------------------------- MÉTODO QUE ELIMINA UN PARTIDO
        public static void EliminarPartidos(List<int> idsPartidos)
        {
            var dbPath = GetDBPath();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
            }

            try
            {
                using var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;");
                conexion.Open();

                foreach (int id in idsPartidos)
                {
                    using var comando = conexion.CreateCommand();
                    comando.CommandText = @"DELETE FROM partidos WHERE id_partido = @IdPartido";
                    comando.Parameters.AddWithValue("@IdPartido", id);

                    comando.ExecuteNonQuery();

                    comando.CommandText = @"DELETE FROM partidos_copaNacional WHERE id_partido = @IdPartido";
                    comando.Parameters.AddWithValue("@IdPartido", id);

                    comando.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                // Manejo de errores
                Debug.Log($"Error al eliminar los partidos: {ex.Message}");
            }
        }
    }
}