using Cameo.Mono;
using Cameo.NonMono;
using System.Collections.Generic;
using UnityEngine;
using FickleFrameGames.Systems;


namespace Assets.Scripts.Testing
{
    public class Testing_01 : MonoBehaviour
    {
        public int playerCount = 4;
        public PlayerController controller;
        public RoundManager manager;

        CardDock dock;

        private void Awake()
        {
        }

        private void Start()
        {
            dock = CardDock.CreateCardDock(transform.position, null);
            controller?.InstantiatePlayers(playerCount);
        }

        private void Update()
        {
        }
    }
}