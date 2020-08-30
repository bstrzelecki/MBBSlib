using Microsoft.Xna.Framework;

namespace MBBSlib.MonoGame
{
    /// <summary>
    /// Registry of supported resolutions
    /// </summary>
    public struct Resolution
    {

        /// <summary>
        /// Width in pixels
        /// </summary>
        public int Width;
        /// <summary>
        /// Height in pixels
        /// </summary>
        public int Height;
        /// <summary>
        /// Size of window in vector2
        /// </summary>
        public Vector2 Size => new Vector2(Width, Height);

        /// <summary>
        /// 3840x2160
        /// </summary>
        public static Resolution UHD => new Resolution(3840, 2160);
        /// <summary>
        /// 3200x1800
        /// </summary>
        public static Resolution QXGA => new Resolution(3200, 1800);
        /// <summary>
        /// 2560x1440
        /// </summary>
        public static Resolution QHD => new Resolution(2560, 1440);
        /// <summary>
        /// 2048x1152
        /// </summary>
        public static Resolution QWXGA => new Resolution(2048, 1152);
        /// <summary>
        /// 1920x1080
        /// </summary>
        public static Resolution FHD => new Resolution(1920, 1080);
        /// <summary>
        /// 1600x900
        /// </summary>
        public static Resolution HDp => new Resolution(1600, 900);
        /// <summary>
        /// 1280x720
        /// </summary>
        public static Resolution XGA => new Resolution(1280, 720);


        /// <summary>
        /// 960x540
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Current nameing style will create disambiguation between qHD and QHD")]
        public static Resolution qHD => new Resolution(960, 540);

        private Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}