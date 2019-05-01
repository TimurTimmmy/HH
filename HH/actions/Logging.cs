using System;
using System.IO;
using System.Text;

namespace HH.actions
{
    class Logging
    {
        private static string Directory = AppDomain.CurrentDomain.BaseDirectory;
        private static string CurDate = DateTime.Today.ToShortDateString().Replace(".", "");
        private static string FileName = @"hh.log";

        public static void WriteLog(string text)
        {
            string FileNameOld = String.Format(@"hh{0}.log", CurDate);
            FileInfo fileInf = new FileInfo(Directory + FileName);

            if (!fileInf.Exists)
            {
                WriteFile(Directory + FileName, text);
            }
            else
            if (fileInf.Exists && fileInf.Length > 512000)
            {
                fileInf.MoveTo(Directory + FileNameOld);
                WriteFile(Directory + FileName, text);
            }
            else
            {
                WriteFile(Directory + FileName, text);                
            }            
        }

        private static void WriteFile(string directory, string text)
        {
            using (FileStream fstream = new FileStream(directory, FileMode.Append))
            {
                byte[] array = Encoding.Default.GetBytes(DateTime.Now.ToString() + "\n"+ text + "\n");
                fstream.Write(array, 0, array.Length);
            }
        }
    }
}
