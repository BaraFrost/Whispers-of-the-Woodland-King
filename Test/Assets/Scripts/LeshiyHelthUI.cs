using UnityEngine;
using UnityEngine.UI;

namespace Game {

    public class LeshiyHelthUI : MonoBehaviour {

        [SerializeField]
        private Image[] _images;

        [SerializeField]
        private LeshyHealth _leshyHealth;

        private void Start() {
            _leshyHealth._onHealtChanged += OnHealthChanged;
        }

        private void OnHealthChanged() {
            foreach(var image in _images) {
                image.gameObject.SetActive(false);
            }
            for(int i = 0; i < _leshyHealth.LeshiyHealth; i++) {
                _images[i].gameObject.SetActive(true);
            }
        }
    }
}
