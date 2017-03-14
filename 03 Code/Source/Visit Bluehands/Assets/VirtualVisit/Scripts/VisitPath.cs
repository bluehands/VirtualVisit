using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.VirtualVisit.Scripts
{
    public class VisitPath : MonoBehaviour, ButtonListener {

        public ButtonStep ButtonStepPrefab;
        public VisitPoint FromPoint { get; private set; }
        public VisitPoint ToPoint { get; private set; }

        private ButtonStep _buttonStep;
        private VisitPathListener _visitPathListener;

        public void Initialize(VisitPoint formPoint, VisitPoint toPoint, VisitPathListener visitPathListener)
        {
            Init(formPoint, toPoint, visitPathListener);

            SetRotation(formPoint.Position, toPoint.Position);
        }

        public void Initialize(VisitPoint formPoint, VisitPoint toPoint, VisitPathListener visitPathListener, float u, float v)
        {
            Init(formPoint, toPoint, visitPathListener);

            if ((0 <= u && u <= 1) && (0 <= v && v <= 1))
            {
                SetRotation(u, v);
            }
            else
            {
                SetRotation(formPoint.Position, toPoint.Position);
            }
        }

        private void Init(VisitPoint formPoint, VisitPoint toPoint, VisitPathListener visitPathListener)
        {
            _visitPathListener = visitPathListener;
            var canvasNext = GetComponentInChildren<Canvas>();
            var texts = canvasNext.GetComponentsInChildren<Text>();

            FromPoint = formPoint;
            ToPoint = toPoint;

            name = string.Format("VisitPath({0},{1})", formPoint.Id, toPoint.Id);
            transform.parent = formPoint.transform;

            foreach(var text in texts)
            {
                text.text = toPoint.Title;
            }
        
            _buttonStep = Instantiate(ButtonStepPrefab);
            _buttonStep.Initialize(canvasNext.transform, this);
        }

        private void SetRotation(Vector3 fromNodePosition, Vector3 toNodePosition)
        {
            var dir = toNodePosition - fromNodePosition;
            transform.rotation = Quaternion.LookRotation(dir);
        }

        private void SetRotation(float u, float v)
        {
            float xRot = 180 * v - 90;
            float yRot = 360 * u + 184;
            Quaternion rotation = Quaternion.Euler(new Vector3(xRot, yRot, 0));
            transform.rotation = rotation;
        }

        public void DoButtonAction(Type clazz)
        {
            if (clazz == typeof(ButtonStep))
            {
                _visitPathListener.Go(this);
            }
        }
    }
}
