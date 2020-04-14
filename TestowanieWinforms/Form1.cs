using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestowanieWinforms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static int _X;
        private static int _Y;
        private static int _SoManyMinesWoW;
        private static MineButton[,] mineFieldButtons;


        private void Set_Buttons()
        {

            for (int i = 0; i < _X; i++)
            {
                for (int j = 0; j < _Y; j++) 
                {
                    
                    mineFieldButtons[i, j].Location = new Point(10 + 45 * i, 10 + 45 * j);
                    mineFieldButtons[i, j].Size = new Size(45, 45);
                    mineFieldButtons[i, j].Text = "";
                    mineFieldButtons[i, j].Name = "" + (i + j * _Y);
                    mineFieldButtons[i, j].MouseUp += new MouseEventHandler(mineButton_Click);
                    mineFieldButtons[i, j].MouseDown += new MouseEventHandler(mineButton_HighLight);
                    //mineFieldButtons[i, j].

                    Controls.Add(mineFieldButtons[i, j]);
                }
            }
        }

        private void mineButton_HighLight(object sender, MouseEventArgs e)
        {
            var thisButton = (MineButton)sender;


            if (e.Button.Equals(MouseButtons.Right))
                foreach (MineButton x in thisButton._BondedButtons)
                    if (!x._Checked)
                        x.BackColor = Color.White;
        }
            
        private void mineButton_Click(object sender, MouseEventArgs e)
        {
            var thisButton = (MineButton)sender;


            if (e.Button.Equals(MouseButtons.Left))
            {
                thisButton._Checked = true;
                if (thisButton._Mine)
                {
                    thisButton.Text = "☼";
                    //Form BOOM = new Form();
                }

                else
                {
                    thisButton.BackColor = Color.White;
                    thisButton.Text = Convert.ToString(thisButton._MinesValue);
                    
                }

                if (thisButton._MinesValue == 0 && !thisButton._Mine)
                    foreach (MineButton x in thisButton._BondedButtons)
                        if (!x._Checked)
                        {
                            mineButton_Click(x as object, new MouseEventArgs(MouseButtons.Left, 1, x.Location.X, x.Location.Y, 0));
                            
                        }
            }
            else if (e.Button.Equals(MouseButtons.Right))
            {
                thisButton.Text = "F";
                foreach (MineButton x in thisButton._BondedButtons)
                    if (!x._Checked)
                        x.BackColor = Color.LightSlateGray;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
                        
            _X = Convert.ToInt32(textBox1.Text);
            _Y = Convert.ToInt32(textBox2.Text);
            _SoManyMinesWoW = Convert.ToInt32(textBox3.Text);
            Controls.Clear();
            mineFieldButtons = new MineButton[_X, _Y];
            Create_Mine_Field(_SoManyMinesWoW);
            Set_Buttons();


        }

        private class MineButton : Button
        {
            public int _MinesValue = 0;
            public bool _Mine = false;
            public bool _Checked = false;
            public bool _HightLighted = false;
            public List<MineButton> _BondedButtons = new List<MineButton>();

        }

        
        private static void Bound_Mines_Buttons()
        {
            for (int i = 0; i < mineFieldButtons.GetLength(0); i++)
            {
                for (int j = 0; j < mineFieldButtons.GetLength(1); j++)
                {
                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        if (k < 0)
                            k++;
                        else if (k >= mineFieldButtons.GetLength(0))
                            break;
                        for (int n = j - 1; n <= j + 1; n++)
                        {
                            if (n < 0)
                                n++;
                            else if (n >= mineFieldButtons.GetLength(1))
                                break;
                            if (mineFieldButtons[i, j] == mineFieldButtons[k, n])
                                continue;

                            mineFieldButtons[i, j]._BondedButtons.Add(mineFieldButtons[k, n]);

                            if (mineFieldButtons[k, n]._Mine)
                                mineFieldButtons[i, j]._MinesValue++;

                        }
                    }
                }
            }
        }


        private static void Create_Mine_Field(int howManyMines)
        {


            List<bool> minesList = new List<bool>();
            for (int i = 0; i < howManyMines; i++)
                minesList.Add(true);
            for (int i = 0; i < mineFieldButtons.Length - howManyMines; i++)
                minesList.Add(false);
            Random getRandom = new Random();
            int index;
            for (int i = 0; i < mineFieldButtons.GetLength(0); i++)
            {
                for (int j = 0; j < mineFieldButtons.GetLength(1); j++)
                {
                    mineFieldButtons[i, j] = new MineButton();
                    mineFieldButtons[i, j].Font = new Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                    mineFieldButtons[i, j].BackColor = Color.LightSlateGray;
                    index = getRandom.Next(0, minesList.Count - 1);
                    mineFieldButtons[i, j]._Mine = minesList[index];
                    minesList.RemoveAt(index);
                }
            }
            Bound_Mines_Buttons();
        }


    }
}

