using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chip8_sharp
{
    public partial class Form1 : Form
    {
        private Machine.Machine machine = new Machine.Machine();

        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text.ToLower();
            string temp = "";
            List<byte> memImage = new List<byte>();
            for (int i = 0; i < input.Length; i++)
            {
                // Look for valid character
                if (input[i] >= 'a' || input[i] <= 'f' || input[i] >= '0' || input[i] <= '9')
                {
                    temp += input[i];
                }
                // Check if temp buffer is full.
                if (temp.Length == 2)
                {
                    byte value = Convert.ToByte(temp, 16);
                    memImage.Add(value);
                    temp = "";
                }
            }
            // Dump image into memory. By default, CHIP-8 programs start at address 200.
            machine.RAMBurn(0x200, memImage.ToArray());
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}