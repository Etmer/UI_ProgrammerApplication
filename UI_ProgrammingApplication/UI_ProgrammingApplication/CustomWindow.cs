using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace UI_ProgrammingApplication
{
    public class CustomWindow
    {
        public Rectangle headerRectangle;
        public Rectangle bodyRectangle;
        public Rectangle backgroundRect;

        public float YRatio = 0.2f;

        public List<CustomElement> elements = new List<CustomElement>();
        public List<CustomButton> buttons = new List<CustomButton>();

        public CustomWindow(Form f)
        {
            CalculateWindow((int)f.ClientRectangle.Width, (int)f.ClientRectangle.Height);
        }

        public void AddButtonsForDebugging(string Content, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                buttons.Add(new CustomButton(bodyRectangle,new Padding(0,0,0,0),Color.Beige,Color.Black,Color.Azure,Color.Aquamarine));
                buttons[i].SetText(Content, new Font("Arial", 12), 12, Color.Black);
                elements.Add(buttons[i]);
            }
            ResizeWindow();
        }
        
        public void ResizeWindow()
        {
            CalculateElements();
        }

        private void CalculateElements()
        {
            Point startPoint = new Point(bodyRectangle.Left, bodyRectangle.Top);

            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].rectangle = GetElement(elements.Count, startPoint);
                startPoint.Y += bodyRectangle.Height / elements.Count;
            }
        }

        private Rectangle GetElement(int amount, Point startingPoint)
        {
            int Height = bodyRectangle.Height / amount;
            int Width = bodyRectangle.Width;

            Rectangle partRectangle = new Rectangle(startingPoint.X, startingPoint.Y, Width, Height);
            return RectFactory.CreateRect(partRectangle, new Padding(10,10,10,10));
        }

        public void CalculateWindow(int sizeX, int sizeY)
        {
            headerRectangle = new Rectangle(0, 0, sizeX, (int)(sizeY * YRatio));
            bodyRectangle = new Rectangle(0, headerRectangle.Bottom, sizeX, (int)(sizeY * (1 - YRatio)));
        }

        public void DrawWindow(Graphics g, int sizeX, int sizeY)
        {
            CalculateWindow(sizeX, sizeY);
            DrawHeader(g);
            DrawBody(g);
        }

        private void DrawHeader(Graphics g)
        {
            ControlPaint.DrawBorder(g, headerRectangle, Color.Black, ButtonBorderStyle.Solid);
            DisplayText(headerRectangle, "Christopher Etmer UI", g);
        }

        private void DisplayText(Rectangle rect, string content, Graphics g)
        {
            Font font = new Font("Arial", 12);
            SolidBrush brush = new SolidBrush(Color.Black);
            StringFormat s = new StringFormat();
            TextRenderer.DrawText(g, content, font, rect, Color.Black);

        }

        private void DrawBody(Graphics g)
        {
            ControlPaint.DrawBorder(g, bodyRectangle, Color.Black, ButtonBorderStyle.Solid);
            foreach (CustomElement b in elements)
            {
                b.DrawElement(g);
            }
        }

        public bool CheckForButtonOverlapping(Rectangle mouseRect, ref CustomButton possibleButton)
        {
            foreach (CustomButton b in buttons)
            {
                if (b.rectangle.IntersectsWith(mouseRect))
                {
                    b.HighlightButton(true);
                    possibleButton = b;
                    return true;
                }
            }
            return false;
        }

        private void Allign(List<CustomButton> buttons, Rectangle parent, Graphics g)
        {
            int spacing = 20;
            int Height = (parent.Height - ((buttons.Count - 1) * spacing)) / buttons.Count;

            Point startingPoint = new Point(parent.Top - spacing);

            for (int y = 0; y < buttons.Count; y++)
            {
                
            }
        }
    }
}
