using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Woska.Bakalarka
{
    public class SpikyWheelController : GameController
    {
        private float _timer;
        protected override void Start()
        {
            base.Start();
            PhotonNetwork.Instantiate("PhotonPlayer", Vector3.up*2 + Random.Range(-3,3)*Vector3.right, Quaternion.identity);
        }
        private void Update()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                SpawnWheel();
                _timer = Random.Range(2f, 5f);
            }
        }
        private void SpawnWheel()
        {
            int direction_x = 1;
            if (Random.Range(0, 100) >= 50)
                direction_x = -1;

            Vector3 spawnPosition = new Vector3(10, 0) * direction_x + Vector3.up;

            GameObject wheelGameobject;
            if (Random.Range(0, 100) >= 25)
                wheelGameobject = PhotonNetwork.Instantiate("WheelFull", spawnPosition, Quaternion.identity);
            else
            {
                wheelGameobject = PhotonNetwork.Instantiate("WheelPart", spawnPosition, Quaternion.identity);
            }

            wheelGameobject.GetComponent<WheelController>().Direction = -direction_x;
        }
        
        public override void PlayerDied()
        {
            if(!PhotonNetwork.IsMasterClient)
                return;

            _playersLeft--;
            if (_playersLeft == 0)
            {
                _gameManager.StartNextMiniGame();
                PhotonNetwork.Destroy(gameObject);
            }
        }

    }
}