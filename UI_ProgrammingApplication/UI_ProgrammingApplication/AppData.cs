using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace UI_ProgrammingApplication
{
    public class AppData
    {
        public Form1 _host;

        private Timer t = new Timer();
        private Rectangle mouseRect;
        private CustomButton currentSelectedButton;
        public List<CustomWindow> windows = new List<CustomWindow>();
        private CustomWindow _currentWindow;
        private CustomWindow _mainWindow;
        public int mainWindowIndex = 0;

        public AppData(Form1 host)
        {
            _host = host;
            _host.WindowState = FormWindowState.Maximized;

            _host.SizeChanged += (sender, e) => { _mainWindow.ResizeWindow(_host.ClientRectangle.Size); };
            _host.SizeChanged += (sender, e) => { RefreshAppContent(); };

            _host.MaximizedBoundsChanged += (sender, e) => { _mainWindow.ResizeWindow(_host.ClientRectangle.Size); };
            _host.MaximizedBoundsChanged += (sender, e) => { RefreshAppContent(); };

            _host.Resize += (sender, e) => { _mainWindow.ResizeWindow(_host.ClientRectangle.Size); };
            _host.Resize += (sender, e) => { RefreshAppContent(); };

            _host.Paint += DrawAppContent;

            _host.MouseDown += (sender, e) => { FlagWindowAsDraggable(true); };
            _host.MouseMove += (sender, e) => { DragWindow(); };

            _host.MouseDown += (sender, e) => { Interact(); };
            _host.MouseUp += (sender, e) => { ReleaseInteracion(); };
            _host.MouseUp += (sender, e) => { FlagWindowAsDraggable(false); };

            _host.MouseEnter += (sender, e) => { HideCursor(true); };
            _host.MouseLeave += (sender, e) => { HideCursor(false); };
            
            InitializeAppContent();

            t.Interval = 10;
            t.Tick += new EventHandler((myObject, myEventArgs) => { UILoop(); });
            mouseRect = new Rectangle(Control.MousePosition.X - _host.ClientRectangle.Width, Control.MousePosition.Y - _host.ClientRectangle.Y, 10, 10);
            t.Start();
        }

        private void InitializeAppContent()
        {
            _mainWindow = new CustomWindow(_host.ClientRectangle, new Padding(0, 0, 0, 0), new Point(0, 0), this, Color.Yellow);
            _mainWindow.Open(true);

            windows.Add(new CustomWindow(new Rectangle(new Point(100, 100), new Size(600, 900)), new Padding(0, 0, 0, 0), new Point(0, 0), this, Color.Green));
            _mainWindow.elements.Add(new CustomLabel(new Rectangle(), new Padding(0, 0, 0, 0), this, "I am a Label"));
            _mainWindow.AddButtonsForDebugging("To Page 2", 1);
            
            windows[0].elements.Add(new CustomLabel(new Rectangle(), new Padding(0, 0, 0, 0), this, "I am another Label"));
            windows[0].AddButtonsForDebugging("Start Game", 3);

            _mainWindow.buttons[0].ButtonWasClicked += (sender, e) => { windows[0].Open(true); };

            windows[0].buttons[0].ButtonWasClicked += (sender, e) => { windows[0].buttons[0].ChangeTextContent("Sadly no Game"); };
            windows[0].buttons[1].ButtonWasClicked += (sender, e) => { windows[0].buttons[1].ChangeTextContent("Sadly no Game"); };
            windows[0].buttons[2].ButtonWasClicked += (sender, e) => { windows[0].buttons[2].ChangeTextContent("Sadly no Game"); };

            windows[mainWindowIndex].ResizeWindow(_host.ClientRectangle.Size);
            _currentWindow = _mainWindow;
            RefreshAppContent();
        }

        public void DrawWindows(Graphics g)
        {
            _currentWindow = GetCurrentWindow();
            _mainWindow.DrawElement(g);
            for(int i= 0; i< windows.Count; i++)
            {
                if (windows[i].isOpen)
                {
                    windows[i].DrawElement(g);
                }
            }
        }

        private void FlagWindowAsDraggable(bool state)
        {
            if (_currentWindow != _mainWindow && mouseRect.IntersectsWith(_currentWindow.headerRectangle) && state)
            {
                _currentWindow.BecomeDraggable(true, mouseRect.Location);
                return;
            }
            _currentWindow.BecomeDraggable(false, mouseRect.Location);
        }

        private void DragWindow()
        {
            if (_currentWindow.draggable)
            {
                _currentWindow.RepositionWindow(Control.MousePosition);
            }
        }
        
        private void DrawAppContent(object sender, PaintEventArgs e)
        {
            DrawWindows(e.Graphics);
            ControlPaint.DrawBorder(e.Graphics, mouseRect, Color.Red, ButtonBorderStyle.Solid);
        }

        private void UILoop()
        {
            mouseRect.Location = new Point(Control.MousePosition.X - _host.Location.X, Control.MousePosition.Y - _host.Location.Y);

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
            _host.Refresh();
        }

        private CustomWindow GetCurrentWindow()
        {
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].backgroundRect.IntersectsWith(mouseRect))
                {
                    return windows[i];
                }
            }
            return _mainWindow;
        }

        private void CheckMouseCollision()
        {
            for (int i = 0; i < windows.Count; i++)
            {
                if (_currentWindow.CheckForButtonOverlapping(mouseRect, ref currentSelectedButton))
                {
                    _host.Refresh();
                }
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

        private void RefreshAppContent()
        {
            _host.Refresh();
        }
        
    }
}
