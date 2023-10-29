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

        [SerializeField]
        private Animator _playerAnimator;

        [SerializeField]
        private AudioSource _stepAudioSource;

        [SerializeField]
        private AudioSource _dyingAudio;

        [SerializeField]
        private float _defaultPitch;

        [SerializeField]
        private float _runPitch;

        public static bool isDead;

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
            if (isDead) {
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
            if (isDead) {
                return;
            }
            _rigidbody.velocity = _moveDirection;
            ExecuteEffects();
            Rotate();
        }

        private void ExecuteEffects() {
            if (_moveDirection != Vector3.zero) {
                if (!_stepAudioSource.isPlaying) {
                    _stepAudioSource.Play();
                }
                if (Input.GetKey(KeyCode.LeftShift)) {
                    _playerAnimator.ResetTrigger("Stay");
                    _playerAnimator.ResetTrigger("Go");
                    _playerAnimator.SetTrigger("Run");
                    _stepAudioSource.pitch = _runPitch;
                } else {
                    _playerAnimator.ResetTrigger("Stay");
                    _playerAnimator.ResetTrigger("Run");
                    _playerAnimator.SetTrigger("Go");
                    _stepAudioSource.pitch = _defaultPitch;
                }
            } else {
                if (_stepAudioSource.isPlaying) {
                    _stepAudioSource.Stop();
                }
                _playerAnimator.ResetTrigger("Go");
                _playerAnimator.ResetTrigger("Run");
                _playerAnimator.SetTrigger("Stay");
            }
        }

        public void SetDamage() {
            if(isDead) {
                return;
            }
            isDead = true;
            _stepAudioSource.Stop();
            _dyingAudio.Play();
            _playerAnimator.ResetTrigger("Go");
            _playerAnimator.ResetTrigger("Run");
            _playerAnimator.ResetTrigger("Stay");
            _playerAnimator.SetTrigger("Die");
        }
    }
}
