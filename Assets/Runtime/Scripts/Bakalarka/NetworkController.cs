using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Woska.Core;
using Random = UnityEngine.Random;

namespace Woska.Bakalarka
{
    public class NetworkController : MonoBehaviourPunCallbacks
    {
        private RoomController _roomController;
        private readonly Logger _logger = new Logger(new CustomLogHandler());
        
        private const int MaxReconnectionAttempts = 5;
        private int _connectionsAttempts = 0;
        
        private bool QuitApplication => _connectionsAttempts >= MaxReconnectionAttempts-1;

        private TextMeshProUGUI _connectedField;
        private TextMeshProUGUI _playersOnlineField;
        private void Awake()
        {
            GetComponents();
        }
        private void Start()
        {
            PhotonNetwork.NickName = "Player_" + Random.Range(0, 99);
            UpdateStatus();
            
            ConnectToServer();
            
            InvokeRepeating(nameof(UpdateStatus), 5f, 10f);
        }
        private void ConnectToServer()
        {
            PhotonNetwork.ConnectUsingSettings();
            _connectionsAttempts++;
        }
        private void GetComponents()
        {
            _roomController = gameObject.GetOrAddComponent<RoomController>();
        }

        public void FindStatusFields()
        {
            _connectedField = GameObject.FindWithTag("Connected").GetComponent<TextMeshProUGUI>();
            _playersOnlineField = GameObject.FindWithTag("PlayersOnline").GetComponent<TextMeshProUGUI>();
        }
        private void UpdateStatus()
        {
            _connectedField.text = PhotonNetwork.IsConnected ? "Connected" : "Disconnected";
            _playersOnlineField.text = PhotonNetwork.CountOfPlayers + " online";
        }
        #region CallBacks
        public override void OnConnectedToMaster()
        {
            _connectionsAttempts = 0;
            _logger.Log("Connected to master server.");
            PhotonNetwork.JoinLobby();
            UpdateStatus();
        }

        public override void OnJoinedLobby()
        {
            UpdateStatus();
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            _logger.Log($"Connection lost: {cause}. Attempt #{_connectionsAttempts}");
            if (QuitApplication) 
                AppHelper.Quit();
            ConnectToServer();
            UpdateStatus();
        }
        #endregion
    }
}