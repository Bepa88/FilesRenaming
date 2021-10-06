using System;
using System.Collections.Generic;
using System.IO;

namespace FilesRenaming
{
    class Program
    {
        static int counter = 1;
        static List<string> filesInPathNew = new List<string>();
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string sourcePath = args[1];
                string targetPath = args.Length > 2 ? args[2] : Path.Combine(sourcePath, "_Output_");
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
                        if (args.Length > 3)
                        {
                            NameMatchChecking(sourcePath, targetPath, file, args[0], args[3]);
                        }
                        else
                        {
                            NameMatchChecking(sourcePath, targetPath, file, args[0]);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("format, source path, target path, desired date");
                Console.ReadKey();
            }
        }

        static void NameMatchChecking(string path, string pathNew, string file, string format, string desiredDate = "")
        {
            FileInfo fileInf = new FileInfo(file);

            if (filesInPathNew.Count == 0)
            {
                File.Copy(Path.Combine(path, fileInf.Name), Path.Combine(pathNew, desiredDate == "" ? File.GetLastWriteTime(file).ToString(format) : desiredDate + System.IO.Path.GetExtension(file)));
                filesInPathNew.Add(desiredDate == "" ? File.GetLastWriteTime(file).ToString(format) : desiredDate + System.IO.Path.GetExtension(file));
            }
            else
            {
                if (filesInPathNew.Contains(desiredDate == "" ? File.GetLastWriteTime(file).ToString(format) : desiredDate + System.IO.Path.GetExtension(file)))
                {
                    if (filesInPathNew.Contains(desiredDate == "" ? File.GetLastWriteTime(file).ToString(format) : desiredDate + "_" + counter + System.IO.Path.GetExtension(file)))
                    {
                        counter++;
                        NameMatchChecking(path, pathNew, file, format);
                    }
                    else
                    {
                        File.Copy(Path.Combine(path, fileInf.Name), Path.Combine(pathNew, desiredDate == "" ? File.GetLastWriteTime(file).ToString(format) : desiredDate + "_" + counter + System.IO.Path.GetExtension(file)));
                        filesInPathNew.Add(desiredDate == "" ? File.GetLastWriteTime(file).ToString(format) : desiredDate + "_" + counter + System.IO.Path.GetExtension(file));
                    }
                }
                else
                {
                    File.Copy(Path.Combine(path, fileInf.Name), Path.Combine(pathNew, desiredDate == "" ? File.GetLastWriteTime(file).ToString(format) : desiredDate + System.IO.Path.GetExtension(file)));
                    filesInPathNew.Add(desiredDate == "" ? File.GetLastWriteTime(file).ToString(format) : desiredDate + System.IO.Path.GetExtension(file));
                }
            }

        }
    }
}
