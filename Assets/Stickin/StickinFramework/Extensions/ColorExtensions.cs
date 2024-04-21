using UnityEngine;

namespace stickin
{
    public static class ColorExtensions
    {
        public static Color32 IntToColor(int aCol)
        {
            Color32 c = new Color32();
            c.b = (byte) ((aCol) & 0xFF);
            c.g = (byte) ((aCol >> 8) & 0xFF);
            c.r = (byte) ((aCol >> 16) & 0xFF);
            c.a = (byte) ((aCol >> 24) & 0xFF);

            return c;
        }
    }
}