using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BitshiftGames.Essentials
{
    public class SettingsManager : MonoBehaviour
    {
        //Singleton Stuff
        public static SettingsManager singleton;

        [Header("Components")]
        public Setting[] settings;

        [Header("Saving")]
        public string settingsSaveBaseKey;

        private void Awake()
        {
            //Singleton Initialization
            if (SettingsManager.singleton == null)
                singleton = this;
            else
                Destroy(this.gameObject);

            ReloadUserInterface();
        }

        #region AccessFunctions
        public void SetSetting(string settingId)
        {
            Setting targetSetting = GetSetting(settingId);

            if (targetSetting == null)
                return;

            switch (targetSetting.settingType)
            {
                case Setting.SettingType.intVal:
                    int setValInt = 0;

                    if(targetSetting.settingToggle != null) { setValInt = targetSetting.settingToggle.isOn ? 1 : 0; }
                    else if (targetSetting.settingSlider != null) { setValInt = Mathf.RoundToInt(targetSetting.settingSlider.value); }
                    else if (targetSetting.settingInputfield != null) { int.TryParse(targetSetting.settingInputfield.text, out setValInt); }
                    else if (targetSetting.settingDropdown != null) { setValInt = targetSetting.settingDropdown.value; }

                    string saveKeyInt = settingsSaveBaseKey + targetSetting.settingId;
                    PlayerPrefs.SetInt(saveKeyInt, setValInt);

                    break;
                case Setting.SettingType.floatVal:
                    float setValFloat = 0;

                    if (targetSetting.settingToggle != null) { setValFloat = targetSetting.settingToggle.isOn ? 1f : 0f; }
                    else if (targetSetting.settingSlider != null) { setValFloat = targetSetting.settingSlider.value; }
                    else if (targetSetting.settingInputfield != null) { float.TryParse(targetSetting.settingInputfield.text, out setValFloat); }
                    else if (targetSetting.settingDropdown != null) { setValFloat = targetSetting.settingDropdown.value; }

                    string saveKeyFloat = settingsSaveBaseKey + targetSetting.settingId;
                    PlayerPrefs.SetFloat(saveKeyFloat, setValFloat);

                    break;
                case Setting.SettingType.boolVal:
                    bool setValBool = false;

                    if (targetSetting.settingToggle != null) { setValBool = targetSetting.settingToggle.isOn ? true : false; }
                    else if (targetSetting.settingSlider != null) { setValBool = targetSetting.settingSlider.value != 0 ? true : false; }
                    else if (targetSetting.settingInputfield != null) { setValBool = targetSetting.settingInputfield.text != "" ? true : false; }
                    else if (targetSetting.settingDropdown != null) { setValBool = targetSetting.settingDropdown.value != 0 ? true : false; }

                    string saveKeyBool = settingsSaveBaseKey + targetSetting.settingId;
                    PlayerPrefs.SetInt(saveKeyBool, setValBool ? 1 : 0);

                    break;
                case Setting.SettingType.stringVal:
                    string setValString = "";

                    if (targetSetting.settingToggle != null) { setValString = targetSetting.settingToggle.isOn ? "Enabled" : "Disabled"; }
                    else if (targetSetting.settingSlider != null) { setValString = targetSetting.settingSlider.value.ToString(); }
                    else if (targetSetting.settingInputfield != null) { setValString = targetSetting.settingInputfield.text; }
                    else if (targetSetting.settingDropdown != null) { setValString = targetSetting.settingDropdown.value.ToString(); }

                    string saveKeyString = settingsSaveBaseKey + targetSetting.settingId;
                    PlayerPrefs.SetString(saveKeyString, setValString);

                    break;
            }
        }
        public void GetSettingValue(string settingId, out int valueVar)
        {
            Setting targetSetting = GetSetting(settingId);
            if (targetSetting == null)
            {
                valueVar = -1;
                return;
            }

            string saveKey = settingsSaveBaseKey + targetSetting.settingId;
            valueVar = PlayerPrefs.GetInt(saveKey, 0);
        }
        public void GetSettingValue(string settingId, out float valueVar)
        {
            Setting targetSetting = GetSetting(settingId);
            if (targetSetting == null)
            {
                valueVar = -1f;
                return;
            }

            string saveKey = settingsSaveBaseKey + targetSetting.settingId;
            valueVar = PlayerPrefs.GetFloat(saveKey, 0f);
        }
        public void GetSettingValue(string settingId, out bool valueVar)
        {
            Setting targetSetting = GetSetting(settingId);
            if (targetSetting == null)
            {
                valueVar = false;
                return;
            }

            string saveKey = settingsSaveBaseKey + targetSetting.settingId;
            valueVar = PlayerPrefs.GetInt(saveKey, 0) != 0 ? true : false;
        }
        public void GetSettingValue(string settingId, out string valueVar)
        {
            Setting targetSetting = GetSetting(settingId);
            if (targetSetting == null)
            {
                valueVar = "";
                return;
            }

            string saveKey = settingsSaveBaseKey + targetSetting.settingId;
            valueVar = PlayerPrefs.GetString(saveKey, "");
        }

        public void ReloadUserInterface()
        {
            foreach(Setting s in settings)
            {
                switch (s.settingType)
                {
                    case Setting.SettingType.intVal:
                        int currentIntVal = 0;
                        GetSettingValue(s.settingId, out currentIntVal);

                        if (s.settingToggle != null) { s.settingToggle.isOn = currentIntVal == 1 ? true : false; }
                        else if (s.settingSlider != null) { s.settingSlider.value = (float)currentIntVal; }
                        else if (s.settingInputfield != null) { s.settingInputfield.text = currentIntVal.ToString(); }
                        else if (s.settingDropdown != null) { s.settingDropdown.value = currentIntVal; }

                        break;
                    case Setting.SettingType.floatVal:
                        float currentFloatVal = 0;
                        GetSettingValue(s.settingId, out currentFloatVal);

                        if (s.settingToggle != null) { s.settingToggle.isOn = currentFloatVal != 0f ? true : false; }
                        else if (s.settingSlider != null) { s.settingSlider.value = currentFloatVal; }
                        else if (s.settingInputfield != null) { s.settingInputfield.text = currentFloatVal.ToString(); }
                        else if (s.settingDropdown != null) { s.settingDropdown.value = Mathf.RoundToInt(currentFloatVal); }

                        break;
                    case Setting.SettingType.boolVal:
                        bool currentBoolVal = false;
                        GetSettingValue(s.settingId, out currentBoolVal);

                        if (s.settingToggle != null) { s.settingToggle.isOn = currentBoolVal; }
                        else if (s.settingSlider != null) { s.settingSlider.value = currentBoolVal ? 1f : 0f; }
                        else if (s.settingInputfield != null) { s.settingInputfield.text = currentBoolVal ? "True" : "False"; }
                        else if (s.settingDropdown != null) { s.settingDropdown.value = 0; }

                        break;
                    case Setting.SettingType.stringVal:
                        string currentStringVal = "";
                        float stringSliderFloatVal = 0;
                        GetSettingValue(s.settingId, out currentStringVal);

                        if (s.settingToggle != null) { s.settingToggle.isOn = currentStringVal != "" ? true : false; }
                        else if (s.settingSlider != null) { float.TryParse(currentStringVal, out stringSliderFloatVal); }
                        else if (s.settingInputfield != null) { s.settingInputfield.text = currentStringVal; }
                        else if (s.settingDropdown != null) { s.settingDropdown.value = 0; }

                        s.settingSlider.value = stringSliderFloatVal;
                        break;
                }
            }
        }
        #endregion
        #region MiscFunctions
        public Setting GetSetting(string settingId)
        {
            Setting foundSetting = null;
            foreach(Setting s in settings)
            {
                if(s.settingId == settingId)
                    foundSetting = s;
            }

            return foundSetting;
        }
        #endregion
        #region Classes
        [System.Serializable]
        public class Setting
        {
            public enum SettingType
            {
                intVal, floatVal, boolVal, stringVal 
            }

            [Header("Base Settings")]
            public string settingName;
            public string settingId;
            public SettingType settingType;

            [Header("UI Components")]
            public Toggle settingToggle;
            public Slider settingSlider;
            public TMP_InputField settingInputfield;
            public TMP_Dropdown settingDropdown;
        }
        #endregion
    }
}
