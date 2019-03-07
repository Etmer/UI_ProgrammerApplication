using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace UI_ProgrammingApplication
{
    public class CustomWindow : CustomElement
    {
        public bool isOpen;
        public Rectangle headerRectangle;
        public Rectangle bodyRectangle;
        public Rectangle backgroundRect;
        private Color _backgroundColor;

        int OffsetX;
        int OffsetY;

        public bool draggable;
        public float YRatio = 0.2f;

        public List<CustomElement> elements = new List<CustomElement>();
        public List<CustomButton> buttons = new List<CustomButton>();

        public CustomWindow(Rectangle Rect, Padding ButtonPadding,Point rectLocation,AppData data, Color backgroundColor) : base (Rect, ButtonPadding, data)
        {
            _backgroundColor = backgroundColor;
            backgroundRect = Rect;
            RecalculateWindow(backgroundRect.Width, backgroundRect.Height);
        }

        public void AddButtonsForDebugging(string Content, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                buttons.Add(new CustomButton(bodyRectangle,new Padding(0,0,0,0), data,Color.Beige,Color.Black,Color.Azure,Color.Aquamarine));
                buttons[i].SetText(Content, new Font("Arial", 12), 12, Color.Black);
                elements.Add(buttons[i]);
            }
            ResizeWindow(backgroundRect.Size);
        }
        
        public void ResizeWindow(Size newSize)
        {
            backgroundRect.Size = newSize;
            RecalculateWindow(newSize.Width, newSize.Height);
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

        public void RecalculateWindow(int sizeX, int sizeY)
        {
            backgroundRect.Size = new Size(sizeX, sizeY);
            headerRectangle.Size = new Size (sizeX, (int)(sizeY * YRatio));
            headerRectangle.Location = backgroundRect.Location;
            bodyRectangle.Location = new Point(headerRectangle.Left,headerRectangle.Bottom);
            bodyRectangle.Size = new Size(sizeX, (int)(sizeY * (1 - YRatio)));
            CalculateElements();
        }

        public void RepositionWindow(Point targetPoint)
        {
            Location = backgroundRect.Location = new Point(targetPoint.X + OffsetX, targetPoint.Y + OffsetY);
        }

        public void DrawWindow(Graphics g, int sizeX, int sizeY)
        {
            RecalculateWindow(sizeX, sizeY);
            DrawBackGround(g);
            DrawHeader(g);
            DrawBody(g);
        }

        private void DrawHeader(Graphics g)
        {
            SolidBrush brush = new SolidBrush(Color.White);
            g.FillRectangle(brush, headerRectangle);
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

        public override void DrawElement(Graphics g)
        {
            DrawWindow(g, backgroundRect.Width, backgroundRect.Height);
        }

        public void Open(bool state)
        {
            isOpen = state;
        }

        private void DrawBackGround(Graphics g)
        {
            SolidBrush brush = new SolidBrush(_backgroundColor);
            g.FillRectangle(brush, backgroundRect);
            ControlPaint.DrawBorder(g, backgroundRect, Color.Black, ButtonBorderStyle.Solid);
        }

        public void BecomeDraggable(bool state, Point Offset)
        {
            draggable = state;
            if (state)
            {
                OffsetX = Location.X - Offset.X;
                OffsetY = Location.Y - Offset.Y;
            }
        }
    }
}
