using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenDaysProfileEditor
{
    struct UIIconData
    {
        public byte[] data;
        public int width;
        public int height;

        public UIIconData(byte[] data, int width, int height)
        {
            this.data = data;
            this.width = width;
            this.height = height;
        }
    }
}
