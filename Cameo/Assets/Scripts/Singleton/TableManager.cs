using System.Collections.Generic;
using UnityEngine;
using Cameo.Utils;
using Cameo.NonMono;
using Cameo.Static;

namespace Cameo.Singleton
{
    public class TableManager : Singleton<TableManager>
    {
        private Dictionary<int, Card> deck = new Dictionary<int, Card>();
        private Dictionary<int, Card> usedDeck = new Dictionary<int, Card>();
        private Dictionary<int, Card> discardedPile = new Dictionary<int, Card>();
        private List<int> deckIndexer = new List<int>();

        // Cached suits
        private List<e_suits> suits = new List<e_suits>()
        {
            e_suits.diamond,
            e_suits.heart,
            e_suits.spade,
            e_suits.clubs
        };

        // Cached values
        private List<e_values> values = new List<e_values>()
        {
            e_values.seven,
            e_values.two,
            e_values.six,
            e_values.jack,
            e_values.ace,
            e_values.three,
            e_values.four,
            e_values.eight,
            e_values.king,
            e_values.nine,
            e_values.ten,
            e_values.five,
            e_values.queen
        };

        protected new void Awake()
        {
            base.Awake();
            ReferenceHandler.Add(this, "TableManager");
        }

        /// <summary>
        /// This method creates a standard deck which isn't shuffled
        /// </summary>
        private void createDeck()
        {
            int index = 1;
            foreach (e_suits suit in suits)
                foreach (e_values value in values)
                {
                    deck.Add(index, new Card(suit, value));
                    index++;
                }
        }

        /// <summary>
        /// To get the local index of the card
        /// </summary>
        /// <param name="card">Target card</param>
        private int findLocalCardIndex(Card card)
        {
            foreach (var temp in deck)
                if (temp.Value == card)
                    return temp.Key;
            Debug.LogError("This error shouldn't exist, check the deck data or the card data!!");
            return -1;
        }

        /// <summary>
        /// Call this function to get shuffle the deck without including the used cards
        /// </summary>
        public void shuffleDeck()
        {
            List<int> tempIndexList = new List<int>();
            deckIndexer.Clear();

            for (int i = 1; i <= 52; ++i)
            {
                if (usedDeck.ContainsKey(i))
                    continue;
                tempIndexList.Add(i);
            }

            while (true)
            {
                if (tempIndexList.Count == 1)
                {
                    deckIndexer.Add(tempIndexList[0]);
                    tempIndexList.Clear();
                    break;
                }
                else
                {
                    int index = Random.Range(0, tempIndexList.Count - 1);
                    deckIndexer.Add(tempIndexList[index]);
                    tempIndexList.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// This function return the top card in the deck
        /// </summary>
        public Card getTopCardFromDeck()
        {
            if (deckIndexer.Count == 0)
                return null;
            int index = deckIndexer[0];
            deckIndexer.RemoveAt(0);

            Card retCard = deck[index];
            usedDeck.Add(index, retCard);
            return retCard;
        }

        /// <summary>
        /// To add a card to the discard pile
        /// </summary>
        /// <param name="card">The target card to be added</param>
        public void addToDiscardPile(Card card)
        {
            discardedPile.Add(findLocalCardIndex(card), card);
        }

        /// <summary>
        /// Call this function to recollect all the cards from all the players
        /// </summary>
        public void clearPiles(bool discardPile = true, bool usedPile = true)
        {
            if (usedPile)
                usedDeck.Clear();
            if (discardPile)
                discardedPile.Clear();
        }

        /// <summary>
        /// Returns the top card from the discard pile
        /// </summary>
        public Card getTopCardFromDiscardedPile()
        {
            if (discardedPile.Count == 0)
                return null;
            return discardedPile[discardedPile.Count - 1];
        }
    }
}