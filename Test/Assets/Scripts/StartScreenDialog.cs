using System.Collections;
using TMPro;
using UnityEngine;

namespace Game {

    public class StartScreenDialog : MonoBehaviour {

        [SerializeField]
        private float _fadeTime;

        [SerializeField]
        private TextMeshProUGUI _textMeshPro;

        [SerializeField]
        private float _showTime;

        public void Activate() {
            StartCoroutine(ShowTextCoroutine());
        }

        private IEnumerator ShowTextCoroutine() {
            var fadeTime = 0f;
            while (fadeTime < _fadeTime) {
                var color = _textMeshPro.color;
                color.a = fadeTime / _fadeTime;
                _textMeshPro.color = color;
                fadeTime += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(_showTime);
            while (fadeTime > 0) {
                var color = _textMeshPro.color;
                color.a = fadeTime / _fadeTime;
                _textMeshPro.color = color;
                fadeTime -= Time.deltaTime;
                yield return null;
            }
            _textMeshPro.gameObject.SetActive(false);
        }
    }
}
