
namespace namasdev.Core.Types
{
    public class OrdenYPaginacionParametros
    {
        public string Orden { get; set; }
        public string OrdenDefault { get; set; }
        public int Pagina { get; set; }
        public int CantRegistrosPorPagina { get; set; }
        public int CantRegistrosTotales { get; set; }
    }
}
