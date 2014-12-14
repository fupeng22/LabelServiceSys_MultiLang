using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Util
{
    public class MD5Util
    {

        public static string EncodingString(string str)
        {
            byte[] result = Encoding.Default.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            str = BitConverter.ToString(output).Replace("-", "");
            return str;
        }
    }
}
