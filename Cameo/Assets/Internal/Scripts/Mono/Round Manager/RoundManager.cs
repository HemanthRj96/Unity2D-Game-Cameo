using Cameo;
using Cameo.NonMono;
using FickleFrameGames.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameo.Mono
{
    public class RoundManager : MonoBehaviour
    {
        // Private fields

        private readonly int _startingHandCount = 4;
        private int _rounds = 0;
        private int _dealerButton = 0;
        private bool _isReady = false;
        private bool _isRoundUpdating = false;
        private List<Player> _registeredPlayers = new List<Player>();
        private ERoundPhase _roundPhase = ERoundPhase.Start;
        private CardPile _cardPile = new CardPile();

        // Private methods


        private void Awake()
        {
            ActionSystem.RegisterAction(dealCards, "DealCards");
        }

        private void Update()
        {
            if (_isReady && _isRoundUpdating)
                roundUpdate();
        }

        private void roundUpdate()
        {
            _isRoundUpdating = true;
            switch (_roundPhase)
            {
                case ERoundPhase.Start:
                    dealCards();
                    break;
                case ERoundPhase.Running:
                    runRound();
                    break;
                case ERoundPhase.Cameo:
                    runFinalRound();
                    break;
                case ERoundPhase.End:
                    doOnRoundEnd();
                    break;
            }
            _isRoundUpdating = false;
        }

        private void dealCards(IActionData data = null)
        {
            _dealerButton = _rounds % _registeredPlayers.Count;
            _roundPhase = ERoundPhase.Running;
            _cardPile.CreateNewDeck();
            for (int i = 0; i < _startingHandCount; ++i)
                for(int j = 0; j <_registeredPlayers.Count; ++j)
                {
                    if (j + _dealerButton >= _registeredPlayers.Count)
                        j = 0;
                    _registeredPlayers[j + _dealerButton].Dock.AddCard(_cardPile.Draw());
                }
        }

        private void runRound()
        {

            // change the value when player calls cameo
            _roundPhase = ERoundPhase.Cameo;
        }

        private void runFinalRound()
        {

            _roundPhase = ERoundPhase.End;
        }

        private void doOnRoundEnd()
        {
            _isReady = false;
            _roundPhase = ERoundPhase.Start;
            ++_rounds;
            foreach (var player in _registeredPlayers)
                player.Dock.ClearDock();
        }

        // Public methods

        public void IsReady()
        {
            _isReady = true;
        }

        public void IsNotReady()
        {
            _isReady = false;
        }

        public void RegisterPlayer(Player player)
        {
            int index = _registeredPlayers.Count;
            _registeredPlayers.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            _registeredPlayers.TryRemove(player);
        }
    }
}