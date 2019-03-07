using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace UI_ProgrammingApplication
{
    public class CustomLabel : CustomElement
    {
        public CustomLabel(Rectangle ButtonRect, Padding ButtonPadding, AppData data, string content): base(ButtonRect, ButtonPadding, data)
        {
            rectangle = RectFactory.CreateRect(ButtonRect, ButtonPadding);
            padding = ButtonPadding;
            SetText(content, new Font("Arial", 12), 12, Color.Black);
        }

        public override void DrawElement(Graphics g)
        {
            ControlPaint.DrawBorder(g, rectangle, Color.Black, ButtonBorderStyle.Solid);
            DisplayText(g);
        }
    }
}
