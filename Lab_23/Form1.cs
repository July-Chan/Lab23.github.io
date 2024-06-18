using System.Windows.Forms;

namespace Lab_23
{
    public partial class Form1 : Form
    {
        private double a;
        private double scale = 30;

        public Form1()
        {
            InitializeComponent();
            panel1.Paint += PanelGraph_Paint;
        }

        // Кнопка для побудови графіка
        private void button1_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(textBox1.Text, out a))
            {
                MessageBox.Show("Будь ласка, введіть дійсний номер для a.");
                return;
            }

            if (!double.TryParse(textBox2.Text, out scale) || scale <= 0)
            {
                MessageBox.Show("Будь ласка, введіть дійсне додатне число для шкали.");
                return;
            }

            panel1.Invalidate();
        }

        // Метод для зображення усього графіка
        private void PanelGraph_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);

            DrawAxes(g);
            DrawGraph(g);
        }

        // Метод, що малює координатні осі
        private void DrawAxes(Graphics g)
        {
            int width = panel1.Width;
            int height = panel1.Height;

            Pen axisPen = new Pen(Color.Black, 2);
            g.DrawLine(axisPen, 0, height / 2, width, height / 2); // X-axis
            g.DrawLine(axisPen, width / 2, 0, width / 2, height);  // Y-axis

            Font font = new Font("Arial", 10);
            Brush brush = Brushes.Black;
            g.DrawString("X", font, brush, width - 20, height / 2 + 5);
            g.DrawString("Y", font, brush, width / 2 + 5, 0);

            for (int i = -10; i <= 10; i++)
            {
                int x = width / 2 + i * (int)scale;
                int y = height / 2 + i * (int)scale;
                g.DrawLine(axisPen, x, height / 2 - 5, x, height / 2 + 5);
                g.DrawLine(axisPen, width / 2 - 5, y, width / 2 + 5, y);

                if (i != 0)
                {
                    g.DrawString(i.ToString(), font, brush, x - 10, height / 2 + 5);
                    g.DrawString((-i).ToString(), font, brush, width / 2 + 5, y - 10);
                }
            }
        }

        // Метод для малювання функцій
        private void DrawGraph(Graphics g)
        {
            if (a == 0) return;

            Pen graphPenX = new Pen(Color.Blue, 1);
            Pen graphPenY = new Pen(Color.Green, 1);
            int width = panel1.Width;
            int height = panel1.Height;

            double minT = -10;
            double maxT = 10;

            for (double t = minT; t <= maxT; t += 0.01)
            {
                if (Math.Abs(1 + Math.Pow(t, 3)) < double.Epsilon)
                    continue;

                double x = (3 / (a * t)) / (1 + Math.Pow(t, 3));
                double y = (3 * a * Math.Pow(t, 2)) / (1 + Math.Pow(t, 3));

                if (double.IsInfinity(x) || double.IsInfinity(y))
                    continue;

                int screenX = (int)(width / 2 + x * scale);
                int screenY = (int)(height / 2 - y * scale);

                if (screenX < 0 || screenX >= width || screenY < 0 || screenY >= height)
                    continue;

                g.DrawLine(graphPenX, screenX, screenY, screenX + 3, screenY + 3);

                screenY = (int)(height / 2 - y * scale);
                g.DrawLine(graphPenY, screenX, screenY, screenX + 3, screenY + 3);
            }
        }
    }
}
