using UnityEngine;
using System.Collections;
using System;

public class VisitSettingsFactory {

    private VisitSetting[] m_VisitSettings;

    internal VisitSetting GetVisitSetting(string id)
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

    internal VisitSetting[] GetVisitSettings()
    {
        if (m_VisitSettings == null)
        {
            loadSettings();
        }
        return m_VisitSettings;
    }

    internal bool TryToLoadTextures(VisitSetting visitSettings, VisitNodeSetting visitNodeSettings, out Texture textureLeft, out Texture textureRight)
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
        if(textureLeft == null)
        {
            Debug.LogWarning(String.Format("Couln't load Panoramas\\{0}_{1} (l/r)", visitSettings.id, visitNodeSettings.id));
        }
        return isStereo;
    }

    private void loadSettings()
    {
        Debug.Log("Try to load visit settings!");
        var visitSettingJsons = Resources.LoadAll<TextAsset>("Visits\\");

        m_VisitSettings = new VisitSetting[visitSettingJsons.Length];

        if (visitSettingJsons.Length == 0)
        {
            Debug.Log("Couln't load any visit settings!");
            var visitSetting = Resources.Load("Visits\\bluehands") as TextAsset;
            if (visitSetting != null)
            {
                m_VisitSettings = new VisitSetting[1];
                m_VisitSettings[0] = loadVisitSettings(visitSetting);
            }
            else
            {
                Debug.Log("Couln't load one visit setting!");
            }
        }

        for(int i=0; i<visitSettingJsons.Length; i++)
        {
            m_VisitSettings[i] = loadVisitSettings(visitSettingJsons[i]);
        }
    }

    private VisitSetting loadVisitSettings(TextAsset visitSettingJson)
    {
        VisitSetting visitSetting = null;
        try
        {
            visitSetting = JsonUtility.FromJson<VisitSetting>(visitSettingJson.text);
        } catch(Exception e)
        {
            Debug.LogError("Text File " + visitSettingJson.name + " have an error! Message: " + e.Message + " Inner Excapion: " + e.InnerException);
        }
        return visitSetting;
    }
}
