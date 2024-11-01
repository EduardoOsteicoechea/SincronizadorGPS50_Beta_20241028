﻿using System;

namespace SincronizadorGPS50
{
   public class Sage50IssuedBillModel
   {
      // Sage50 fields
      public string GUID_ID { get; set; } = "";
      public string EMPRESA { get; set; } = "";
      public string LETRA { get; set; } = "";
      public string NUMERO { get; set; } = "";
      public DateTime FECHA { get; set; } = DateTime.Now;
      public string OBRA { get; set; } = "";
      public string CLIENTE { get; set; } = "";
      public decimal IMPORTE { get; set; } = 0;
      public decimal TOTALDOC { get; set; } = 0;
      public string OBSERVACIO { get; set; } = "";


      // Additional reference fields
      public string IMP_TIPO { get; set; } = "";
   }
}
