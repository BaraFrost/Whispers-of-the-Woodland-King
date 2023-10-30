using UnityEngine;

namespace Game {

    [CreateAssetMenu(fileName = "HideSettings", menuName = "HideSettings")]
    public class HideSettings : ScriptableObject {

        public float hideDistance;
        public float timeBeforeAttack;

        public AudioClip _heartAudio;

        public LeshiyController leshiy;

        public DestroyableEffect hideItemeDestroyEffect;
    }
}

