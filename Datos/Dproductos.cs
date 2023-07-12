using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using TiendaApi.Conexion;
using TiendaApi.Interfaces;
using TiendaApi.modelo;
namespace TiendaApi.Datos
{
    public class Dproductos : IDproductos
    {
        public ConnectionStrings _ConnectionStrings { get; set; }

        public Dproductos(IOptions<ConnectionStrings> ConnectionStrings)
        {
            _ConnectionStrings = ConnectionStrings.Value;
        }

        public async Task<List<Mproductos>> mostrarProductos()
        {
            var lista = new List<Mproductos>();

            
                try
                {
                using (var conn = new OracleConnection(_ConnectionStrings.WebConnection))
                {
                    await conn.OpenAsync();
                    var cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT ID, DESCRIPCION, PRECIO FROM DBTIENDA.PRODUCTOS";

                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);

                    await conn.CloseAsync();

                    if (data.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in data.Tables[0].Rows)
                        {
                            lista.Add(new Mproductos
                            {
                                id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                                descripcion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToString(item.ItemArray[1]) : "SIN DESCRIPCION",
                                precio = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToInt64(item.ItemArray[2]) : 0,

                            });
                        }
                    }
                }
                }
                catch (Exception ex)
                {
                 
                }
            return lista;
        }
    }
}
