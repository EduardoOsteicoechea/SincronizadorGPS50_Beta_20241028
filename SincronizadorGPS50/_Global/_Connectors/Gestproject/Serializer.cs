using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Dinq.Gestproject
{
   public class Serializer
   {
      public static bool SerializeObject(Object objectInstance, String filePath)
      {
         FileStream fileStream = null;
         BinaryFormatter binaryFormater;
         bool returnValue;

         try
         {
            fileStream = File.Create(filePath);
            binaryFormater = new BinaryFormatter();
            binaryFormater.Serialize(fileStream, objectInstance);
            fileStream.Close();
            returnValue = true;
         }
         catch
         {
            returnValue = false;
         }
         finally
         {
            binaryFormater = null;
         }
         return returnValue;
      }
      public static Object DeserializeObject(String filePath)
      {
         FileStream fileStream = null;
         BinaryFormatter binaryFormatter = null;
         Object tempObject = null;

         try
         {
            if(File.Exists(filePath))
            {
               fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
               binaryFormatter = new BinaryFormatter();
               binaryFormatter.Binder = MySerializationBinder.MyCustomBinder;
               tempObject = binaryFormatter.Deserialize(fileStream);
               fileStream.Close();
            }
         }
         catch
         {
            tempObject = null;
         }
         finally
         {
            binaryFormatter = null;
         }
         return tempObject;
      }

   }
}
