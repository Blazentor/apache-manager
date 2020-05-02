using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace apache_manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
     
        public void listBoxReader(string siteName)
        {
            
            // create new object from Functions class
            Functions fu = new Functions();

            string path = @"C:\xampp\htdocs\" + folderName.Text.ToLower();

            string plat = platform.GetItemText(platform.SelectedItem);
            if (plat.Contains("wordpress"))
            {
                fu.WordPress(path,siteName);
            }
            else if(plat.Contains("laravel"))
            {
                fu.Laravel(path,siteName);
            }
            else if(plat.Contains("none"))
            {
                MessageBox.Show("empty folder");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBoxReader(siteName.Text);
            //check for empty input boxes
            if (folderName.Text == "" || folderName.Text == null || folderName.Text == " " || siteName.Text == "" || siteName.Text == null || siteName.Text == " ")
            {
                MessageBox.Show("Please Enter the folder Name!");
            } else
            {
                // apache path + folder name
                string path = @"C:\xampp\htdocs\" + folderName.Text.ToLower();
                // hosts file (only windows -- need admin privileges)
                string hosts = @"C:\Windows\System32\drivers\etc\hosts";
                try
                {
                    /**
                     * check directory exist
                     */
                    if (Directory.Exists(path))
                    {
                        MessageBox.Show("Directory already Exist!");
                    }
                    else
                    {
                        // create directory
                        DirectoryInfo dir = Directory.CreateDirectory(path);
                        // add site url to hosts file
                        using (StreamWriter writer = File.AppendText(hosts))
                        {
                            writer.WriteLine("127.0.0.1    " + siteName.Text.ToLower());
                        }
                        MessageBox.Show("1- Directory created! \n " + dir.FullName + "\n" + "2- hosts file writen! \n" + "127.0.0.1    " + siteName.Text);

                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                    throw;
                }
            }
            
          
        }


    }
}
