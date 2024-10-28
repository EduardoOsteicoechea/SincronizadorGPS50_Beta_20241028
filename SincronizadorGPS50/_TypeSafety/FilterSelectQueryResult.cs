using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public static class FilterSelectQueryResult
   {
      public static dynamic Filter(Type valueType, dynamic value, string columnName, dynamic defaultValue = default)
      {
         try
         {
            //MessageBox.Show($"The value was: {value}. It's type was: " + value.GetType());
            //MessageBox.Show($"The valueType was: " + valueType);
            if(valueType == typeof(string))
            {
               //MessageBox.Show("The value was a string: " + value.GetType());
               return Convert.ToString(value.GetType().Name == typeof(DBNull).Name ? defaultValue : value);
            }
            else if(valueType == typeof(int))
            {
               //MessageBox.Show("The value was an integer: " + value.GetType() + ". defaultValue: " + defaultValue + ". value: " + value);
               return Convert.ToInt32(value.GetType().Name == typeof(DBNull).Name ? defaultValue : value);
            }
            else if(valueType == typeof(decimal))
            {
               //MessageBox.Show("The value was a decimal: " + value.GetType());
               return Convert.ToDecimal(value.GetType().Name == typeof(DBNull).Name ? defaultValue : value);
            }
            else if(valueType == typeof(DateTime))
            {
               //MessageBox.Show("The value was a DateTime: " + value.GetType());
               return Convert.ToDateTime(value.GetType().Name == typeof(DBNull).Name ? defaultValue : value);
            }

            throw new ArgumentException("You supplied an invalid type at: " + columnName + ". It's value was: " + value);
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
      }

   }
}
