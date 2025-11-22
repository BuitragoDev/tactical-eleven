using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public class PalmaresData
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

        // ------------------------------------------------------------------- MÉTODO PARA MOSTRAR EL PALMARÉS DEL MÁNAGER
        public static List<PalmaresManager> MostrarPalmaresManager(int equipo)
        {
            var dbPath = GetDBPath();
            List<PalmaresManager> listadoPalmares = new List<PalmaresManager>();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return null;
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = @"SELECT id_palmares, id_competicion, id_equipo, temporada
                                            FROM palmares_manager
                                            WHERE id_equipo = @IdEquipo
                                            ORDER BY id_palmares DESC";

                    comando.Parameters.AddWithValue("@IdEquipo", equipo);

                    using (SQLiteDataReader dr = comando.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listadoPalmares.Add(new PalmaresManager()
                            {
                                // Usamos el operador de coalescencia nula para evitar la asignación de null
                                IdEquipo = dr["id_equipo"] != DBNull.Value ? Convert.ToInt32(dr["id_equipo"]) : 0,
                                IdCompeticion = dr["id_competicion"] != DBNull.Value ? Convert.ToInt32(dr["id_competicion"]) : 0,
                                Temporada = dr["temporada"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }
            }

            return listadoPalmares;
        }
    }
}