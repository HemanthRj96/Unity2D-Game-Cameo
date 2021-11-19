using System.Collections;
using UnityEngine;

namespace Cameo.Mono
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Player targetPlayerPrefab;

        public void instantiatePlayer()
        {
            Instantiate(targetPlayerPrefab);
        }
    }
}