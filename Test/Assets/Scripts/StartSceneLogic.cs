using System.Collections;
using UnityEngine;

namespace Game {

    public class StartSceneLogic : MonoBehaviour {

        [SerializeField]
        private StartSceneTrigger _textTrigger;

        [SerializeField]
        private StartSceneTrigger _darkTrigger;

        [SerializeField]
        private StartSceneTrigger _leshiyTrigger;

        [SerializeField]
        private StartScreenDialog _darkDialog;

        [SerializeField]
        private StartScreenDialog _startDialog;

        [SerializeField]
        private Light _light;

        [SerializeField]
        private PlayerMoveController _playerMoveController;

        [SerializeField]
        private AudioSource _leshiyAudio;

        [SerializeField]
        private float _endTime;

        [SerializeField]
        private float _lightOffSpeed;

        [SerializeField]
        private float _rotationSpeed;

        private void Awake() {
            _darkTrigger.onPlayerEnter += MakeDark;
            _textTrigger.onPlayerEnter += ShowDarkText;
            _leshiyTrigger.onPlayerEnter += LeshiyActivate;
        }

        private void Start() {
            _startDialog.Activate();
        }

        private void MakeDark() {
            _darkTrigger.onPlayerEnter -= MakeDark;
            StartCoroutine(LightFadeCoroutine());
        }

        private IEnumerator LightFadeCoroutine() {
            while (_light.intensity > 0) {
                _light.intensity -= Time.deltaTime * _lightOffSpeed;
                yield return null;
            }
        }

        private void ShowDarkText() {
            _textTrigger.onPlayerEnter -= ShowDarkText;
            _darkDialog.Activate();
        }

        private void LeshiyActivate() {
            _leshiyAudio.Play();
            _playerMoveController.enabled = false;
            var animator = _playerMoveController.GetComponentInChildren<Animator>();
            animator.ResetTrigger("Go");
            animator.ResetTrigger("Run");
            animator.SetTrigger("Stay");
            // _playerMoveController.gameObject.transform.LookAt(_leshiyAudio.gameObject.transform, Vector3.up);
            StartCoroutine(EndCoroutine());
        }

        private IEnumerator EndCoroutine() {
            var targetRotationVector = _leshiyAudio.gameObject.transform.position - _playerMoveController.transform.position;
            while (Vector3.Angle(targetRotationVector, _playerMoveController.transform.transform.forward) > 10) {
                targetRotationVector = _leshiyAudio.gameObject.transform.position - _playerMoveController.transform.position;
                var rotationVector = Vector2.Lerp(new Vector2(_playerMoveController.transform.forward.x, _playerMoveController.transform.forward.z), new Vector2(targetRotationVector.x, targetRotationVector.z), Time.deltaTime * _rotationSpeed);
                _playerMoveController.gameObject.transform.forward = new Vector3(rotationVector.x, 0, rotationVector.y);
                yield return null;
            }

            yield return new WaitForSeconds(_endTime);
            GameSceneManager.instance.LoadTutorialScene();
        }
    }
}

