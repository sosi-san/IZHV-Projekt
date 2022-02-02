using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Woska.Bakalarka.UI;
using Woska.Core;

namespace Woska.Bakalarka
{
    public class RoomController : MonoBehaviourPunCallbacks
    {
        private GameManager _gameManager;
        
        private const int MaxPlayers = 4;
        private bool _roomOpen = true;
        private bool _isVisible = true;

        private bool _isJoining = false;

        public bool IsJoining => PhotonNetwork.NetworkClientState == ClientState.Joining;
        public bool IsLeaving => PhotonNetwork.NetworkClientState == ClientState.Leaving;
        public bool Joined  => PhotonNetwork.NetworkClientState == ClientState.Joined;
        private readonly Logger _logger = new Logger(new CustomLogHandler());


        private List<Player> players => PhotonNetwork.CurrentRoom.Players.Values.ToList();
        [SerializeField] private TextMeshProUGUI[] playerSlots;
        public void CreateRoom()
        {
            if (IsJoining)
            {
                _logger.Log("I am joining fuck off");
                return;
            }
            PhotonNetwork.CreateRoom(Utils.RandomString(4), new RoomOptions() {IsOpen = _roomOpen, MaxPlayers = MaxPlayers, IsVisible = _isVisible});
        }
        public void JoinRoom(string roomCode = "")
        {
            if (IsJoining)
            {
                _logger.Log("I am joining fuck off");
                return;
            }
            if (roomCode.Equals(String.Empty))
                PhotonNetwork.JoinRandomRoom();
            else
                PhotonNetwork.JoinRoom(roomCode);
            
        }
        public void LeaveRoom()
        {
            if(!IsLeaving)
                PhotonNetwork.LeaveRoom();
        }
        public void StartGame()
        {
            if (PhotonNetwork.IsMasterClient && players.Count > 0)
            {
                Debug.Log("Starting Game");
                App.Instance.GameManager.StartGame();
            }
        }
        private void RoomJoined()
        {
            CanvasManager.Instance.SwitchToCanvas(CanvasType.Room);
            PlayerSlotManager.Instance.UpdateSlots(players);
        }
        #region CallBacks
        public override void OnJoinedRoom()
        {
            _logger.Log($"Room {PhotonNetwork.CurrentRoom.Name} joined!");
            RoomJoined();
        }
        public override void OnLeftRoom()
        {
            CanvasManager.Instance.SwitchToCanvas(CanvasType.JoinOrCreateLobby);
            _logger.Log($"Room left!");
        }
        /*
         * Called when other player enters room
         */
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _logger.Log($"Player {newPlayer.NickName} entered room!");
            PlayerSlotManager.Instance.UpdateSlots(players);
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            _logger.Log($"Player {otherPlayer.NickName} left room!");
            PlayerSlotManager.Instance.UpdateSlots(players);
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            _logger.Log($"Creating room failed! {message}");
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            _logger.Log($"Joining room failed! {message}");
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            _logger.Log($"Joining random room failed! {message}");
        }
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if(Equals(newMasterClient, PhotonNetwork.LocalPlayer))
                Debug.Log("I am new master client!");
            LeaveRoom();
        }

        #endregion
    }
}