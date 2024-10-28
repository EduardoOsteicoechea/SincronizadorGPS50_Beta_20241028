using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class VisualizationForm
   {
      public VisualizationForm(string title, string content)
      {
         var form = new System.Windows.Forms.Form();
         form.Text = title;
         form.Height = 600;
         form.Width = 600;
         form.Font = new System.Drawing.Font("Arial", 11);

         RichTextBox richTextBox = new RichTextBox();
         richTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
         richTextBox.Dock = DockStyle.Fill;
         richTextBox.Text = content;

         form.Controls.Add(richTextBox);
         form.ShowDialog();
      }
   }
}
