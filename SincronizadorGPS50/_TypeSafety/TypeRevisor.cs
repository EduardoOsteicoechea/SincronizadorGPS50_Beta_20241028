using Infragistics.Designers.SqlEditor;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
	internal static class TypeRevisor<T>
	{
		public static void Check(Type columnType, string columnName, T entity, SqlDataReader reader, int i, PropertyInfo[] properties)
		{
			try
			{
				//MessageBox.Show(
				//   "columnName: " + columnName + "\n\n" +
				//   columnType + "\n\n" +
				//   "Is System.Decimal: " + (columnType == typeof(System.Decimal))  + "\n\n" + 
				//   reader.GetValue(i) + ""
				//);

				if(columnType == typeof(System.Decimal))
				{
					var scrutinizedValue = TypeProtector<System.Decimal>.Scrutinize(reader, i, 0);
					typeof(T).GetProperty(columnName).SetValue(entity, scrutinizedValue);
				}
				else if(columnType == typeof(int))
				{
					var scrutinizedValue = TypeProtector<int>.Scrutinize(reader, i, 0);
					typeof(T).GetProperty(columnName).SetValue(entity, scrutinizedValue);
				}
				else if(columnType == typeof(string))
				{
					var scrutinizedValue = TypeProtector<string>.Scrutinize(reader, i, string.Empty);
					//if(columnName == "NOMBRE_COMPLETO")
					//	MessageBox.Show("scrutinizedValue.Trim(): " + scrutinizedValue.Trim());
					typeof(T).GetProperty(columnName).SetValue(entity, scrutinizedValue.Trim());
					
					//if(columnName == "NOMBRE_COMPLETO")
						//MessageBox.Show("typeof(T).GetProperty(columnName).Name: " + typeof(T).GetProperty(columnName).Name + "");
				}
				else if(columnType == typeof(DateTime))
				{
					var scrutinizedValue = TypeProtector<DateTime>.Scrutinize(reader, i, DateTime.Now);
					typeof(T).GetProperty(columnName).SetValue(entity, scrutinizedValue);
				}
				else
				{
					throw new Exception($"Unallowed type \"{reader.GetValue(i).GetType().Name}\" on \"{typeof(T).Name}\" on property \"{properties[i].Name}\", please check the data schema you're using.");
				};
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
