using UnityEngine;
using System.Collections;
using System;

public class VisitSettingsFactory {

    private VisitSettings[] m_VisitSettings;

    internal VisitSettings GetVisitSetting(string id)
    {
        if (m_VisitSettings == null)
        {
            loadSettings();
        }
        foreach(var visitSetting in m_VisitSettings)
        {
            if(visitSetting.id.Equals(id))
            {
                return visitSetting;
            }
        }
        Debug.Log("Could not found any visitSetting with id: " + id + ". Use Default.");
        if(m_VisitSettings.Length >= 1)
        {
            return m_VisitSettings[0];
        }
        Debug.Log("Not visitSettings found return null!");
        return null;
    }

    internal VisitSettings[] GetVisitSettings()
    {
        if (m_VisitSettings == null)
        {
            loadSettings();
        }
        return m_VisitSettings;
    }

    internal bool TryToLoadTextures(VisitSettings visitSettings, VisitNodeSettings visitNodeSettings, out Texture textureLeft, out Texture textureRight)
    {
        bool isStereo;
        textureLeft = Resources.Load(String.Format("Panoramas\\{0}_{1}_{2}", visitSettings.id, visitNodeSettings.id, "l")) as Texture;
        textureRight = Resources.Load(String.Format("Panoramas\\{0}_{1}_{2}", visitSettings.id, visitNodeSettings.id, "r")) as Texture;
        if (textureLeft != null && textureRight != null)
        {
            isStereo = true;
        }
        else
        {
            textureLeft = Resources.Load(String.Format("Panoramas\\{0}_{1}", visitSettings.id, visitNodeSettings.id)) as Texture;
            isStereo = false;
        }
        return isStereo;
    }

    private void loadSettings()
    {
        var visitSettingJsons = Resources.LoadAll<TextAsset>("Visits\\");

        m_VisitSettings = new VisitSettings[visitSettingJsons.Length];

        for(int i=0; i<visitSettingJsons.Length; i++)
        {
            m_VisitSettings[i] = loadVisitSettings(visitSettingJsons[i]);
        }
    }

    private VisitSettings loadVisitSettings(TextAsset visitSettingJson)
    {
        return JsonUtility.FromJson<VisitSettings>(visitSettingJson.text);
    }
}
