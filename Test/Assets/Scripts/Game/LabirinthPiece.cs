using System.Collections.Generic;
using UnityEngine;

namespace Game {

    public class LabirinthPiece : MonoBehaviour {

        public enum SideDirection {
            Left,
            Right,
            Forward,
            Back,
        }

        public struct Side {
            public SideDirection Direction;
            public bool isActive;
        }

        [SerializeField]
        private Side[] _sides = new Side[]{
            new Side(){ Direction = SideDirection.Left },
            new Side(){ Direction = SideDirection.Right },
            new Side(){ Direction = SideDirection.Forward},
            new Side(){ Direction = SideDirection.Back },
        };

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

        
    }
}
