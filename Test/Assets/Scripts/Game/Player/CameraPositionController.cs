using UnityEngine;

namespace Game {

    public class CameraPositionController : MonoBehaviour {

        [SerializeField]
        private PlayerMoveController _player;

        private Vector3 _cameraPlayerPositionDifference;

        void Start() {
            _cameraPlayerPositionDifference = _player.gameObject.transform.position - gameObject.transform.position;
        }

        void LateUpdate() {
            gameObject.transform.position = _player.gameObject.transform.position - _cameraPlayerPositionDifference;
            _player.Rotate();
        }
    }
}

