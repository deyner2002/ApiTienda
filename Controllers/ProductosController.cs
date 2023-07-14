using Microsoft.AspNetCore.Mvc;
using TiendaApi.Datos;
using TiendaApi.Interfaces;
using TiendaApi.modelo;

namespace TiendaApi.Controllers
{
    [ApiController]
    [Route("api/productos")]
    public class ProductosController:ControllerBase
    {
        public readonly IDproductos _Dproductos;

        public ProductosController(IDproductos dproductos)
        {
            _Dproductos= dproductos;
        }

        [HttpGet]
        public async Task<ActionResult<List<Mproductos>>> Get()
        {
            var lista = await _Dproductos.mostrarProductos();
            return lista;
        }
        [HttpPost]
        public async Task Post([FromBody] Mproductos PRODUCTOS)
        {
            var info = new Dproductos();
            await info.insertarProductos(PRODUCTOS);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id,[FromBody] Mproductos PRODUCTOS)
        {
            var info = new Dproductos();
            PRODUCTOS.id = id;
            await info.editarProductos(PRODUCTOS);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var info = new Dproductos();
            var PRODUCTOS=new Mproductos();
            PRODUCTOS.id = id;
            await info.editarProductos(PRODUCTOS);
            return NoContent();
        }
    }


}
