using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace UI_ProgrammingApplication
{
    public static class Extensions
    {
        public static Point Center(this Rectangle r)
        {
            return new Point(r.X + r.Size.Width / 2, r.Y + r.Size.Height / 2);
        }

    }
}
