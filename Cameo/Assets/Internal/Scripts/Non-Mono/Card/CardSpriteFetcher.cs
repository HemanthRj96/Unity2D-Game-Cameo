using UnityEngine;
using System.Collections.Generic;


namespace Cameo.NonMono
{
    public static class CardSpriteFetcher
    {
        private static bool isSpriteCached = false;
        public static Dictionary<Card, Sprite> _cardSpriteslookup = new Dictionary<Card, Sprite>();

        public static Sprite FetchSprite(Card card)
        {
            if (isSpriteCached == false)
                loadSprites();
            if (_cardSpriteslookup.ContainsKey(card))
                return _cardSpriteslookup[card];
            return null;
        }

        private static void loadSprites()
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Card Sprites/card_sprites");
            List<Card> cards = new List<Card>();

            for (int i = 1; i <= 13; ++i)
                for (int j = 1; j <= 4; ++j)
                    cards.Add(new Card((ESuits)j, (EValues)i));
            cards.Add(new Card(ESuits.blank, EValues.joker_1));
            cards.Add(new Card(ESuits.blank, EValues.joker_2));
            cards.Add(new Card(ESuits.blank, EValues.defaultNull));

            for (int i = 0; i < 55; ++i)
                _cardSpriteslookup.Add(cards[i], sprites[i]);

            isSpriteCached = true;
        }
    }
}