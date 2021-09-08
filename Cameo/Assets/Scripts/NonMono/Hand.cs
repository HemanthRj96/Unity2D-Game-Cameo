using System.Collections.Generic;
using UnityEngine;

namespace Cameo.NonMono
{
    public class Hand
    {
        public int currentHandSize => cardList.Count;
        public const int INITIAL_HAND_SIZE = 4;
        private List<Card> cardList = new List<Card>();

        /// <summary>
        /// Returns the total value of the hand
        /// </summary>
        public int getCurrentHandValue()
        {
            int value = 0;
            foreach (Card card in cardList)
                value += card.actualValue;
            return value;
        }

        /// <summary>
        /// To add a card to the hand
        /// </summary>
        /// <param name="card">Target card to be added</param>
        public void addCardToHand(Card card)
        {
            if (cardList.Contains(card))
                return;
            cardList.Add(card);
        }

        /// <summary>
        /// To remove a card from the hand
        /// </summary>
        /// <param name="card">Target card to be removed</param>
        public void removeCardFromHand(Card card)
        {
            if (!cardList.Contains(card))
                return;
            cardList.Remove(card);
        }
    }
}