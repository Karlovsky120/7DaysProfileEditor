namespace SevenDaysProfileEditor.Data {

    /// <summary>
    /// Holds information about each uiIcon.
    /// </summary>
    internal struct UIIconData {
        public byte[] data;
        public int height;
        public int width;

        /// <summary>
        /// Creates UIIconData struct holding the provided info.
        /// </summary>
        /// <param name="data">Byte array holding image pixels</param>
        /// <param name="width">Width of the image</param>
        /// <param name="height">Height of the image</param>
        public UIIconData(byte[] data, int width, int height) {
            this.data = data;
            this.width = width;
            this.height = height;
        }
    }
}