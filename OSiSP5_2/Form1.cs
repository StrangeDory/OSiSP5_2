using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OSiSP5_2
{
    public partial class Form1 : Form
    {
        String[][] values;
        int countColoums;
        public Form1()
        {
            InitializeComponent();
            List<String> list = new List<String>();
            using (StreamReader reader = new StreamReader("text.txt"))
            {
                string line = "";
                int count = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    count++;
                    foreach (string item in line.Split(';'))
                        if (item != "") list.Add(item);
                }
                countColoums = list.Count / count;
                values = new String[count][];
                for (int i = 0; i < values.Length; i++)
                    values[i] = new String[countColoums];
                
            }
            int index = 0;
            for (int i = 0; i < values.Length; i++)
                for (int j = 0; j < values[i].Length; j++)
                {
                    values[i][j] = list[index];
                    index++;
                }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int width = ClientSize.Width;
            float widthColoum = countColoums != 0 ? width / countColoums : 0;
            int separatorWidth = 2;
            Pen pen = new Pen(Color.Black, separatorWidth);
            int height = 0;
            int LineY = 0;
            for (int j = 0; j < (values.Length); j++)
            {
                height = 0;
                for (int i = 0; i < countColoums; i++)
                {
                    Label label = new Label
                    {
                        Text = values[j][i],
                        AutoSize = true,
                        MaximumSize = new Size((int)widthColoum - 4, 0),
                        Location = new Point(i * (int)widthColoum + 2, LineY + 4)
                    };
                    int cur = (int)Math.Ceiling((decimal)((int)widthColoum - 4) / TextRenderer.MeasureText(label.Text[0].ToString(), label.Font, label.MaximumSize).Width);
                    int countLettersInLine = cur < label.Text.Length ? cur : label.Text.Length;
                    int countLines = (int)Math.Ceiling((decimal)label.Text.Length / countLettersInLine);
                    label.Height = (TextRenderer.MeasureText(label.Text, label.Font, label.MaximumSize).Height + 5) * countLines;
                    height = (height < label.Height) ? label.Height : height;
                    
                    Controls.Add(label);
                }
                for (int i = 0; i < countColoums; i++)
                {
                    e.Graphics.DrawLine(pen, i * widthColoum, LineY, i * widthColoum, LineY + height);
                }
                e.Graphics.DrawLine(pen, 0, LineY + 1, ClientSize.Width, LineY + 1);
                LineY += height;
                
            }
            for (int i = 0; i < countColoums; i++)
                e.Graphics.DrawLine(pen, i * widthColoum, LineY - Controls[Controls.Count - 1].Height, i * widthColoum, LineY);
            e.Graphics.DrawLine(pen, 0, LineY + 1, ClientSize.Width, LineY + 1);

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (Controls.Count != 0) Controls.Clear();
            Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (Controls.Count != 0) Controls.Clear();
                for (int i = 0; i < values.Length; i++)
                {
                    Array.Resize(ref values[i], values[i].Length + 1);
                    values[i][values[i].Length - 1] = "nothing";
                }
                countColoums++;
            }
            if (e.KeyCode == Keys.Down && countColoums != 0)
            {
                if (Controls.Count != 0) Controls.Clear();
                for (int i = 0; i < values.Length; i++)
                {
                    Array.Resize(ref values[i], values[i].Length - 1);
                }
                countColoums--;
            }
            Invalidate();
        }
    }
}
