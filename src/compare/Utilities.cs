namespace compare
{
    public class Utilities
    {
        public static bool isdir(string dir)
        {
            bool exists = Directory.Exists(dir);
            return exists;
        }

        public static string GetDirInfo(string dir_slash)
        {
            // return full path info
            // return last occurence of /

            DirectoryInfo info = new DirectoryInfo(dir_slash);
            string dir = string.Format("{0}\\{1}", info.Parent, info.Name);
            return dir;
        }

        public static List<string> GetFiles(string directoryPath)
        {
            List<string> files = new List<string>();

            if (Directory.Exists(directoryPath))
            {
                string[] subfiles = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
                for (int f = 0; f < subfiles.Length; ++f)
                {
                    subfiles[f] = subfiles[f].Replace(directoryPath, "");
                }
                files.AddRange(subfiles);
            }

            return files;
        }
    }
}
