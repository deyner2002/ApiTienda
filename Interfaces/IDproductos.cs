using TiendaApi.modelo;

namespace TiendaApi.Interfaces
{
    public interface IDproductos
    {
        Task<List<Mproductos>> MostrarProductos();

        Task<List<Mproductos>> InsertarProductos(Mproductos producto);

        Task<List<Mproductos>> EditarProductos(Mproductos producto);

        Task<List<Mproductos>> EliminarProductos(long id);

    }
}
