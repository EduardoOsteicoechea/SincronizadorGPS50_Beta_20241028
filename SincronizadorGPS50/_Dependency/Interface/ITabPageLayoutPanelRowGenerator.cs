using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   internal interface ITabPageLayoutPanelRowGenerator
   {
      Infragistics.Win.Misc.UltraPanel RowPanel { get; set; }
      Infragistics.Win.Misc.UltraPanel GenerateRowPanel();
   }
}
