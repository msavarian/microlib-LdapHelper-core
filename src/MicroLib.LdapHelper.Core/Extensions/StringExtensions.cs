using System;
using System.IO;
using System.Security;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace MicroLib.LdapHelper.Core.Extensions
{
    public static class StringExtensions
    {
        public static sbyte[] ToSbyteArray(this string value)
        {
            // convert string to byte array
            byte[] bytes = Encoding.ASCII.GetBytes(value);

            // convert byte array to sbyte array
            sbyte[] sbytes = new sbyte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                sbytes[i] = (sbyte)bytes[i];

            return sbytes;
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();

            if (str == null)
                return null;

            foreach (var c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ')
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static SecureString ToSecureString(this string unsecureString)
        {
            var secureString = new SecureString();

            if (unsecureString == null)
                return null;

            foreach (var c in unsecureString)
            {
                secureString.AppendChar(c);
            }

            secureString.MakeReadOnly();

            return secureString;
        }

        public static T DeserializeFromXmlString<T>(this string xmlString)
        {
            return (T)DeserializeFromXmlString(xmlString, typeof(T));
        }

        private static object DeserializeFromXmlString(this string xmlString, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(xmlString))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        public static T DeserializeFromJsonString<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
