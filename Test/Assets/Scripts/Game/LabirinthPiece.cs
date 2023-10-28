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
        private SideDirection _passagesDirection;
        public SideDirection PassagesDirection => _passagesDirection;
       /* private HashSet<SideDirection> _passagesCache;
        public HashSet<SideDirection> Passages => _passagesCache;*/

        public Dictionary<SideDirection, LabirinthPiece> _connectedPieces = new Dictionary<SideDirection, LabirinthPiece>();

        public Action<LabirinthPiece> onActivate;

        private void Awake() {
            //_passagesCache = _passages.ToHashSet();
            foreach (var passage in _passages) {
                _passagesDirection |= passage;
            }
        }

        private Vector3 SideDirectionToVector(SideDirection sideDirection) {
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

        private Vector3 GetSidePosition() {
            return Vector3.zero;
        }

        private void OnCollisionEnter(Collision collision) {
            if(collision.gameObject.TryGetComponent<PlayerMoveController>(out var player)) {
                onActivate?.Invoke(this);
            }
        }
    }
}
