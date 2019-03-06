using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace UI_ProgrammingApplication
{
    public class CustomElement
    {
        public Rectangle rectangle;
        public Padding padding;

        public string text;
        public Font font;
        public int fontSize = 12;
        public Color fontColor;

        public Point Center { get { return rectangle.Center(); } }
        public Point Location { get { return rectangle.Location; } private set { rectangle.Location = value; } }

        public CustomElement(Rectangle ButtonRect, Padding ButtonPadding)
        {
            rectangle = RectFactory.CreateRect(ButtonRect, ButtonPadding);
            padding = ButtonPadding;
        }
        public virtual void DrawElement(Graphics g)
        {
        }

        public void DisplayText(Graphics g)
        {
            if (text != null)
            {
                font = new Font(font.ToString(), (int)fontSize, FontStyle.Bold);
                SolidBrush brush = new SolidBrush(fontColor);
                StringFormat s = new StringFormat();
                TextRenderer.DrawText(g, text, font, rectangle, Color.Black);
            }
        }

        public void SetText(string TextContent, Font TextFont, int TextFontSize, Color TextColor)
        {
            text = TextContent;
            font = TextFont;
            fontSize = TextFontSize;
            fontColor = TextColor;
        }

    }
}
