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
        public async Task<ActionResult<List<Mproductos>>> Get()
        {
            return await _Dproductos.MostrarProductos();
        }
        [HttpPost]
        public async Task<long> Post([FromBody] Mproductos PRODUCTOS)
        {
            return await _Dproductos.InsertarProductos(PRODUCTOS);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Mproductos PRODUCTOS)
        {
            PRODUCTOS.id = id;
            await _Dproductos.EditarProductos(PRODUCTOS);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var PRODUCTOS = new Mproductos();
            PRODUCTOS.id = id;
            await _Dproductos.EliminarProductos(PRODUCTOS);
            return NoContent();
        }
    }


}
