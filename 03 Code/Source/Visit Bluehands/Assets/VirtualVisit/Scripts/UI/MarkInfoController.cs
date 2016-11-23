using UnityEngine;
using UnityEngine.UI;

public class MarkInfoController : MonoBehaviour {

    private Text m_MarkInfoTextLeft;
    private Text m_MarkInfoTextRight;
    private Text m_MarkInfoTextShadowLeft;
    private Text m_MarkInfoTextShadowRight;

    void Start () {

        m_MarkInfoTextLeft = GameObject.Find("MarkInfo Left").GetComponent<Text>();
        m_MarkInfoTextRight = GameObject.Find("MarkInfo Right").GetComponent<Text>();

        m_MarkInfoTextShadowLeft = GameObject.Find("MarkInfo Left Shadow").GetComponent<Text>();
        m_MarkInfoTextShadowRight = GameObject.Find("MarkInfo Right Shadow").GetComponent<Text>();

        resetMarkInfoText();
    }
	
	void Update () {
	
	}

    void Awake()
    {
        if(m_MarkInfoTextLeft == null || m_MarkInfoTextRight == null || m_MarkInfoTextShadowLeft == null || m_MarkInfoTextShadowRight == null)
        {
            return;
        }
        resetMarkInfoText();
    }

    private void resetMarkInfoText()
    {
        var screenWidth = Screen.width;
        var screenHeight = Screen.height;

        var size = new Vector2(screenWidth / 2 - ((screenWidth / 16) * 2), screenHeight / 2);
        var positionLeft = new Vector3(-(screenWidth / 4), 0, 0);
        var positionShaowLeft = new Vector3(-(screenWidth / 4) - 1, -1, 0);
        var positionRight = new Vector3((screenWidth / 4), 0, 0);
        var positionShadowRight = new Vector3((screenWidth / 4) - 1, -1, 0);
        var fontSize = (int)((30f / 824f) * screenHeight);

        m_MarkInfoTextLeft.GetComponent<RectTransform>().sizeDelta = size;
        m_MarkInfoTextLeft.GetComponent<RectTransform>().anchoredPosition = positionLeft;
        m_MarkInfoTextLeft.GetComponent<Text>().fontSize = fontSize;

        m_MarkInfoTextShadowLeft.GetComponent<RectTransform>().sizeDelta = size;
        m_MarkInfoTextShadowLeft.GetComponent<RectTransform>().anchoredPosition = positionShaowLeft;
        m_MarkInfoTextShadowLeft.GetComponent<Text>().fontSize = fontSize;

        m_MarkInfoTextRight.GetComponent<RectTransform>().sizeDelta = size;
        m_MarkInfoTextRight.GetComponent<RectTransform>().anchoredPosition = positionRight;
        m_MarkInfoTextRight.GetComponent<Text>().fontSize = fontSize;

        m_MarkInfoTextShadowRight.GetComponent<RectTransform>().sizeDelta = size;
        m_MarkInfoTextShadowRight.GetComponent<RectTransform>().anchoredPosition = positionShadowRight;
        m_MarkInfoTextShadowRight.GetComponent<Text>().fontSize = fontSize;
    }
}
