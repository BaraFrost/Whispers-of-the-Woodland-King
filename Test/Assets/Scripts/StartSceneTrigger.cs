using System;
using UnityEngine;

namespace Game {

    public class StartSceneTrigger : MonoBehaviour {

        public Action onPlayerEnter;

        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.TryGetComponent<PlayerMoveController>(out var player)) {
                onPlayerEnter?.Invoke();
            }
        }
    }
}

