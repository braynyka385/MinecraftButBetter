using MinecraftButBetter.Screens;

namespace MinecraftButBetter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ChangeScreen(this, new GameScreen());
        }
        public static void ChangeScreen(object sender, UserControl next)
        {
            Form f; // will either be the sender or parent of sender

            if (sender is Form)
            {
                f = (Form)sender;
            }
            else
            {
                UserControl current = (UserControl)sender;
                f = current.FindForm();
                f.Controls.Remove(current);
            }

            next.Location = new Point((f.ClientSize.Width - next.Width) / 2,
                (f.ClientSize.Height - next.Height) / 2);

            f.Controls.Add(next);
            next.Focus();
        }
    }
}