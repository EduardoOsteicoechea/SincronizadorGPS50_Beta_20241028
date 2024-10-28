using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class AksIfDuplicationOrUpdateIsDesired
   {
      public bool IsDuplicationDesired { get; set; } = false;
      public bool WasOperationCanceled { get; set; } = false;
      public System.Windows.Forms.Form CurrentForm { get; set; } = null;
      public TableLayoutPanel TableLayoutPanel { get; set; } = null;
      public AksIfDuplicationOrUpdateIsDesired(string tipoDeEntidad, string clientFullName, string matchMessage) 
      {
         CurrentForm = new System.Windows.Forms.Form();
         CurrentForm.Text = $"Confirmación de acción sobre {tipoDeEntidad}";
         CurrentForm.Icon = Resources.appicon;
         CurrentForm.Width = 400;
         CurrentForm.Height = 150;

         UltraLabel label1 = new UltraLabel();
         label1.Text = matchMessage;
         label1.WrapText = true;
         label1.Location = new System.Drawing.Point(15, 10);
         label1.Height = 25;
         label1.Width = 350;

         UltraLabel label2 = new UltraLabel();
         label2.Text = $"¿Desea duplicar o sincronizar o pasar por alto este {tipoDeEntidad}?";
         label2.Location = new System.Drawing.Point(15, 40);
         label2.AutoSize = true;

         UltraButton Syncronizebutton = new UltraButton();
         Syncronizebutton.Text = "Sincronizar";
         Syncronizebutton.Click += Syncronizebutton_Click;
         Syncronizebutton.Location = new System.Drawing.Point(15, 70);
         Syncronizebutton.Width = 110;

         UltraButton Duplicatebutton = new UltraButton();
         Duplicatebutton.Text = "Duplicar";
         Duplicatebutton.Click += Duplicatebutton_Click;
         Duplicatebutton.Location = new System.Drawing.Point(135, 70);
         Duplicatebutton.Width = 110;

         UltraButton Cancelbutton = new UltraButton();
         Cancelbutton.Text = "Cancelar";
         Cancelbutton.Click += Cancelbutton_Click;
         Cancelbutton.Location = new System.Drawing.Point(255, 70);
         Cancelbutton.Width = 110;

         CurrentForm.Controls.Add(label1);
         CurrentForm.Controls.Add(label2);
         CurrentForm.Controls.Add(Syncronizebutton);
         CurrentForm.Controls.Add(Duplicatebutton);
         CurrentForm.Controls.Add(Cancelbutton);

         CurrentForm.ShowDialog();
      }

      private void Cancelbutton_Click(object sender, EventArgs e)
      {
         WasOperationCanceled = true;
         CurrentForm.Close();
      }
      private void Syncronizebutton_Click(object sender, EventArgs e)
      {
         IsDuplicationDesired = false;
         CurrentForm.Close();
      }
      private void Duplicatebutton_Click(object sender, EventArgs e)
      {
         IsDuplicationDesired = true;
         CurrentForm.Close();
      }
   }
}
