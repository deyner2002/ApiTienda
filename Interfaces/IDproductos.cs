using TiendaApi.modelo;

namespace TiendaApi.Interfaces
{
    public interface IDproductos
    {
        Task<List<Mproductos>> mostrarProductos();
    }
}
