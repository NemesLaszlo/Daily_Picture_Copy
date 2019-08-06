using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace DailyPictureCopy
{
    public class Program
    {
        private static Dictionary<string, string> directoryInfo_data = new Dictionary<string, string>();

        /// <summary>
        /// Adatok inicializálása az app.configbol.
        /// </summary>
        private static  void Init()
        {
            var datas = ConfigurationManager.GetSection("DirectoryInfo") as NameValueCollection;
            foreach (var data in datas.AllKeys)
            {
                directoryInfo_data.Add(data, datas[data]);
                Trace.TraceInformation((DateTime.Now.ToString() +" "+  data + " -> " + datas[data]));
            }
            Trace.TraceInformation((DateTime.Now.ToString() + " Config init Fine!"));
            Trace.Flush();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("----------PROCESS START----------");
            Trace.TraceInformation((DateTime.Now.ToString() + " ----------PROCESS START----------"));
            Trace.Flush();
            Init();
            CopyPicture cP = new CopyPicture();
            cP.PictureCopy(directoryInfo_data["sourcePath"], directoryInfo_data["targetPath"], directoryInfo_data["CopiedFileName"]);
            Console.ReadKey();
        }
    }
}
