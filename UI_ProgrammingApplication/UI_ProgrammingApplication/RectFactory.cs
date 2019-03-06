using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace UI_ProgrammingApplication
{
    public static class RectFactory
    {
        public static Rectangle CreateRect(Rectangle parent, Padding padding)
        {
            int x = parent.X + padding.Left;
            int y = parent.Y + padding.Top;
            int width = parent.Size.Width - padding.Horizontal;
            int height = parent.Size.Height - padding.Vertical;

            return new Rectangle(x, y, width, height);
        }
    }
}
