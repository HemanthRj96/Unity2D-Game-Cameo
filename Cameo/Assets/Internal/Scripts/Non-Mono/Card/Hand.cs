using System.Collections.Generic;
using UnityEngine;

namespace Cameo.NonMono
{

    public class Hand
    {
        // Private fields

        private List<Card> _hand = new List<Card>();

        // Properties

        public int HandSize => _hand.Count;

        // Public methods

        public void AddCard(Card card) => _hand.TryAdd(card);

        public void RemoveCard(Card card) => _hand.TryRemove(card);

        public void TruncateHand() => _hand.Clear();

        public void ReplaceCard(Card oldCard, Card newCard)
        {
            _hand.TryRemove(oldCard);
            _hand.TryAdd(newCard);
        }

        public int GetHandValue()
        {
            int value = 0;
            _hand.ForEach(card => value += card.CardValue);
            return Mathf.Max(0, value); ;
        }

        public Card[] GetHand() => _hand.ToArray();
    }
}