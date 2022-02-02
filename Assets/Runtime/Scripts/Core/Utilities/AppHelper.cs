using UnityEngine;

namespace Woska.Core
{
    public static class AppHelper
    {
        #if UNITY_WEBPLAYER || UNITY_WEBGL
        public static string webplayerQuitURL = Application.absoluteURL;
        #endif
        public static void Quit()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_WEBPLAYER ||UNITY_WEBGL
                Application.ExternalEval("document.location.reload()");
            #else
                Application.Quit();
            #endif
        }   
    }
}