using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common
{
    public class FileUtil
    {

        public static bool WriteFile(string filePath, string input)
        {
            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.Write(input);
            }
            return true;
        }

        //public static DateTime ReadFile(string filePath)
        //{
        //    using (StreamReader outfile = new StreamReader(filePath))
        //    {
        //        return DateTime.Parse(outfile.ReadToEnd());
        //    }
        //}

        public static string ReadFile(string filePath)
        {
            using (StreamReader outfile = new StreamReader(filePath))
            {
                return outfile.ReadToEnd();
            }
        }
    }
}
