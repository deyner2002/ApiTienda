using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Oracle.ManagedDataAccess.Client;
using System.Data;
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
        private async Task<long> ExistProducto(string? DESCRIPCION, long? PRECIO)
        {
            long id = 0;

            try
            {
                using (var conn = new OracleConnection(_ConnectionStrings.WebConnection))
                {
                    await conn.OpenAsync();
                    var cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT id, DESCRIPCION FROM DBTIENDA.PRODUCTOS WHERE DESCRIPCION = :P_DESCRIPCION AND PRECIO = :P_PRECIO";

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_DESCRIPCION", Value = DESCRIPCION });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_PRECIO", Value = PRECIO });

                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);

                    await conn.CloseAsync();

                    if (data.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in data.Tables[0].Rows)
                        {
                            id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                        }
                    }

                    return id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error: " + ex.ToString());
            }
            return id;
        }
        public async Task<List<Mproductos>> MostrarProductos()
        {
            var lista = new List<Mproductos>();

            using (var conn = new OracleConnection(_ConnectionStrings.WebConnection))
            {
                try
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
                                DESCRIPCION = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToString(item.ItemArray[1]) : "SIN DESCRIPCION",
                                PRECIO = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToInt64(item.ItemArray[2]) : 0,

                            });
                        }
                    }

                }

                catch (Exception ex)
                {
                    Console.WriteLine("Ha ocurrido un error inesperado: " + ex.ToString());
                }
                return lista;
            }
        }
        public async Task<long> InsertarProductos(Mproductos PRODUCTOS)
        {
                long id = 0;
                try
                {
                    using (var conn = new OracleConnection(_ConnectionStrings.WebConnection))
                    {

                        if (PRODUCTOS.DESCRIPCION != "" && PRODUCTOS.PRECIO != 0)
                        {
                            id = await ExistProducto(PRODUCTOS.DESCRIPCION, PRODUCTOS.PRECIO);
                            if (id > 0)
                            {
                                return id;
                            }
                            else
                            {
                                await conn.OpenAsync();
                                var cmd = new OracleCommand();
                                cmd.Connection = conn;
                                cmd.CommandText = @"
                                        INSERT INTO DBTIENDA.PRODUCTOS
                                        (id, DESCRIPCION, PRECIO)
                                        VALUES(DBTIENDA.SECUENCIAPRODUCTOS.NEXTVAL, :P_DESCRIPCION, :P_PRECIO)
                                        ";
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_DESCRIPCION", Value = PRODUCTOS.DESCRIPCION });
                                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_PRECIO", Value = PRODUCTOS.PRECIO });
                               
                                await cmd.ExecuteNonQueryAsync();

                                cmd.CommandText = @"
                                        select DBTIENDA.SECUENCIAPRODUCTOS.currval from dual
                                        ";


                                await cmd.ExecuteNonQueryAsync();

                                var adapter = new OracleDataAdapter(cmd);
                                var data = new DataSet("Datos");
                                adapter.Fill(data);


                                await conn.CloseAsync();

                                if (data.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow item in data.Tables[0].Rows)
                                    {
                                        id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            return -2;
                        }
                    }
                }
                catch (Exception)
                {
                    return -1;
                }
                return id;
            }
        public async Task<long> EditarProductos(int id, long PRECIO)
        {
            try
            {
                using (var conn = new OracleConnection(_ConnectionStrings.WebConnection))
                {

                    if (PRECIO != 0)
                    {
                        await conn.OpenAsync();
                        var cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = string.Format(@"
                                        UPDATE DBTIENDA.PRODUCTOS
                                        set PRECIO = {1}
                                        where id = {0}
                                        ", id, PRECIO);  

                       int row =  await cmd.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        return -2;
                    }
                }
            }
            catch (Exception)
            {
                return -1;
            }
            return id;
        }
        public async Task<long> EliminarProductos(Mproductos PRODUCTOS)
        {
            long id = 0;
            try
            {
                using (var conn = new OracleConnection(_ConnectionStrings.WebConnection))
                {

                    if (PRODUCTOS.id > 0)
                        {
                            id = await ExistProducto(PRODUCTOS.DESCRIPCION, PRODUCTOS.PRECIO);
                            if (id > 0)
                            {
                                return id;
                            }
                            else
                            {
                                await conn.OpenAsync();
                                var cmd = new OracleCommand();
                                cmd.Connection = conn;
                                cmd.CommandText = @"
                                        DELETE FROM DBTIENDA.PRODUCTOS
                                        where id = :P_id
                                        ";
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.Input, ParameterName = "P_id", Value = PRODUCTOS.id });

                                await cmd.ExecuteNonQueryAsync();
                        }
                        }
                        else
                        {
                            return -2;
                        }
                    }
                }
                catch (Exception)
                {
                return -1;
                }
                return id;
            }
    }
}