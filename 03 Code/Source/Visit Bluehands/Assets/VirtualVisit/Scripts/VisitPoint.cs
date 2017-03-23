using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.VirtualVisit.Scripts
{
    public class VisitPoint : MonoBehaviour {

        enum PointState
        {
            None,
            Visible,
            ChangePoint,
            FadedOut,
            FadingIn,
            FadingOut
        }

        // ReSharper disable once InconsistentNaming
        private const int LEFT_EYE = 0;
        // ReSharper disable once InconsistentNaming
        private const int RIGHT_EYE = 1;

        // ReSharper disable once InconsistentNaming
        private const string MAIN_TEXTURE = "_MainTex";
        // ReSharper disable once InconsistentNaming
        private const string BLEND_TEXTURE = "_BlendTex";
        // ReSharper disable once InconsistentNaming
        private const string BLEND_ALPHA = "_BlendAlpha";

        public string VisitId { get; private set; }

        public string Id { get; private set; }

        public string Title { get; private set; }

        public Vector3 Position { get; private set; }

        public Texture SphereTextureLeft { get; private set; }

        public Texture SphereTextureRight { get; private set; }

        public Texture FadeOutTexture;

        public bool IsStereo { get; private set; }

        public VisitPath PathPrefab;

        public VisitMark MarkPrefab;

        private Texture _fadeOutTextureLeft;
        private Texture _fadeOutTextureRight;

        private List<VisitPath> _paths;

        private List<VisitMark> _marks;

        private float _blendAlpha = 1;

        private PointState _pointState = PointState.None;

        private bool _isLoaded;

        public enum BlendMode
        {
            Opaque,
            Cutout,
            Fade,
            Transparent
        }

        private Transform GetLeftSphere()
        {
            return transform.GetChild(LEFT_EYE);
        }

        private Transform GetRightSphere()
        {
            return transform.GetChild(RIGHT_EYE);
        }

        public void Initialize(VisitSetting visitSettings, VisitNodeSetting nodeSetting, Transform parent)
        {
            VisitId = visitSettings.id;
            Id = nodeSetting.id;
            Title = nodeSetting.title;
            Position = nodeSetting.position;

            transform.parent = parent;
            name = string.Format("VisitPoint({0})", Id);

            _paths = new List<VisitPath>();
            _marks = new List<VisitMark>();

            LoadTextures();
        }

        public void Generate(VisitNodeSetting visitNodeSetting, VisitPointDictionary visitPointDictionary, VisitPathListener visitPathListener)
        {
            GeneratePaths(visitNodeSetting, visitPointDictionary, visitPathListener);

            GenerateMarks(visitNodeSetting, visitPointDictionary);

            Leave();
        }


        private void GeneratePaths(VisitNodeSetting visitNodeSetting, VisitPointDictionary visitPointDictionary, VisitPathListener visitPathListener)
        {
            var fromPoint = visitPointDictionary.LockUp(Id);

            foreach (var edgeId in visitNodeSetting.getEdgeIds())
            {
                var toPoint = visitPointDictionary.LockUp(edgeId);
                fromPoint.CreateEdge(toPoint, visitPathListener);
            }
            if (visitNodeSetting.edgeSettings != null)
            {
                foreach (var edgeSetting in visitNodeSetting.edgeSettings)
                {
                    var toPoint = visitPointDictionary.LockUp(edgeSetting.toId);
                    fromPoint.CreateEdge(toPoint, visitPathListener, edgeSetting.u, edgeSetting.v);
                }
            }
        }

        private void GenerateMarks(VisitNodeSetting visitNodeSetting, VisitPointDictionary visitPointDictionary)
        {
            var fromPoint = visitPointDictionary.LockUp(Id);

            if (visitNodeSetting.markSettings != null)
            {
                foreach (var visitMarkSetting in visitNodeSetting.markSettings)
                {
                    fromPoint.CreateMark(visitMarkSetting.title, visitMarkSetting.description, visitMarkSetting.u, visitMarkSetting.v);
                }
            }
        }

        private void CheckIfTexturesLoaded()
        {
            if (!_isLoaded)
            {
                LoadTextures();
            }
        }

        private void LoadTextures()
        {
            Texture textureLeft;
            Texture textureRight;

            IsStereo = TexturesFactory.TryToLoadPointTextures(VisitId, Id, out textureLeft, out textureRight);

            SphereTextureLeft = textureLeft;
            SphereTextureRight = textureRight;

            InitSphere(GetLeftSphere(), SphereTextureLeft, LayerMask.NameToLayer("Left Eye"));
        
            if (IsStereo)
            {
                InitSphere(GetRightSphere(), SphereTextureRight, LayerMask.NameToLayer("Right Eye"));
            }
            else
            {
                GetRightSphere().GetComponent<Renderer>().enabled = false;
            }

            _isLoaded = true;
        }

        private void CreateEdge(VisitPoint toPoint, VisitPathListener visitPathListener)
        {
            var path = Instantiate(PathPrefab);

            path.Initialize(this, toPoint, visitPathListener);

            _paths.Add(path);
        }

        private void CreateEdge(VisitPoint toPoint, VisitPathListener visitPathListener, float u, float v)
        {
            var path = Instantiate(PathPrefab);

            path.Initialize(this, toPoint, visitPathListener, u, v);

            _paths.Add(path);
        }

        private void CreateMark(string title, string description, float u, float v)
        {
            VisitMark mark = Instantiate(MarkPrefab);

            mark.Initialize(title, description, u, v, transform);

            _marks.Add(mark);
        }

        private void InitSphere(Transform sphere, Texture texture, int layerMask)
        {
            Renderer ren = sphere.GetComponent<Renderer>();
            ren.enabled = true;
            ren.material.SetTextureScale(MAIN_TEXTURE, new Vector2(-1, 1));
            ren.material.mainTexture = texture;
            sphere.gameObject.layer = layerMask;
        }

        internal bool IsStereoView()
        {
            return GetLeftSphere().gameObject.layer == LayerMask.NameToLayer("Left Eye");
        }

        internal bool ToggleStereoView()
        {
            if (IsStereo)
            {
                if (GetLeftSphere().gameObject.layer == LayerMask.NameToLayer("Left Eye"))
                {
                    GetLeftSphere().gameObject.layer = LayerMask.NameToLayer("Default");
                    GetRightSphere().gameObject.SetActive(false);
                    return false;
                }
                else
                {
                    GetLeftSphere().gameObject.layer = LayerMask.NameToLayer("Left Eye");
                    GetRightSphere().gameObject.SetActive(true);
                    return true;
                }
            }
            return false;
        }

        public void AddPath(VisitPath path)
        {
            _paths.Add(path);
        }

        public List<VisitPath> GetPaths()
        {
            return _paths;
        }

        public void SetEdgesActive(bool active)
        {
            foreach (var edge in _paths)
            {
                edge.gameObject.SetActive(active);
            }
        }

        public void SetMarksActive(bool active)
        {
            foreach (var mark in _marks)
            {
                mark.gameObject.SetActive(active);
            }
        }

        internal void Leave()
        {
            gameObject.SetActive(false);
        }

        internal void GoThere()
        {
            CheckIfTexturesLoaded();

            SetAlpha(0.0f);

            gameObject.SetActive(true);
        }

        internal void GoThereFrom(VisitPoint point)
        {
            CheckIfTexturesLoaded();

            SetAlpha(1.0f);

            _blendAlpha = 1;
            _fadeOutTextureLeft = point.SphereTextureLeft;
            _fadeOutTextureRight = IsStereo && point.IsStereo ? point.SphereTextureRight : point.SphereTextureLeft;

            SetEdgesActive(false);
            gameObject.SetActive(true);
            _pointState = PointState.ChangePoint;
        }

        private void SetAlpha(float alpha)
        {
            Renderer rendLeft = GetLeftSphere().GetComponent<Renderer>();
            Renderer rendRight = GetRightSphere().GetComponent<Renderer>();

            rendLeft.material.SetFloat(BLEND_ALPHA, alpha);
            rendRight.material.SetFloat(BLEND_ALPHA, alpha);
        }

        public void FadeOut()
        {
            _blendAlpha = 1;

            _pointState = PointState.FadingOut;
        }

        public void FadeIn()
        {
            Renderer rendLeft = GetLeftSphere().GetComponent<Renderer>();
            Renderer rendRight = GetRightSphere().GetComponent<Renderer>();

            rendLeft.material.SetTexture(BLEND_TEXTURE, FadeOutTexture);
            rendRight.material.SetTexture(BLEND_TEXTURE, FadeOutTexture);

            SetAlpha(0);
        }

        public void Update()
        {
            Renderer rendLeft = GetLeftSphere().GetComponent<Renderer>();
            Renderer rendRight = GetRightSphere().GetComponent<Renderer>();
            if (_pointState == PointState.ChangePoint)
            {
                if (_blendAlpha == 1)
                {
                    rendLeft.material.SetTexture(BLEND_TEXTURE, _fadeOutTextureLeft);
                    rendRight.material.SetTexture(BLEND_TEXTURE, _fadeOutTextureRight);
                }
                if (_blendAlpha >= 0)
                {
                    SetAlpha(_blendAlpha);
                }
                if (_blendAlpha <= 0)
                {
                    SetAlpha(0);
                    _fadeOutTextureLeft = null;
                    _fadeOutTextureRight = null;
                    SetEdgesActive(true);
                    _pointState = PointState.Visible;
                }
                _blendAlpha = _blendAlpha - 0.1f;
            } else if(_pointState == PointState.FadingOut)
            {
                if (_blendAlpha == 1)
                {
                    rendLeft.material.SetTexture(BLEND_TEXTURE, FadeOutTexture);
                    rendRight.material.SetTexture(BLEND_TEXTURE, FadeOutTexture);
                }
                if (_blendAlpha >= 0)
                {
                    SetAlpha(0.5f - (_blendAlpha/2));
                }
                if (_blendAlpha <= 0)
                {
                    SetAlpha(0.5f);
                    _pointState = PointState.FadedOut;
                }
                _blendAlpha = _blendAlpha - 0.1f;
            } else if (_pointState == PointState.FadingIn)
            {
                if (_blendAlpha == 1)
                {
                    rendLeft.material.SetTexture(BLEND_TEXTURE, FadeOutTexture);
                    rendRight.material.SetTexture(BLEND_TEXTURE, FadeOutTexture);
                }
                if (_blendAlpha >= 0)
                {
                    SetAlpha(_blendAlpha);
                }
                if (_blendAlpha <= 0)
                {
                    SetAlpha(0);
                    _pointState = PointState.Visible;
                }
                _blendAlpha = _blendAlpha - 0.1f;
            }
        }

    }
}
