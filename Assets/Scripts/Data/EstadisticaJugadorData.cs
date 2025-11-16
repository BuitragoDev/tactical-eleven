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

        // -------------------------------------------------------------- MÉTODO QUE DEVUELVE EL JUGADOR CON MÁS GOLES
        public static Estadistica MostrarJugadorConMasGoles(int equipo)
        {
            Estadistica stats = new Estadistica
            {
                IdJugador = 0,
                Goles = 0,
                PartidosJugados = 0
            };

            try
            {
                string dbPath = DatabaseManager.GetActiveDatabasePath(); // Usa la base activa (temporal si existe)

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    // Consulta combinada: jugadores con id_jugador >= 5000 O jugadores cuyo equipo compite en Europa
                    string query = @"SELECT 
                                        ej.id_jugador, ej.goles, ej.partidosJugados
                                     FROM estadisticas_jugadores ej
                                     JOIN jugadores j ON ej.id_jugador = j.id_jugador
                                     JOIN equipos e ON j.id_equipo = e.id_equipo
                                     WHERE e.id_equipo = @IdEquipo
                                     ORDER BY ej.goles DESC
                                     LIMIT 1";

                    using (var cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Si se encuentra un registro
                            {
                                stats.IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador"));
                                stats.Goles = reader.GetInt32(reader.GetOrdinal("goles"));
                                stats.PartidosJugados = reader.GetInt32(reader.GetOrdinal("partidosJugados"));
                            }
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return stats;
        }

        // -------------------------------------------------------------- MÉTODO QUE DEVUELVE EL JUGADOR CON MÁS ASISTENCIAS
        public static Estadistica MostrarJugadorConMasAsistencias(int equipo)
        {
            Estadistica stats = new Estadistica
            {
                IdJugador = 0,
                Asistencias = 0,
                PartidosJugados = 0
            };

            try
            {
                string dbPath = DatabaseManager.GetActiveDatabasePath(); // Usa la base activa (temporal si existe)

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    // Consulta combinada: jugadores con id_jugador >= 5000 O jugadores cuyo equipo compite en Europa
                    string query = @"SELECT 
                                        ej.id_jugador, ej.asistencias, ej.partidosJugados
                                     FROM estadisticas_jugadores ej
                                     JOIN jugadores j ON ej.id_jugador = j.id_jugador
                                     JOIN equipos e ON j.id_equipo = e.id_equipo
                                     WHERE e.id_equipo = @IdEquipo
                                     ORDER BY ej.asistencias DESC
                                     LIMIT 1";

                    using (var cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Si se encuentra un registro
                            {
                                stats.IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador"));
                                stats.Asistencias = reader.GetInt32(reader.GetOrdinal("asistencias"));
                                stats.PartidosJugados = reader.GetInt32(reader.GetOrdinal("partidosJugados"));
                            }
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return stats;
        }

        // -------------------------------------------------------------- MÉTODO QUE DEVUELVE EL JUGADOR CON MÁS MVP
        public static Estadistica MostrarJugadorConMasMvp(int equipo)
        {
            Estadistica stats = new Estadistica
            {
                IdJugador = 0,
                MVP = 0,
                PartidosJugados = 0
            };

            try
            {
                string dbPath = DatabaseManager.GetActiveDatabasePath(); // Usa la base activa (temporal si existe)

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    // Consulta combinada: jugadores con id_jugador >= 5000 O jugadores cuyo equipo compite en Europa
                    string query = @"SELECT 
                                        ej.id_jugador, ej.mvp, ej.partidosJugados
                                     FROM estadisticas_jugadores ej
                                     JOIN jugadores j ON ej.id_jugador = j.id_jugador
                                     JOIN equipos e ON j.id_equipo = e.id_equipo
                                     WHERE e.id_equipo = @IdEquipo
                                     ORDER BY ej.mvp DESC
                                     LIMIT 1";

                    using (var cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Si se encuentra un registro
                            {
                                stats.IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador"));
                                stats.MVP = reader.GetInt32(reader.GetOrdinal("mvp"));
                                stats.PartidosJugados = reader.GetInt32(reader.GetOrdinal("partidosJugados"));
                            }
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return stats;
        }

        // -------------------------------------------------------------- MÉTODO QUE DEVUELVE EL JUGADOR CON MÁS TARJETAS AMARILLAS
        public static Estadistica MostrarJugadorConMasTarjetasAmarillas(int equipo)
        {
            Estadistica stats = new Estadistica
            {
                IdJugador = 0,
                TarjetasAmarillas = 0,
                PartidosJugados = 0
            };

            try
            {
                string dbPath = DatabaseManager.GetActiveDatabasePath(); // Usa la base activa (temporal si existe)

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    // Consulta combinada: jugadores con id_jugador >= 5000 O jugadores cuyo equipo compite en Europa
                    string query = @"SELECT 
                                        ej.id_jugador, ej.tarjetasAmarillas, ej.partidosJugados
                                     FROM estadisticas_jugadores ej
                                     JOIN jugadores j ON ej.id_jugador = j.id_jugador
                                     JOIN equipos e ON j.id_equipo = e.id_equipo
                                     WHERE e.id_equipo = @IdEquipo
                                     ORDER BY ej.tarjetasAmarillas DESC
                                     LIMIT 1";

                    using (var cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Si se encuentra un registro
                            {
                                stats.IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador"));
                                stats.TarjetasAmarillas = reader.GetInt32(reader.GetOrdinal("tarjetasAmarillas"));
                                stats.PartidosJugados = reader.GetInt32(reader.GetOrdinal("partidosJugados"));
                            }
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return stats;
        }

        // -------------------------------------------------------------- MÉTODO QUE DEVUELVE EL JUGADOR CON MÁS TARJETAS ROJAS
        public static Estadistica MostrarJugadorConMasTarjetasRojas(int equipo)
        {
            Estadistica stats = new Estadistica
            {
                IdJugador = 0,
                TarjetasRojas = 0,
                PartidosJugados = 0
            };

            try
            {
                string dbPath = DatabaseManager.GetActiveDatabasePath(); // Usa la base activa (temporal si existe)

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string connString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(connString))
                {
                    conexion.Open();

                    // Consulta combinada: jugadores con id_jugador >= 5000 O jugadores cuyo equipo compite en Europa
                    string query = @"SELECT 
                                        ej.id_jugador, ej.tarjetasRojas, ej.partidosJugados
                                     FROM estadisticas_jugadores ej
                                     JOIN jugadores j ON ej.id_jugador = j.id_jugador
                                     JOIN equipos e ON j.id_equipo = e.id_equipo
                                     WHERE e.id_equipo = @IdEquipo
                                     ORDER BY ej.tarjetasRojas DESC
                                     LIMIT 1";

                    using (var cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Si se encuentra un registro
                            {
                                stats.IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador"));
                                stats.TarjetasRojas = reader.GetInt32(reader.GetOrdinal("tarjetasRojas"));
                                stats.PartidosJugados = reader.GetInt32(reader.GetOrdinal("partidosJugados"));
                            }
                        }
                    }

                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error al guardar en la base de datos: {ex.Message}");
            }

            return stats;
        }
    }
}
