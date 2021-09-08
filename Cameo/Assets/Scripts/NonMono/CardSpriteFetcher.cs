using UnityEngine;
using System.Collections.Generic;

namespace Cameo.NonMono
{
    public static class CardSpriteFetcher
    {
        private static bool isSpriteCached = false;
        private static Dictionary<Card, Sprite> cardSprites = new Dictionary<Card, Sprite>();

        /// <summary>
        /// Method returns sprite of the respective card
        /// </summary>
        /// <param name="card">Target card</param>
        /// <returns>Returns sprite if found and null otherwise</returns>
        public static Sprite FetchSprite(Card card)
        {
            if (!isSpriteCached)
                LoadSprites();

            // This error shouldn't occur and if it does something is wrong with the card loader
            if(!cardSprites.ContainsKey(card))
            {
                Debug.LogError("UKNOWN BEHAVIOUR!!");
                return null;
            }
            return cardSprites[card];
        }

        /// <summary>
        /// Loads the dictionary with the sprites
        /// </summary>
        private static void LoadSprites()
        {

        }
    }
}