using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Woska.Core
{
    public class CustomLogHandler : ILogHandler
    {
        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            Debug.unityLogger.LogFormat(logType,context,format,args);
        }

        public void LogException(Exception exception, Object context)
        {
            Debug.unityLogger.LogException(exception,context);
        }
    }
}