using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;
using Woska.Core;

namespace Woska.Bakalarka.UI
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        [SerializeField] private List<PlayerScore> playerScores;

        private void Awake()
        {
            //playerScores = GetComponentsInChildren<PlayerScore>().ToList();
        }

        public void Setup(List<Player> players)
        {
            players = players.OrderBy(p => p.ActorNumber).ToList();
            for (int i = 0; i < 4; i++)
            {
                if (i < players.Count)
                {
                    var player = players[i];
                    Debug.Log(player.NickName);
                    var color = App.Instance.GameManager.GetColorOfPlayer(player);
                    playerScores[i].Setup(player.NickName, color);
                }
                else
                {
                    playerScores[i].SlotIsEmpty();
                }
            }
        }
    }
}