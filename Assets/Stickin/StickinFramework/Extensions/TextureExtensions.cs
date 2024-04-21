using UnityEngine;

namespace stickin
{
    public static class TextureExtensions
    {
        public static Texture2D Clone(this Texture2D oldTexture, float maxWidth = 0, float maxHeight = 0)
        {
            var newTexture = new Texture2D(oldTexture.width, oldTexture.height, oldTexture.format, false);
            Graphics.CopyTexture(oldTexture, newTexture);

//        var bytes = oldTexture.EncodeToJPG();
//        newTexture.LoadImage(bytes);
            newTexture.LoadRawTextureData(oldTexture.GetRawTextureData());
            newTexture.Apply();

            return newTexture;
        }

        public static Texture2D ChangeFormat(this Texture2D oldTexture, TextureFormat newFormat)
        {
//        TextureScale.Bilinear(oldTexture, 500, 500);

            var newTexture = new Texture2D(oldTexture.width, oldTexture.height, newFormat, false);
            newTexture.SetPixels(oldTexture.GetPixels());
//        newTexture.Compress(false);
            newTexture.Apply();

            return newTexture;
        }
    }
}