using Cameo.Enums;
using Cameo.Mono;
using Cameo.NonMono;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Testing
{
    public class Testing_01 : MonoBehaviour
    {
        public List<Card> cardList = new List<Card>();

        float rotation = 0;
        CardDock cardDock = null;
        CardPile cardPile = new CardPile();


        private void Start()
        {
            cardDock = CardDock.CreateCardDock(transform.position);
            cardPile.CreateNewDeck();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Card card = cardPile.Draw(EPile.DeckPile);
                cardList.Add(card);
                cardDock.AddCard(card);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                int index = Random.Range(0, cardList.Count - 1);
                Card card = cardList[index];
                cardList.RemoveAt(index);
                cardDock.RemoveCard(card);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                StartCoroutine(cardDock.LerpCellSize(0.5f, 2f));
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                StartCoroutine(cardDock.LerpCellSize(0.5f, 1f));
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                rotation += 20;
                cardDock.ApplyRotation(rotation, Vector3.right);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                rotation -= 20;
                cardDock.ApplyRotation(rotation, Vector3.right);
            }
        }
    }
}