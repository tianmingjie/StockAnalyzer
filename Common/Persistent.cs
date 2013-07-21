using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common
{
    public class Persistent
    {

        public static bool WriteFile(string filePath, string input)
        {
            using (StreamWriter outfile = new StreamWriter(filePath))
            {
                outfile.Write(input);
            }
            return true;
        }
    }
}
