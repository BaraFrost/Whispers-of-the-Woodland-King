using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game {

    public class GameSceneManager : MonoBehaviour {

        public static GameSceneManager instance = null; 

        private void Start() {
            if (instance == null) { 
                instance = this; 
            } else if (instance == this) { 
                Destroy(gameObject); 
            }
            DontDestroyOnLoad(gameObject);
        }

        public void LoadStartGameScene() {
            SceneManager.LoadScene(1);
        }

        public void LoadMenuScene() {
            SceneManager.LoadScene(0);
        }

        public void LoadTutorialScene() {
            SceneManager.LoadScene(2);
        }

        public void LoadGameScene() {
            SceneManager.LoadScene(3);
        }

        public void RestartThisScene() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

