using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class EstadisticaJugadorData
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

        // ------------------------------------------------------------- MÉTODO QUE INSERTA UNA FILA DE ESTADÍSTICA POR CADA JUGADOR DE LIGA 
        public static void InsertarEstadisticasJugadores()
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

                    // Obtener todos los id_jugador de la tabla jugadores
                    string query = "SELECT id_jugador FROM jugadores WHERE id_jugador < 5000";
                    List<int> listaJugadores = new List<int>();

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaJugadores.Add(reader.GetInt32(0));
                        }
                    }

                    // Insertar una fila en estadisticas_jugadores por cada jugador
                    string insertQuery = @"INSERT INTO estadisticas_jugadores (id_jugador)
                                           VALUES (@IdJugador)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, conexion))
                    {
                        insertCommand.Parameters.Add(new SQLiteParameter("@IdJugador"));

                        foreach (int idJugador in listaJugadores)
                        {
                            insertCommand.Parameters["@IdJugador"].Value = idJugador;
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

        // ------------------------------------------------------ MÉTODO QUE INSERTA UNA FILA DE ESTADÍSTICA POR CADA JUGADOR DE EQUIPO EUROPEO
        public static void InsertarEstadisticasJugadoresEuropa()
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

                    // Consulta combinada: jugadores con id_jugador >= 5000 O jugadores cuyo equipo compite en Europa
                    string query = @"SELECT j.id_jugador
                                     FROM jugadores j
                                     INNER JOIN equipos e ON j.id_equipo = e.id_equipo
                                     WHERE j.id_jugador >= 5000 OR e.competicion_europea != 0";
                    List<int> listaJugadores = new List<int>();

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaJugadores.Add(reader.GetInt32(0));
                        }
                    }

                    // Insertar una fila en estadisticas_jugadores por cada jugador
                    string insertQuery = @"INSERT INTO estadisticas_jugadores_europa (id_jugador)
                                           VALUES (@IdJugador)";
                    using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, conexion))
                    {
                        insertCommand.Parameters.Add(new SQLiteParameter("@IdJugador"));

                        foreach (int idJugador in listaJugadores)
                        {
                            insertCommand.Parameters["@IdJugador"].Value = idJugador;
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
