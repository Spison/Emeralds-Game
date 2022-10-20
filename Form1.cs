using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KanashLesta
{
    public partial class Form1 : Form
    {
        //List<Panel> panelList = new List<Panel>(25); // Первоначально делал списком, но понял, что с определенным количеством элементов, а его не так много - проще массив
        Panel []panelArray = new Panel[allPanels];
        const int width = 120;
        const int height = 120;
        const int distance = 5;
        const int sizeButton = 40;
        const int count = 15;
        const int allPanels = 25;
        Color []colors = new Color[5] { Color.Red, Color.Green, Color.Blue,Color.Black,Color.White };
        public Form1()
        {
            InitializeComponent();
        }       
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < allPanels; i++)
            {
                panelArray[i] = new Panel();
            }
            for (int i = 0, j = 0; i < 5; i = i + 2)//создание сверху цветов - эталонов
            {
                Panel panel = new Panel();
                panel.Width = width;
                panel.Height = height;
                panel.Name = "color" + j;
                panel.Location = new Point(i * (width + distance) + distance, 5);
                panel.BackColor = colors[j++];
                Controls.Add(panel);
            }
            int locationX = 0,locationY=0;
            //"блокирующие" панели
            for(int i = 1; i<allPanels;i=i+10)
            {
                panelArray[i].BackColor = colors[3];
            }
            for(int i = 3;i<allPanels;i=i+10)
            {
                panelArray[i].BackColor=colors[3];
            }

            //"свободные" панели
            panelArray[6].BackColor = colors[4];
            panelArray[8].BackColor = colors[4];
            panelArray[16].BackColor = colors[4];
            panelArray[18].BackColor = colors[4];

            byte[] colorsGeneration = new byte[3] {5,5,5};
            for (int i=0;i<allPanels;i++)//цикл панелей
            {
                locationX = (i%5) * (width + distance) + distance;
                locationY = (i/5) * (height + distance) + distance + 150;
                panelArray[i].Location = new Point(locationX, locationY);
                panelArray[i].Name = "Panel" + i;
                panelArray[i].Visible = true;
                panelArray[i].Width = width;
                panelArray[i].Height = height;
                if(panelArray[i].BackColor!=colors[3] && panelArray[i].BackColor!=colors[4])//добавляем кнопки
                {
                    List<Button> buttons = new List<Button>()
                        {
                            new Button (){Text = "↑",Width=sizeButton,BackColor=Color.White,Height=sizeButton,Location = new Point (width/2-sizeButton/2,0)},
                            new Button (){Text ="←",Width=sizeButton,BackColor=Color.White,Height=sizeButton,Location = new Point (0,height/2-sizeButton/2)},
                            new Button (){Text ="→",Width=sizeButton,BackColor=Color.White,Height=sizeButton,Location = new Point (width-sizeButton,height/2-sizeButton/2)},
                            new Button (){Text ="↓",Width=sizeButton,BackColor=Color.White,Height=sizeButton,Location = new Point (width/2-sizeButton/2,height-sizeButton)},
                        };
                    buttons[0].Click += swapLocationUp;
                    buttons[1].Click += swapLocationLeft;
                    buttons[2].Click += swapLocationRight;
                    buttons[3].Click += swapLocationDown;
                    foreach (Button btn in buttons)
                    {
                        panelArray[i].Controls.Add(btn);
                    }
                    //Рандомизируем цвета
                    Random rnd = new Random();
                    do
                    {
                        int x=rnd.Next(0,3);
                        for(int j = 0; j < 10; j++) x=rnd.Next(0,3);
                        if (colorsGeneration[x]>0) { panelArray[i].BackColor = colors[x]; colorsGeneration[x]--; break; };                      
                    } while (true);
                    
                }
                panelArray[i].Location = new Point(locationX, locationY);
                Controls.Add(panelArray[i]);
                checkFree();
            }
        }
        public void checkFree()
        {
            for(int i = 0; i < allPanels;i++)
            {
                foreach(Button control in panelArray[i].Controls)
                {
                    control.Enabled = false;
                    control.Visible = false;//закомментировать для проверки кнопок
                    if(i>4 && control.Text== "↑" && panelArray[i-5].BackColor==colors[4])
                    {
                        control.Enabled=true;
                        control.Visible=true;
                        continue;
                    }
                    if(i%5!=4 && control.Text== "→" && panelArray[i+1].BackColor==colors[4])
                    {
                        control.Enabled = true;
                        control.Visible = true;
                        continue;
                    }
                    if (i <20 && control.Text == "↓" && panelArray[i +5].BackColor == colors[4])
                    {
                        control.Enabled = true;
                        control.Visible = true;
                        continue;
                    }
                    if (i%5!=0 && control.Text == "←" && panelArray[i - 1].BackColor == colors[4])
                    {
                        control.Enabled = true;
                        control.Visible = true;
                        continue;
                    }
                }
            }
            checkVictory();
        }
        public void checkVictory()
        {
            bool checkWin = true;
            for (int i = 0; i < 25; i = i + 5)
            {
                if (panelArray[i].BackColor != colors[0])
                {
                    checkWin = false;
                    return;
                }
            }
            for (int i = 2; i < 25; i = i + 5)
            {
                if (panelArray[i].BackColor != colors[1])
                {
                    checkWin = false;
                    return;
                }
            }
            for (int i = 4; i < 25; i = i + 5)
            {
                if (panelArray[i].BackColor != colors[2])
                {
                    checkWin = false;
                    return;
                }
            }
            MessageBox.Show("Поздравляем, Вы - Победили!");
            Application.Exit();
        }
        public void swapLocationUp(object sender,EventArgs e)
        {
            Button button = sender as Button;
            Panel panelStart = (Panel)button.Parent;
            string name1 = panelStart.Name;
            int n = int.Parse(name1.Remove(0, 5));
            Panel panelNext = panelArray[n - 5];

            int xStart = panelArray[n].Location.X;
            int yStart = panelArray[n].Location.Y;

            string name2 = panelArray[n - 5].Name;

            panelStart.Name = name2;
            panelNext.Name = name1;

            panelStart.Location = panelArray[n - 5].Location;
            panelNext.Location = new Point(xStart, yStart);

            panelArray[n] = panelNext;
            panelArray[n - 5] = panelStart;
            checkFree();
        }
        public void swapLocationRight(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Panel panelStart = (Panel)button.Parent;
            string name1 = panelStart.Name;
            int n = int.Parse(name1.Remove(0, 5));
            Panel panelNext = panelArray[n + 1];

            int xStart = panelArray[n].Location.X;
            int yStart = panelArray[n].Location.Y;

            string name2 = panelArray[n + 1].Name;

            panelStart.Name = name2;
            panelNext.Name = name1;

            panelStart.Location = panelArray[n + 1].Location;
            panelNext.Location = new Point(xStart, yStart);

            panelArray[n] = panelNext;
            panelArray[n + 1] = panelStart;
            checkFree();
        }
        public void swapLocationDown(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Panel panelStart = (Panel)button.Parent;
            string name1 = panelStart.Name;
            int n = int.Parse(name1.Remove(0, 5));
            Panel panelNext = panelArray[n+5];

            int xStart = panelArray[n].Location.X;
            int yStart = panelArray[n].Location.Y;

            string name2 = panelArray[n +5].Name;

            panelStart.Name = name2;
            panelNext.Name = name1;

            panelStart.Location = panelArray[n + 5].Location;
            panelNext.Location = new Point(xStart, yStart);

            panelArray[n] = panelNext;
            panelArray[n +5] = panelStart;
            checkFree();
        }
        public void swapLocationLeft(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Panel panelStart = (Panel)button.Parent;
            string name1 = panelStart.Name;
            int n = int.Parse(name1.Remove(0, 5));
            Panel panelNext = panelArray[n - 1];

            int xStart = panelArray[n].Location.X;
            int yStart = panelArray[n].Location.Y;

            string name2 = panelArray[n-1].Name;

            panelStart.Name = name2;
            panelNext.Name = name1;

            panelStart.Location = panelArray[n - 1].Location;
            panelNext.Location = new Point(xStart,yStart);
            
            panelArray[n] = panelNext;
            panelArray[n - 1] = panelStart;
            checkFree();
        }
    }
}
