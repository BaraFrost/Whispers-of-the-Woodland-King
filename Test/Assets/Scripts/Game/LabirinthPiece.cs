using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game {

    public class LabirinthPiece : MonoBehaviour {

        public enum SideDirection {
            Left,
            Right,
            Forward,
            Back,
        }

        [SerializeField]
        private List<SideDirection> _passages;
        private HashSet<SideDirection> _passagesCache;
        public HashSet<SideDirection> Passages => _passagesCache;

        public Dictionary<SideDirection, LabirinthPiece> connectedPieces = new Dictionary<SideDirection, LabirinthPiece>();

        public Action<LabirinthPiece> onActivate;

        private PlayerMoveController _player;

        private LabirinthPieceHideController _hideController;

        private void Awake() {
            _passagesCache = _passages.ToHashSet();
            _hideController = GetComponent<LabirinthPieceHideController>();
        }

        public static Vector3 SideDirectionToVector(SideDirection sideDirection) {
            switch (sideDirection) {
                case SideDirection.Left:
                    return Vector3.left;
                case SideDirection.Right:
                    return Vector3.right;
                case SideDirection.Forward:
                    return Vector3.forward;
                case SideDirection.Back:
                    return Vector3.back;
            }
            return Vector3.zero;
        }

        public static SideDirection GetOppositeDirection(SideDirection sideDirection) {
            switch (sideDirection) {
                case SideDirection.Left:
                    return SideDirection.Right;
                case SideDirection.Right:
                    return SideDirection.Left;
                case SideDirection.Forward:
                    return SideDirection.Back;
                case SideDirection.Back:
                default:
                    return SideDirection.Forward;
            }
        }

        public Vector3 GetSidePosition(SideDirection sideDirection) {
            return SideDirectionToVector(sideDirection) * gameObject.transform.lossyScale.x + gameObject.transform.position;
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.TryGetComponent<PlayerMoveController>(out var player)) {
                onActivate?.Invoke(this);
                _player = player;
            }
        }

        public bool TryToStartHideEvent() {
            if(_player == null) {
                return false;
            }
            return _hideController.TryToSpawnEnemy(_player);
        }
    }
}
