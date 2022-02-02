using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Woska.Core;

namespace Woska.Bakalarka.UI
{
    public class PlayerSlotManager : Singleton<PlayerSlotManager>
    {
        [SerializeField] private TextMeshProUGUI[] playerSlots;
        
        public void UpdateSlots(List<Player> players)
        {
            players = players.OrderBy(p => p.ActorNumber).ToList();
            for (int i = 0; i < 4; i++)
            {
                if (i < players.Count)
                    playerSlots[i].text = players[i].NickName;
                else
                    playerSlots[i].text = "EMPTY";
            }
        }
    }
}