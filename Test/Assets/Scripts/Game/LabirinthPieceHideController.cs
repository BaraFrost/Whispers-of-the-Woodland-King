using UnityEngine;

namespace Game {

    public class LabirinthPieceHideController : MonoBehaviour {

        [SerializeField]
        private HideItem[] _hideItems;

        private DogSpawnPoint[] _spawnPoints;

        [SerializeField]
        private DogController _dogController;

        private void Awake() {
            _hideItems = GetComponentsInChildren<HideItem>();
            _spawnPoints = GetComponentsInChildren<DogSpawnPoint>();
        }

        public bool TryToSpawnEnemy(PlayerMoveController playerMoveController) {
            if(_hideItems == null || _hideItems.Length == 0) {
                return false;
            }
            var randomIndex = Random.Range(0, _hideItems.Length);
            _hideItems[randomIndex].Activate(playerMoveController);
            return true;
        }


        public bool TryToSpawnDog(PlayerMoveController playerMoveController) {
            if (_spawnPoints == null || _spawnPoints.Length == 0) {
                return false;
            }
            var randomIndex = Random.Range(0, _spawnPoints.Length);
            var dog = Instantiate(_dogController, _spawnPoints[randomIndex].transform.position, Quaternion.identity);
            dog.Init(playerMoveController);
            return true;
        }
    }
}

