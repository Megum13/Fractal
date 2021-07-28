using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;


namespace frak
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region переменные
        int provSize = 0;
        int naz = 0;
        int x1, x2, x3, y1, y2, y3, sh = 0;
        bool stop = false;
        int size = 3;
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            stop = true; //остановка цикла в потоке (Почему то останавливается один из потоков, а не все сразу) ***ИСПРАВИТЬ***
        }

        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            #region проверка textbox1
            try
            {
                provSize = int.Parse(textBox1.Text);
                if (provSize > 0 && provSize < 999)
                {
                    size = provSize;
                }
                else
                {
                    textBox1.Text = "";
                }
            }
            catch
            {
                textBox1.Text = "";
            }
            #endregion
        }


        private void OnPictureBoxClicked(object sender, MouseEventArgs e)
        {
            #region рисовка на picturebox1 и выбор трех координат
            if (e.Button == MouseButtons.Left)
            {
                Random ran = new Random();
                Graphics g = pictureBox1.CreateGraphics();
                Pen pen = new Pen(Color.Black, size);

                g.DrawRectangle(pen, e.X, e.Y, size, size);
                if (sh == 0)
                {
                    x1 = e.X;
                    y1 = e.Y;
                    sh++;
                }
                else if (sh == 1)
                {
                    x2 = e.X;
                    y2 = e.Y;
                    sh++;
                }
                else if (sh == 2)
                {
                    x3 = e.X;
                    y3 = e.Y;
                    sh = 0;
                }
            }
            #endregion

            #region запуск потока и передача туда данных
            else if (e.Button == MouseButtons.Right)
            {
                naz++;
                Thread th = new Thread(() => Start(x1, x2, x3, y1, y2, y3, size));
                th.IsBackground = true;
                th.Start();
            }
            #endregion
        }

        //Глав. метод в потоке
        void Start(int x1, int x2, int x3, int y1, int y2, int y3 , int size)
        {
            
            #region переменные
            Random ran = new Random();
            Graphics g = pictureBox1.CreateGraphics();
            Pen pen = new Pen(Color.Black, size);

            int startPosX = x1,
                startPosY = y1,
                r,
                X = 0,
                Y = 0;
            #endregion

            for (int i = 0; i < 999999; i++)
            {
                
                r = ran.Next(1,4);
                if (r == 1)
                {
                    X = x1;
                    Y = y1;
                }
                else if (r == 2)
                {
                    X = x2;
                    Y = y2;
                }
                else 
                {
                    X = x3;
                    Y = y3;
                }


                startPosX = (startPosX + X) / 2;
                startPosY = (startPosY + Y) / 2;

                g.DrawRectangle(pen, startPosX, startPosY, size, size);

                label2.Invoke(new Action(() => label2.Text = "Пкм нажато - " + naz));

                if (stop == true)//остановка потока
                {
                    stop = false;
                    break;
                }

                //DEL fireshow
                if (checkBox1.Checked == true)
                {
                    Pen penTest = new Pen(Color.FromArgb(ran.Next(1, 255), ran.Next(1, 255), ran.Next(1, 255)), 1);
                    g.DrawLine(penTest, startPosX, startPosY, X, Y);
                }

            }
        }
    }
}
