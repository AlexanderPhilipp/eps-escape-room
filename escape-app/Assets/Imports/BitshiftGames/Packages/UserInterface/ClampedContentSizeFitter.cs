using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AlexanderPhilipp.EasyUI
{

    [AddComponentMenu("Layout/Clamped Content Size Fitter", 141)]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class ClampedContentSizeFitter : UIBehaviour, ILayoutSelfController
    {
        /// <summary>
        /// The size fit modes avaliable to use.
        /// </summary>
        public enum FitMode
        {
            /// <summary>
            /// Don't perform any resizing.
            /// </summary>
            Unconstrained,
            /// <summary>
            /// Resize to the minimum size of the content.
            /// </summary>
            MinSize,
            /// <summary>
            /// Resize to the preferred size of the content.
            /// </summary>
            PreferredSize
        }

        /// <summary>
        /// The fit mode to use to determine the width.
        /// </summary>
        public FitMode horizontalFit;

        /// <summary>
        /// The fit mode to use to determine the height.
        /// </summary>
        public FitMode verticalFit;

        public bool clampWidth = false;
        public bool clampHeight = false;

        public float targetWidth = 0;
        public float targetHeight = 0;


        [System.NonSerialized] private RectTransform m_Rect;
        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        // field is never assigned warning
#pragma warning disable 649
        private DrivenRectTransformTracker m_Tracker;
#pragma warning restore 649

        protected ClampedContentSizeFitter()
        { }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        protected override void OnDisable()
        {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            SetDirty();
        }

        private void HandleSelfFittingAlongAxis(int axis)
        {
            FitMode fitting = (axis == 0 ? horizontalFit : verticalFit);
            if (fitting == FitMode.Unconstrained)
            {
                // Keep a reference to the tracked transform, but don't control its properties:
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.None);
                return;
            }

            m_Tracker.Add(this, rectTransform, (axis == 0 ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY));


            if (axis == 0)
            {
                if (clampWidth)
                {
                    // Set size to min or preferred size
                    if (fitting == FitMode.MinSize)
                        rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, Mathf.Clamp(LayoutUtility.GetMinSize(m_Rect, axis), 0, targetWidth));
                    else
                        rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, Mathf.Clamp(LayoutUtility.GetPreferredSize(m_Rect, axis), 0, targetWidth));
                }
                else
                {
                    // Set size to min or preferred size
                    if (fitting == FitMode.MinSize)
                        rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetMinSize(m_Rect, axis));
                    else
                        rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetPreferredSize(m_Rect, axis));
                }
            }
            else if (axis == 1)
            {
                if (clampHeight)
                {
                    // Set size to min or preferred size
                    if (fitting == FitMode.MinSize)
                        rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, Mathf.Clamp(LayoutUtility.GetMinSize(m_Rect, axis), 0, targetHeight));
                    else
                        rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, Mathf.Clamp(LayoutUtility.GetPreferredSize(m_Rect, axis), 0, targetHeight));
                }
                else
                {
                    // Set size to min or preferred size
                    if (fitting == FitMode.MinSize)
                        rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetMinSize(m_Rect, axis));
                    else
                        rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetPreferredSize(m_Rect, axis));
                }

            }
        }

        /// <summary>
        /// Calculate and apply the horizontal component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutHorizontal()
        {
            m_Tracker.Clear();
            HandleSelfFittingAlongAxis(0);
        }

        /// <summary>
        /// Calculate and apply the vertical component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutVertical()
        {
            HandleSelfFittingAlongAxis(1);
        }

        protected void SetDirty()
        {
            if (!IsActive())
                return;

            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }

#endif
    }
}

