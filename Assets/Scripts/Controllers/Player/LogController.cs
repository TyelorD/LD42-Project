using UnityEngine;
using System.Collections.Generic;
using System.Text;

// Made by myself prior to LD 42 (Not needed for gameplay, just to help test on the browser).
public class LogController : MonoBehaviour {

    public static bool debugOn { get; private set; }

    private static StringBuilder myLog = new StringBuilder();

    private static Log log;
    
    private static LogController _logController;
    public static LogController logController
    {
        get
        {
            if (_logController == null)
                _logController = FindObjectOfType<LogController>();

            return _logController;
        }
    }

    private void Awake()
    {
        if (Debug.isDebugBuild)
            SetActive(true);
    }

    private void OnDisable()
    {
        SetActive(false);
    }

    private void OnGUI()
    {
        if(debugOn)
            GUILayout.Label("Debug Logger (press F8 to close):\n" + log.ToString());
    }

    public static void SetActive(bool active)
    {
        if(active != debugOn)
        {
            debugOn = active;

            if (debugOn)
                Application.logMessageReceived += HandleLog;
            else
                Application.logMessageReceived -= HandleLog;
        }
    }

    private static void HandleLog(string logString, string stackTrace, LogType type)
    {
        log.AddLog(logString, stackTrace, type);
    }

    private struct Log {

        private int count;
        private Queue<string> logStrings;
        StringBuilder stringBuilder;

        public void AddLog(string logString, string stackTrace, LogType type)
        {
            if (logStrings == null)
                logStrings = new Queue<string>(25);

            if (stringBuilder == null)
                stringBuilder = new StringBuilder();
            else
                stringBuilder.Length = 0;

            if (logStrings.Count >= 25)
                logStrings.Dequeue();

            stringBuilder.Append("\n (");
            stringBuilder.Append(++count);
            stringBuilder.Append(") [");
            stringBuilder.Append(type.ToString());
            stringBuilder.Append("] : ");
            stringBuilder.Append(logString);

            if (type == LogType.Exception)
            {
                stringBuilder.Append("\n");
                stringBuilder.Append(stackTrace);
            }

            logStrings.Enqueue(stringBuilder.ToString());
        }

        public override string ToString()
        {
            if (stringBuilder == null)
                return "";
            else
                stringBuilder.Length = 0;

            foreach (string log in logStrings)
            {
                stringBuilder.Append(log);
            }

            return stringBuilder.ToString();
        }

    }
}

