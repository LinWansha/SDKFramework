using System;
using System.Collections.Generic;
using Habby.Events;
 
using UnityEditor;
using UnityEngine;

namespace Aya.Events.Editor.script
{
    public class EventMonitor: EditorWindow
    {
        #region Menu
      
        public static EventMonitor Instance;

        [MenuItem("habby/Event Monitor", false, 0)]
        public static void ShowWindow()
        {
            if (Instance == null)
            {
                Instance = CreateInstance<EventMonitor>();
                Instance.titleContent.text = "Event Monitor";
                // Instance.minSize = new Vector2(1280, 800);
                // Instance.maxSize = new Vector2(Screen.width, Screen.height);
                Instance.maximized = false;
            }

            Instance.Show();
        }

        #endregion

      
    }
}