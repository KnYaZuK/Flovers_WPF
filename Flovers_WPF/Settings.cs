using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace Flovers_WPF
{
    public class Settings
    {
        public string dbpath { get; set; }

        public Settings() { }

        public bool Set_DB_Path( )
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.SelectedPath = Directory.GetCurrentDirectory();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dbpath = fbd.SelectedPath + "\\Flowers.sqlite";
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
