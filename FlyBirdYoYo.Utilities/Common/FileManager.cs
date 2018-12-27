using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;


namespace FlyBirdYoYo.Utilities
{
    /// <summary>
    /// Exposes methods to manage files.
    /// </summary>
    public class FileManager
    {
        #region Properties

        private IDictionary<string, string> _contentTypes;

        protected IDictionary<string, string> ContentTypes
        {
            get
            {
                if (_contentTypes == null)
                {
                    _contentTypes = new Dictionary<string, string>();
                   // _contentTypes.Add("resx", "text/plain");---当从新安装的时候 需要同步文件  取消这行注释 即可进行同步覆盖resx文件
                    _contentTypes.Add("txt", "text/plain");
                    _contentTypes.Add("htm", "text/html");
                    _contentTypes.Add("html", "text/html");
                    _contentTypes.Add("rtf", "text/richtext");
                    _contentTypes.Add("jpg", "image/jpeg");
                    _contentTypes.Add("jpeg", "image/jpeg");
                    _contentTypes.Add("gif", "image/gif");
                    _contentTypes.Add("bmp", "image/bmp");
                    _contentTypes.Add("png", "image/png");
                    _contentTypes.Add("ico", "image/x-icon");
                    _contentTypes.Add("mp3", "audio/mpeg");
                    _contentTypes.Add("wma", "audio/x-ms-wma");
                    _contentTypes.Add("mpg", "video/mpeg");
                    _contentTypes.Add("mpeg", "video/mpeg");
                    _contentTypes.Add("avi", "video/avi");
                    _contentTypes.Add("mp4", "video/mp4");
                    _contentTypes.Add("wmv", "video/x-ms-wmv");
                    _contentTypes.Add("pdf", "application/pdf");
                    _contentTypes.Add("doc", "application/msword");
                    _contentTypes.Add("dot", "application/msword");
                    _contentTypes.Add("docx", "application/msword");
                    _contentTypes.Add("dotx", "application/msword");
                    _contentTypes.Add("csv", "text/csv");
                    _contentTypes.Add("xls", "application/x-msexcel");
                    _contentTypes.Add("xlt", "application/x-msexcel");
                    _contentTypes.Add("xlsx", "application/x-msexcel");
                    _contentTypes.Add("xltx", "application/x-msexcel");
                    _contentTypes.Add("ppt", "application/vnd.ms-powerpoint");
                    _contentTypes.Add("pps", "application/vnd.ms-powerpoint");
                    _contentTypes.Add("pptx", "application/vnd.ms-powerpoint");
                    _contentTypes.Add("ppsx", "application/vnd.ms-powerpoint");
                }

                return _contentTypes;
            }
        }
      
        
        #endregion
        #region Constants

        private const int BufferSize = 4096;

        #endregion

        #region Constructor

        internal FileManager()
        {
        }

        #endregion

        #region Public Methods

     
        /// <summary>
        /// Gets the Content Type for the specified file extension.
        /// </summary>
        /// <param name="extension">The file extension.</param>
        /// <returns>The Content Type for the specified extension.</returns>
        public virtual string GetContentType(string extension)
        {
           

            if (string.IsNullOrEmpty(extension)) return "application/octet-stream";

            var key = extension.TrimStart('.').ToLowerInvariant();
            return ContentTypes.ContainsKey(key) ? ContentTypes[key] : "application/octet-stream";
        }



        public static Stream Create(string path)
        {
            return File.Create(path);
        }

        public static void Delete(string path)
        {
            File.Delete(path);
        }

        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static FileAttributes GetAttributes(string path)
        {
            return File.GetAttributes(path);
        }

        public static DateTime GetLastWriteTime(string path)
        {
            return File.GetLastWriteTime(path);
        }

        public static void Move(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }

        public static Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }

        public static byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static void SetAttributes(string path, FileAttributes fileAttributes)
        {
            File.SetAttributes(path, fileAttributes);
        }


        #endregion

        #region 加压解压方法
        /// <summary>
        /// 功能：压缩文件（暂时只压缩文件夹下一级目录中的文件，文件夹及其子级被忽略）
        /// </summary>
        /// <param name="dirPath">被压缩的文件夹夹路径</param>
        /// <param name="zipFilePath">生成压缩文件的路径，为空则默认与被压缩文件夹同一级目录，名称为：文件夹名+.zip</param>
        /// <param name="err">出错信息</param>
        /// <returns>是否压缩成功</returns>
        public bool ZipFile(string dirPath, string zipFilePath, out string err)
        {
            err = "";
            if (dirPath == string.Empty)
            {
                err = "要压缩的文件夹不能为空！";
                return false;
            }
            if (!Directory.Exists(dirPath))
            {
                err = "要压缩的文件夹不存在！";
                return false;
            }
            //压缩文件名为空时使用文件夹名＋.zip
            if (zipFilePath == string.Empty)
            {
                if (dirPath.EndsWith("\\"))
                {
                    dirPath = dirPath.Substring(0, dirPath.Length - 1);
                }
                zipFilePath = dirPath + ".zip";
            }

            try
            {
                string[] filenames = Directory.GetFiles(dirPath);
                using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFilePath)))
                {
                    s.SetLevel(9);
                    byte[] buffer = new byte[4096];
                    foreach (string file in filenames)
                    {
                        ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                        entry.DateTime = DateTime.Now;
                        s.PutNextEntry(entry);
                        using (FileStream fs = File.OpenRead(file))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                    s.Finish();
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 功能：解压zip格式的文件。
        /// </summary>
        /// <param name="zipFilePath">压缩文件路径</param>
        /// <param name="unZipDir">解压文件存放路径,为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹</param>
        /// <param name="err">出错信息</param>
        /// <returns>解压是否成功</returns>
        public bool UnZipFile(string zipFilePath, string unZipDir, out string err)
        {
            err = "";
            if (zipFilePath == string.Empty)
            {
                err = "压缩文件不能为空！";
                return false;
            }
            if (!File.Exists(zipFilePath))
            {
                err = "压缩文件不存在！";
                return false;
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("\\"))
                unZipDir += "\\";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);

            try
            {
                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
                {

                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (directoryName.Length > 0)
                        {
                            Directory.CreateDirectory(unZipDir + directoryName);
                        }
                        if (!directoryName.EndsWith("\\"))
                            directoryName += "\\";
                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter = File.Create(unZipDir + theEntry.Name))
                            {

                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }//while
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                return false;
            }
            return true;
        }//解压结束
        #endregion



    }
}
