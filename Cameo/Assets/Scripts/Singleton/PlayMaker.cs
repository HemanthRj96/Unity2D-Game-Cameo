using System.Collections;
using UnityEngine;
using Cameo.Mono;
using Cameo.NonMono;
using Cameo.Utils;
using Cameo.Static;

namespace Cameo.Singleton
{
    public class PlayMaker : Singleton<PlayMaker>
    {
        private TableManager tableManager = null;
        private Player cachedPlayer = null;
        private UIHandler uiHandler = null;

        private void Start()
        {
            tableManager = ReferenceHandler.GetReference("TableManager") as TableManager;
            uiHandler = ReferenceHandler.GetReference("UIHandler") as UIHandler;
        }

        public void beginPlay(Player player)
        {
            Card card = null;
            card = tableManager.getTopCardFromDeck();
            if(card == null)
            {
                tableManager.clearPiles(true, false);
                tableManager.shuffleDeck();
            }
            uiHandler.activateUI();
        }

        public void endPlay()
        {
            cachedPlayer = null;
        }

        public void discardCard()
        {

        }
    }
}