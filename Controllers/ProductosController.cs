using Microsoft.AspNetCore.Mvc;
using TiendaApi.Datos;
using TiendaApi.Interfaces;
using TiendaApi.modelo;

namespace TiendaApi.Controllers
{
    [ApiController]
    [Route("api/productos")]
    public class ProductosController
    {
        public readonly IDproductos _Dproductos;

        public ProductosController(IDproductos dproductos)
        {
            _Dproductos= dproductos;
        }

        [HttpGet]
    public async Task <ActionResult<List<Mproductos>>> Get()
        {
            var lista = await _Dproductos.mostrarProductos();
            return lista;


        }
    }
}
