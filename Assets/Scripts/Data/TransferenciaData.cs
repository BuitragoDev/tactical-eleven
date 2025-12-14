using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class TransferenciaData
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

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR LA LISTA DE TRASPASOS Y CESIONES
        public static List<Transferencia> ListarTraspasos()
        {
            List<Transferencia> transferencias = new List<Transferencia>();

            try
            {
                var dbPath = GetDBPath();

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string conexionString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(conexionString))
                {
                    conexion.Open();

                    string query = @"SELECT * FROM transferencias ORDER BY fecha_traspaso ASC";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transferencias.Add(new Transferencia()
                                {
                                    // Usamos el operador de coalescencia nula para evitar la asignación de null
                                    IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador")),
                                    IdEquipoOrigen = reader.GetInt32(reader.GetOrdinal("id_equipo_origen")),
                                    IdEquipoDestino = reader.GetInt32(reader.GetOrdinal("id_equipo_destino")),
                                    TipoFichaje = reader.GetInt32(reader.GetOrdinal("tipo_fichaje")),
                                    FechaOferta = reader["fecha_oferta"]?.ToString() ?? string.Empty,
                                    FechaTraspaso = reader["fecha_traspaso"]?.ToString() ?? string.Empty,
                                    RespuestaEquipo = reader.IsDBNull(reader.GetOrdinal("respuesta_equipo"))
                                                        ? (int?)null
                                                        : reader.GetInt32(reader.GetOrdinal("respuesta_equipo")),
                                    RespuestaJugador = reader.IsDBNull(reader.GetOrdinal("respuesta_jugador"))
                                                        ? (int?)null
                                                        : reader.GetInt32(reader.GetOrdinal("respuesta_jugador")),
                                    MontoOferta = reader.GetInt32(reader.GetOrdinal("monto_oferta")),
                                    SalarioAnual = reader.GetInt32(reader.GetOrdinal("salario_anual")),
                                    ClausulaRescision = reader.GetInt32(reader.GetOrdinal("clausula_rescision")),
                                    Duracion = reader.GetInt32(reader.GetOrdinal("duracion")),
                                    BonoPorGoles = reader.GetInt32(reader.GetOrdinal("bono_por_goles")),
                                    BonoPorPartidos = reader.GetInt32(reader.GetOrdinal("bono_por_partidos")),
                                });
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

            return transferencias;
        }

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR LA LISTA DE OFERTAS SIN FINALIZAR
        public static List<Transferencia> ListarOfertasSinFinalizar()
        {
            List<Transferencia> transferencias = new List<Transferencia>();

            try
            {
                var dbPath = GetDBPath();

                if (!File.Exists(dbPath))
                {
                    Debug.LogError($"No se encontró la base de datos en {dbPath}");
                }

                string conexionString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(conexionString))
                {
                    conexion.Open();

                    string query = @"SELECT * FROM ofertas WHERE respuesta_jugador = 0";

                    using (SQLiteCommand comando = new SQLiteCommand(query, conexion))
                    {
                        using (SQLiteDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transferencias.Add(new Transferencia()
                                {
                                    // Usamos el operador de coalescencia nula para evitar la asignación de null
                                    IdJugador = reader.GetInt32(reader.GetOrdinal("id_jugador")),
                                    IdEquipoOrigen = reader.GetInt32(reader.GetOrdinal("id_equipo_origen")),
                                    IdEquipoDestino = reader.GetInt32(reader.GetOrdinal("id_equipo_destino")),
                                    TipoFichaje = reader.GetInt32(reader.GetOrdinal("tipo_fichaje")),
                                    FechaOferta = reader["fecha_oferta"]?.ToString() ?? string.Empty,
                                    FechaTraspaso = reader["fecha_traspaso"]?.ToString() ?? string.Empty,
                                    RespuestaEquipo = reader.IsDBNull(reader.GetOrdinal("respuesta_equipo"))
                                                        ? (int?)null
                                                        : reader.GetInt32(reader.GetOrdinal("respuesta_equipo")),
                                    RespuestaJugador = reader.IsDBNull(reader.GetOrdinal("respuesta_jugador"))
                                                        ? (int?)null
                                                        : reader.GetInt32(reader.GetOrdinal("respuesta_jugador")),
                                    MontoOferta = reader.GetInt32(reader.GetOrdinal("monto_oferta")),
                                    SalarioAnual = reader.GetInt32(reader.GetOrdinal("salario_anual")),
                                    ClausulaRescision = reader.GetInt32(reader.GetOrdinal("clausula_rescision")),
                                    Duracion = reader.GetInt32(reader.GetOrdinal("duracion")),
                                    BonoPorGoles = reader.GetInt32(reader.GetOrdinal("bono_por_goles")),
                                    BonoPorPartidos = reader.GetInt32(reader.GetOrdinal("bono_por_partidos")),
                                });
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

            return transferencias;
        }

        // ----------------------------------------------------------------------------------- MÉTODO QUE BORRA UNA OFERTA POR UN JUGADOR
        public static void BorrarOferta(int jugador)
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

                    string query = @"DELETE FROM ofertas WHERE id_jugador = @IdJugador";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IdJugador", jugador);
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