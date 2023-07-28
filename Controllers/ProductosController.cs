using Microsoft.AspNetCore.Mvc;
using TiendaApi.Datos;
using TiendaApi.Interfaces;
using TiendaApi.modelo;

namespace TiendaApi.Controllers
{
    [ApiController]
    [Route("api/productos")]
    public class ProductosController : ControllerBase
    {
        public readonly IDproductos _Dproductos;

        public ProductosController(IDproductos dproductos)
        {
            _Dproductos = dproductos;
        }

        [HttpGet]
        public async Task<Mrespuesta> Get()
        {
            Mrespuesta response = new Mrespuesta();

            try
            {
                response.Datos = _Dproductos.MostrarProductos().Result;
                if (response.Datos != null)
                {
                    response.IsError = false;
                    response.Mensaje = "Acceso permitido a los datos del sistema";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                }

            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.Mensaje = "Acceso denegado, ha ocurrido un error en el sistema" + ex.ToString();
            }

            return response;
        }
        [HttpPost]
        public async Task<Mrespuesta> Post([FromBody] Mproductos PRODUCTOS)
        {
            Mrespuesta response = new Mrespuesta();

            try
            {
                long codigoRespuesta = _Dproductos.InsertarProductos(PRODUCTOS).Result;
                response.Datos = codigoRespuesta;
                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = "La descripcion del producto no puede quedar vacio ni su precio ser cero, Por favor llene los campos correctamente ";
                }
                else
                {
                    if (codigoRespuesta == -1)
                    {
                        response.IsError = true;
                        response.Mensaje = "Error del sistema";
                    }
                    else
                    {
                        response.IsError = false;
                        response.Mensaje = "Producto Guardado con exito";
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.Mensaje = "Error del sistema" + ex.ToString();
            }
            return response;
        }
        [HttpPut("{id}")]
        public async Task<Mrespuesta> Put(int id, long PRECIO)
        {
            Mrespuesta response = new Mrespuesta();

            try
            {
                long codigoRespuesta = _Dproductos.EditarProductos(id, PRECIO).Result;
                response.Datos = codigoRespuesta;
                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = " El precio del producto no puede ser cero, Por favor asigne el precio correctamente ";
                }
                else
                {
                    if (codigoRespuesta == -1)
                    {
                        response.IsError = true;
                        response.Mensaje = "Error del sistema";
                    }
                    else
                    {
                        response.IsError = false;
                        response.Mensaje = "Producto Guardado con exito";
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.Mensaje = "Error del sistema" + ex.ToString();
            }
            return response;
     
        }
        [HttpDelete("{id}")]
        public async Task<Mrespuesta> Delete(int id)
        {
            var PRODUCTOS = new Mproductos();
            PRODUCTOS.id = id;
            Mrespuesta response = new Mrespuesta();

            try
            {
                long codigoRespuesta = _Dproductos.EliminarProductos(PRODUCTOS).Result;
                response.Datos = codigoRespuesta;
                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = " El producto que desea eliminar no existe";
                }
                else
                {
                    if (codigoRespuesta == -1)
                    {
                        response.IsError = true;
                        response.Mensaje = "Error del sistema";
                    }
                    else
                    {
                        response.IsError = false;
                        response.Mensaje = "Producto Eliminado con exito";
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.Mensaje = "Error del sistema" + ex.ToString();
            }
            return response;
        }
    }


}
