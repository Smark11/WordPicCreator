using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WordPicCreator
{
    public static class ISKeys
    {
        public static string FONT = "FONT";
        public static string FONTSIZE = "FONTSIZE";
        public static string FONTCOLOR = "FONTCOLOR";
        public static string BOLD = "BOLD";
        public static string ITALIC = "ITALIC";

        public static SolidColorBrush GetBrushFromString(string colorName)
        {
            SolidColorBrush returnValue = new SolidColorBrush();

            switch (colorName.ToUpper())
            {
                case "BLACK":
                    returnValue = new SolidColorBrush(Colors.Black);
                    break;
                case "BLUE":
                    returnValue = new SolidColorBrush(Colors.Blue);
                    break;
                case "BROWN":
                    returnValue = new SolidColorBrush(Colors.Brown);
                    break;
                case "CYAN":
                    returnValue = new SolidColorBrush(Colors.Cyan);
                    break;
                case "DARK GRAY":
                    returnValue = new SolidColorBrush(Colors.DarkGray);
                    break;
                case "GRAY":
                    returnValue = new SolidColorBrush(Colors.Gray);
                    break;
                case "GREEN":
                    returnValue = new SolidColorBrush(Colors.Green);
                    break;
                case "LIGHT GRAY":
                    returnValue = new SolidColorBrush(Colors.LightGray);
                    break;
                case "MAGENTA":
                    returnValue = new SolidColorBrush(Colors.Magenta);
                    break;
                case "ORANGE":
                    returnValue = new SolidColorBrush(Colors.Orange);
                    break;
                case "PURPLE":
                    returnValue = new SolidColorBrush(Colors.Purple);
                    break;
                case "RED":
                    returnValue = new SolidColorBrush(Colors.Red);
                    break;
                case "WHITE":
                    returnValue = new SolidColorBrush(Colors.White);
                    break;
                case "YELLOW":
                    returnValue = new SolidColorBrush(Colors.Yellow);
                    break;
            }

            return returnValue;
        }
    }
}
