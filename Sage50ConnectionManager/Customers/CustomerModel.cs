

namespace SincronizadorGPS50.Sage50Connector
{
    public class Sage50Customer
    {
        public string CODIGO { get; set; }
        public string CIF { get; set; }
        public string NOMBRE { get; set; }
        public string NOMBRE2 { get; set; }
        public string DIRECCION { get; set; }
        public string CODPOST { get; set; }
        public string POBLACION { get; set; }
        public string PROVINCIA { get; set; }
        public string PAIS { get; set; }
        public string EMAIL { get; set; }
        public string HTTP { get; set; }
        public string GUID_ID { get; set; }
        public string CODIGO_TIPO
        {
            get
            {
                return CODIGO.Substring(0, 4);
            }
        }
        public int CODIGO_NUMERO
        {
            get
            {
                return int.Parse(CODIGO.Substring(4));
            }
        }
    }
}
