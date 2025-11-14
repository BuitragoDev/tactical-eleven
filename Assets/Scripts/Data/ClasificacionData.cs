using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class ClasificacionData
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

        // ------------------------------------------------------------------ MÉTODO QUE RELLENA LA CLASIFICACIÓN DE LIGA 1
        public static void RellenarClasificacionLiga1(int competicion)
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

                    // Consulta para obtener los ID de los equipos de la competición
                    string query = @"SELECT id_equipo FROM equipos WHERE id_competicion = @competicion";
                    List<int> equipos = new List<int>();

                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@competicion", competicion);
                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                equipos.Add(reader.GetInt32(0));
                            }
                        }
                    }

                    // Eliminar clasificación anterior del manager
                    string queryDelete = @"DELETE FROM clasificacion";
                    using (SQLiteCommand deleteCommand = new SQLiteCommand(queryDelete, conexion))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }

                    // Insertar cada equipo en la tabla clasificacion
                    string queryInsert = @"INSERT INTO clasificacion (id_equipo) VALUES (@idEquipo)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(queryInsert, conexion))
                    {
                        foreach (int idEquipo in equipos)
                        {
                            insertCommand.Parameters.Clear();
                            insertCommand.Parameters.AddWithValue("@idEquipo", idEquipo);
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }

        // ------------------------------------------------------------------ MÉTODO QUE RELLENA LA CLASIFICACIÓN DE LIGA 2
        public static void RellenarClasificacionLiga2(int competicion)
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

                    // Consulta para obtener los ID de los equipos de la competición
                    string query = @"SELECT id_equipo FROM equipos WHERE id_competicion = @competicion";
                    List<int> equipos = new List<int>();

                    using (var comando = new SQLiteCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@competicion", competicion);
                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                equipos.Add(reader.GetInt32(0));
                            }
                        }
                    }

                    // Eliminar clasificación anterior del manager
                    string queryDelete = @"DELETE FROM clasificacion2";
                    using (SQLiteCommand deleteCommand = new SQLiteCommand(queryDelete, conexion))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }

                    // Insertar cada equipo en la tabla clasificacion
                    string queryInsert = @"INSERT INTO clasificacion2 (id_equipo) VALUES (@idEquipo)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(queryInsert, conexion))
                    {
                        foreach (int idEquipo in equipos)
                        {
                            insertCommand.Parameters.Clear();
                            insertCommand.Parameters.AddWithValue("@idEquipo", idEquipo);
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }

        // ------------------------------------------------------------------ MÉTODO QUE RELLENA LA CLASIFICACIÓN DE EUROPA 1
        public static void RellenarClasificacionEuropa1(List<Equipo> equiposEuropa1)
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

                    // Insertar cada equipo en la tabla clasificacion
                    string query = @"INSERT INTO clasificacion_europa1 (id_equipo) VALUES (@idEquipo)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(query, conexion))
                    {
                        foreach (Equipo equipo in equiposEuropa1)
                        {
                            insertCommand.Parameters.Clear();
                            insertCommand.Parameters.AddWithValue("@idEquipo", equipo.IdEquipo);
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }
        }

        // ------------------------------------------------------------------ MÉTODO QUE RELLENA LA CLASIFICACIÓN DE EUROPA 1
        public static void RellenarClasificacionEuropa2(List<Equipo> equiposEuropa2)
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

                    // Insertar cada equipo en la tabla clasificacion
                    string query = @"INSERT INTO clasificacion_europa2 (id_equipo) VALUES (@idEquipo)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(query, conexion))
                    {
                        foreach (Equipo equipo in equiposEuropa2)
                        {
                            insertCommand.Parameters.Clear();
                            insertCommand.Parameters.AddWithValue("@idEquipo", equipo.IdEquipo);
                            insertCommand.ExecuteNonQuery();
                        }
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