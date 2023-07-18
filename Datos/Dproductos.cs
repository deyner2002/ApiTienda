using Microsoft.Extensions.Options;
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
        private async Task<long> ExistProducto(string? descripcion, long? precio)
        {
            long id = 0;

            try
            {
                using (var conn = new OracleConnection(_ConnectionStrings.WebConnection))
                {
                    await conn.OpenAsync();
                    var cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT id, descripcion FROM DBTIENDA.PRODUCTOS WHERE descripcion = :P_descripion AND precio = :P_precio";

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_descripcion", Value = descripcion });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_precio", Value = precio });

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
                                descripcion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToString(item.ItemArray[1]) : "SIN DESCRIPCION",
                                precio = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToInt64(item.ItemArray[2]) : 0,

                            });
                        }
                    }

                }

                catch (Exception ex)
                {

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

                        if (PRODUCTOS.descripcion != "" && PRODUCTOS.precio != 0)
                        {
                            id = await ExistProducto(PRODUCTOS.descripcion, PRODUCTOS.precio);
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
                                        (id, descripion, precio)
                                        VALUES(DBTIENDA.SEQUENCEPRODUCTOS.NEXTVAL, :P_descripcion, :P_precio)
                                        ";
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_descripcion", Value = PRODUCTOS.descripcion });
                                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_precio", Value = PRODUCTOS.precio });
                               
                                await cmd.ExecuteNonQueryAsync();

                                cmd.CommandText = @"
                                        select DBTIENDA.SEQUENCEPRODUCTOS.currval from dual
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
                catch (Exception ex)
                {
                    Console.WriteLine("Ha ocurrido un error: " + ex.ToString());
                }
                return id;
            }
        public async Task<long> EditarProductos(Mproductos PRODUCTOS)
        {
            long id = 0;
            try
            {
                using (var conn = new OracleConnection(_ConnectionStrings.WebConnection))
                {

                    if (PRODUCTOS.descripcion != "" && PRODUCTOS.precio != 0)
                    {
                        id = await ExistProducto(PRODUCTOS.descripcion, PRODUCTOS.precio);
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
                                        (id, descripion, precio)
                                        VALUES(DBTIENDASEQUENCEPRODUCTOS.NEXTVAL, :P_descripcion, :P_precio)
                                        ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_id", Value = PRODUCTOS.id });
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_precio", Value = PRODUCTOS.precio });

                            await cmd.ExecuteNonQueryAsync();

                            cmd.CommandText = @"
                                        select DBTIENDA.SEQUENCEPRODUCTOS.currval from dual
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
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error: " + ex.ToString());
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

                        if (PRODUCTOS.descripcion != "" && PRODUCTOS.precio != 0)
                        {
                            id = await ExistProducto(PRODUCTOS.descripcion, PRODUCTOS.precio);
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
                                        (id, descripion, precio)
                                        VALUES(DBTIENDASEQUENCEPRODUCTOS.NEXTVAL, :P_descripcion, :P_precio)
                                        ";
                                cmd.Parameters.Clear();
                                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_id", Value = PRODUCTOS.id });

                                await cmd.ExecuteNonQueryAsync();

                                cmd.CommandText = @"
                                        select DBTIENDA.SEQUENCEPRODUCTOS.currval from dual
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
                catch (Exception ex)
                {
                    Console.WriteLine("Ha ocurrido un error: " + ex.ToString());
                }
                return id;
            }
    }
}