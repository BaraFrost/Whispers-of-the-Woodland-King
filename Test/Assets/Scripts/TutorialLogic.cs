using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class TutorialLogic : MonoBehaviour {

        [SerializeField]
        private Button _button;

        private void Awake() {
            _button.onClick.AddListener(LoadGameScene);
        }

        private void LoadGameScene() {
            GameSceneManager.instance.LoadGameScene();
        }
    }
}

