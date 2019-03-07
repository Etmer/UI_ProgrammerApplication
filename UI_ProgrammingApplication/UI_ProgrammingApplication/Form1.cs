using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_ProgrammingApplication
{
    public partial class Form1 : Form
    {
        public static Form1 instance;
        private AppData _data;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            _data = new AppData(this);
        }
        //privates


    }
}
