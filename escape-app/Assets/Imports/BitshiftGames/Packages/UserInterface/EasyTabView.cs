using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BitshiftGames.Essentials.UI
{
    public class EasyTabView : MonoBehaviour
    {
        [Header("Components")] 
        public Tab[] selectableTabs;

        private int lastSelectedTab = 0;

        #region TabSelection
        public void SelectTab(int index)
        {
            for (int i = 0; i < selectableTabs.Length; i++)
            {
                if(i == index)
                    selectableTabs[i].SelectThisTab();
                else
                    selectableTabs[i].DeselectThisTab();
            }
        }
        #endregion

        #region Classes
        [System.Serializable]
        public class Tab
        {
            [Header("Settings")]
            public string tabName;

            [Space(5)] 
            public Color tabSelectedIconColor;
            public Color tabDeselectedIconColor;

            [Header("Components")] 
            public Image tabIcon;
            public RectTransform tabFocusHighlighter;
            public GameObject tabPage;

            public void SelectThisTab()
            {
                tabIcon.color = tabSelectedIconColor;
                tabFocusHighlighter.LeanScale(Vector3.one, 0.15f).setEaseOutBack();
                tabPage.SetActive(true);
            }

            public void DeselectThisTab()
            {
                tabIcon.color = tabDeselectedIconColor;
                tabFocusHighlighter.LeanScale(new Vector3(0, 1, 1), 0.15f).setEaseInBack();
                tabPage.SetActive(false);
            }
        }
        #endregion
    }   
}
