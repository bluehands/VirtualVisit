using UnityEngine;
using System.Collections;
using System;

public class VisitSettingsFactory {

    private VisitSettingsStereo[] visitSettings;


    internal VisitSettingsStereo getVisitSetting(string id)
    {
        if (visitSettings == null)
        {
            LoadSettings();
        }
        foreach(var visitSetting in visitSettings)
        {
            if(visitSetting.id.Equals(id))
            {
                return visitSetting;
            }
        }
        Debug.Log("Could not found any visitSetting with id: " + id + ". Use Default.");
        if(visitSettings.Length >= 1)
        {
            return visitSettings[0];
        }
        Debug.Log("Not visitSettings found return null!");
        return null;
    }

    public VisitSettingsStereo[] getVisitSettings()
    {
        if (visitSettings == null)
        {
            LoadSettings();
        }
        return visitSettings;
    }

    public bool tryToLoadTextures(VisitSettingsStereo visitSettings, VisitNodeSettingsStereo visitNodeSettings, out Texture textureLeft, out Texture textureRight)
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

    private void LoadSettings()
    {
        var visitSettingJsons = Resources.LoadAll<TextAsset>("Visits\\");

        visitSettings = new VisitSettingsStereo[visitSettingJsons.Length];

        for(int i=0; i<visitSettingJsons.Length; i++)
        {
            visitSettings[i] = LoadVisitSettings(visitSettingJsons[i]);
        }
    }

    private VisitSettingsStereo LoadVisitSettings(TextAsset visitSettingJson)
    {
        return JsonUtility.FromJson<VisitSettingsStereo>(visitSettingJson.text);
    }
}
