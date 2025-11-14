using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class JugadorData
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

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR LOS TITULARES DEL EQUIPO
        public static void CrearAlineacion(int equipo)
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

                string conexionString = $"Data Source={dbPath};Version=3;";
                using (var conexion = new SQLiteConnection(conexionString))
                {
                    conexion.Open();

                    string query = @"SELECT id_jugador, rol_id,
                                           (velocidad + resistencia + agresividad + calidad + estado_forma + moral) / 6.0 as media
                                     FROM jugadores 
                                     WHERE id_equipo = @IdEquipo
                                     ORDER BY 
                                        CASE 
                                            WHEN rol_id = 1 THEN 1
                                            WHEN rol_id = 4 THEN 2
                                            WHEN rol_id = 2 THEN 3
                                            WHEN rol_id = 3 THEN 4
                                            WHEN rol_id BETWEEN 5 AND 7 THEN 5
                                            WHEN rol_id BETWEEN 8 AND 10 THEN 6
                                            ELSE 7 
                                        END,
                                        media DESC";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdEquipo", equipo);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            List<(int id_jugador, int rol_id, double media)> jugadores = new();

                            while (reader.Read())
                            {
                                jugadores.Add((
                                    reader.GetInt32(0),
                                    reader.GetInt32(1),
                                    reader.GetDouble(2)
                                ));
                            }

                            if (jugadores.Count == 0) return;

                            // Agrupar por rol
                            var porteros = jugadores.Where(j => j.rol_id == 1).ToList();
                            var centrales = jugadores.Where(j => j.rol_id == 4).ToList();
                            var lateralDer = jugadores.Where(j => j.rol_id == 2).ToList();
                            var lateralIzq = jugadores.Where(j => j.rol_id == 3).ToList();
                            var mediocampistas = jugadores.Where(j => j.rol_id >= 5 && j.rol_id <= 7).ToList();
                            var delanteros = jugadores.Where(j => j.rol_id >= 8 && j.rol_id <= 10).ToList();

                            Dictionary<int, int> posiciones = new();
                            int posicion = 1;

                            if (porteros.Count >= 1) posiciones[posicion++] = porteros[0].id_jugador;
                            if (centrales.Count >= 2) { posiciones[posicion++] = centrales[0].id_jugador; posiciones[posicion++] = centrales[1].id_jugador; }
                            if (lateralDer.Count >= 1) posiciones[posicion++] = lateralDer[0].id_jugador;
                            if (lateralIzq.Count >= 1) posiciones[posicion++] = lateralIzq[0].id_jugador;

                            for (int i = 0; i < Math.Min(4, mediocampistas.Count); i++)
                                posiciones[posicion++] = mediocampistas[i].id_jugador;

                            for (int i = 0; i < Math.Min(2, delanteros.Count); i++)
                                posiciones[posicion++] = delanteros[i].id_jugador;

                            // Si hay menos de 11 jugadores asignados, completar con los mejores disponibles restantes
                            var usados = new HashSet<int>(posiciones.Values);
                            var restantes = jugadores.Where(j => !usados.Contains(j.id_jugador)).ToList();

                            while (posiciones.Count < 11 && restantes.Count > 0)
                            {
                                posiciones[posicion++] = restantes[0].id_jugador;
                                usados.Add(restantes[0].id_jugador);
                                restantes.RemoveAt(0);
                            }

                            // Insertar titulares
                            foreach (var kvp in posiciones)
                            {
                                string insertQuery = "INSERT INTO alineacion (id_jugador, posicion) VALUES (@id_jugador, @posicion)";
                                using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conexion))
                                {
                                    insertCmd.Parameters.AddWithValue("@id_jugador", kvp.Value);
                                    insertCmd.Parameters.AddWithValue("@posicion", kvp.Key);
                                    insertCmd.ExecuteNonQuery();
                                }
                            }

                            // Insertar suplentes
                            int pos = 12;
                            foreach (var suplente in jugadores.Where(j => !posiciones.Values.Contains(j.id_jugador)))
                            {
                                string insertQuery = "INSERT INTO alineacion (id_jugador, posicion) VALUES (@id_jugador, @posicion)";
                                using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conexion))
                                {
                                    insertCmd.Parameters.AddWithValue("@id_jugador", suplente.id_jugador);
                                    insertCmd.Parameters.AddWithValue("@posicion", pos++);
                                    insertCmd.ExecuteNonQuery();
                                }
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
        }
    }
}