using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace apache_manager
{
    class Functions
    {
        /**
         * this function will run composer command to create new laravel(latest version , user must have a php composer) project in directory
         */
         public void Laravel(string dirPath,string siteName)
        {
            try
            {
                string command = "/C cd /d" + dirPath + " && laravel new .";
                Process.Start("cmd.exe", command);
                /**
                * add site Name to apache vhosts config file
                */
                string vHost = @"C:\xampp\apache\conf\extra\httpd-vhosts.conf";

                string toWrite = @"<VirtualHost *:80>
ServerAdmin waltonmohsen@gmail.com
ServerName " + siteName  + @"
DocumentRoot " + dirPath + @"\public" + @"
<Directory " + dirPath + @"\public" +  @">
</Directory>
</VirtualHost>";
                using (StreamWriter writer = File.AppendText(vHost))
                {
                    writer.WriteLine(toWrite);
                }

            }
            catch (System.Exception error)
            {
                System.Windows.Forms.MessageBox.Show(error.Message);
                throw;
            }

        }
        /**
         * this function will check wp-persian for latest persian wordpress version to download
         */
        public void WordPress(string dirPath,string siteName)
        {
            var web = new HtmlWeb();

            try
            {
                var document = web.Load("http://wp-persian.com/download/");
                // wordpress download link  Query selector
                var link = document.DocumentNode.QuerySelector("#pagebody > div > div.col-3 > a").Attributes["href"].Value;

                // download wordpress compressed file and save it into temp folder and extract it
                string path = Path.GetTempPath() + @"\apache-site-temp\wordpress";
                string wpPath = path + @"\wordpress.zip";
                WebClient client = new WebClient();
                if (Directory.Exists(path))
                {
                    client.DownloadFile(link, wpPath);
                    ZipFile.ExtractToDirectory(wpPath, dirPath);
                    if (Directory.Exists(dirPath + @"\wordpress"))
                    {
                        /**
                        * copy wordpress main files and folders to main directory then delete to wordpress folder
                        */
                        DirectoryCopy(dirPath + @"\wordpress", dirPath, true);
                        Directory.Delete(dirPath + @"\wordpress", true);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("somthing wrong!");
                    }
                }
                else
                {
                    DirectoryInfo dir = Directory.CreateDirectory(path);
                    client.DownloadFile(link, wpPath);
                    ZipFile.ExtractToDirectory(wpPath, dirPath);
                    if (Directory.Exists(dirPath + @"\wordpress"))
                    {
                        /**
                        * copy wordpress main files and folders to main directory then delete to wordpress folder
                        */
                        DirectoryCopy(dirPath + @"\wordpress", dirPath, true);
                        Directory.Delete(dirPath + @"\wordpress", true);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("somthing wrong!");
                    }
                }
                /**
                 * add site Name to apache vhosts config file
                 */
                string vHost = @"C:\xampp\apache\conf\extra\httpd-vhosts.conf";
                
                string toWrite = @"<VirtualHost *:80>
ServerAdmin waltonmohsen@gmail.com
ServerName " + siteName + @"
DocumentRoot " + dirPath + @"
<Directory "+ dirPath + @">
</Directory>
</VirtualHost>";
                using (StreamWriter writer = File.AppendText(vHost))
                {
                    writer.WriteLine(toWrite);
                }

            }
            catch (System.Exception error)
            {
                System.Windows.Forms.MessageBox.Show(error.Message);
                throw;
            }


        }
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
