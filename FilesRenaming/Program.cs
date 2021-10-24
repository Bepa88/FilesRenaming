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
                             { "f=", "format(optional) Format of the output file name(yyyy-MM-dd_HH-mm by default)",x=>format= x},
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
                        NameMatchChecking(sourcePath, targetPath, file, format, desiredDate);
                    }
                }
            }
            else
            {
                Console.WriteLine(@"-s, -source Source folder
- d, -destination(optional) Destination folder(< source >/ _Output_ by default)
- f, -format(optional) Format of the output file name(yyyy-MM-dd_HH-mm by default)
- dt, -override-datetime(optional) DateTime for replacement");
                Console.ReadKey();
            }
        }

        static void NameMatchChecking(string path, string pathNew, string file, string format, string desiredDate = null)
        {
            if(format == null)
            {
                format = "yyyy-MM-dd_HH-mm";
            }

            FileInfo fileInf = new FileInfo(file);

            if (filesInPathNew.Count == 0)
            {
                File.Copy(Path.Combine(path, fileInf.Name), Path.Combine(pathNew, desiredDate == null ? File.GetCreationTime(file).ToString(format) + System.IO.Path.GetExtension(file).ToLower().ToLower().ToLower() : desiredDate + System.IO.Path.GetExtension(file).ToLower().ToLower()));
                filesInPathNew.Add(desiredDate == null ? File.GetCreationTime(file).ToString(format) + System.IO.Path.GetExtension(file).ToLower().ToLower() : desiredDate + System.IO.Path.GetExtension(file).ToLower().ToLower());
            }
            else
            {
                if (filesInPathNew.Contains(desiredDate == null ? File.GetCreationTime(file).ToString(format) + System.IO.Path.GetExtension(file).ToLower().ToLower() : desiredDate + System.IO.Path.GetExtension(file).ToLower().ToLower()))
                {
                    if (filesInPathNew.Contains(desiredDate == null ? File.GetCreationTime(file).ToString(format) + "_" + counter + System.IO.Path.GetExtension(file).ToLower().ToLower() : desiredDate + "_" + counter + System.IO.Path.GetExtension(file).ToLower()))
                    {
                        counter++;
                        NameMatchChecking(path, pathNew, file, format);
                    }
                    else
                    {
                        File.Copy(Path.Combine(path, fileInf.Name), Path.Combine(pathNew, desiredDate == null ? File.GetCreationTime(file).ToString(format) + "_" + counter + System.IO.Path.GetExtension(file).ToLower() : desiredDate + "_" + counter + System.IO.Path.GetExtension(file).ToLower()));
                        filesInPathNew.Add(desiredDate == null ? File.GetCreationTime(file).ToString(format) + "_" + counter + System.IO.Path.GetExtension(file).ToLower() : desiredDate + "_" + counter + System.IO.Path.GetExtension(file).ToLower());
                    }
                }
                else
                {
                    File.Copy(Path.Combine(path, fileInf.Name), Path.Combine(pathNew, desiredDate == null ? File.GetCreationTime(file).ToString(format) + System.IO.Path.GetExtension(file).ToLower() : desiredDate + System.IO.Path.GetExtension(file).ToLower()));
                    filesInPathNew.Add(desiredDate == null ? File.GetCreationTime(file).ToString(format) + System.IO.Path.GetExtension(file).ToLower() : desiredDate + System.IO.Path.GetExtension(file).ToLower());
                }
            }

        }
    }
}
