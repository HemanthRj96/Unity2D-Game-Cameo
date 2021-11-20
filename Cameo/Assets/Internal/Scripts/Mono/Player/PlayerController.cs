using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameo.Mono
{
    public class PlayerController : MonoBehaviour
    {
        // Private fields

        [SerializeField]
        private Player targetPlayerPrefab;
        [SerializeField]
        private RoundManager _roundManager;
        [SerializeField]
        private List<Transform> _spawnPoints = new List<Transform>();

        private List<Player> _playerInstances = new List<Player>();

        // Public methods

        public void InstantiatePlayers(int count)
        {
            for (int i = 0; i < count; ++i)
                _playerInstances.Add(Instantiate(targetPlayerPrefab, _spawnPoints[i]));
            foreach (var player in _playerInstances)
                _roundManager.RegisterPlayer(player);
        }
    }
}