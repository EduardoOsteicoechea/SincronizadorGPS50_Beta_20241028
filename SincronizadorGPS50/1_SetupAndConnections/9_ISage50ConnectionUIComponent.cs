using System;

namespace SincronizadorGPS50.Workflows.Sage50Connection
{
   public class ParentClass
   {
      public event EventHandler StateChanged;

      protected virtual void OnStateChanged()
      {
         StateChanged?.Invoke(this, EventArgs.Empty);
      }
   }
   internal interface ISage50ConnectionUIComponent : IDisposable
   {
      bool IsDataCleared { get; }
      void Forget();
      void Remember();

      event EventHandler DataCleared;
   }
   internal interface ISage50ConnectionUIModifiableControls : IDisposable
   {
      bool AreControlsEnabled { get; }
      void EnableControls();
      void DisableControls();
   }
   internal interface ISage50ConnectionUIStateTracker : IDisposable
   {
      bool IsConnected { get; }
      void SetUIToConnected();
      void SetUIToDisconnected();
   }
}
