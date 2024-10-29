using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
    public class GestprojectClient
    {
        public int PAR_ID { get; set; }
        public string PAR_SUBCTA_CONTABLE { get; set; }
        public string PAR_NOMBRE { get; set; }
        public string PAR_NOMBRE_COMERCIAL { get; set; }
        public string PAR_CIF_NIF { get; set; }
        public string PAR_DIRECCION_1 { get; set; }
        public string PAR_CP_1 { get; set; }
        public string PAR_LOCALIDAD_1 { get; set; }
        public string PAR_PROVINCIA_1 { get; set; }
        public string PAR_PAIS_1 { get; set; }
        public string synchronization_status { get; set; } = "Nunca ha sido sincronizado";
        public string sage50_client_code { get; set; } = "";
        public string sage50_guid_id { get; set; } = "";
        public string sage50_instance { get; set; } = "";
        public string comments { get; set; } = "";
        public DateTime last_record { get; set; } = DateTime.Now;
        public int synchronization_table_id { get; set; }
    }

    public class Sage50Client
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
