using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Game.LabirinthPiece;

namespace Game {

    public class LabyrinthController : MonoBehaviour {

        [SerializeField]
        private List<LabirinthPiece> _piecesPrefabs;
        [SerializeField]
        private int _duplicatesCount;

        private List<LabirinthPiece> _pieces;
        private List<LabirinthPiece> _activePieces;

        private void Awake() {
            _pieces = new List<LabirinthPiece>();
            _activePieces = new List<LabirinthPiece>();
            InstantiatePieces();
            SpawnPiece(Vector3.zero, SideDirection.Back | SideDirection.Right | SideDirection.Left | SideDirection.Forward);
        }

        private void InstantiatePieces() {
            foreach (var prefab in _activePieces) {
                for (int i = 0; i <= _duplicatesCount; i++) {
                    var instance = Instantiate(prefab);
                    instance.gameObject.SetActive(false);
                    _pieces.Add(instance);
                }
            }
        }

        private LabirinthPiece SpawnPiece(Vector3 position, SideDirection side) {
            var piecesToSpawn = _pieces.Where(piece => piece.PassagesDirection == side).ToArray();
            var piece = piecesToSpawn.FirstOrDefault();
            if(_pieces.Count > 1) {
                piece = piecesToSpawn[Random.Range(0, piecesToSpawn.Length)];
            }
            piece.gameObject.SetActive(true);
            piece.transform.position = position;
            _activePieces.Add(piece);
            _pieces.Remove(piece);
            piece.onActivate = ActivatePiece;
            return piece;
        }

        private void ActivatePiece(LabirinthPiece labirinthPiece) {

        }
    }
}
