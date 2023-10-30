using UnityEngine;

namespace Game {

    public class DestroyableEffect : MonoBehaviour {

        [SerializeField]
        private float _timeToDestroy;

        private void Start() {
            Destroy(gameObject, _timeToDestroy);
        }
    }
}

