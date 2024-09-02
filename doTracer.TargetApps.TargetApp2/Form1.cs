using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TargetApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.ReadAllBytes(@"C:\Users\Sphere\Desktop\temp.txt");
            File.ReadAllLines(@"C:\Users\Sphere\Desktop\temp.txt");
            File.ReadAllText(@"C:\Users\Sphere\Desktop\temp.txt");
            File.ReadLines(@"C:\Users\Sphere\Desktop\temp.txt");
        }
    }
}
