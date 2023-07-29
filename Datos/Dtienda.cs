using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using TiendaApi.Interfaces;
using TiendaApi.modelo;

namespace TiendaApi.Datos
{
    public class Dtienda
    {
        public ConnectionStrings _ConnectionStrings { get; set; }
        public Dtienda(IOptions<ConnectionStrings> ConnectionStrings)
        {
            _ConnectionStrings = ConnectionStrings.Value;
        }

        private async Task<long> ExistTienda(string? NOMBRE, string? CUIDAD, string? DIRECCION)
        {
            long CONSECUTIVO = 0;

            try
            {
                using (var conn = new OracleConnection(_ConnectionStrings.WebConnection))
                {
                    await conn.OpenAsync();
                    var cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT CONSECUTIVO, NOMBRE, CUIDAD, DIRECCION FROM DBTIENDA.TIENDA WHERE NOMBRE = :P_NOMBRE AND CUIDAD = :P_CUIDAD AND DIRECCION = :P_DIRECCION";

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBRE", Value = NOMBRE });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_CUIDAD", Value = CUIDAD });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_DIRECCION", Value = DIRECCION });

                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);

                    await conn.CloseAsync();

                    if (data.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in data.Tables[0].Rows)
                        {
                            CONSECUTIVO = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                        }
                    }

                    return CONSECUTIVO;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error: " + ex.ToString());
            }
            return CONSECUTIVO;
        }
        public async Task<List<Mtienda>> MostrarTiendas()
        {
            var lista = new List<Mtienda>();

            using (var conn = new OracleConnection(_ConnectionStrings.WebConnection))
            {
                try
                {

                    await conn.OpenAsync();
                    var cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT CONSECUTIVO, NOMBRE, CUIDAD, DIRECCION FROM DBTIENDA.TIENDA";

                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);

                    await conn.CloseAsync();

                    if (data.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in data.Tables[0].Rows)
                        {
                            lista.Add(new Mtienda
                            {
                                CONSECUTIVO = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                                NOMBRE = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToString(item.ItemArray[1]) : "SIN DESCRIPCION",
                                CUIDAD = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToString(item.ItemArray[2]) : "SIN DESCRIPCION",
                                DIRECCION = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN DESCRIPCION",

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
    }
}
//        public async Task<long> InsertarTiendas(Mtienda PRODUCTOS)
//    {
//        long id = 0;
//        try
//        {
//            using (var conn = new OracleConnection(_ConnectionStrings.WebConnection))
//            {

//                if (PRODUCTOS.DESCRIPCION != "" && PRODUCTOS.PRECIO != 0)
//                {
//                    id = await ExistProducto(PRODUCTOS.DESCRIPCION, PRODUCTOS.PRECIO, PRODUCTOS.CONSECUTIVOTIENDA);
//                    if (id > 0)
//                    {
//                        return id;
//                    }
//                    else
//                    {
//                        await conn.OpenAsync();
//                        var cmd = new OracleCommand();
//                        cmd.Connection = conn;
//                        cmd.CommandText = @"
//                                        INSERT INTO DBTIENDA.PRODUCTOS
//                                        (id, DESCRIPCION, PRECIO, CONSECUTIVOTIENDA)
//                                        VALUES(DBTIENDA.SECUENCIAPRODUCTOS.NEXTVAL, :P_DESCRIPCION, :P_PRECIO, :P_CONSECUTIVOTIENDA)
//                                        ";
//                        cmd.Parameters.Clear();
//                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_DESCRIPCION", Value = PRODUCTOS.DESCRIPCION });
//                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_PRECIO", Value = PRODUCTOS.PRECIO });
//                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_CONSECUTIVOTIENDA", Value = PRODUCTOS.CONSECUTIVOTIENDA });

//                        await cmd.ExecuteNonQueryAsync();

//                        cmd.CommandText = @"
//                                        select DBTIENDA.SECUENCIAPRODUCTOS.currval from dual
//                                        ";


//                        await cmd.ExecuteNonQueryAsync();

//                        var adapter = new OracleDataAdapter(cmd);
//                        var data = new DataSet("Datos");
//                        adapter.Fill(data);


//                        await conn.CloseAsync();

//                        if (data.Tables[0].Rows.Count > 0)
//                        {
//                            foreach (DataRow item in data.Tables[0].Rows)
//                            {
//                                id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    return -2;
//                }
//            }
//        }
//        catch (Exception)
//        {
//            return -1;
//        }
//        return id;
//    }
//        public async Task<long> EditarTiendas(int id, long PRECIO, int CONSECUTIVOTIENDA)
//    {
//        try
//        {
//            using (var conn = new OracleConnection(_ConnectionStrings.WebConnection))
//            {

//                if (PRECIO != 0)
//                {
//                    await conn.OpenAsync();
//                    var cmd = new OracleCommand();
//                    cmd.Connection = conn;
//                    cmd.CommandText = string.Format(@"
//                                        UPDATE DBTIENDA.PRODUCTOS
//                                        set CONSECUTIVOTIENDA = {2}
//                                        set PRECIO = {1}
//                                        where id = {0}
//                                        ", id, PRECIO, CONSECUTIVOTIENDA);

//                    int row = await cmd.ExecuteNonQueryAsync();
//                }
//                else
//                {
//                    return -2;
//                }
//            }
//        }
//        catch (Exception)
//        {
//            return -1;
//        }
//        return id;
//    }
//        public async Task<long> EliminarTiendas(Mtienda PRODUCTOS)
//    {
//        long id = 0;
//        try
//        {
//            using (var conn = new OracleConnection(_ConnectionStrings.WebConnection))
//            {

//                if (PRODUCTOS.id > 0)
//                {
//                    id = await ExistProducto(PRODUCTOS.DESCRIPCION, PRODUCTOS.PRECIO, PRODUCTOS.CONSECUTIVOTIENDA);
//                    if (id > 0)
//                    {
//                        return id;
//                    }
//                    else
//                    {
//                        await conn.OpenAsync();
//                        var cmd = new OracleCommand();
//                        cmd.Connection = conn;
//                        cmd.CommandText = @"
//                                        DELETE FROM DBTIENDA.PRODUCTOS
//                                        where id = :P_id
//                                        ";
//                        cmd.Parameters.Clear();
//                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Int64, Direction = ParameterDirection.Input, ParameterName = "P_id", Value = PRODUCTOS.id });

//                        await cmd.ExecuteNonQueryAsync();
//                    }
//                }
//                else
//                {
//                    return -2;
//                }
//            }
//        }
//        catch (Exception)
//        {
//            return -1;
//        }
//        return id;
//    }
//}
//}

