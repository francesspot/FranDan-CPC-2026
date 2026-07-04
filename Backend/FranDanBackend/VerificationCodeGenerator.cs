using System;
using System.Globalization;
namespace FranDanBackend
{
    public static class VerificationCodeGenerator
    {
        private static Random myRandom = new Random((int)(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond));
        private static string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 +/";
        public static string generate()
        {
            string str="";
            for (int i = 0; i < 20; i++)
                str += alphabet[myRandom.Next()%64];
            return str;
        }

    }
}
