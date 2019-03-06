using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace UI_ProgrammingApplication
{
    public delegate void ClickEventListener();

    public class CustomButton : CustomElement
    {
        public Color highlightingColor;
        public Color outlineColor;
        public Color fillColor;
        public Color currentColor;
        public Color pressedColor;

        public event EventHandler ButtonWasClicked;

        public CustomButton(Rectangle ButtonRect, Padding ButtonPadding, Color ButtonFillColor, Color ButtonOutlineColor, Color ButtonHighlightColor,Color ButtonPressedColor) : base(ButtonRect,ButtonPadding)
        {
            rectangle = RectFactory.CreateRect(ButtonRect, ButtonPadding);
            highlightingColor = ButtonHighlightColor;
            padding = ButtonPadding;
            currentColor = ButtonFillColor;
            fillColor = ButtonFillColor;
            outlineColor = ButtonOutlineColor;
            highlightingColor = ButtonHighlightColor;
            pressedColor = ButtonPressedColor;
        }

        public void HighlightButton(bool state)
        {
            if (state)
            {
                currentColor = highlightingColor;
            }
            else
            {
                currentColor = fillColor;
            }
        }
        
        public void ChangeTextContent(string TextContent)
        {
            text = TextContent;
        }

        public override void DrawElement(Graphics g)
        {
            SolidBrush brush = new SolidBrush(currentColor);
            g.FillRectangle(brush, rectangle);
            ControlPaint.DrawBorder(g, rectangle, Color.Black, ButtonBorderStyle.Solid);
            DisplayText(g);
        }

        public void ResizeInParent(Rectangle parent)
        {
            rectangle = RectFactory.CreateRect(parent, padding);
        }

        public void ButtonClicked()
        {
            currentColor = pressedColor;
        }

        public void ButtonReleased()
        {
            ButtonWasClicked.Invoke(this,EventArgs.Empty);
        }

        
    }
}
