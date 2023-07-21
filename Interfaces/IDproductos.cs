using TiendaApi.modelo;

namespace TiendaApi.Interfaces
{
    public interface IDproductos
    {
        Task<List<Mproductos>> MostrarProductos();

        Task<long> InsertarProductos(Mproductos PRODUCTOS);

        Task<long> EditarProductos(int id, long PRECIO);

        Task<long> EliminarProductos(Mproductos PRODUCTOS);

    }
}
