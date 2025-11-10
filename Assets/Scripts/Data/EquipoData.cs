using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class EquipoData
    {
        private static string dbPath = Path.Combine(Application.streamingAssetsPath, Constants.DATABASE_NAME);

        // --------------------------------------------------------------------- MÉTODO QUE DEVUELVE EL NOMBRE DEL EQUIPO SEGÚN SU ID_EQUIPO
        public static string GetTeamNameById(int idEquipo)
        {
            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return "";
            }

            using (var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT nombre FROM equipos WHERE id_equipo=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", idEquipo);
                    var result = command.ExecuteScalar();
                    if (result != null)
                        return result.ToString();
                }
            }

            return "";
        }

        // ------------------------------------------------------------------------- MÉTODO QUE DEVUELVE LOS DETALLES DE UN EQUIPO POR SU ID
        // ------------------------------------------------------------------------- MÉTODO QUE DEVUELVE LOS DETALLES DE UN EQUIPO POR SU ID
        public static Equipo ObtenerDetallesEquipo(int idEquipo)
        {
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
                    comando.CommandText = @"SELECT 
                                                id_equipo, nombre, nombre_corto, presidente, presupuesto, ciudad, estadio, 
                                                aforo, reputacion, objetivo, rival, id_competicion, competicion_europea, 
                                                ruta_imagen, ruta_imagen120, ruta_imagen80, ruta_imagen64, ruta_imagen32, 
                                                ruta_estadio_interior, ruta_estadio_exterior, ruta_kit_local, ruta_kit_visitante
                                            FROM equipos 
                                            WHERE id_equipo = @idEquipo";

                    comando.Parameters.AddWithValue("@idEquipo", idEquipo);

                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Equipo
                            {
                                IdEquipo = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                NombreCorto = reader.GetString(2),
                                Presidente = reader.GetString(3),
                                Presupuesto = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                Ciudad = reader.GetString(5),
                                Estadio = reader.GetString(6),
                                Aforo = reader.GetInt32(7),
                                Reputacion = reader.GetInt32(8),
                                Objetivo = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                Rival = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                                IdCompeticion = reader.GetInt32(11),
                                CompeticionEuropea = reader.IsDBNull(12) ? 0 : reader.GetInt32(12),
                                RutaImagen = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                                RutaImagen120 = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                                RutaImagen80 = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                                RutaImagen64 = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                                RutaImagen32 = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                                RutaEstadioInterior = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                                RutaEstadioExterior = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                                RutaKitLocal = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                                RutaKitVisitante = reader.IsDBNull(21) ? string.Empty : reader.GetString(21)
                            };
                        }
                    }
                }
            }

            return null;
        }

        // --------------------------------------------------------------------- MÉTODO QUE MUESTRA UNA LISTA DE LOS EQUIPOS POR COMPETICIÓN 
        public static List<Equipo> ObtenerEquiposPorCompeticion(int idCompeticion)
        {
            List<Equipo> lista = new List<Equipo>();

            if (!File.Exists(dbPath))
            {
                Debug.LogError($"No se encontró la base de datos en {dbPath}");
                return lista;
            }

            using (var conexion = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conexion.Open();
                using (var comando = conexion.CreateCommand())
                {
                    comando.CommandText = "SELECT id_equipo, nombre, nombre_corto FROM equipos WHERE id_competicion=@idComp";
                    comando.Parameters.AddWithValue("@idComp", idCompeticion);

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Equipo
                            {
                                IdEquipo = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                NombreCorto = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            return lista;
        }
    }
}