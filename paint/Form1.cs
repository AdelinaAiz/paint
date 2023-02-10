using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint
{
    public partial class Form1 : Form
    {
        Graphics g;
        int x = -1, y = -1;
        bool b = false;
        Pen pen, temp;
        ColorDialog colorDialog = new ColorDialog();
        int cur = 0;
        public Form1()
        {
           // panel3.Size = new Size { Width = 2000, Height = 2000 };
            InitializeComponent();
            g = panel3.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pen = new Pen(Color.Black, 1);
            temp = new Pen(Color.Black, 1);
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round; 
            label5.BackColor = Color.Black;
            panel3.Size = new Size { Width = Convert.ToInt32(textBox1.Text), Height = Convert.ToInt32(textBox2.Text) };
            panel3.Cursor = Cursors.Default;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            pen.Color = p.BackColor;
            temp.Color = p.BackColor;
            label5.BackColor = p.BackColor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK) //Если окно закрылось с OK, то меняем цвет для пера и фона label2
            {
                pen.Color = colorDialog.Color; //меняем цвет для пера
                temp.Color = colorDialog.Color;
                label5.BackColor = colorDialog.Color;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            pen.Width = (int)numericUpDown1.Value;
            temp.Width = (int)numericUpDown1.Value;
        }

        private void button2_Click(object sender, EventArgs e)//размерчик кнопочками
        {
            try
            {
                panel3.Size = new Size { Width = Convert.ToInt16(textBox1.Text), Height = Convert.ToInt16(textBox2.Text) };
            }
            catch(Exception ex)
            {
                MessageBox.Show("Введите целое число");
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e){}

        private void Form1_Load(object sender, EventArgs e){}



        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (cur == 2 || cur == 1)
            {
                b = true;
                x = e.X;
                y = e.Y;
            }
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (cur == 2 || cur == 1)
                if (b && x != -1 && y != -1)
                {
                    if(cur == 2) { pen.Color = Color.White; pen.Width = 8; }
                    if(cur == 1) { pen.Color = temp.Color; pen.Width = temp.Width; }
                    g.DrawLine(pen, new Point(x, y), e.Location);
                    x = e.X;
                    y = e.Y;
                    label6.Text = Convert.ToString(e.Location.X) + " : " + Convert.ToString(e.Location.Y);
                }
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            if (cur == 2 || cur == 1)
            {
                b = false;
                x = -1;
                y = -1;
                //panel3.Cursor = Cursors.Default;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e){}

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e){}

        private void panel3_MouseEnter(object sender, EventArgs e)
        {
        //    label6.Text = Convert.ToString(Cursor.Position.X);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel3.Cursor = Cursors.Cross;
            //panel3.Cursor = new Cursor("C:\\Users\\Wait4n\\Desktop\\занятие на каждый день\\Borland Delphi c#\\pen.cur");
            cur = 1;
        }

        private void открытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.ShowDialog();
            System.Drawing.Image img = System.Drawing.Image.FromFile(of.FileName);
            panel3.BackgroundImage = img;
            panel3.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
           var crtl = panel3;
            Bitmap bmp = new Bitmap(crtl.Width, crtl.Height);
            crtl.DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));

            Size BorderSize = new Size(this.panel3.Width,this.panel3.Height);
            Graphics gr = Graphics.FromImage(bmp);
            gr.CopyFromScreen(this.Location.X - (BorderSize.Width/2), this.Location.Y - (BorderSize.Height), 0, 0, new Size(this.Width - BorderSize.Width, this.Height - BorderSize.Height));

            //Bitmap bmp = new Bitmap(panel3.Width, panel3.Height);
            //Graphics gr = Graphics.FromImage(bmp as Image);
            //gr.CopyFromScreen(this.Location.X + panel3.Location.X, this.Location.Y + panel3.Location.Y, this.Location.X + panel3.Location.X+ panel3.Width, this.Location.Y + panel3.Location.Y + panel3.Height, bmp.Size);

            SaveFileDialog sf = new SaveFileDialog();
            sf.Title = "Сохранить картинку как ...";
            sf.OverwritePrompt = true; //на случай если тай файл уже есть
            sf.CheckPathExists = true; //на случай, если путь тоже неправильный
            sf.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
            //отображается ли кнопка "Справка" в диалоговом окне
            sf.ShowHelp = true;
            if (sf.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
            {
                try
                {
                    bmp.Save(sf.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch
                {
                    MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }

        private void button6_Click(object sender, EventArgs e) { cur = 2; } 
         
        private void button5_Click_1(object sender, EventArgs e)//при очистке работает только invalidate
        {
            panel3.Invalidate();
            panel3.Update();
        }
        
    }
}
