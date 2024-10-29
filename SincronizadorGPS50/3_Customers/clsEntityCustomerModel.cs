using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   public class clsEntityCustomerModel
    {
        public string codigo { get; set; } = "";
        public string pais { get; set; } = "";
        public string cif { get; set; } = "";
        public string nombre { get; set; } = "";
        public string razoncomercial { get; set; } = "";
        public bool contado { get; set; } = false; 
        public string telefono { get; set; } = "";
        public string fpago { get; set; } = "";
        public string direccion { get; set; } = "";
        public string poblacion { get; set; } = "";
        public string provincia { get; set; } = "";
        public string codpos { get; set; } = "";
        public string nombrebanco { get; set; } = "";
        public string iban { get; set; } = "";
        public string swift { get; set; } = "";
        public clsEntityMandate mandato { get; set; } = null;
        public string tipo_iva { get; set; } = "";
        public string tipo_ret { get; set; } = "";
        public int modoret { get; set; } = 1; // 1 - Sobre base, 2 - Sobrefactura
        public Boolean recargo { get; set; } = false;
        public Boolean existeregistro { get; set; } = false;
    }
}
