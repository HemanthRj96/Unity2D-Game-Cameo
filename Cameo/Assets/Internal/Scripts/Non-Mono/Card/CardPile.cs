using System.Collections.Generic;
using System.Linq;
using Cameo;

namespace Cameo.NonMono
{
    public class CardPile
    {
        // Private fields

        private List<Card> _deckPile = new List<Card>();
        private List<Card> _discardPile = new List<Card>();
        private List<Card> _playingPile = new List<Card>();

        // Properties

        public int DeckPileCount => _deckPile.Count;
        public int DiscardPileCount => _discardPile.Count;

        // Private methods

        private void shufflePile() => _deckPile.Shuffle();

        private void fullDeckReset()
        {
            _deckPile.Clear();
            _discardPile.Clear();
            _playingPile.Clear();
        }

        // Public methods

        public void CreateNewDeck()
        {
            fullDeckReset();
            for (int i = 1; i <= 4; ++i)
                for (int j = 1; j <= 13; ++j)
                    _deckPile.Add(new Card((ESuits)i, (EValues)j));
            _deckPile.Add(new Card(ESuits.blank, EValues.joker_1));
            _deckPile.Add(new Card(ESuits.blank, EValues.joker_2));
            shufflePile();
        }

        public void RecreateDeck()
        {
            _deckPile.AddRange(_discardPile);
            _discardPile.Clear();
            shufflePile();
        }

        public Card Draw(EPile pile = EPile.DeckPile)
        {
            Card card = new Card();

            switch (pile)
            {
                case EPile.DeckPile:
                    if (DeckPileCount != 0)
                    {
                        card = _deckPile.Last();
                        _deckPile.Remove(card);
                    }
                    break;
                case EPile.DiscardPile:
                    if (DiscardPileCount > 0)
                    {
                        card = _discardPile.Last();
                        _discardPile.Remove(card);
                    }
                    break;
            }

            return card;
        }

        public void Discard(Card newCard)
        {
            _discardPile.Add(newCard);
        }
    }
}