using UnityEngine;
using Cameo.NonMono;

namespace Cameo.Mono
{
    [System.Serializable]
    public struct PlayerData
    {
        public PlayerData(string playerName, int playerID)
        {
            PlayerName = playerName;
            PlayerID = playerID;
        }

        public string PlayerName;
        public int PlayerID;
    }

    public class Player : MonoBehaviour
    {
        // Public properties

        public Hand Hand { get; private set; }
        public CardDock Dock { get; private set; }

        // Private methods

        private void Start()
        {
            Hand = new Hand();
            Dock = CardDock.CreateCardDock(transform.position, Hand);
        }

        // Public methods

        public void Play()
        {
            
        }
    }
}