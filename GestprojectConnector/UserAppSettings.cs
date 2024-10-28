using System;

namespace Dinq.Gestproject
{
   /// <summary>
   /// Clase que contiene los elementos de configuración de la aplicación de ambito usuario (relativas a cada usuario)
   /// </summary>
   [Serializable()]
   class UserAppSettings
   {
      private bool confirmEndSession = true;
      public bool ConfirmEndSession
      {
         get { return confirmEndSession; }
         set { confirmEndSession = value; }
      }

      private string defaultCountry = "";
      public string DefaultCountry
      {
         get { return defaultCountry; }
         set { defaultCountry = value; }
      }

      private string defaultProvince = "";
      public string DefaultProvince
      {
         get { return defaultProvince; }
         set { defaultProvince = value; }
      }

      private string defaultLocality = "";
      public string DefaultLocality
      {
         get { return defaultLocality; }
         set { defaultLocality = value; }
      }

      private string connectionID = "";
      public string ConnectionID
      {
         get { return connectionID; }
         set { connectionID = value; }
      }

      private string lastUser = "";
      public string LastUser
      {
         get { return lastUser; }
         set { lastUser = value; }
      }


      private int messageInterval = 5;
      public int MessageInterval
      {
         get { return messageInterval; }
         set { messageInterval = value; }
      }

      private bool sendMessages = false;
      public bool SendMessages
      {
         get { return sendMessages; }
         set { sendMessages = value; }
      }

      private string schedulerBeginHour = "08:00";
      public string SchedulerBeginHour
      {
         get { return schedulerBeginHour; }
         set { schedulerBeginHour = value; }
      }

      private string schedulerEndHour = "20:00";
      public string SchedulerEndHour
      {
         get { return schedulerEndHour; }
         set { schedulerEndHour = value; }
      }

      private int schedulerInterval = 5;
      public int SchedulerInterval
      {
         get { return schedulerInterval; }
         set { schedulerInterval = value; }
      }

      private bool useAppStyles = true;
      public bool UseAppStyles
      {
         get { return useAppStyles; }
         set { useAppStyles = value; }
      }

      private string styleFileName = "default.isl";
      public string StyleFileName
      {
         get { return styleFileName; }
         set { styleFileName = value; }
      }

      //DE07-108
      private int vigencia = 0;
      public int Vigencia
      {
         get { return vigencia; }
         set { vigencia = value; }
      }

      private bool restoreOpenWindows = false;
      public bool RestoreOpenWindows
      {
         get { return restoreOpenWindows; }
         set { restoreOpenWindows = value; }
      }

      //DE11-04-007 20190218
      private bool informarInicioRealTarea = true;
      public bool InformarInicioRealTarea
      {
         get { return informarInicioRealTarea; }
         set { informarInicioRealTarea = value; }
      }

      //DE12-00-007 20200420
      public string JornadaSession1BeginAt { get; set; } = "09:00";
      public string JornadaSession1EndAt { get; set; } = "14:00";
      public string JornadaSession2BeginAt { get; set; } = "15:00";
      public string JornadaSession2EndAt { get; set; } = "18:00";
   }
}
