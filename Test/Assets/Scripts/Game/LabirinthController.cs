using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game {

    public class LabirinthController : MonoBehaviour {

        [SerializeField]
        private List<LabirinthPiece> _piecesPrefabs;
        [SerializeField]
        private int _duplicatesCount;

        [SerializeField]
        private float distanceToDisable;

        private List<LabirinthPiece> _pieces;
        private List<LabirinthPiece> _activePieces;

        private LabirinthPiece _currentPiece;

        private bool _canStartHideEvent;

        [SerializeField]
        private float _timeBeforStartEvent;

        private void Awake() {
            _pieces = new List<LabirinthPiece>();
            _activePieces = new List<LabirinthPiece>();
            InstantiatePieces();
            SpawnPiece(Vector3.zero, new LabirinthPiece.SideDirection[] {
                LabirinthPiece.SideDirection.Left,
                LabirinthPiece.SideDirection.Right,
                LabirinthPiece.SideDirection.Forward,
                LabirinthPiece.SideDirection.Back
            });
            StartCoroutine(DelayedStartHideEvent());
        }

        private System.Collections.IEnumerator DelayedStartHideEvent() {
            _canStartHideEvent = false;
            yield return new WaitForSeconds(_timeBeforStartEvent);
            _canStartHideEvent = true;
        }

        private void InstantiatePieces() {
            foreach (var prefab in _piecesPrefabs) {
                for (int i = 0; i <= _duplicatesCount; i++) {
                    var instance = Instantiate(prefab);
                    instance.gameObject.SetActive(false);
                    _pieces.Add(instance);
                }
            }
        }

        private LabirinthPiece SpawnPiece(Vector3 position, LabirinthPiece.SideDirection[] sides) {
            var piecesToSpawn = _pieces.Where(piece => piece.Passages.Count >= sides.Length && sides.Where(side => piece.Passages.Contains(side)).Count() == sides.Length).ToArray();
            var piece = piecesToSpawn.First();
            if (piecesToSpawn.Length > 1) {
                var randomIndex = UnityEngine.Random.Range(0, piecesToSpawn.Length);
                piece = piecesToSpawn[randomIndex];
            }
            piece.transform.position = position;
            piece.gameObject.SetActive(true);
            _activePieces.Add(piece);
            _pieces.Remove(piece);
            piece.onActivate = ActivatePiece;
            return piece;
        }

        private void ActivatePiece(LabirinthPiece labirinthPiece) {
            _currentPiece = labirinthPiece;
            foreach (LabirinthPiece.SideDirection side in Enum.GetValues(typeof(LabirinthPiece.SideDirection))) {
                if (labirinthPiece.connectedPieces.Count > 0 && labirinthPiece.connectedPieces.ContainsKey(side)
                    && labirinthPiece.connectedPieces[side] != null && labirinthPiece.connectedPieces[side].gameObject.activeSelf) {
                    continue;
                }
                if (labirinthPiece.Passages.Contains(side)) {
                    var oppoziteSide = LabirinthPiece.GetOppositeDirection(side);
                    var piece = SpawnPiece(labirinthPiece.GetSidePosition(side), new LabirinthPiece.SideDirection[] {
                       oppoziteSide
                    });
                    if (piece == null) {
                        continue;
                    }
                    labirinthPiece.connectedPieces[side] = piece;
                    piece.connectedPieces[oppoziteSide] = labirinthPiece;
                }
            }
            if (UnityEngine.Random.Range(0, 5) != 0 && !PlayerMoveController.isDead && !DogController.isActive && _canStartHideEvent && !LeshyHealth.isDead) {
                if(!HideItem.hideItemActive) {
                    var isHideStart = labirinthPiece.TryToStartHideEvent();
                    Debug.Log($"hide event result: {isHideStart}");
                    if (isHideStart) {
                        StartCoroutine(DelaydDogSpawn());
                    }
                }
            }
            DisablePieces(labirinthPiece);
        }

        private System.Collections.IEnumerator DelaydDogSpawn() {
            yield return new WaitForSeconds(15);
            if(!DogController.isActive && !LeshyHealth.isDead && !PlayerMoveController.isDead) {
                _currentPiece.TryToStartDogEvent();
            }
        }

        private void DisablePieces(LabirinthPiece activeLabirinthPiece) {
            for (int i = _activePieces.Count - 1; i >= 0; i--) {
                if (Vector3.Distance(_activePieces[i].gameObject.transform.position, activeLabirinthPiece.gameObject.transform.position) >= distanceToDisable) {
                    _activePieces[i].gameObject.SetActive(false);
                    _pieces.Add(_activePieces[i]);
                    foreach (var piece in _activePieces[i].connectedPieces) {
                        if (piece.Value != null) {
                            piece.Value.connectedPieces[LabirinthPiece.GetOppositeDirection(piece.Key)] = null;
                        }
                    }
                    _activePieces[i].connectedPieces = new Dictionary<LabirinthPiece.SideDirection, LabirinthPiece>();
                    _activePieces[i].onActivate -= ActivatePiece;
                    _activePieces.RemoveAt(i);
                }
            }
        }
    }
}