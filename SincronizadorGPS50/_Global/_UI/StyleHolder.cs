using Infragistics.Documents.Reports.Graphics;
using System;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
    internal class StyleHolder
    {
        internal static System.Drawing.Rectangle ScreenWorkableArea = Screen.PrimaryScreen.WorkingArea;
        internal static int ScreenWorkableWidth { get; set; } =
        Convert.ToInt32(Math.Round((double)ScreenWorkableArea.Width * .7)) - 7;
        internal static int ScreenWorkableHeight { get; set; } =
        Convert.ToInt32(Math.Round((double)ScreenWorkableArea.Height * .85)) - 37;
        internal static int Row1Top { get; set; } = 10;
        internal static int LeftColumnLeft { get; set; } = 4;
        internal static int TopRowHeight { get; set; } = (int)(ScreenWorkableHeight * .055);
        internal static int CenterRowHeight { get; set; } = (int)(ScreenWorkableHeight - (TopRowHeight * 2)  - 50);
        internal static int BottomRowHeight { get; set; } = TopRowHeight;
        internal static int Row1Height { get; set; } = (int)(ScreenWorkableHeight * .075);
        internal static int Column1Left { get; set; } = 10;
        internal static int NextControlVerticalDistance { get; set; } = 25;
        internal static int LabelControlVerticalDistance { get; set; } = 10;
        internal static int ButtonHeight { get; set; } = 35;


        // Font
        // Font
        // Font
        // Font


        //public static System.Drawing.Font GlobalFontBold = new Font("Helvetica", 10, FontStyle.Bold);
        //public static System.Drawing.Font GlobalFontBold2 = new Font("Helvetica", 11, FontStyle.Bold);
        //public static System.Drawing.Font GlobalFontBold3 = new Font("Helvetica", 12, FontStyle.Bold);
        //public static System.Drawing.Font GlobalFontBold4 = new Font("Helvetica", 13, FontStyle.Bold);
        //public static System.Drawing.Font GlobalFontBold5 = new Font("Helvetica", 14, FontStyle.Bold);
        //public static System.Drawing.Font GlobalFontBold6 = new Font("Helvetica", 15, FontStyle.Bold);
        //public static System.Drawing.Font GlobalFontBold7 = new Font("Helvetica", 16, FontStyle.Bold);
        //public static System.Drawing.Font GlobalFontBold8 = new Font("Helvetica", 17, FontStyle.Bold);
        //public static System.Drawing.Font GlobalFontBold9 = new Font("Helvetica", 18, FontStyle.Bold);
        //public static System.Drawing.Font GlobalFontBold10 = new Font("Helvetica", 19, FontStyle.Bold);
        //public static System.Drawing.Font GlobalFontBold11 = new Font("Helvetica", 20, FontStyle.Bold);
        //public static System.Drawing.Font GlobalFont = new Font("Helvetica", 10, FontStyle.Regular);
        //public static System.Drawing.Font GlobalFont2 = new Font("Helvetica", 11, FontStyle.Regular);
        //public static System.Drawing.Font GlobalFont3 = new Font("Helvetica", 12, FontStyle.Regular);
        //public static System.Drawing.Font GlobalFont4 = new Font("Helvetica", 13, FontStyle.Regular);
        //public static System.Drawing.Font GlobalFont5 = new Font("Helvetica", 14, FontStyle.Regular);
        //public static System.Drawing.Font GlobalFont6= new Font("Helvetica", 15, FontStyle.Regular);
        //public static System.Drawing.Font GlobalFont7 = new Font("Helvetica", 16, FontStyle.Regular);
        //public static System.Drawing.Font GlobalFont8 = new Font("Helvetica", 17, FontStyle.Regular);
        //public static System.Drawing.Font GlobalFont9 = new Font("Helvetica", 18, FontStyle.Regular);
        //public static System.Drawing.Font GlobalFont10 = new Font("Helvetica", 19, FontStyle.Regular);
        //public static System.Drawing.Font GlobalFont11 = new Font("Helvetica", 20, FontStyle.Regular);


        // Colors
        // Colors
        // Colors
        // Colors


        internal static System.Drawing.Color ActionEnabledBlueButton { get; set; } = System.Drawing.Color.FromArgb(255, 6, 124, 252);
        public static System.Drawing.Color c_transparent = System.Drawing.Color.FromArgb(0, 255,255,255);
        public static System.Drawing.Color c_white = System.Drawing.Color.FromArgb(255, 255,255,255);
        public static System.Drawing.Color c_gray_253 = System.Drawing.Color.FromArgb(255, 253,253,253);
        public static System.Drawing.Color c_gray_252 = System.Drawing.Color.FromArgb(255, 252,252,252);
        public static System.Drawing.Color c_gray_250 = System.Drawing.Color.FromArgb(255, 250,250,250);
        public static System.Drawing.Color c_gray_242 = System.Drawing.Color.FromArgb(255, 244,244,244);
        public static System.Drawing.Color c_gray_240 = System.Drawing.Color.FromArgb(255, 240,240,240);
        public static System.Drawing.Color c_gray_230 = System.Drawing.Color.FromArgb(255, 230,230,230);
        public static System.Drawing.Color c_gray_200 = System.Drawing.Color.FromArgb(255, 200,200,200);
        public static System.Drawing.Color c_gray_100 = System.Drawing.Color.FromArgb(255, 150,150,150);
        public static System.Drawing.Color c_gray_75 = System.Drawing.Color.FromArgb(255, 75,75,75);
        public static System.Drawing.Color c_blue_1 = System.Drawing.Color.FromArgb(255, 6, 124, 252);
        public static System.Drawing.Color c_red_1 = System.Drawing.Color.FromArgb(255, 228, 8, 10);
        public static System.Drawing.Color c_green_1 = System.Drawing.Color.FromArgb(255, 40, 167, 69);
    }
}
