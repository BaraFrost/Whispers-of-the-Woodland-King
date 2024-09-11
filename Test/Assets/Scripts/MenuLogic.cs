using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class MenuLogic : MonoBehaviour {

        [SerializeField]
        private Button _newGameButton;

        [SerializeField]
        private Button _continueButton;

        [SerializeField]
        private Button _exiteButton;

        [SerializeField]
        private GameData _gameData;

        private void Start() {
            if (_gameData.isFirstGame) {
                _continueButton.gameObject.SetActive(false);
            } else {
                _continueButton.gameObject.SetActive(true);
            }
            _newGameButton.onClick.AddListener(OpenStartGameScene);
            _continueButton.onClick.AddListener(Continue);
            _exiteButton.onClick.AddListener(ExiteGame);
        }

        private void OpenStartGameScene() {
            _gameData.isFirstGame = false;
            GameSceneManager.instance.LoadStartGameScene();
        }

        private void Continue() {
            GameSceneManager.instance.LoadGameScene();
        }

        private void ExiteGame() {
            Application.Quit();
        }
    }
}
