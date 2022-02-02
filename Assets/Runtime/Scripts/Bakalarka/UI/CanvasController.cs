using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Woska.Bakalarka.UI
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private CanvasType _canvasType;
        public CanvasType CanvasType => _canvasType;

        private void Update()
        {
            if (CanvasType == CanvasType.MainMenu)
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame && !Keyboard.current.escapeKey.wasPressedThisFrame && PhotonNetwork.InLobby)
                {
                    CanvasManager.Instance.SwitchToCanvas(CanvasType.JoinOrCreateLobby);
                }
            }
        }
    }
}