using TiendaApi.modelo;

namespace TiendaApi.Interfaces
{
    public interface IDproductos
    {
        Task<List<Mproductos>> MostrarProductos();

        Task<long> InsertarProductos(Mproductos PRODUCTOS);

        Task<long> EditarProductos(int id, long PRECIO, int CONSECUTIVOTIENDA);

        Task<long> EliminarProductos(Mproductos PRODUCTOS);

    }
    
}
