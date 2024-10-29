using Microsoft.VisualBasic;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SincronizadorGPS50
{
    public static class Encryptor
    {
        private static string baseString = "{YAs8@3I>Z}0R*JQ|p/bvh?(OmXB<+C,2aL1[cVj=P^Gou]nt47#eTS)69FKkw%-:r~ld.yD;MHU&z5`$gW\"qE\\f!Nx'_i ";
        private static string finalString = "\"f;eWvxjn$u<Mo(3r|d4>)&*PT 68sCp%t9_w1OF/:Z!5E-HU7,=c+zy^AD#qbY{`.I[\\LGgRN0?V'mBXhi2lJK}kQ@a]S~";

        private static byte[] tripledesKey = new byte[] { 154, 47, 111, 125, 59, 230, 135, 84, 167, 161, 126, 95, 200, 26, 197, 220, 8, 46, 108, 227, 136, 106, 162, 238 };
        private static byte[] tripledesIV = new byte[] { 146, 54, 171, 16, 24, 115, 156, 218 };

        public static string Encrypt(string sourceText)
        {
            string resultText = "";
            for(int i = 0; i < sourceText.Length; i++)
            {
                resultText += EncriptCharacter(sourceText.Substring(i, 1), sourceText.Length, i);
            }
            return resultText;
        }

        private static string EncriptCharacter(string character, int length, int index)
        {
            if(baseString.IndexOf(character) != -1)
            {
                return finalString.Substring((baseString.IndexOf(character) + length + index) % baseString.Length, 1);
            }
            else
            {
                return character;
            }
        }

        public static string UnEncrypt(string sourceText)
        {
            string resultText = "";
            for(int i = 0; i < sourceText.Length; i++)
            {
                resultText += UnEncriptCharacter(sourceText.Substring(i, 1), sourceText.Length, i);
            }
            return resultText;
        }

        private static string UnEncriptCharacter(string character, int length, int index)
        {
            int auxIndex = 0;
            if(finalString.IndexOf(character) != -1)
            {
                if(finalString.IndexOf(character) - length - index > 0)
                    auxIndex = (finalString.IndexOf(character) - length - index) % finalString.Length;
                else
                    auxIndex = baseString.Length + ((finalString.IndexOf(character) - length - index) % finalString.Length);
                auxIndex = auxIndex % finalString.Length;
                return baseString.Substring(auxIndex, 1);
            }
            else
            {
                return character;
            }
        }

        public static string GeneraCadena(bool inverse)
        {
            string cadena = "", resultString = "";
            for(int i = 32; i <= 126; i++)
            {
                cadena += Strings.Chr(i);
            }
            if(inverse)
            {
                string aux = "";
                for(int i = cadena.Length - 1; i >= 0; i--)
                {
                    aux += cadena[i];
                }
                cadena = aux;
            }

            Random indexGen = new Random();
            int nextIndex = 0;

            while(cadena.Length > 0)
            {
                nextIndex = indexGen.Next(0, cadena.Length - 1);
                resultString += cadena.Substring(nextIndex, 1);
                cadena = cadena.Remove(nextIndex, 1);
            }
            return resultString;
        }

        private static TripleDES tDes = null;
        private static TripleDES TDes
        {
            get
            {
                if(tDes == null)
                {
                    tDes = new TripleDESCryptoServiceProvider();
                    tDes.Key = tripledesKey;
                    tDes.IV = tripledesIV;
                }
                return tDes;
            }
        }

        public static byte[] TriDESEncript(string source)
        {
            ICryptoTransform ct = TDes.CreateEncryptor();
            byte[] input = Encoding.Default.GetBytes(source);
            byte[] output = ct.TransformFinalBlock(input, 0, input.Length);
            return output; // Encoding.Default.GetString(output);
        }

        public static string TriDESDecrypt(byte[] source)
        {

            byte[] input = source; // Encoding.Default.GetBytes(source);
            ICryptoTransform ct = TDes.CreateDecryptor();
            byte[] output = ct.TransformFinalBlock(input, 0, input.Length);
            return Encoding.Default.GetString(output);
        }

        public static byte[] AutoGenerateKey()
        {
            TripleDES tripleDes = new TripleDESCryptoServiceProvider();
            tripleDes.GenerateKey();
            return tripleDes.Key;
        }

        public static byte[] AutoGenerateIV()
        {
            TripleDES tripleDes = new TripleDESCryptoServiceProvider();
            tripleDes.GenerateIV();
            return tripleDes.IV;
        }

        public static string AutoGenerateKeyAsString()
        {
            byte[] output = AutoGenerateKey();
            string[] format = new string[output.Length];
            for(int i = 0; i < output.Length; i++)
                format[i] = output[i].ToString();
            return "{" + string.Join(",", format) + "}";
        }

        public static string AutoGenerateIVAsString()
        {
            byte[] output = Encryptor.AutoGenerateIV();
            string[] format = new string[output.Length];
            for(int i = 0; i < output.Length; i++)
                format[i] = output[i].ToString();
            return "{" + string.Join(",", format) + "}";
        }
    }
}
