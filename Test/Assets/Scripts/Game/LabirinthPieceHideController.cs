using UnityEngine;

namespace Game {

    public class LabirinthPieceHideController : MonoBehaviour {

        [SerializeField]
        private HideItem[] _hideItems;

        public bool TryToSpawnEnemy(PlayerMoveController playerMoveController) {
            if(_hideItems == null || _hideItems.Length == 0) {
                return false;
            }
            var randomIndex = Random.Range(0, _hideItems.Length);
            _hideItems[randomIndex].Activate(playerMoveController);
            return true;
        }
    }
}

