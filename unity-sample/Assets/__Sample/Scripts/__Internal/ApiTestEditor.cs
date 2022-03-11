using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;
using System.Linq;

namespace Sundaytoz
{
    public static class RectTransformExtension
    {
        ////////////////////////////////////////////////////////////////////////////////////////////
        /// Enums
        ////////////////////////////////////////////////////////////////////////////////////////////
        
        public enum EAnchorTypeX
        {
            LEFT,
            CENTER,
            RIGHT,
            STRETCH
        }
        
        public enum EAnchorTypeY
        {
            TOP,
            MIDDLE,
            BOTTOM,
            STRETCH
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////
        /// Functions
        ////////////////////////////////////////////////////////////////////////////////////////////
    
        public static RectTransform SetAnchorTypeX(this RectTransform inRectTransform, EAnchorTypeX inAnchorTypeX)
        {
            switch (inAnchorTypeX)
            {
                case EAnchorTypeX.LEFT:
                    inRectTransform.SetAnchorMinX(0).SetAnchorMaxX(0);
                    break;
                
                case EAnchorTypeX.CENTER:
                    inRectTransform.SetAnchorMinX(0.5f).SetAnchorMaxX(0.5f);
                    break;
                
                case EAnchorTypeX.RIGHT:
                    inRectTransform.SetAnchorMinX(1).SetAnchorMaxX(1);
                    break;
                
                case EAnchorTypeX.STRETCH:
                    inRectTransform.SetAnchorMinX(0).SetAnchorMaxX(1);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(inAnchorTypeX), inAnchorTypeX, null);
            }
    
            return inRectTransform;
        }
        
        public static RectTransform SetAnchorTypeY(this RectTransform inRectTransform, EAnchorTypeY inAnchorTypeY)
        {
            switch (inAnchorTypeY)
            {
                case EAnchorTypeY.TOP:
                    inRectTransform.SetAnchorMinY(1).SetAnchorMaxY(1);
                    break;
                case EAnchorTypeY.MIDDLE:
                    inRectTransform.SetAnchorMinY(0.5f).SetAnchorMaxY(0.5f);
                    break;
                case EAnchorTypeY.BOTTOM:
                    inRectTransform.SetAnchorMinY(0).SetAnchorMaxY(0);
                    break;
                case EAnchorTypeY.STRETCH:
                    inRectTransform.SetAnchorMinY(0).SetAnchorMaxY(1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inAnchorTypeY), inAnchorTypeY, null);
            }
    
            return inRectTransform;
        }
        
        public static RectTransform SetSizeDeltaWidth(this RectTransform inRectTransform, float inWidth)
        {
            inRectTransform.sizeDelta = new Vector2(inWidth, inRectTransform.sizeDelta.y);
            return inRectTransform;
        }
    
        public static RectTransform SetSizeDeltaHeight(this RectTransform inRectTransform, float inHeight)
        {
            inRectTransform.sizeDelta = new Vector2(inRectTransform.sizeDelta.x, inHeight);
            return inRectTransform;
        }
        
        public static RectTransform SetAnchorMinX(this RectTransform inRectTransform, float inAnchorMinX)
        {
            inRectTransform.anchorMin = new Vector2(inAnchorMinX, inRectTransform.anchorMin.y);
            return inRectTransform;
        }
    
        public static RectTransform SetAnchorMinY(this RectTransform inRectTransform, float inAnchorMinY)
        {
            inRectTransform.anchorMin = new Vector2(inRectTransform.anchorMin.x, inAnchorMinY);
            return inRectTransform;
        }
        
        public static RectTransform SetAnchorMaxX(this RectTransform inRectTransform, float inAnchorMaxX)
        {
            inRectTransform.anchorMax = new Vector2(inAnchorMaxX, inRectTransform.anchorMax.y);
            return inRectTransform;
        }
    
        public static RectTransform SetAnchorMaxY(this RectTransform inRectTransform, float inAnchorMaxY)
        {
            inRectTransform.anchorMax = new Vector2(inRectTransform.anchorMax.x, inAnchorMaxY);
            return inRectTransform;
        }
        
        public static RectTransform SetPivotX(this RectTransform inRectTransform, float inPivotX)
        {
            inRectTransform.pivot = new Vector2(inPivotX, inRectTransform.pivot.y);
            return inRectTransform;
        }
        
        public static RectTransform SetPivotY(this RectTransform inRectTransform, float inPivotY)
        {
            inRectTransform.pivot = new Vector2(inRectTransform.pivot.x, inPivotY);
            return inRectTransform;
        }
    
        public static RectTransform SetOffsetLeft(this RectTransform inRectTransform, float inOffsetLeft)
        {
            inRectTransform.offsetMin = new Vector2(inOffsetLeft, inRectTransform.offsetMin.y);
            return inRectTransform;
        }
        
        public static RectTransform SetOffsetBottom(this RectTransform inRectTransform, float inOffsetBottom)
        {
            inRectTransform.offsetMin = new Vector2(inRectTransform.offsetMin.x, inOffsetBottom);
            return inRectTransform;
        }
        
        public static RectTransform SetOffsetRight(this RectTransform inRectTransform, float inOffsetRight)
        {
            inRectTransform.offsetMax = new Vector2(-inOffsetRight, inRectTransform.offsetMax.y);
            return inRectTransform;
        }
        
        public static RectTransform SetOffsetTop(this RectTransform inRectTransform, float inOffsetTop)
        {
            inRectTransform.offsetMax = new Vector2(inRectTransform.offsetMax.x, -inOffsetTop);
            return inRectTransform;
        }
        
        public static RectTransform SetAnchoredPositionX(this RectTransform inRectTransform, float inAnchoredPositionX)
        {
            var pos = inRectTransform.anchoredPosition;
            pos.x = inAnchoredPositionX;
            inRectTransform.anchoredPosition = pos;
            return inRectTransform;
        }
        
        public static RectTransform SetAnchoredPositionY(this RectTransform inRectTransform, float inAnchoredPositionY)
        {
            var pos = inRectTransform.anchoredPosition;
            pos.y = inAnchoredPositionY;
            inRectTransform.anchoredPosition = pos;
            return inRectTransform;
        }
    }
    
    public sealed class ApiTestEditor
    {
        #region Initialize
        
        ////////////////////////////////////////////////////////////////////////////////////////////
        /// Enums
        ////////////////////////////////////////////////////////////////////////////////////////////
        
        private enum EScrollType
        {
            VERTICAL,
            HORIZONTAL
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////
        /// Variables
        ////////////////////////////////////////////////////////////////////////////////////////////
    
        private static ApiTestEditor _instance;
        
        private ScrollRect _tabScroll;
        private ScrollRect _inputScroll;
        private ScrollRect _buttonScroll;
        private ScrollRect _outputScroll;
        private Text _outputText;
        
        ////////////////////////////////////////////////////////////////////////////////////////////
        /// Functions
        ////////////////////////////////////////////////////////////////////////////////////////////
        
        private ApiTestEditor()
        {
            // gameobject
            var go = new GameObject("Canvas");
            
            // canvas
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    
            // canvas scaler
            var canvasScaler = go.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(640, 960);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            canvasScaler.referencePixelsPerUnit = 1;
            
            // garphic raycaster
            var graphicRaycaster = go.AddComponent<GraphicRaycaster>();
            
            // event system
            var eventSystem = go.AddComponent<EventSystem>();
            
            // stand alone input module
            var standAloneInputModule = go.AddComponent<StandaloneInputModule>();
    
            _tabScroll = CreateScrollRect(EScrollType.HORIZONTAL);
            _tabScroll.transform.SetParent(go.transform, true);
            _tabScroll.GetComponent<RectTransform>()
                .SetPivotX(0)
                .SetPivotY(1)
                .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.TOP)
                .SetAnchoredPositionX(0)
                .SetAnchoredPositionY(0)
                .SetOffsetLeft(0)
                .SetOffsetRight(0)
                .SetSizeDeltaHeight(100);

            var tabScrollGroup = _tabScroll.content.gameObject.AddComponent<HorizontalLayoutGroup>();
            tabScrollGroup.padding = new RectOffset(5, 5, 5, 5);
            tabScrollGroup.spacing = 5;
            tabScrollGroup.childAlignment = TextAnchor.UpperLeft;
            
            _inputScroll = CreateScrollRect(EScrollType.VERTICAL);
            _inputScroll.transform.SetParent(go.transform, true);
            _inputScroll.GetComponent<RectTransform>()
                .SetPivotX(0)
                .SetPivotY(1)
                .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.TOP)
                .SetAnchoredPositionX(0)
                .SetAnchoredPositionY(-110)
                .SetOffsetLeft(0)
                .SetOffsetRight(0)
                .SetSizeDeltaHeight(200);
            
            var inputScrollGroup = _inputScroll.content.gameObject.AddComponent<VerticalLayoutGroup>();
            inputScrollGroup.padding = new RectOffset(5, 5, 5, 5);
            inputScrollGroup.spacing = 5;
            inputScrollGroup.childAlignment = TextAnchor.UpperLeft;
            
            _buttonScroll = CreateScrollRect(EScrollType.VERTICAL);
            _buttonScroll.transform.SetParent(go.transform, true);
            _buttonScroll.GetComponent<RectTransform>()
                .SetPivotX(0)
                .SetPivotY(1)
                .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.TOP)
                .SetAnchoredPositionX(0)
                .SetAnchoredPositionY(-320)
                .SetOffsetLeft(0)
                .SetOffsetRight(0)
                .SetSizeDeltaHeight(200);
            
            var buttonScrollGroup = _buttonScroll.content.gameObject.AddComponent<VerticalLayoutGroup>();
            buttonScrollGroup.padding = new RectOffset(5, 5, 5, 5);
            buttonScrollGroup.spacing = 5;
            buttonScrollGroup.childAlignment = TextAnchor.UpperLeft;
            
            _outputScroll = CreateScrollRect(EScrollType.VERTICAL);
            _outputScroll.transform.SetParent(go.transform, true);
            _outputScroll.GetComponent<RectTransform>()
                .SetPivotX(0)
                .SetPivotY(1)
                .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.STRETCH)
                .SetAnchoredPositionX(0)
                .SetAnchoredPositionY(0)
                .SetOffsetLeft(0)
                .SetOffsetRight(0)
                .SetOffsetTop(530)
                .SetOffsetBottom(0);

            _outputText = (new GameObject("Text")).AddComponent<Text>();
            _outputText.transform.SetParent(_outputScroll.content.transform, true);
            _outputText.gameObject.GetComponent<RectTransform>()
                .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.STRETCH)
                .SetOffsetTop(5)
                .SetOffsetBottom(5)
                .SetOffsetLeft(5)
                .SetOffsetRight(5);
            
            _outputText.transform.localScale = Vector3.one;
            _outputText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            _outputText.fontSize = 30;
            _outputText.alignByGeometry = true;
            _outputText.color = Color.black;
            _outputText.resizeTextForBestFit = false;
            _outputText.verticalOverflow = VerticalWrapMode.Overflow;
            _outputText.alignment = TextAnchor.UpperLeft;
        }
        
        private ScrollRect CreateScrollRect(EScrollType inScrollType)
        {
            // ScrollView
            var go = new GameObject("ScrollView");
            go.transform.localPosition = Vector3.zero;
            
            var scrollView = go.AddComponent<ScrollRect>();
            
            var bg = go.AddComponent<Image>();
            bg.color = new Color(1, 1, 1, 1);
    
            // Viewport
            var viewport = new GameObject("Viewport");
            viewport.transform.SetParent(go.transform, true);
            viewport.transform.localPosition = Vector3.zero;
    
            var viewportRT = viewport.AddComponent<RectTransform>();
            viewportRT.SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.STRETCH)
                .SetPivotX(0)
                .SetPivotY(1)
                .SetOffsetLeft(0)
                .SetOffsetRight(0)
                .SetOffsetTop(0)
                .SetOffsetBottom(0);
            
            scrollView.viewport = viewportRT;
    
            var viewPortMask = viewport.AddComponent<RectMask2D>();
            var viewPortBG = viewport.AddComponent<Image>();
    
            // Content
            var content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, true);
            
            var contentRT = content.AddComponent<RectTransform>();
            scrollView.content = contentRT;
    
            switch (inScrollType)
            {
                case EScrollType.VERTICAL:
                    scrollView.horizontal = false;
                    scrollView.vertical = true;

                    viewportRT.SetOffsetRight(10);

                    contentRT.SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                        .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.TOP)
                        .SetPivotX(0)
                        .SetPivotY(1)
                        .SetAnchoredPositionX(0)
                        .SetAnchoredPositionY(0)
                        .SetOffsetLeft(0)
                        .SetOffsetRight(0)
                        .SetSizeDeltaHeight(viewportRT.rect.height);
                    
                    scrollView.verticalScrollbar = CreateScrollbar(inScrollType, scrollView);
                    break;
                
                case EScrollType.HORIZONTAL:
                    scrollView.horizontal = true;
                    scrollView.vertical = false;

                    viewportRT.SetOffsetBottom(10);
                    
                    contentRT.SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.LEFT)
                        .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.STRETCH)
                        .SetPivotX(0)
                        .SetPivotY(0)
                        .SetAnchoredPositionX(0)
                        .SetAnchoredPositionY(0)
                        .SetOffsetTop(0)
                        .SetOffsetBottom(0)
                        .SetSizeDeltaWidth(viewportRT.rect.width);
                    
                    scrollView.horizontalScrollbar = CreateScrollbar(inScrollType, scrollView);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(inScrollType), inScrollType, null);
            }
            
            return scrollView;
        }
    
        private Scrollbar CreateScrollbar(EScrollType inScrollType, ScrollRect inScrollRect)
        {
            // scrollbar
            var go = new GameObject("Scrollbar");
            go.transform.SetParent(inScrollRect.transform, true);
    
            var bg = go.AddComponent<Image>();
            bg.type = Image.Type.Sliced;
    
            var scrollbar = go.AddComponent<Scrollbar>();
    
            var scrollbarRT = go.GetComponent<RectTransform>();

            var scrollbarImage = go.GetComponent<Image>();
            scrollbarImage.color = new Color(0, 0, 0, 0.8f);
    
            // sliding area
            var slidingArea = new GameObject("SlidingArea");
            slidingArea.transform.SetParent(go.transform, true);
            
            slidingArea.AddComponent<RectTransform>().SetOffsetTop(0)
                .SetOffsetBottom(0)
                .SetOffsetLeft(0)
                .SetOffsetRight(0)
                .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.STRETCH)
                .SetPivotX(0.5f)
                .SetPivotY(0.5f);
            
            // handle
            var handle = new GameObject("Handle");
            handle.transform.SetParent(slidingArea.transform, true);
    
            var handleRT = handle.AddComponent<RectTransform>();
            handleRT.SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.STRETCH)
                .SetOffsetTop(0)
                .SetOffsetBottom(0)
                .SetOffsetLeft(0)
                .SetOffsetRight(0);
    
            var handleBG = handle.AddComponent<Image>();
            handleBG.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
    
            scrollbar.targetGraphic = handleBG;
            scrollbar.handleRect = handleRT;
    
            switch (inScrollType)
            {
                case EScrollType.VERTICAL:
                    scrollbar.direction = Scrollbar.Direction.BottomToTop;
                    scrollbarRT
                        .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.RIGHT)
                        .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.STRETCH)
                        .SetPivotX(1)
                        .SetPivotY(1)
                        .SetAnchoredPositionX(0)
                        .SetOffsetTop(0)
                        .SetOffsetBottom(0)
                        .SetSizeDeltaWidth(10);
                    break;
                
                case EScrollType.HORIZONTAL:
                    scrollbar.direction = Scrollbar.Direction.LeftToRight;
                    scrollbarRT
                        .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                        .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.BOTTOM)
                        .SetPivotX(0)
                        .SetPivotY(0)
                        .SetAnchoredPositionY(0)
                        .SetOffsetLeft(0)
                        .SetOffsetRight(0)
                        .SetSizeDeltaHeight(10);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(inScrollType), inScrollType, null);
            }
    
            return scrollbar;
        }
    
        #endregion

        #region Tab

        ////////////////////////////////////////////////////////////////////////////////////////////
        /// Classes
        ////////////////////////////////////////////////////////////////////////////////////////////

        private class ParamData
        {
            public string Id { get; }
            public string Param { get; }
            public string Description { get; }

            public ParamData(string inId, string inParam, string inDescription)
            {
                Id = inId;
                Param = inParam;
                Description = inDescription;
            }
        }

        private class ActionData
        {
            public string Id { get; }
            public Action Action { get; }

            public ActionData(string inId, Action inAction)
            {
                Id = inId;
                Action = inAction;
            }
        }
        
        private class TabData
        {
            ////////////////////////////////////////////////////////////////////////////////////////////
            /// Variables
            ////////////////////////////////////////////////////////////////////////////////////////////

            public string Id { get; }

            private List<ParamData> _params = new List<ParamData>();
            private List<ActionData> _actions = new List<ActionData>();
            
            ////////////////////////////////////////////////////////////////////////////////////////////
            /// Functions
            ////////////////////////////////////////////////////////////////////////////////////////////
            
            public TabData(string inId)
            {
                if (string.IsNullOrEmpty(inId))
                {
                    Debug.LogErrorFormat("[TabData][Creation][id: {0}] id is null.", inId);
                    return;
                }
                
                Id = inId;
            }

            public TabData AddParam(string inKey, string inValue, string inDescription)
            {
                if (string.IsNullOrEmpty(Id))
                {
                    Debug.LogErrorFormat("[TabData][SetParam][id: {0}] id is null.", Id);
                    return this;
                }
                
                if (string.IsNullOrEmpty(inKey))
                {
                    Debug.LogErrorFormat("[TabData][SetParam][id: {0}] key is null.", Id);
                    return this;
                }
                
                if (_params.Any(x => x.Id == inKey))
                {
                    Debug.LogErrorFormat("[TabData][SetParam][id: {0}] key is already exist.", Id);
                    return this;
                }

                _params.Add(new ParamData(inKey, inValue, inDescription));

                return this;
            }
            
            public TabData AddAction(string inKey, Action inAction)
            {
                if (string.IsNullOrEmpty(Id))
                {
                    Debug.LogErrorFormat("[TabData][SetAction][id: {0}] id is null.", Id);
                    return this;
                }
                
                if (string.IsNullOrEmpty(inKey))
                {
                    Debug.LogErrorFormat("[TabData][SetAction][id: {0}] key is null.", Id);
                    return this;
                }
                
                if (inAction == null)
                {
                    Debug.LogErrorFormat("[TabData][SetAction][id: {0}] action is null.", Id);
                    return this;
                }
                
                if (_actions.Any(x => x.Id == inKey))
                {
                    Debug.LogErrorFormat("[TabData][SetAction][id: {0}] key is already exist.", Id);
                    return this;
                }
                
                _actions.Add(new ActionData(inKey, inAction));

                return this;
            }
            
            public IEnumerable GetParams()
            {
                return _params.AsEnumerable();
            }

            public IEnumerable GetActions()
            {
                return _actions.AsEnumerable();
            }

            public void Clear()
            {
                _params.Clear();
                _actions.Clear();
            }
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////
        /// Variables
        ////////////////////////////////////////////////////////////////////////////////////////////

        List<TabData> _tabDatas = new List<TabData>();

        private TabData _tempTabData = null;
        
        ////////////////////////////////////////////////////////////////////////////////////////////
        /// Functions
        ////////////////////////////////////////////////////////////////////////////////////////////
        
        protected ApiTestEditor DataBegin(string inId)
        {
            if (_tempTabData != null)
            {
                Debug.LogErrorFormat("[TestEditor][TabDataBegin][id: {0}] tab data to write is already began.", _tempTabData.Id);
                return this;
            }

            if (_tabDatas.Any(x => x.Id == inId))
            {
                Debug.LogErrorFormat("[TestEditor][TabDataBegin][id: {0}] tab data already exists.", _tempTabData.Id);
                return this;
            }

            _tempTabData = new TabData(inId);
            return this;
        }

        public ApiTestEditor SetDataParam(string inKey, string inValue, string inDescription)
        {
            if (_tempTabData == null)
            {
                Debug.LogErrorFormat("[TestEditor][SetParam] tab data is null. need to begin.");
                return this;
            }

            _tempTabData.AddParam(inKey, inValue, inDescription);
            return this;
        }

        public ApiTestEditor SetDataAction(string inKey, Action inAction)
        {
            if (_tempTabData == null)
            {
                Debug.LogErrorFormat("[TestEditor][SetAction] tab data is null. need to begin.");
                return this;
            }

            _tempTabData.AddAction(inKey, inAction);
            return this;
        }

        public ApiTestEditor End()
        {
            if (_tempTabData == null)
            {
                return this;
            }

            _tabDatas.Add(_tempTabData);
            _tempTabData = null;

            return this;
        }
        
        #endregion

        #region Draw UI

        private Dictionary<string, InputField> _inputFields = new Dictionary<string, InputField>();

        private ApiTestEditor ClearTabUI()
        {
            foreach (Transform tr in _tabScroll.content.transform)
            {
                UnityEngine.Object.Destroy(tr.gameObject);
            }
            return this;
        }
        
        private ApiTestEditor ClearInputUI()
        {
            _inputFields.Clear();
            foreach (Transform tr in _inputScroll.content.transform)
            {
                UnityEngine.Object.Destroy(tr.gameObject);
            }
            return this;
        }
        
        private ApiTestEditor ClearButtonUI()
        {
            foreach (Transform tr in _buttonScroll.content.transform)
            {
                UnityEngine.Object.Destroy(tr.gameObject);
            }
            return this;
        }

        private ApiTestEditor SetLog(string inText)
        {
            _outputText.text = inText ?? string.Empty;
            _outputScroll.content.SetSizeDeltaHeight(_outputText.preferredHeight + 10);
            return this;
        }

        private ApiTestEditor ResetUI()
        {
            ClearTabUI();
            ClearInputUI();
            ClearButtonUI();
            SetLog(string.Empty);

            foreach (var tabData in _tabDatas)
            {
                var button = (new GameObject(tabData.Id)).AddComponent<Button>();
                button.transform.SetParent(_tabScroll.content.transform, true);
                button.transform.localScale = Vector3.one;
                button.onClick.AddListener(() => OnClickTab(tabData.Id));

                button.colors = new ColorBlock()
                {
                    normalColor = Color.black,
                    highlightedColor = Color.black,
                    pressedColor = Color.green,
                    disabledColor = Color.gray,
                    fadeDuration = 0.1f,
                    colorMultiplier = 1,
                };

                var buttonImg = button.gameObject.AddComponent<Image>();
                buttonImg.color = Color.white;

                button.targetGraphic = buttonImg;

                var buttonRT = button.gameObject.GetComponent<RectTransform>();
                
                var text = (new GameObject("Text")).AddComponent<Text>();
                text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text.fontSize = 30;
                text.alignByGeometry = true;
                text.color = Color.white;
                text.resizeTextForBestFit = true;
                text.resizeTextMinSize = 10;
                text.resizeTextMaxSize = 30;
                text.transform.SetParent(button.transform, true);
                text.text = tabData.Id;
                text.alignment = TextAnchor.MiddleCenter;
                
                text.gameObject.GetComponent<RectTransform>()
                    .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                    .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.STRETCH)
                    .SetOffsetTop(5)
                    .SetOffsetBottom(5)
                    .SetOffsetLeft(5)
                    .SetOffsetRight(5);

                text.gameObject.AddComponent<Shadow>();
            }

            _tabScroll.content.SetSizeDeltaWidth(200 * _tabDatas.Count);
            _tabScroll.horizontalNormalizedPosition = 0;

            return this;
        }
        
        private void OnClickTab(string inKey)
        {
            var data = _tabDatas.FirstOrDefault(x => x.Id == inKey);
            if (data == null)
            {
                return;
            }
            
            CreateInputs(data);
            CreateButtons(data);
        }

        private ApiTestEditor CreateInputs(TabData inTabData)
        {
            ClearInputUI();

            var count = 0;
            foreach (ParamData param in inTabData.GetParams())
            {
                count++;
                
                var inputFieldBG = (new GameObject(param.Id)).AddComponent<Image>();
                inputFieldBG.transform.SetParent(_inputScroll.content, true);
                inputFieldBG.transform.localScale = Vector3.one;
                inputFieldBG.color = Color.gray;

                var label = (new GameObject("Label")).AddComponent<Text>();
                label.transform.SetParent(inputFieldBG.transform);
                label.transform.localScale = Vector3.one;
                label.text = param.Id;
                label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                label.fontSize = 30;
                label.alignByGeometry = true;
                label.color = Color.white;
                label.resizeTextForBestFit = true;
                label.resizeTextMinSize = 20;
                label.resizeTextMaxSize = 30;
                label.alignment = TextAnchor.MiddleRight;

                label.GetComponent<RectTransform>()
                    .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.LEFT)
                    .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.STRETCH)
                    .SetPivotX(0)
                    .SetAnchoredPositionX(5)
                    .SetAnchoredPositionY(0)
                    .SetSizeDeltaWidth(100)
                    .SetOffsetTop(5)
                    .SetOffsetBottom(5);

                var inputField = (new GameObject("InputField")).AddComponent<InputField>();
                _inputFields.Add(param.Id, inputField);
                inputField.transform.SetParent(inputFieldBG.transform, true);
                inputField.transform.localScale = Vector3.one;
                inputField.lineType = InputField.LineType.MultiLineSubmit;

                var inputFiledImg = inputField.gameObject.AddComponent<Image>();
                
                inputField.gameObject.GetComponent<RectTransform>()
                    .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                    .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.STRETCH)
                    .SetAnchoredPositionX(0)
                    .SetAnchoredPositionY(0)
                    .SetOffsetLeft(120)
                    .SetOffsetRight(5)
                    .SetOffsetTop(5)
                    .SetOffsetBottom(5);
                
                var placeHolder = (new GameObject("PlaceHolder")).AddComponent<Text>();
                placeHolder.transform.SetParent(inputField.transform, true);
                placeHolder.transform.localScale = Vector3.one;
                placeHolder.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                placeHolder.fontSize = 30;
                placeHolder.alignByGeometry = true;
                placeHolder.color = Color.gray;
                placeHolder.resizeTextForBestFit = false;
                placeHolder.alignment = TextAnchor.UpperLeft;
                
                placeHolder.GetComponent<RectTransform>()
                    .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                    .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.STRETCH)
                    .SetAnchoredPositionX(0)
                    .SetAnchoredPositionY(0)
                    .SetOffsetLeft(5)
                    .SetOffsetRight(5)
                    .SetOffsetTop(5)
                    .SetOffsetBottom(5);
                
                var text = (new GameObject("Text")).AddComponent<Text>();
                text.transform.SetParent(inputField.transform, true);
                text.transform.localScale = Vector3.one;
                text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text.fontSize = 30;
                text.alignByGeometry = true;
                text.color = Color.black;
                text.resizeTextForBestFit = false;
                text.alignment = TextAnchor.UpperLeft;
                
                text.GetComponent<RectTransform>()
                    .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                    .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.STRETCH)
                    .SetAnchoredPositionX(0)
                    .SetAnchoredPositionY(0)
                    .SetOffsetLeft(5)
                    .SetOffsetRight(5)
                    .SetOffsetTop(5)
                    .SetOffsetBottom(5);

                inputField.placeholder = placeHolder;
                inputField.textComponent = text;

                inputField.text = param.Param;
                placeHolder.text = param.Description;
            }

            _inputScroll.content.SetSizeDeltaHeight(100 * count);
            _inputScroll.verticalNormalizedPosition = 1;

            return this;
        }
        
        private ApiTestEditor CreateButtons(TabData inTabData)
        {
            ClearButtonUI();
            SetLog(string.Empty);

            var count = 0;
            foreach (ActionData action in inTabData.GetActions())
            {
                count++;
                
                var button = (new GameObject(action.Id)).AddComponent<Button>();
                button.transform.SetParent(_buttonScroll.content.transform, true);
                button.transform.localScale = Vector3.one;
                button.onClick.AddListener(() =>
                {
                    SetLog(string.Empty);
                    _outputScroll.verticalNormalizedPosition = 1;
                    action.Action();
                });

                button.colors = new ColorBlock()
                {
                    normalColor = Color.black,
                    highlightedColor = Color.black,
                    pressedColor = Color.green,
                    disabledColor = Color.gray,
                    fadeDuration = 0.1f,
                    colorMultiplier = 1,
                };

                var buttonImg = button.gameObject.AddComponent<Image>();
                buttonImg.color = Color.white;

                button.targetGraphic = buttonImg;

                var buttonRT = button.gameObject.GetComponent<RectTransform>();
                
                var text = (new GameObject("Text")).AddComponent<Text>();
                text.transform.SetParent(button.transform, true);
                text.transform.localScale = Vector3.one;
                text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text.fontSize = 30;
                text.alignByGeometry = true;
                text.color = Color.white;
                text.resizeTextForBestFit = true;
                text.resizeTextMinSize = 10;
                text.resizeTextMaxSize = 30;
                text.text = action.Id;
                text.alignment = TextAnchor.MiddleCenter;
                
                text.gameObject.GetComponent<RectTransform>()
                    .SetAnchorTypeX(RectTransformExtension.EAnchorTypeX.STRETCH)
                    .SetAnchorTypeY(RectTransformExtension.EAnchorTypeY.STRETCH)
                    .SetOffsetTop(5)
                    .SetOffsetBottom(5)
                    .SetOffsetLeft(5)
                    .SetOffsetRight(5);

                text.gameObject.AddComponent<Shadow>();
            }

            _buttonScroll.content.SetSizeDeltaHeight(100 * count);
            _buttonScroll.verticalNormalizedPosition = 1;

            return this;
        }

        #endregion

        #region public functions

        public static void Log(string inText)
        {
            _instance.SetLog(inText);
        }
        
        public static ApiTestEditor Begin(string inId)
        {
            if (_instance == null)
            {
                _instance = new ApiTestEditor();
            }

            return _instance.DataBegin(inId);
        }
        
        public static string GetInputText(string inKey)
        {
            return _instance._inputFields.ContainsKey(inKey) ? _instance._inputFields[inKey].text : string.Empty;
        }
        
        public void UpdateUI()
        {
            _instance.ResetUI();
        }

        #endregion
    }

}


