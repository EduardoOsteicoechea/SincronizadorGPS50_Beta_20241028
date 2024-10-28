using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class ConfirmationDialog<T>
   {
      public DialogResult Result { get; set; } = DialogResult.Cancel;
      public ConfirmationDialog(string entityTypeName, IList<T> existingEntityList, IList<T> unexistingEntityList) 
      {
         string dialogMessage = "";
         if(existingEntityList.Count > 0 && unexistingEntityList.Count > 0)
         {
            dialogMessage = $"Partiendo de la selección encontramos {existingEntityList.Count} {entityTypeName} desactualizados y {unexistingEntityList.Count} inexistentes en Sage50.\n\n¿Desea vincular los clientes existentes y crear los faltantes en Sage50?";
         }
         else if(existingEntityList.Count > 0 && unexistingEntityList.Count == 0)
         {
            dialogMessage = $"Partiendo de la selección encontramos {existingEntityList.Count} {entityTypeName} que ya existen en Sage50.\n\n¿Desea vincularlo(s)?";
         }
         else if(existingEntityList.Count == 0 && unexistingEntityList.Count > 0)
         {
            dialogMessage = $"Partiendo de la selección encontramos {unexistingEntityList.Count} {entityTypeName} inexistentes en Sage50.\n\n¿Desea crearlos y sincronizar sus datos?";
         };
            
         Result = MessageBox.Show(dialogMessage, "Confirmación de actualización y creación", MessageBoxButtons.OKCancel);      
      }
   }
}
