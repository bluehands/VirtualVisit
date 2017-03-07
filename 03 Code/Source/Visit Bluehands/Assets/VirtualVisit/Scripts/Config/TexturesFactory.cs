using UnityEngine;
using System;


public class TexturesFactory
{
    internal static bool TryToLoadPointTextures(string visitId, string pointId, out Texture textureLeft, out Texture textureRight)
    {
        bool isStereo;
        textureLeft = Resources.Load(String.Format("Panoramas\\{0}_{1}_{2}", visitId, pointId, "l")) as Texture;
        textureRight = Resources.Load(String.Format("Panoramas\\{0}_{1}_{2}", visitId, pointId, "r")) as Texture;
        if (textureLeft != null && textureRight != null)
        {
            isStereo = true;
        }
        else
        {
            textureLeft = Resources.Load(String.Format("Panoramas\\{0}_{1}", visitId, pointId)) as Texture;
            isStereo = false;
        }
        if (textureLeft == null)
        {
            Debug.LogWarning(String.Format("Couln't load Panoramas\\{0}_{1} (l/r)", visitId, pointId));
        }
        return isStereo;
    }

    internal static bool TryToLoadPreviewTexture(string visitId, out Texture preview)
    {
        preview = Resources.Load(String.Format("Panoramas\\{0}_Preview", visitId)) as Texture;
        return preview != null;
    }
}

