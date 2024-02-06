using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldLimiter : MonoBehaviour
{
    TMP_InputField inputField;

    public string bannedCharacters = "!§$%&/()=?`*'#´,}][{<>|";

    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    public void ValidateInputField()
    {
        string inputFieldText = inputField.text;
        foreach(char c in bannedCharacters)
        {
            inputFieldText.Replace(c, '\0');
        }

        inputField.text = inputFieldText;
    }
}
