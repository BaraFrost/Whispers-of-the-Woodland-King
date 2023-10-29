using System.Collections;
using UnityEngine;

namespace Game {

    public class HideItem : MonoBehaviour {

        [System.Serializable]
        public struct Sign {
            public GameObject signPrefab;
            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale;
        }

        public static bool hideItemActive;

        [SerializeField]
        private Sign[] _signs;

        private GameObject _signInstance;
        private PlayerMoveController _player;

        [SerializeField]
        private HideSettings _hideSettings;

        private bool _isActive = false;

        private IEnumerator _attackCoroutine;

        private AudioSource _audioSource;

        private void Awake() {
            _audioSource = GetComponent<AudioSource>();
            Activate(FindObjectOfType<PlayerMoveController>());
        }

        public void Activate(PlayerMoveController player) {
            var randomIndex = Random.Range(0, _signs.Length);
            var sign = _signs[randomIndex];
            _signInstance = Instantiate(sign.signPrefab, sign.position, Quaternion.identity, gameObject.transform);
            _signInstance.transform.localPosition = sign.position;
            _signInstance.transform.localRotation = Quaternion.Euler(sign.rotation);
            _signInstance.transform.localScale = sign.scale;
            _player = player;
            _isActive = true;
            hideItemActive = true;
        }

        private void Update() {
            if (!_isActive) {
                return;
            }
            if (_attackCoroutine == null && IsPlayerInActivationRadius()) {
                _attackCoroutine = AttackCoroutine();
                StartCoroutine(_attackCoroutine);
            }
        }

        private IEnumerator AttackCoroutine() {
            _audioSource.clip = _hideSettings._heartAudio;
            _audioSource.maxDistance = _hideSettings.hideDistance;
            _audioSource.Play();
            yield return new WaitForSeconds(_hideSettings.timeBeforeAttack);
            if (IsPlayerInActivationRadius()) {
                var leshiy = Instantiate(_hideSettings.leshiy, gameObject.transform.position, Quaternion.identity);
                leshiy.Init(_player);
            }
            Deactivate();
        }

        private bool IsPlayerInActivationRadius() {
            return Vector3.Distance(_player.gameObject.transform.position, gameObject.transform.position) <= _hideSettings.hideDistance;
        }

        public void Deactivate() {
            if(_signInstance != null) {
                Destroy(_signInstance);
            }
            if (_attackCoroutine != null) {
                StopCoroutine(_attackCoroutine);
            }
            if(_audioSource != null) {
                _audioSource.Stop();
            }
            _isActive = false;
            hideItemActive = false;
        }

        private void OnDisable() {
            Deactivate();
        }
    }
}
