using UnityEngine;

public class TexturesFactory
{
    internal static bool TryToLoadPointTextures(string visitId, string pointId, out Texture textureLeft, out Texture textureRight)
    {
        bool isStereo;
        var leftPath = string.Format("Panoramas\\{0}_{1}_{2}", visitId, pointId, "l");
        var rightPath = string.Format("Panoramas\\{0}_{1}_{2}", visitId, pointId, "r");
        var leftOnlyPath = string.Format("Panoramas\\{0}_{1}", visitId, pointId);

        Debug.Log(string.Format("Try to load texture {0}.", leftPath));
        Debug.Log(string.Format("Try to load texture {0}.", rightPath));
        textureLeft = Resources.Load(leftPath) as Texture;
        textureRight = Resources.Load(rightPath) as Texture;
        if (textureLeft != null && textureRight != null)
        {
            isStereo = true;
        }
        else
        {
            Debug.Log(string.Format("Try to load texture {0}.", leftOnlyPath));
            textureLeft = Resources.Load(leftOnlyPath) as Texture;
            isStereo = false;
        }
        if (textureLeft == null)
        {
            Debug.LogWarning(string.Format("Couln't load panoramas for visit {0} and point {1}.", visitId, pointId));
        }
        return isStereo;
    }

    internal static bool TryToLoadPreviewTexture(string visitId, out Texture preview)
    {
        var path = string.Format("Panoramas\\{0}_Preview", visitId);

        Debug.Log(path);
        preview = Resources.Load(path) as Texture;
        return preview != null;
    }

    internal static bool TryToLoadLogoTexture(string visitId, out Texture logo)
    {
        var path = string.Format("Panoramas\\{0}_Logo", visitId);

        Debug.Log(path);
        logo = Resources.Load(path) as Texture;
        return logo != null;
    }
}

