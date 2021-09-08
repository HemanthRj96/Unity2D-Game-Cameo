using System.Collections;
using UnityEngine;
using Cameo.Static;

namespace Cameo.Mono
{
    public class UIHandler : MonoBehaviour
    {
        private void Awake()
        {
            ReferenceHandler.Add(this, "UIHandler");
        }

        public void activateUI()
        {

        }
    }
}