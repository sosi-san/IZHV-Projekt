using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Woska.Core;

namespace Woska.Bakalarka.UI
{
    public enum CanvasType
    {
        MainMenu,
        JoinOrCreateLobby,
        Room,
        ExitMenu,
        Score,
    
        None
    }
    public class CanvasManager : Singleton<CanvasManager>
    {
        private List<CanvasController> _canvases;

        private CanvasController _currentCanvas;
        public CanvasController CurrentCanvas =>_currentCanvas;

        private void Awake()
        {
            GetCanvases();
            _canvases.ForEach(x => x.gameObject.SetActive(false));
            SwitchToCanvas(CanvasType.MainMenu);
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                ToggleCanvas(CanvasType.ExitMenu);
            }
        }

        private void GetCanvases()
        {
            _canvases = GetComponentsInChildren<CanvasController>().ToList();
        }
        public void SwitchToCanvas(CanvasType canvasType)
        {
            if(_currentCanvas != null) 
                _currentCanvas.gameObject.SetActive(false);
            if (canvasType == CanvasType.None)
                _currentCanvas = null;
            else
            {
                _currentCanvas = _canvases.Find(x => x.CanvasType == canvasType);
                _currentCanvas.gameObject.SetActive(true);
            }
        }
        public void ToggleCanvas(CanvasType canvasType)
        {
            var canvasToToggle = _canvases.Find(x => x.CanvasType == canvasType);  
            canvasToToggle.gameObject.SetActive(!canvasToToggle.gameObject.activeSelf);
        }
    }
}