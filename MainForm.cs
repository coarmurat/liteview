using System.Diagnostics;

namespace LiteView
{
    public partial class MainForm : Form
    {
        readonly Helpers.Keyboard keyboard;
        bool qPressed = false;
        public MainForm()
        {
            InitializeComponent();
            keyboard = new();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            keyboard.ClearKeys();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            keyboard.Listen(Keys.Q, (int wParam) => {
                if (!qPressed && wParam == Helpers.Keyboard.WM_KEYDOWN)
                {
                    qPressed = true;
                    Debug.WriteLine("huu");
                }
                else if(qPressed && wParam == Helpers.Keyboard.WM_KEYUP)
                {
                    qPressed = false;
                }
            });
        }
    }
}