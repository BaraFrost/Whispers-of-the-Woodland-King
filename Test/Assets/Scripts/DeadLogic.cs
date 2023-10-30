using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game {

    public class DeadLogic : MonoBehaviour {

        [SerializeField]
        private AudioSource _deadAudio;

        [SerializeField]
        private GameObject _deadScreen;

        [SerializeField]
        private Button _restartButton;

        [SerializeField]
        private Button _menuButton;

        [SerializeField]
        private float _timeBeforeActivateDeadScreen;

        private bool _isActive = false;

        private void Start () {
            _restartButton.onClick.AddListener(Restart);
            _menuButton.onClick.AddListener(Menu);
        }

        private void Update() {
            if(_isActive) {
                return;
            }
            if(PlayerMoveController.isDead) {
                _isActive = true;
                _deadAudio.Play();
                StartCoroutine(DeadScreenCoroutine());
            }
        }

        private IEnumerator DeadScreenCoroutine() {
            yield return new WaitForSeconds(_timeBeforeActivateDeadScreen);
            _deadScreen.SetActive(true);
        }

        private void Restart() {
            GameSceneManager.instance.RestartThisScene();
        }

        private void Menu() {
            GameSceneManager.instance.LoadMenuScene();
        }
    }
}
