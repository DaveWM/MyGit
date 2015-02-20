using System.Globalization;
using Windows.UI;

namespace NotificationsBackgroundTask
{
    public static class ColourParser
    {
        public static Color ParseHexString(string hexString)
        {
            var r = hexString.Substring(0, 2);
            var g = hexString.Substring(2, 2);
            var b = hexString.Substring(4, 2);

            return Color.FromArgb(255, 
                byte.Parse(r, NumberStyles.HexNumber), 
                byte.Parse(g, NumberStyles.HexNumber),
                byte.Parse(b, NumberStyles.HexNumber));
        }
    }
}
