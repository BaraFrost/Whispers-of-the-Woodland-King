using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class PauseLogic : MonoBehaviour {

        [SerializeField]
        private GameObject _pause;

        [SerializeField]
        private Button _restartButton;

        [SerializeField]
        private Button _returnButton;

        [SerializeField]
        private Button _menuButton;

        public static bool isPaused;

        private void Start () {
            _restartButton.onClick.AddListener(Restart);
            _menuButton.onClick.AddListener(Menu);
            _returnButton.onClick.AddListener(Return);
            isPaused = false;
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                if(_pause.activeSelf) {
                    Time.timeScale = 1.0f;
                    _pause.gameObject.SetActive(false);
                    isPaused = false;
                }
                else
                {
                    isPaused = true;
                    Time.timeScale = 0f;
                    _pause.gameObject.SetActive(true);
                }
            }
        }

        private void Menu() {
            GameSceneManager.instance.LoadMenuScene();
        }

        private void Return() {
            isPaused = false;
            Time.timeScale = 1.0f;
            _pause.gameObject.SetActive(false);
        }

        private void Restart() {
            GameSceneManager.instance.RestartThisScene();
        }

        private void OnDisable() {
            isPaused = false;
            Time.timeScale = 1.0f;
        }
    }
}

