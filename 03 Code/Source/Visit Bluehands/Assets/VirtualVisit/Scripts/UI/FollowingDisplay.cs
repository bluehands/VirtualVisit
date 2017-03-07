using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowingDisplay : FollowingBase, FollowingMenuListener, ButtonListener {

    public FollowingDisplayItem followingDisplayItemPrefab;

    public FollowingDisplayBackItem followingDisplayBackItemPrefab;

    private List<FollowingDisplayItem> m_FollowingDisplayItems;

    private FollowingDisplayBackItem m_FollowingDisplayBackItem;

    private List<FollowingDisplayListener> m_FollowingDisplayListeners;

    public float anglePitch = 15;
    public float angleYaw = 30;

    internal void Initialize(VisitSetting[] visitSettings, SwitchTourListener switchTourListener)
    {
        m_FollowingDisplayItems = new List<FollowingDisplayItem>();
        m_FollowingDisplayListeners = new List<FollowingDisplayListener>();

        m_FollowingDisplayBackItem = Instantiate(followingDisplayBackItemPrefab) as FollowingDisplayBackItem;
        m_FollowingDisplayBackItem.Initialize(transform, 0, 20, this);

        var pitchAngle = 10;
        var yawAngle = 40;
        var offsetPitch = -5;
        var offsetYaw = -5;
        var fullYawAngle = (visitSettings.Length / 2) * yawAngle;
        var startYawAngle = -(fullYawAngle / 2);
        

        for (int i=0; i< visitSettings.Length; i++)
        {
            var yaw = 0f;
            var pitch = 0f;

            if(i%2 == 0)
            {
                yaw = startYawAngle;
                pitch = pitchAngle;

                startYawAngle += yawAngle;
            } else
            {
                yaw = startYawAngle;
                pitch = -pitchAngle;
            }

            var followingDisplayItem = Instantiate(followingDisplayItemPrefab) as FollowingDisplayItem;
            followingDisplayItem.Initialize(transform, visitSettings[i].id, visitSettings[i].title, yaw + offsetYaw, pitch + offsetPitch, switchTourListener);

            m_FollowingDisplayItems.Add(followingDisplayItem);
        }
    }

    void Update () {
        var rotCamEuler = getCamera().transform.rotation.eulerAngles;
        var rotDisEuler = transform.rotation.eulerAngles;

        var distanceX = getDistanceAngle(rotCamEuler.x, rotDisEuler.x);
        var distanceY = getDistanceAngle(rotCamEuler.y, rotDisEuler.y);

        var correctionX = getCorrection(distanceX, anglePitch);
        var correctionY = getCorrection(distanceY, angleYaw);

        var eulerAnlges = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAnlges.x + correctionX, eulerAnlges.y + correctionY, 0);
    }

    public void open()
    {
        gameObject.SetActive(true);
        transform.rotation = getCamera().transform.rotation;

        foreach (var listener in m_FollowingDisplayListeners)
        {
            listener.openDisplay();
        }
    }

    public void close()
    {
        gameObject.SetActive(false);
        foreach (var listener in m_FollowingDisplayListeners)
        {
            listener.closeDisplay();
        }
    }

    public void addListener(FollowingDisplayListener followingDisplayListener)
    {
        m_FollowingDisplayListeners.Add(followingDisplayListener);
    }

    public void openMenu()
    {
        open();
    }

    public void DoButtonAction(Type clazz)
    {
        if (clazz.Equals(typeof(ButtonInfoBack)))
        {
            close();
        }
    }
}
