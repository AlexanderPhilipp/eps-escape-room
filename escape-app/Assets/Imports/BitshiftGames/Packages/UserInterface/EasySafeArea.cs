using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlexanderPhilipp.EasyUI
{
    [ExecuteAlways]
    public class EasySafeArea : MonoBehaviour
    {

        RectTransform Panel;
        Rect LastSafeArea = new Rect(0, 0, 0, 0);
        Vector2Int LastScreenSize = new Vector2Int(0, 0);
        ScreenOrientation LastOrientation = ScreenOrientation.AutoRotation;

        public bool resizeHorizontal = true;
        public bool resizeVerticaly = true;

        void Awake()
        {
            Panel = GetComponent<RectTransform>();

            if (Panel == null)
            {
                Debug.LogError("Easy Safe Area must be added to an object with a rectTransform");
                Destroy(gameObject);
            }

            Refresh();
        }

        void Update()
        {
            Refresh();
        }

        void Refresh()
        {
            Rect safeArea = Screen.safeArea;

            if (safeArea != LastSafeArea || Screen.width != LastScreenSize.x || Screen.height != LastScreenSize.y || Screen.orientation != LastOrientation)
            {
                LastScreenSize.x = Screen.width;
                LastScreenSize.y = Screen.height;
                LastOrientation = Screen.orientation;

                ApplySafeArea(safeArea);
            }
        }

        void ApplySafeArea(Rect r)
        {
            LastSafeArea = r;

            if (!resizeHorizontal)
            {
                r.x = 0;
                r.width = Screen.width;
            }

            if (!resizeVerticaly)
            {
                r.y = 0;
                r.height = Screen.height;
            }

            if (Screen.width > 0 && Screen.height > 0)
            {
                Vector2 anchorMin = r.position;
                Vector2 anchorMax = r.position + r.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                if (anchorMin.x >= 0 && anchorMin.y >= 0 && anchorMax.x >= 0 && anchorMax.y >= 0)
                {
                    Panel.anchorMin = anchorMin;
                    Panel.anchorMax = anchorMax;
                }
            }
        }
    }
}
