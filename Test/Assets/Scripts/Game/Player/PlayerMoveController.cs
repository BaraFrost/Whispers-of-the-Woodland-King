using System;
using UnityEngine;

namespace Game {

    public class PlayerMoveController : MonoBehaviour {

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _runSpeed;

        [SerializeField]
        private LayerMask _groundLayerMask;

        [SerializeField]
        private float _rotationSmooth;

        [SerializeField]
        private Bullet _bullet;

        [SerializeField]
        private Transform _shootPoint;

        private bool _isDead;

        private Vector3 _mouseWorldPosition = new Vector3();

        private Vector3 _moveDirection;

        private float Speed {
            get {
                if (Input.GetKey(KeyCode.LeftShift)) {
                    return _runSpeed;
                }
                return _speed;
            }
        }

        private Rigidbody _rigidbody;
        private Camera _camera;

        private void Start() {
            _rigidbody = GetComponent<Rigidbody>();
            _camera = Camera.main;
        }

        private void Update() {
            if (_isDead) {
                return;
            }
            if (Input.GetMouseButtonDown(0)) {
                Shoot();
            }
            HandleMoveVector();
        }

        private void Shoot() {
            Instantiate(_bullet, _shootPoint.transform.position, Quaternion.LookRotation(_shootPoint.transform.forward, Vector3.up));
        }

        public void Rotate() {
            if (!Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hitInfo, 100f, _groundLayerMask.value)) {
                return;
            }
            _mouseWorldPosition = Vector3.Lerp(_mouseWorldPosition, new Vector3(hitInfo.point.x, gameObject.transform.position.y, hitInfo.point.z), Time.deltaTime * _rotationSmooth);
            gameObject.transform.LookAt(_mouseWorldPosition, Vector3.up);
        }

        private void HandleMoveVector() {
            var moveVectorX = Input.GetAxis("Horizontal");
            var moveVectorZ = Input.GetAxis("Vertical");
            _moveDirection = new Vector3(moveVectorX, 0, moveVectorZ) * Speed;
        }

        private void FixedUpdate() {
            if (_isDead) {
                return;
            }
            _rigidbody.velocity = _moveDirection;
            Rotate();
        }

        public void SetDamage() {
            _isDead = true;
        }
    }
}
