

namespace SincronizadorGPS50.Workflows.Sage50Connection
{
   internal static class StateManager
   {
      internal static string State { get; set; } = UIStates.StatefulStart;
   }
   internal struct UIStates
   {
      public const string StatefulStart = "StatefulStart";
      public const string StatelessStart = "StatelessStart";
      public const string StatefulAwaitForFullDataRevision = "StatefulAwaitForFullDataRevision";
      public const string AwaitingLocalTerminalUserData = "AwaitingLocalTerminalUserData";
      public const string AwaitingConnectionWithUserDataAction = "AwaitingConnectionWithUserDataAction";
      public const string AwaitingCompanyGroupSelection = "AwaitingCompanyGroupSelection";
      public const string AwaitingConnectToCompanyGroupAndRememberAllInstruction = "AwaitingConnectToCompanyGroupAndRememberAllInstruction";
      public const string Connected = "Connected";
      public const string EditingConnection = "EditingConnection";
   };
}
