using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FlyBirdYoYo.Utilities.Common
{
    public class FileHelper
    {
        public static string GetFileSource(string file)
        {
            if (File.Exists(file))
            {
                StringBuilder htm = new StringBuilder();
                using (StreamReader sr = new StreamReader(file))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        htm.Append(line);
                        htm.Append(Convert.ToChar(13));
                    }
                }
                return htm.ToString();
            }
            else
            {
                return "";
            }
        }

        public static string GetFileSource(string file, string[] format, string start, string end)
        {
            if (File.Exists(file))
            {
                StringBuilder htm = new StringBuilder();
                using (StreamReader sr = new StreamReader(file))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        htm.Append(line);
                        htm.Append(Convert.ToChar(13));
                    }
                    for (int i = 0; i < format.Length; i++)
                    {
                        htm.Replace(start + i + end, format[i]);
                    }
                }
                return htm.ToString();
            }
            else
            {
                return "";
            }
        }

        public static bool SetFileSource(string htm, string ofile)
        {
            try
            {
                if (File.Exists(ofile))
                {
                    File.Delete(ofile);
                }
                using (StreamWriter Html = new StreamWriter(ofile, false, System.Text.Encoding.GetEncoding("UTF-8")))
                {
                    Html.Write(htm);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }


        }
        

        public static bool SetFileSource(string sfile, string ofile, string[] format, string start, string end)
        {

            if (File.Exists(sfile))
            {
                StringBuilder htm = new StringBuilder();
                using (StreamReader sr = new StreamReader(sfile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        htm.Append(line);
                        htm.Append(Convert.ToChar(13));
                    }
                    for (int i = 0; i < format.Length; i++)
                    {
                        htm.Replace(start + i + end, format[i]);
                    }
                }
                if (File.Exists(ofile))
                {
                    File.Delete(ofile);
                }
                using (StreamWriter Html = new StreamWriter(ofile, false, System.Text.Encoding.GetEncoding("UTF-8")))
                {
                    Html.Write(htm.ToString());

                }
                return true;
            }
            else
            {
                return false;
            }
        }



        public static bool SetFile(string Path, string Content)
        {
            try
            {
                using (StreamWriter Html = new StreamWriter(Path, false, System.Text.Encoding.GetEncoding("UTF-8")))
                {
                    Html.Write(Content);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
       
    }
}
