using TiendaApi.modelo;

namespace TiendaApi.Interfaces
{
    public interface IDTiendas
    {
        Task<List<Mproductos>> MostrarTiendas();

        Task<long> InsertarTiendas(Mproductos PRODUCTOS);

        Task<long> EditarTiendas(int id, long PRECIO);

        Task<long> EliminarTiendas(Mproductos PRODUCTOS);
    }
}
