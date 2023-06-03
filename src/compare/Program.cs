namespace compare
{
    // Compare files of 2 directories.
    // Colors hinting: https://www.codeproject.com/Tips/5255355/How-to-Put-Color-on-Windows-Console
    internal class Program
    {
        private static string dir1 = "";
        private static string dir2 = "";

        static void Main(string[] args)
        {
            // remove these lines in production!
            //args[0] = "d:\\Desktop\\delete.me\\f1";
            //args[1] = "d:\\Desktop\\delete.me\\f2";
            // compare "d:\Desktop\delete.me\f1" "d:\Desktop\delete.me\f2"

            if (args.Count() == 2)
            {
                dir1 = Utilities.GetDirInfo(args[0]);
                dir2 = Utilities.GetDirInfo(args[1]);

                //Console.WriteLine(string.Format("Comparing {0} vs {1}", dir1, dir2));
                if (!Utilities.isdir(dir1) || !Utilities.isdir(dir2))
                {
                    Console.WriteLine("Both directories MUST exist.");
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Directory #1 = " + dir1);
                    Console.WriteLine("Directory #2 = " + dir2);
                    Console.WriteLine("");

                    compare_directories(dir1, dir2);
                }
            }
            else
            {
                Console.WriteLine("Must pass two directories as the arguments.");
            }
        }

        

        private static void compare_directories(string dir1, string dir2)
        {
            List<string> directory1Files = Utilities.GetFiles(dir1);
            List<string> directory2Files = Utilities.GetFiles(dir2);

            Console.WriteLine("Files in \u001b[33mDirectory #1\u001b[0m but not in \u001b[32mDirectory #2\u001b[0m:");
            CompareFiles(directory1Files, directory2Files);

            Console.WriteLine("\nFiles in \u001b[32mDirectory #2\u001b[0m but not in \u001b[33mDirectory #1\u001b[0m:");
            CompareFiles(directory2Files, directory1Files);

            Console.WriteLine("\n\u001b[35mCRC32 Hash differences!\u001b[0m");
            List<string> crcs = HashDifferences(directory1Files, directory2Files);
            foreach (string crc in crcs)
            {
                Console.WriteLine(crc);
            }
        }        

        static List<string> HashDifferences(List<string> files1, List<string> files2)
        {
            List<string> crcs = new List<string>();

            foreach (string file in files1)
            {
                string relativePath = GetRelativePath(file);
                if (files2.Contains(relativePath))
                {
                    if (!CompareCRC32(relativePath))
                    {
                        crcs.Add(relativePath);
                    }
                }
            }

            return crcs;
        }

        static List<string> CompareFiles(List<string> files1, List<string> files2)
        {
            List<string> crcs = new List<string>();

            foreach (string file in files1)
            {
                string relativePath = GetRelativePath(file);
                if (!files2.Contains(relativePath))
                {
                    Console.WriteLine(relativePath);
                }
                else
                {
                    //Console.WriteLine("Compare equality of hashes of file1 == files2");
                    if (!CompareCRC32(relativePath))
                    {
                        crcs.Add(relativePath);
                    }

                }
            }

            return crcs;
        }

        private static bool CompareCRC32(string relativePath)
        {
            byte[] data1 = File.ReadAllBytes(dir1 + "\\" + relativePath); // Encoding.ASCII.GetBytes(input);
            byte[] data2 = File.ReadAllBytes(dir2 + "\\" + relativePath); // Encoding.ASCII.GetBytes(input);

            CRC32 crc32 = new CRC32();
            byte[] hash1 = crc32.ComputeHash(data1);
            byte[] hash2 = crc32.ComputeHash(data2);

            return BitConverter.ToUInt32(hash1, 0) == BitConverter.ToUInt32(hash2, 0);
        }

        private static string GetRelativePath(string fullPath)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return fullPath.Replace(baseDirectory, "");
            //return fullPath;
        }
    }
}