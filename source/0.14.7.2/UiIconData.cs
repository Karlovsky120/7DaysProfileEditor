namespace SevenDaysProfileEditor.Data
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
