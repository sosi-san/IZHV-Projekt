using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Woska.Bakalarka.UI;
using Woska.Core;
using Random = UnityEngine.Random;

namespace Woska.Bakalarka
{
    public class GameManager : MonoBehaviourPun
    {
        [SerializeField] private Color[] playerColors;

        [SerializeField] private SceneIndexes[] miniGames;

        public SceneIndexes currentMinigame => miniGames[_currentMinigameIndex];
        
        private int _currentMinigameIndex = 0;

        private List<Player> players => PhotonNetwork.CurrentRoom.Players.Values.ToList();

        private bool _gameStarted = false;
        private float _timer = 3f;

        private int _playerAlive;

        private int _gameRounds = 4;
        private int _currentRound = 0;

        private GameController currentGameController;
        
        public void StartGame()
        {
            _currentRound = _currentMinigameIndex = 0;
            _gameRounds = miniGames.Length;
            StartNextMiniGame();
        }

        public void StartNextMiniGame()
        {
            if(_currentRound == _gameRounds)
                photonView.RPC("RPC_GoToMenu", RpcTarget.AllViaServer);
            else
                photonView.RPC("RPC_StartNextMiniGame", RpcTarget.AllViaServer);
        }
        public int GetPlayerIndexInRoom(Player player)
        {
            var playersSorted = players.OrderBy(p => p.ActorNumber).ToList();
            playersSorted.FindIndex(p => p.Equals(player));
            return playersSorted.FindIndex(p => p.Equals(player));
        }

        public Color GetColorOfPlayer(Player player)
        {
            return playerColors[GetPlayerIndexInRoom(player)];
        }
        IEnumerator MiniGameLoadCoroutine()
        {
            CanvasManager.Instance.SwitchToCanvas(CanvasType.Score);
            
            if(_currentRound > 0)
                yield return SceneManager.UnloadSceneAsync((int) currentMinigame);
            
            yield return SceneManager.LoadSceneAsync((int) currentMinigame, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int) currentMinigame));
            
            _currentMinigameIndex++;
            _currentRound++;
            
            currentGameController = FindObjectOfType<GameController>();
            
            
            ScoreManager.Instance.Setup(players);
        }
        IEnumerator LoadLobby()
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            CanvasManager.Instance.SwitchToCanvas(CanvasType.Room);
        }
        #region RPCs
        [PunRPC]
        private void RPC_StartNextMiniGame()
        {
            this.MyCoroutine(MiniGameLoadCoroutine());
        }
        [PunRPC]
        private void RPC_GoToMenu()
        {
            this.MyCoroutine(LoadLobby());
        }
        #endregion
    }
}