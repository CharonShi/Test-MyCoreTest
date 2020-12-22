using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shi.WinFrom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void MenuMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            var type = e.ClickedItem.Text;

            switch (type)
            {
                case "进制转换":

                    break;
                case "线程":

                    break;
                default:
                    break;
            }


            if (false)
            {

            }

        }


    }
}
