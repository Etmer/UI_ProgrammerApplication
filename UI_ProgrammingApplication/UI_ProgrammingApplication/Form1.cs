using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_ProgrammingApplication
{
    public partial class Form1 : Form
    {
        public static Form1 instance;
        private AppData _data;
        private int windowIndex = 0;
        private Timer t = new Timer();
        private Rectangle mouseRect;
        private CustomButton currentSelectedButton;

        public Form1()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            InitializeAppContent();
            Paint += DrawAppContent;
            MouseDown += (sender, e) => { Interact(); };
            MouseUp += (sender, e) => { ReleaseInteracion(); };
            MouseEnter += (sender, e) => { HideCursor(true); };
            MouseLeave += (sender, e) => { HideCursor(false); };
            Resize += (sender, e) => { _data.windows[windowIndex].ResizeWindow(); };
            Resize += (sender, e) => { RefreshAppContent(); };

            t.Interval = 10;
            t.Tick += new EventHandler((myObject, myEventArgs)=> { UILoop(); });
            mouseRect = new Rectangle(MousePosition.X - this.ClientRectangle.Width, MousePosition.Y - this.ClientRectangle.Y, 10, 10);
            t.Start();
        }

        public Form1(int i)
        {
            windowIndex = i;
            this.DoubleBuffered = true;
            InitializeComponent();
            InitializeAppContent();
            Paint += DrawAppContent;
            MouseDown += (sender, e) => { Interact(); };
            MouseUp += (sender, e) => { ReleaseInteracion(); };
            MouseEnter += (sender, e) => { HideCursor(true); };
            MouseLeave += (sender, e) => { HideCursor(false); };
            Resize += (sender, e) => { _data.windows[windowIndex].ResizeWindow(); };
            Resize += (sender, e) => { RefreshAppContent(); };

            t.Interval = 10;
            t.Tick += new EventHandler((myObject, myEventArgs) => { UILoop(); });
            mouseRect = new Rectangle(MousePosition.X - this.ClientRectangle.Width, MousePosition.Y - this.ClientRectangle.Y, 10, 10);
            t.Start();
        }

        //privates

        private void InitializeAppContent()
        {
            MinimumSize = new Size(600,600);

            _data = new AppData(new List<CustomWindow>());

            _data.windows.Add(new CustomWindow(this));
            _data.windows.Add(new CustomWindow(this));

            _data.windows[0].elements.Add(new CustomLabel(new Rectangle(),new Padding(0,0,0,0),"I am a Label"));
            _data.windows[0].AddButtonsForDebugging("To Page 2",1);
            _data.windows[1].elements.Add(new CustomLabel(new Rectangle(), new Padding(0, 0, 0, 0), "I am another Label"));
            _data.windows[1].AddButtonsForDebugging("Start Game",3);

            _data.windows[0].buttons[0].ButtonWasClicked += (sender, e) => { ChangeWindow(1); };

            _data.windows[1].buttons[0].ButtonWasClicked += (sender, e) => { _data.windows[1].buttons[0].ChangeTextContent("Sadly no Game"); };
            _data.windows[1].buttons[1].ButtonWasClicked += (sender, e) => { _data.windows[1].buttons[1].ChangeTextContent("Sadly no Game"); };
            _data.windows[1].buttons[2].ButtonWasClicked += (sender, e) => { _data.windows[1].buttons[2].ChangeTextContent("Sadly no Game"); };
        }

        private void DrawAppContent(object sender, PaintEventArgs e)
        {
            if (_data != null)
            {
                _data.windows[windowIndex].DrawWindow(e.Graphics, (int)this.ClientRectangle.Width, (int)this.ClientRectangle.Height);
                ControlPaint.DrawBorder(e.Graphics, mouseRect, Color.Red, ButtonBorderStyle.Solid);
            }
        }

        private void RefreshAppContent()
        {
            this.Refresh();
        }

        private void UILoop()
        {
            mouseRect.Location = new Point(MousePosition.X - this.Location.X, MousePosition.Y - this.Location.Y);

            if (currentSelectedButton == null)
            {
                CheckMouseCollision();
            }
            else
            {
                if (!CheckIfMouseIsInBounds(currentSelectedButton.rectangle))
                {
                    currentSelectedButton.HighlightButton(false);
                    currentSelectedButton = null;
                } 
            }
            this.Refresh();
        }

        private void CheckMouseCollision()
        {
            if (_data.windows[windowIndex].CheckForButtonOverlapping(mouseRect,ref currentSelectedButton))
            {
                this.Refresh();
            }
        }

        private bool CheckIfMouseIsInBounds(Rectangle rect)
        {
            return mouseRect.IntersectsWith(rect);

        }

        private void Interact()
        {
            if (currentSelectedButton != null)
            {
                currentSelectedButton.ButtonClicked();
            }
        }

        private void ReleaseInteracion()
        {
            if (currentSelectedButton != null)
            {
                currentSelectedButton.ButtonReleased();
                currentSelectedButton.HighlightButton(CheckIfMouseIsInBounds(currentSelectedButton.rectangle));
            }
        }

        private void HideCursor(bool state)
        {
            if (state)
            {
                Cursor.Hide();
            }
            else
            {
                Cursor.Show();
            }
        }

        private void ChangeWindow(int index)
        {
            Form1 f = new Form1(index);
            f.Show();
            this.Refresh();
        }
    }
}
