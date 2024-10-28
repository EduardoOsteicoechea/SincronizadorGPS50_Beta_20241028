using System;
using System.Runtime.Serialization;

namespace Dinq.Gestproject {
   public class MySerializationBinder : System.Runtime.Serialization.SerializationBinder {

      public static SerializationBinder MyCustomBinder = new MySerializationBinder();

      public override Type BindToType(string assemblyName, string typeName) {
         if(assemblyName.StartsWith("Micad.Gestproject.202"))
            return Type.GetType(typeName);
         else
            throw new ArgumentException("Unexpected type", nameof(typeName));
      }
   }
}