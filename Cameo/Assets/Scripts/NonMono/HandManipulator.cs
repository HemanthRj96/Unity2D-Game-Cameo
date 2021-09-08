using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameo.NonMono
{
    public class HandManipulator
    {
        private Dictionary<int, Card> cardDictionary = new Dictionary<int, Card>();
        private Hand controlledHand = null;

        /// <summary>
        /// Constructor to intialize the dictionary
        /// </summary>
        public HandManipulator(Hand hand)
        {
            controlledHand = hand;
            for (int i = 1; i <= 52; ++i)
                cardDictionary.Add(i, null);
        }

        /// <summary>
        /// Return true if the index is empty
        /// </summary>
        /// <param name="index">Target index we need to check</param>
        public bool isIndexEmpty(int index)
        {
            return cardDictionary[index] == null;
        }

        /// <summary>
        /// Returns the card from the target index
        /// </summary>
        /// <param name="index">Target index from where we need the card</param>
        public Card getCard(int index)
        {
            return cardDictionary[index];
        }

        /// <summary>
        /// Exchange cards to the target index
        /// </summary>
        /// <param name="index">Target index</param>
        /// <param name="targetCard">Card to exchange</param>
        /// <param name="returnCard">Card to be exchange with</param>
        public void exchangeCard(int index, Card targetCard,out Card returnCard)
        {
            controlledHand.addCardToHand(targetCard);
            controlledHand.removeCardFromHand(cardDictionary[index]);
            returnCard = cardDictionary[index];
            cardDictionary[index] = targetCard;
        }

        /// <summary>
        /// Removes the card from the dictionary
        /// </summary>
        /// <param name="index">Target index</param>
        public void removeCard(int index, out Card returnCard)
        {
            controlledHand.removeCardFromHand(cardDictionary[index]);
            returnCard = cardDictionary[index];
            cardDictionary[index] = null;
        }

        /// <summary>
        /// Adds a card to the target index
        /// </summary>
        /// <param name="index">Target index</param>
        /// <param name="targetCard">Target card</param>
        public void addCard(int index, Card targetCard)
        {
            controlledHand.addCardToHand(targetCard);
            cardDictionary[index] = targetCard;
        }
    }
}