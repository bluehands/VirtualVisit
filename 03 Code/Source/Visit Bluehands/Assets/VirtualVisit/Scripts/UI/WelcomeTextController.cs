using System;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeTextController : MonoBehaviour {

    private Text m_WelcomeTextLeft;
    private Image m_WelcomeBackgroundImage;

    private string m_WelcomeText;
    private int m_WritingPointer;

    private bool m_IsStarted;
    private float m_DeltaTime;
    private float m_LiveTime;
    private float m_BackgroundAlpha;

    void Start () {

        m_WelcomeTextLeft = GameObject.Find("WelcomeText").GetComponent<Text>();
        m_WelcomeBackgroundImage = GameObject.Find("WelcomeBackground Image").GetComponent<Image>();

        restart();
    }
	
	void Update () {
        if(m_IsStarted)
        {
            m_WelcomeTextLeft.text = m_WelcomeText;

            m_LiveTime -= Time.deltaTime;
            m_DeltaTime += Time.deltaTime;

            if (m_DeltaTime >= 0.05)
            {
                m_DeltaTime = 0;
                if (m_LiveTime <= 0)
                {
                    if (m_BackgroundAlpha >= 0)
                    {
                        m_WelcomeBackgroundImage.color = new Color(1, 1, 1, m_BackgroundAlpha);
                        m_WelcomeTextLeft.color = new Color(0, 0, 0, m_BackgroundAlpha);
                        m_BackgroundAlpha -= 0.05f;
                    } else
                    {
                        m_IsStarted = false;
                        m_WelcomeBackgroundImage.color = new Color(1, 1, 1, 0);
                        m_WelcomeTextLeft.color = new Color(0, 0, 0, 0);
                    }
                }
            }
        }
    }

    internal void SetLoading()
    {
        m_WelcomeText = "Loading ...";
        m_WelcomeTextLeft.text = m_WelcomeText;
        m_WelcomeBackgroundImage.color = new Color(1, 1, 1, 1);
        m_WelcomeTextLeft.color = new Color(0, 0, 0, 1);
    }

    public void SetTourTitle(string title)
    {
        m_WelcomeText = "Herzlich Willkommen\n zu " + title + "!";
    }

    private void restart()
    {
        m_IsStarted = true;
        m_DeltaTime = 0f;
        m_LiveTime = 2f;
        m_BackgroundAlpha = 1f;   
    }

    void Awake()
    {
        if(m_WelcomeTextLeft == null || m_WelcomeBackgroundImage == null)
        {
            return;
        }
        restart();
    }
}
