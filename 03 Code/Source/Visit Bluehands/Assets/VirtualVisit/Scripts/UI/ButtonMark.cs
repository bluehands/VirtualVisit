using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonMark : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string m_Title;
    private string m_Decription;

    private Text m_MarkInfoTextLeft;
    private Text m_MarkInfoTextRight;
    private Text m_MarkInfoTextShadowLeft;
    private Text m_MarkInfoTextShadowRight;

    private string m_InfoText;
    private int m_WritingPointer;

    private bool m_IsSelected;
    private float m_DeltaTime;
    private bool m_ShowCursor;


    public void Initialize(string title, string description, Transform parent)
    {
        transform.SetParent(parent, false);
        m_Title = title;
        m_Decription = description;

        m_InfoText = m_Title + "\n\n" + m_Decription;

        m_MarkInfoTextLeft = GameObject.Find("MarkInfo Left").GetComponent<Text>();
        m_MarkInfoTextRight = GameObject.Find("MarkInfo Right").GetComponent<Text>();

        m_MarkInfoTextShadowLeft = GameObject.Find("MarkInfo Left Shadow").GetComponent<Text>();
        m_MarkInfoTextShadowRight = GameObject.Find("MarkInfo Right Shadow").GetComponent<Text>();

        transform.Translate(new Vector3(2.5f, 2.5f, 0));
        transform.localScale = new Vector3(0.01f, 0.01f, 1);

        //transform.FindChild("Title Text").GetComponent<Text>().text = title;
        //transform.FindChild("Context Text").GetComponent<Text>().text = description;
        //transform.FindChild("Context Text Shadow").GetComponent<Text>().text = description;

        
    }

    void Update()
    {
        m_DeltaTime += Time.deltaTime;
        if (m_DeltaTime >= 0.05)
        {
            m_DeltaTime = 0;
            if (m_IsSelected)
            {
                if(m_WritingPointer < m_InfoText.Length)
                {
                    m_WritingPointer++;
                }
                

                string currentText = "";
                for (int i = 0; i < m_WritingPointer; i++)
                {
                    currentText += m_InfoText[i];
                }
                if(m_ShowCursor)
                {
                    currentText += "|";
                }
                m_ShowCursor = !m_ShowCursor;

                setText(currentText);
            } else
            {
                m_WritingPointer = 0;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_IsSelected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_IsSelected = false;
        setText("");
    }

    private void setText(string text)
    {
        m_MarkInfoTextLeft.text = text;
        m_MarkInfoTextRight.text = text;
        m_MarkInfoTextShadowLeft.text = text;
        m_MarkInfoTextShadowRight.text = text;
    }
}
