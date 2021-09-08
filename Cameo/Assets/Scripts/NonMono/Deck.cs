using Cameo.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameo.NonMono
{
    public class Deck
    {
        // Persistent suits and values for deck creation
        private List<e_suits> persistentSuits = new List<e_suits>()
        {
            e_suits.diamond,
            e_suits.heart,
            e_suits.spade,
            e_suits.clubs
        };
        private List<e_values> persistentValues = new List<e_values>()
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

        // The actual deck which gets initialized upon contruction
        private Dictionary<int, Card> deck = new Dictionary<int, Card>();
        // This is the used deck / card pile used by the players
        private Dictionary<int, Card> usedDeck = new Dictionary<int, Card>();
        // This is the discarded pile of cards
        private Dictionary<int, Card> discardedPile = new Dictionary<int, Card>();
        private int latestDiscardIndex = -1;
        // This list is used to shuffle the deck
        private List<int> localDeckIndexArray = new List<int>();

        private bool isDeckShuffled = false;

        public Deck()
        {
            createDeck();
        }

        /// <summary>
        /// Returns true if you can draw cards from the deck
        /// </summary>
        public bool canDraw()
        {
            return localDeckIndexArray.Count != 0;
        }

        /// <summary>
        /// Call this method to get the top card from the deck, if the return value is null then the deck is empty
        /// </summary>
        public Card drawFromDeck()
        {
            // Get the first element from the list
            int localIndex = localDeckIndexArray[0];

            // Remove the first element from the array
            localDeckIndexArray.RemoveAt(0);

            // Get the corresponding card from the deck
            Card returnCard = deck[localIndex];

            // Add this card to the used deck
            usedDeck.Add(localIndex, returnCard);

            return returnCard;
        }

        /// <summary>
        /// Call this method to get the top card from the discarded pile
        /// </summary>
        public Card drawFromPile()
        {
            if (discardedPile.Count == 0)
                return null;

            Card returnCard = null;
            int cachedIndex = latestDiscardIndex;
            latestDiscardIndex = -1;
            
            if (cachedIndex == -1)
                return null;

            // This condition is always true
            if (discardedPile.ContainsKey(cachedIndex))
            {
                Debug.Log("Removing from discarded pile");
                discardedPile.Remove(cachedIndex);
            }

            returnCard = deck[cachedIndex];
            Debug.Log("Adding card to the used deck");
            usedDeck.Add(cachedIndex, returnCard);



            return returnCard;
        }

        /// <summary>
        /// Call this method to add a card to the discarded pile
        /// </summary>
        /// <param name="card">Target card to be discarded</param>
        public void addCardToPile(Card card)
        {
            int localIndex = findLocalCardIndex(card);
            discardedPile.Add(localIndex, card);

            latestDiscardIndex = localIndex;
        }

        /// <summary>
        /// Shuffles all the cards inside the deck alone
        /// </summary>
        public void shuffleDeck(e_shuffles shuffleType = e_shuffles.totalDeckShuffle)
        {
            List<int> cardIndexArray = new List<int>();

            switch (shuffleType)
            {
                case e_shuffles.discardPile:
                    foreach (int i in discardedPile.Keys)
                        cardIndexArray.Add(i);
                    break;
                case e_shuffles.totalDeckShuffle:
                    for (int i = 1; i <= 52; ++i)
                        cardIndexArray.Add(i);
                    break;
            }

            localDeckIndexArray.Clear();
            while (true)
            {
                if (cardIndexArray.Count == 1)
                {
                    localDeckIndexArray.Add(cardIndexArray[0]);
                    cardIndexArray.Clear();
                    break;
                }
                else
                {
                    int index = Random.Range(0, cardIndexArray.Count - 1);
                    localDeckIndexArray.Add(cardIndexArray[index]);
                    cardIndexArray.RemoveAt(index);
                }
            }

            isDeckShuffled = true;
        }

        /// <summary>
        /// Collect the cards from players and discarded pile
        /// </summary>
        public void collectAllCards()
        {
            // Before doing this we also have to send this information to the GUI
            usedDeck.Clear();
            discardedPile.Clear();
            localDeckIndexArray.Clear();
            isDeckShuffled = false;
        }

        /// <summary>
        /// Local function to create a deck of cards
        /// </summary>
        private void createDeck()
        {
            int index = 1;
            foreach (e_suits suit in persistentSuits)
                foreach (e_values value in persistentValues)
                {
                    deck.Add(index, new Card(suit, value));
                    index++;
                }
            shuffleDeck();
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
    }
}