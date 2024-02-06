using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BitshiftGames.Essentials.UI
{
    public class MouseOverUiDetector : MonoBehaviour
    {
        #region Singleton
        public static MouseOverUiDetector singleton;
        #endregion
        #region Runtime
        public bool PointerIsOverUi = false;
        #endregion

        private void Awake()
        {
            #region Singleton
            if (singleton != null)
                Destroy(this);
            else
                singleton = this;
            #endregion
        }

        private void Update()
        {
            if(Application.isMobilePlatform)
                PointerIsOverUi = EventSystem.current.IsPointerOverGameObject(0);
            else
                PointerIsOverUi = EventSystem.current.IsPointerOverGameObject();
        }
    }
}
