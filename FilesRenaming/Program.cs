using System;
using System.Collections.Generic;
using System.IO;

namespace FilesRenaming
{
    class Program
    {
        static string sourceFolder = null;
        static string destinationFolder = null;
        static string format = null;
        static string desiredDate = null;
        static int counter = 1;
        static List<string> filesInPathNew = new List<string>();
        static void Main(string[] args)
        {
            

            if (args.Length > 0)
            {
                var os = new Mono.Options.OptionSet()
                         {
                             { "s=", "source Source folder", x => sourceFolder = x },
                             { "d=", "destination(optional) Destination folder(< source >/ _Output_ by defaul)",x=> destinationFolder=x},
                             { "f=", "format(optional) Format of the output file name(yyyy - MM - dd_HH - mm by default)",x=>format= x},
                             {"dt=", "override-datetime(optional) DateTime for replacement",x=>desiredDate=x}
                         };
                try
                {
                    os.Parse(args);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Arguments could not be parsed");
                }

                //if (sourceFolder == null)
                //    throw new InvalidOperationException("Missing required option -s=sourcePath");

                //Console.WriteLine(sourceFolder + destinationFolder + format + desiredDate);
                //Console.ReadKey();

                string sourcePath = sourceFolder;
                string targetPath = destinationFolder != null ? destinationFolder : Path.Combine(sourceFolder, "_Output_");
                string[] files = Directory.GetFiles(sourcePath);
                if (Directory.Exists(targetPath) && files.Length > 0)
                {
                    Console.WriteLine("That path exists already.");
                }
                else
                {
                    Directory.CreateDirectory(targetPath);
                }

                foreach (var file in files)
                {
                    filesInPathNew.Clear();
                    string[] filesInNewPath = Directory.GetFiles(targetPath);
                    foreach (var fileInNewPath in filesInNewPath)
                    {
                        FileInfo fileInNewPathInf = new FileInfo(fileInNewPath);
                        filesInPathNew.Add(fileInNewPathInf.Name);
                    }
                    FileInfo fileInf = new FileInfo(file);
                    if (fileInf.Exists)
                    {
                        if (desiredDate == null)
                        {
                            NameMatchChecking(sourcePath, targetPath, file, format, desiredDate);
                        }
                        else
                        {
                            NameMatchChecking(sourcePath, targetPath, file, format);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(@"-s, -source Source folder
- d, -destination(optional) Destination folder(< source >/ _Output_ by default)
- f, -format(optional) Format of the output file name(yyyy - MM - dd_HH - mm by default)
- dt, -override-datetime(optional) DateTime for replacement");
                Console.ReadKey();
            }
        }

        static void NameMatchChecking(string path, string pathNew, string file, string format, string desiredDate = null)
        {
            FileInfo fileInf = new FileInfo(file);

            if (filesInPathNew.Count == 0)
            {
                File.Copy(Path.Combine(path, fileInf.Name), Path.Combine(pathNew, desiredDate == null ? File.GetLastWriteTime(file).ToString(format) + System.IO.Path.GetExtension(file) : desiredDate + System.IO.Path.GetExtension(file)));
                filesInPathNew.Add(desiredDate == null ? File.GetLastWriteTime(file).ToString(format) + System.IO.Path.GetExtension(file) : desiredDate + System.IO.Path.GetExtension(file));
            }
            else
            {
                if (filesInPathNew.Contains(desiredDate == null ? File.GetLastWriteTime(file).ToString(format) + System.IO.Path.GetExtension(file) : desiredDate + System.IO.Path.GetExtension(file)))
                {
                    if (filesInPathNew.Contains(desiredDate == null ? File.GetLastWriteTime(file).ToString(format) + System.IO.Path.GetExtension(file) : desiredDate + "_" + counter + System.IO.Path.GetExtension(file)))
                    {
                        counter++;
                        NameMatchChecking(path, pathNew, file, format);
                    }
                    else
                    {
                        File.Copy(Path.Combine(path, fileInf.Name), Path.Combine(pathNew, desiredDate == null ? File.GetLastWriteTime(file).ToString(format) + System.IO.Path.GetExtension(file) : desiredDate + "_" + counter + System.IO.Path.GetExtension(file)));
                        filesInPathNew.Add(desiredDate == null ? File.GetLastWriteTime(file).ToString(format) + System.IO.Path.GetExtension(file) : desiredDate + "_" + counter + System.IO.Path.GetExtension(file));
                    }
                }
                else
                {
                    File.Copy(Path.Combine(path, fileInf.Name), Path.Combine(pathNew, desiredDate == null ? File.GetLastWriteTime(file).ToString(format) + System.IO.Path.GetExtension(file) : desiredDate + System.IO.Path.GetExtension(file)));
                    filesInPathNew.Add(desiredDate == null ? File.GetLastWriteTime(file).ToString(format) + System.IO.Path.GetExtension(file) : desiredDate + System.IO.Path.GetExtension(file));
                }
            }

        }
    }
}
