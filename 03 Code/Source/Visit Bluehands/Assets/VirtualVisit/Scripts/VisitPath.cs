using UnityEngine;
using UnityEngine.UI;
using System;

public class VisitPath : MonoBehaviour, ButtonListener {

    public ButtonStep buttonStepPrefab;

    public VisitPoint FromPoint { get; private set; }

    public VisitPoint ToPoint { get; private set; }

    private ButtonStep m_ButtonStep;

    private VisitPathListener m_visitPathListener;

    public void Initialize(VisitPoint formPoint, VisitPoint toPoint, VisitPathListener visitPathListener)
    {
        init(formPoint, toPoint, visitPathListener);

        setRotation(formPoint.Position, toPoint.Position);
    }

    public void Initialize(VisitPoint formPoint, VisitPoint toPoint, VisitPathListener visitPathListener, float u, float v)
    {
        init(formPoint, toPoint, visitPathListener);

        if ((0 <= u && u <= 1) && (0 <= v && v <= 1))
        {
            setRotation(u, v);
        }
        else
        {
            setRotation(formPoint.Position, toPoint.Position);
        }
    }

    private void init(VisitPoint formPoint, VisitPoint toPoint, VisitPathListener visitPathListener)
    {
        m_visitPathListener = visitPathListener;
        var canvasNext = GetComponentInChildren<Canvas>();
        var textNext = canvasNext.GetComponentInChildren<Text>();

        FromPoint = formPoint;
        ToPoint = toPoint;

        name = string.Format("VisitPath({0},{1})", formPoint.Id, toPoint.Id);
        transform.parent = formPoint.transform;

        textNext.text = toPoint.Title;

        m_ButtonStep = Instantiate(buttonStepPrefab) as ButtonStep;
        m_ButtonStep.Initialize(canvasNext.transform, this);
    }

    private void setRotation(Vector3 fromNodePosition, Vector3 toNodePosition)
    {
        var dir = toNodePosition - fromNodePosition;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void setRotation(float u, float v)
    {
        float xRot = 180 * v - 90;
        float yRot = 360 * u + 184;
        Quaternion rotation = Quaternion.Euler(new Vector3(xRot, yRot, 0));
        transform.rotation = rotation;
    }

    public void DoButtonAction(Type clazz)
    {
        if (clazz.Equals(typeof(ButtonStep)))
        {
            m_visitPathListener.Go(this);
        }
    }
}
