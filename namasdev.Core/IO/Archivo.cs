
namespace namasdev.Core.IO
{
    public class Archivo
    {
        public string Nombre { get; set; }
        public byte[] Contenido { get; set; }

        public bool EsVacio
        {
            get { return Contenido == null || Contenido.Length == 0; }
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
