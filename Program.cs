using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            string targetDirectory = string.Empty;

            Console.WriteLine("This application sort files into folders, named after the files creation date.");
            Console.Write(@"Please enter the path to folder that contains the files that should be sorted(eg. C:\Users\MyUsername\Downloads): ");

            while (true)
            {
                targetDirectory = Console.ReadLine();
                if (Directory.Exists(targetDirectory)) break;
                Console.WriteLine("Folder path invalid, please try again.");
            }
            Console.WriteLine("Target directory set to: " + targetDirectory);

            Console.Write("What file extentions do you want to sort into the folders? Please enter the file extentions separated by [space] and confirm with [enter] (eg. 'jpg png jpeg'): ");
            string extentions = Console.ReadLine();
            List<string> fileExtentions = extentions.Split(' ').ToList();
            Console.WriteLine("File extentions saved is: ");
            fileExtentions.ForEach(fe => Console.Write(" " + fe));

            var files = Directory.EnumerateFiles(targetDirectory);

            List<FileInfo> fileInfoList = new();

            foreach (var file in files)
            {
                Console.WriteLine(file);
                FileInfo fi = new(file);
                fileInfoList.Add(fi);
            }

            fileInfoList = fileInfoList.OrderBy(fi => fi.CreationTime).ToList();

            fileInfoList.ForEach(fi => Console.WriteLine(fi.FullName + " " + fi.CreationTime));

            string firstYear = fileInfoList[0].CreationTime.ToString().Substring(0, 4);
            string firstMonth = fileInfoList[0].CreationTime.ToString().Substring(5, 2);
            Console.WriteLine(firstYear);
            Console.WriteLine(firstMonth);


            foreach (FileInfo fi in fileInfoList)
            {
                string fileExtention = fi.FullName[(fi.FullName.LastIndexOf('.') + 1)..];
                if (!fileExtentions.Contains(fileExtention)) continue;
                string targetFolderName = fi.CreationTime.ToString().Substring(0, 7);
                string targetFolderPath = Path.Combine(targetDirectory, targetFolderName);
                if (!Directory.Exists(targetFolderPath))
                {
                    Directory.CreateDirectory(targetFolderPath);
                }
                Console.WriteLine("Moving file: " + fi.FullName);
                File.Move(fi.FullName, Path.Combine(targetFolderPath, fi.Name));
            }

            Console.WriteLine("All files moved. Closing application");
            Console.ReadLine();
        }
    }
}
