using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


namespace Woska.Bakalarka
{
    [RequireComponent(typeof(PhotonView))]
    public abstract class GameController : MonoBehaviour
    {
        protected GameManager _gameManager;
        protected int _numberOfPlayers;
        protected int _playersLeft;
        
        public Action MiniGameEnd => PlayerDied;
        
        protected virtual void Awake()
        {
            _gameManager = App.Instance.GameManager;
        }
        protected virtual void Start()
        {
            _playersLeft = _numberOfPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
        }
        public virtual void PlayerDied() {}
    }
}