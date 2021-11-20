using Cameo.NonMono;
using System;
using System.Collections;
using UnityEngine;

namespace Cameo.Mono
{
    public class CardObject : MonoBehaviour
    {
        // Private fields

        private SpriteRenderer _spriteRenderer;
        private Sprite _activeSprite = null;
        private Sprite _cardFront = null;
        private Sprite _cardBack = null;
        private bool _canFlip = true;
        private readonly float _flipDuration = 0.1f;

        // Properties

        public Card Card { get; private set; } = new Card();
        public bool IsFaceDown { get { return _activeSprite == _cardBack; } }

        // Private methods

        private IEnumerator cardFlipper(float flipDuration)
        {
            _canFlip = false;
            float currentXScale = transform.localScale.x;
            float timer = 0;
            bool direction = false;
            while (true)
            {
                yield return new WaitForEndOfFrame();
                timer += Time.deltaTime;
                float fraction = Mathf.Clamp(timer * 2 / flipDuration, 0, 1);
                if (direction == false)
                {
                    transform.localScale = new Vector3(Mathf.Lerp(currentXScale, 0, fraction), transform.localScale.y);
                    if (fraction == 1)
                    {
                        if (IsFaceDown)
                        {
                            _spriteRenderer.sprite = _cardFront;
                            _activeSprite = _cardFront;
                        }
                        else
                        {
                            _spriteRenderer.sprite = _cardBack;
                            _activeSprite = _cardBack;
                        }
                        direction = true;
                        timer = 0;
                        continue;
                    }
                }
                else
                {
                    transform.localScale = new Vector3(Mathf.Lerp(0, currentXScale, fraction), transform.localScale.y);
                    if (fraction == 1)
                        break;
                }
            }
            _canFlip = true;
        }

        // Public method

        public static CardObject CreateNewCardObject(Card card)
        {
            var instance = new GameObject(card.ToString(), typeof(CardObject), typeof(SpriteRenderer), typeof(BoxCollider2D));
            var cardObject = instance.GetComponent<CardObject>();
            var collider = instance.GetComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(1, 1.5f);
            cardObject.SetCard(card);
            return cardObject;
        }

        public void SetCard(Card card)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Card = card;
            _cardFront = CardSpriteFetcher.FetchSprite(Card);
            _cardBack = CardSpriteFetcher.FetchSprite(new Card(ESuits.blank, EValues.defaultNull));
            _activeSprite = _cardBack;
            _spriteRenderer.sprite = _activeSprite;
        }

        public void FlipCard()
        {
            if (_canFlip)
                StartCoroutine(cardFlipper(_flipDuration));
        }

        private void OnMouseUpAsButton()
        {
            FlipCard();
        }
    }
}