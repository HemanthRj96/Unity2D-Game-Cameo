using System.Collections;
using UnityEngine;
using Cameo.NonMono;

namespace Cameo.Mono
{
    public class Player : MonoBehaviour
    {
        private Hand hand;
        private HandManipulator handManipulator;
        private string playerName;
        private int score = 0;

        private void Awake()
        {
            hand = new Hand();
            handManipulator = new HandManipulator(hand);
        }

        /// <summary>
        /// Set the player name
        /// </summary>
        /// <param name="name">Target player name</param>
        public void setName(string name)
        {
            playerName = name;
        }

        /// <summary>
        /// Reset the player name
        /// </summary>
        public void resetName()
        {
            playerName.Remove(0);
        }

        /// <summary>
        /// Adds point to player score
        /// </summary>
        /// <param name="points">Target points we need to add</param>
        public void addScore(int points)
        {
            score += points;
        }

        /// <summary>
        /// Rests the score
        /// </summary>
        public void resetScore()
        {
            score = 0;
        }

        /// <summary>
        /// Return hand object
        /// </summary>
        public Hand getHand()
        {
            return hand;
        }

        /// <summary>
        /// Returns handManipulator object
        /// </summary>
        public HandManipulator getHandManipulator()
        {
            return handManipulator;
        }
    }
}