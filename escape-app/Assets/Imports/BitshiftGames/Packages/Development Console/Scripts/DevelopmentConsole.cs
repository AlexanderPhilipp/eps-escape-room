using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Reflection;
using System.Linq;

namespace BitshiftGames.Essentials.Debugging
{
    public class DevelopmentConsole : MonoBehaviour
    {
        public static DevelopmentConsole Instance;

        public enum LogType
        {
            Info, Warning, Error
        }

        [Header("Components")]
        public RectTransform consoleParent;
        [Space(5)]
        public GameObject expandIcon;
        public GameObject collapseIcon;
        [Space(5)]
        public TMP_InputField commandInputField;
        public TextMeshProUGUI consoleTextLabel;

        [Header("Settings")]
        public float consoleSize;
        public float consoleSizeChangeSpeed;

        [Header("Runtime")]
        public bool isOpen = false;
        public List<DevConCommand> consoleCommands = new List<DevConCommand>();

        private void Start()
        {
            #region Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);

            DontDestroyOnLoad(this.gameObject);
            #endregion
            FetchCommands();
        }

        private void Update()
        {
            #region SizeChange
            consoleParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 
                Mathf.Lerp(consoleParent.rect.height, isOpen ? consoleSize : 0, consoleSizeChangeSpeed * Time.deltaTime));

            if (isOpen)
            {
                expandIcon.SetActive(false);
                collapseIcon.SetActive(true);
            }
            else
            {
                expandIcon.SetActive(true);
                collapseIcon.SetActive(false);
            }
            #endregion
        }
        #region CommandFetching
        void FetchCommands()
        {
            consoleCommands = new List<DevConCommand>();

            List<DevConCommand> commands = new List<DevConCommand>();

            var methods = Assembly.GetExecutingAssembly().GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(DevConCommand), false).Length > 0)
                .ToArray();

            foreach (MethodInfo method in methods)
            {
                DevConCommand newCommand = new DevConCommand(method);
                Type declaringType = method.DeclaringType;
                var typeInstances = GameObject.FindObjectsOfType(declaringType);

                newCommand.implementingObjects = typeInstances;
                consoleCommands.Add(newCommand);
            }
        }
        #endregion
        #region CommandProcessing
        public void SendCommand()
        {
            if(commandInputField.text.Length > 0)
            {
                FetchCommands();

                List<string> arguments = new List<string>();
                string[] fetchedArgs = commandInputField.text.Split(' ');
                for (int i = 1; i < fetchedArgs.Length; i++)
                {
                    arguments.Add(fetchedArgs[i]);
                }

                ProcessCommand(fetchedArgs[0], arguments.ToArray());
                commandInputField.text = "";
            }
        }

        void ProcessCommand(string commandIdentifier, object[] fetchedArgs)
        {
            DevConCommand command = null;
            foreach(DevConCommand c in consoleCommands)
            {
                if(c.commandIdentifier == commandIdentifier)
                {
                    command = c;
                    break;
                }
            }
            
            if(command == null)
            {
                Log("No command found with ID: " + commandIdentifier, LogType.Error);
                return;
            }

            if(fetchedArgs.Length != command.commandParameters.Length)
            {
                Log("No command found with " + fetchedArgs.Length + " parameters.", LogType.Error);
                return;
            }

            List<object> args = new List<object>();
            for (int i = 0; i < fetchedArgs.Length; i++)
            {
                if(i < command.commandParameters.Length)
                {
                    ParameterInfo info = (ParameterInfo)command.commandParameters[i];
                    object convertedArg = Convert.ChangeType(fetchedArgs[i], info.ParameterType);
                    args.Add(convertedArg);
                }
            }


            foreach (object obj in command.implementingObjects)
            {
                command.method.Invoke(obj, args.ToArray());
            }
            
        }
        #endregion
        #region ConsoleOutput
        public void Log(string logText, LogType type)
        {
            string timeStamp = "<b>[" + DateTime.Now.ToString("hh:mm:ss") + "]</b>";
            string infoTag = "";
            switch (type)
            {
                case LogType.Info:
                    infoTag = "<color=grey> INFO </color>";
                    break;
                case LogType.Warning:
                    infoTag = "<color=yellow> WARNING </color>";
                    break;
                case LogType.Error:
                    infoTag = "<color=red> ERROR </color>";
                    break;
                default:
                    infoTag = "<color=grey> INFO </color>";
                    break;
            }

            string consoleText = '\n' + timeStamp + infoTag + logText;
            consoleTextLabel.text += consoleText;
        }
        #endregion
        #region MenuOpeningClosing
        public void ShrinkExpandConsole()
        {
            isOpen = !isOpen;
        }
        #endregion
        #region ConsoleSettings
        [DevConCommand]
        public void SetConsoleSize(float size)
        {
            consoleSize = size;
        }
        #endregion
    }

    [System.Serializable]
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class DevConCommand : System.Attribute
    {
        public string commandIdentifier;
        public object[] commandParameters;

        public object[] implementingObjects;
        public MethodInfo method;

        public DevConCommand(MethodInfo method)
        {
            this.method = method;
            commandIdentifier = method.Name;
            commandParameters = method.GetParameters();
        }
        public DevConCommand()
        {

        }
    }
}
