using TiendaApi.modelo;

namespace TiendaApi.Interfaces
{
    public interface IDproductos
    {
        Task<List<Mproductos>> MostrarProductos();

        Task<long> InsertarProductos(Mproductos PRODUCTOS);

        Task<long> EditarProductos(Mproductos PRODUCTOS);

        Task<long> EliminarProductos(Mproductos PRODUCTOS);

    }
}
