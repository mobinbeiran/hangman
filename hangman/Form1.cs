using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace WindowsFormsApp29
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string word = "";
        List<Label> labels = new List<Label>();
        int amount = 0;
        enum bodyparts
        {
            Head,
            Left_Eye,
            Right_Eye,
            Mouth,
            Body,
            Right_Arm,
            Left_Arm,
            Right_Leg,
            Left_Leg,
        }
        void drawhangpost()
        {
            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Brown, 10);
            g.DrawLine(p, new Point(130, 218), new Point(130, 5));
            g.DrawLine(p, new Point(135, 5), new Point(65, 5));
            g.DrawLine(p, new Point(60, 0), new Point(60, 50));
        }

        void drawbodyparts(bodyparts bp)
        {
            Graphics g = panel1.CreateGraphics();
            Pen p = new Pen(Color.Black, 2);
            if (bp == bodyparts.Head)
                g.DrawEllipse(p, 40, 50, 40, 40);
            else if (bp == bodyparts.Left_Eye)
            {
                SolidBrush s = new SolidBrush(Color.Black);
                g.FillEllipse(s, 50, 60, 5, 5);
            }
            else if (bp == bodyparts.Right_Eye)
            {
                SolidBrush s = new SolidBrush(Color.Black);
                g.FillEllipse(s, 63, 60, 5, 5);
            }
            else if (bp == bodyparts.Mouth)
                g.DrawArc(p, 50, 60, 20, 20, 45, 90);
            else if (bp == bodyparts.Body)
                g.DrawLine(p, new Point(60, 90), new Point(60, 170));
            else if (bp == bodyparts.Left_Arm)
                g.DrawLine(p, new Point(60, 100), new Point(30, 85));
            else if (bp == bodyparts.Right_Arm)
                g.DrawLine(p, new Point(60, 100), new Point(90, 85));
            else if (bp == bodyparts.Left_Leg)
                g.DrawLine(p, new Point(60, 170), new Point(30, 190));
            else if (bp == bodyparts.Right_Leg)
                g.DrawLine(p, new Point(60, 170), new Point(90, 190));
        }

        void MakeLabels()
        {
            word = getrandomword();
            char[] chars = word.ToCharArray();
            int between = 350 / chars.Length - 1;
            for (int i = 0; i < chars.Length - 1; i++)
            {
                labels.Add(new Label());
                labels[i].Location = new Point((i * between) + 10, 80);
                labels[i].Text = "_";
                labels[i].Parent = groupBox1;
                labels[i].BringToFront();
                labels[i].CreateControl();
            }
            label1.Text = "word lenght: " + (chars.Length - 1).ToString();
        }

        string getrandomword()
        {
            WebClient wc = new WebClient();
            string wordlist = wc.DownloadString("https://www.mit.edu/~ecprice/wordlist.10000");
            string[] words = wordlist.Split('\n');
            Random ran = new Random();
            return words[ran.Next(0, words.Length - 1)];
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            drawhangpost();
            MakeLabels();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            char letter = textBox1.Text.ToLower().ToCharArray()[0];
            if (!char.IsLetter(letter))
            {
                MessageBox.Show("you can only submit letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (word.Contains(letter))
            {
                char[] letters = word.ToCharArray();
                for (int i = 0; i < letters.Length; i++)
                {
                    if (letters[i] == letter)
                        labels[i].Text = letter.ToString();
                }
                foreach (Label l in labels)
                    if (l.Text == "_") return;
                MessageBox.Show("you have won!", "Congrats");
                resetgame();
            }

            else
            {
                MessageBox.Show("the letter that you guessed isn't in the word", "Sorry");
                label2.Text += " " + letter.ToString() + ",";
                drawbodyparts((bodyparts)amount);
                amount++;
                if (amount == 9)
                {
                    MessageBox.Show("you lost! the word was " + word);
                    resetgame();
                }
            }
        }

        void resetgame()
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(panel1.BackColor);
            getrandomword();
            MakeLabels();
            drawhangpost();
            label2.Text = "Missed: ";
            textBox1.Text = "";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == word)
            {
                MessageBox.Show("you have won!", "Congrats");
                resetgame();
            }
            else
            {
                MessageBox.Show("the wor that you guessed is wrong!", "Sorry");
                drawbodyparts((bodyparts)amount);
                amount++;
                if (amount == 9)
                {
                    MessageBox.Show("you lost! the word was " + word);
                    resetgame();
                }
            }
        }
    }
}
