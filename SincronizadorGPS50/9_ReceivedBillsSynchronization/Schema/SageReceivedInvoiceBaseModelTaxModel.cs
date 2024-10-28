using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   public class SageReceivedInvoiceBaseModelTaxModel
   {
      public string _Codigo {get;set;} = "";
      public decimal _Base {get;set;} = 0;
      public decimal _PrcIva {get;set;} = 0;
      public decimal _ImpIva {get;set;} = 0;
      public decimal _PrcRec {get;set;} = 0;
      public decimal _ImpRec {get;set;} = 0;
      public decimal _BaseDivisa {get;set;} = 0;
      public decimal _ImpIvaDivisa {get;set;} = 0;
      public decimal _ImpRecDivisa {get;set;} = 0;
   }
}
