using UnityEngine;
using System;


public class FollowingMenu : FollowingBase, ButtonListener, FollowingDisplayListener {

    public ButtonFriends friendsMenuPrefab;

    private ButtonFriends m_FriendsButton;

    private FollowingMenuListener m_FollowingMenuListener;

    public float angleYaw = 30;

    public void Initialize(FollowingMenuListener followingMenuListener)
    {
        m_FollowingMenuListener = followingMenuListener;

        m_FriendsButton = Instantiate(friendsMenuPrefab) as ButtonFriends;
        m_FriendsButton.Initialize(GameObject.Find("Canvas Friends").transform, this);
    }

    void Update()
    {
        var rotCamEuler = getCamera().transform.rotation.eulerAngles;
        var rotDisEuler = transform.rotation.eulerAngles;

        var distanceY = getDistanceAngle(rotCamEuler.y, rotDisEuler.y);

        var correctionY = getCorrection(distanceY, angleYaw);

        var eulerAnlges = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, eulerAnlges.y + correctionY, 0);
    }

    public void DoButtonAction(Type clazz)
    {
        if (clazz.Equals(typeof(ButtonFriends)))
        {
            m_FollowingMenuListener.openMenu();
        }
    }

    public void openDisplay()
    {
        gameObject.SetActive(false);
    }

    public void closeDisplay()
    {
        transform.rotation = getCamera().transform.rotation;
        gameObject.SetActive(true);
    }
}
