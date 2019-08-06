using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DailyPictureCopy
{
    public class CopyPicture
    {
        private readonly string date; // Datum ami alapjan kivalsztjuk az adott filet.

        public CopyPicture()
        {
            try
            {
                string resultDate = string.Empty;
                string bufferDate = DateTime.Now.ToString("MM") + $".{ DateTime.Today.Day - 1}";
                string[] str = bufferDate.Trim().Split('.');
                if (str[1].Contains("-") || str[1].Length == 1)
                {
                    if (int.Parse(str[1]) <= 0)
                    {

                        DateTime today = DateTime.Today;
                        DateTime month = new DateTime(today.Year, today.Month, 1);
                        string lastMonth = month.AddDays(-1).Month.ToString();
                        string lastDay = month.AddDays(-1).Day.ToString();
                        if (lastMonth.Length == 1)
                        {
                            resultDate += "0" + lastMonth + "." + lastDay;
                            this.date = resultDate;
                        }
                        else
                        {
                            resultDate += lastMonth + "." + lastDay;
                            this.date = resultDate;
                        }

                    }
                    else
                    {
                        resultDate += str[0] + ".0" + str[1];
                        this.date = resultDate;
                    }
                }
                else
                {
                    this.date = bufferDate;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Picture Name Date Problem!");
                Trace.TraceInformation((DateTime.Now.ToString() + " Picture Name Date Problem!"));
                Trace.Flush();
            }
        }

        /// <summary>
        /// Az adott futtatas elotti nap kepe bekerul a megadott mappaba.
        /// </summary>
        /// <param name="sourcePath">Adott mappa,fileok elerese ahonnan a kivalasztas tortenik.</param>
        /// <param name="targetPath">Adott mappa ahova kerulnie kell a megfelelo kepnek.</param>
        /// <param name="newFileName">Masolt kep uj neve.</param>
        public void PictureCopy(string sourcePath, string targetPath,string newFileName)
        {
            try
            {
                // Ellenorzes, hogy letezik-e a cel mappa
                if (Directory.Exists(targetPath))
                {
                    Console.WriteLine("targetPath is exist!");
                    Trace.TraceInformation((DateTime.Now.ToString() + " targetPath is exist!"));
                    Trace.Flush();
                }
                else
                {
                    Console.WriteLine("targetPath path does not exist!");
                    Trace.TraceInformation((DateTime.Now.ToString() + " targetPath path does not exist!"));
                    Trace.Flush();
                    return;
                }

                // Ellenorzes, hogy letezik-e a forras mappaja
                if (Directory.Exists(sourcePath))
                {
                    Console.WriteLine("sourcePath is exist!");
                    Trace.TraceInformation((DateTime.Now.ToString() + " sourcePath is exist!"));
                    Trace.Flush();
                    // Egy tomb a filenevekkel, amelyek a forrasbol szarmazo .jpg vagy .png kiterjesztesu fileok, almappak atnezesevel egyutt.
                    string[] files = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".png") || s.EndsWith(".jpg")).Select(Path.GetFileName).ToArray();
                    foreach (var picture in files)
                    {
                        string[] str = picture.Trim().Split('.');
                        if (date.Equals(str[0] + "." + str[1]))
                        {
                            DeleteFileFromDirectory(targetPath);
                            try
                            {
                                File.Copy(Path.Combine(sourcePath + "\\" + str[0], str[0] + "." + str[1] + "." + str[2]), Path.Combine(targetPath, Path.GetFileName(newFileName + "." + str[2])), true);
                                Console.WriteLine("Picture Copy Done!");
                                Trace.TraceInformation((DateTime.Now.ToString() + " Picture Copy Done!"));
                                Trace.Flush();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Folder Structure Problem!");
                                Trace.TraceInformation((DateTime.Now.ToString() + " Folder Structure Problem!"));
                                //Trace.TraceInformation((DateTime.Now.ToString() + ex.StackTrace));
                                Trace.Flush();
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("sourcePath does not exist!");
                    Trace.TraceInformation((DateTime.Now.ToString() + " sourcePath does not exist!"));
                    Trace.Flush();
                    return;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong! Check the logfile!");
                //Trace.TraceInformation((DateTime.Now.ToString() + " " + e.StackTrace));
                Trace.Flush();
            }
            Console.WriteLine("----------PROCESS FINISH---------");
            Trace.TraceInformation((DateTime.Now.ToString() + " ----------PROCESS FINISH---------"));
            Trace.Flush();
        }

        /// <summary>
        /// A megadott mappa teljes tartalmat torli.
        /// </summary>
        /// <param name="targetDir">Adott mappa elerese amibol mindent torolni kell.</param>
        private void DeleteFileFromDirectory(string targetPath)
        {
            DirectoryInfo di = new DirectoryInfo(targetPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
