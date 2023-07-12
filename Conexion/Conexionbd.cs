namespace TiendaApi.Conexion
{
    public class Conexionbd
    {
        private string connectionString = string.Empty;
        public Conexionbd()
        {
            var constructor = new ConfigurationBuilder().SetBasePath
                (Directory.GetCurrentDirectory()).AddJsonFile
                ("appsettings.json").Build();
            connectionString = constructor.GetSection
                ("ConnectionStrings:conexionmaestra").Value;
        }
        public string cadenaSQL()
        { 
            return connectionString;
        }
    }
}
