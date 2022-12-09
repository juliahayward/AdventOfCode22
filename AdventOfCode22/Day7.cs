using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode22
{
    internal class Day7
    {
        private static void DoDay7()
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(new FileStream("C:\\src\\juliahayward\\AdventOfCode22\\RawData\\7.txt",
                       FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
            }

            var topDirectory = new Directory() { Name = "/" };
            var currentDirectory = topDirectory;
            foreach (var line in lines)
            {
                if (line.StartsWith("$ cd /"))
                    currentDirectory = topDirectory;
                else if (line.StartsWith("$ cd .."))
                    currentDirectory = currentDirectory.Parent;
                else if (line.StartsWith("$ cd "))
                {
                    var childName = line.Replace("$ cd ", "").Trim();
                    currentDirectory = currentDirectory.Children.First(x => x.Name == childName);
                }
                else if (line.StartsWith("$ ls"))
                    // we'll assume anyway that we're listing children
                    continue;
                else if (line.StartsWith("dir "))
                {
                    var childName = line.Replace("dir ", "").Trim();
                    var alreadyExists = currentDirectory.Children.Any(x => x.Name == childName);
                    if (!alreadyExists)
                        currentDirectory.Children.Add(new Directory() { Name = childName, Parent = currentDirectory });
                }
                else
                {
                    var parts = line.Split(' ');
                    var file = new File() { Name = parts[1], Size = long.Parse(parts[0]) };
                    currentDirectory.Files.Add(file);
                }
            }

            var dirs = new List<Directory>();
            FindAllDirectories(dirs, topDirectory);
            Console.WriteLine(dirs.Where(x => x.Size < 100000).Sum(x => x.Size));

            var spaceAvailable = 70000000 - topDirectory.Size;
            var spaceNeededToReclaim = 30000000 - spaceAvailable;
            Console.WriteLine(dirs.Where(x => x.Size >= spaceNeededToReclaim).OrderBy(x => x.Size).First().Size);
        }

        static void FindAllDirectories(List<Directory> list, Directory directory)
        {
            list.Add(directory);
            foreach (var child in directory.Children)
                FindAllDirectories(list, child);
        }

        internal class Directory
        {
            public string Name { get; set; }
            public List<Directory> Children = new List<Directory>();
            public List<File> Files = new List<File>();
            public Directory Parent { get; set; }

            private long _size = -1;

            public long Size
            {
                get
                {
                    if (_size == -1)
                        _size = Children.Sum(x => x.Size) + Files.Sum(x => x.Size);
                    return _size;
                }
            }


        }

        internal class File
        {
            public string Name { get; set; }
            public long Size { get; set; }
        }
    }
}
