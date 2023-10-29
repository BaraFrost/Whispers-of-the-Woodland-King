using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField]
        private Slider _staminaSlider;

        [SerializeField]
        private float _timeToReduceStaminaBeforRunAgain;

        [SerializeField]
        private float _maxStamina;

        [SerializeField]
        private float _staminaDecriseSpeed;

        [SerializeField]
        private float _staminaIncreaseSpeed;

        [SerializeField]
        private float _reloadTime;

        [SerializeField]
        private int _ammoCount;

        [SerializeField]
        private Image _ammoImage;

        [SerializeField]
        private TextMeshProUGUI _text;

        private float _currentReloadTime;

        private float _currentStamina;

        public static bool isDead;

        private Vector3 _mouseWorldPosition = new Vector3();

        private Vector3 _moveDirection;

        private float Speed {
            get {
                if (_isRuning) {
                    return _runSpeed;
                }
                return _speed;
            }
        }

        private float _timeBeforReduceStamina;

        private bool _canRun => _currentStamina >= 0 && _timeBeforReduceStamina <= 0;
        private bool _isRuning => _canRun && Input.GetKey(KeyCode.LeftShift);

        private Rigidbody _rigidbody;
        private Camera _camera;

        private void Start() {
            _rigidbody = GetComponent<Rigidbody>();
            _camera = Camera.main;
            _currentStamina = _maxStamina;
            _currentReloadTime = _reloadTime;
        }

        private void Update() {
            if (isDead) {
                return;
            }
            if (Input.GetMouseButtonDown(0)) {
                Shoot();
            }
            HandleMoveVector();
            UpdateStamina();
            UpdateAmmo();
        }

        private void UpdateAmmo() {
            _ammoImage.fillAmount = _currentReloadTime / _reloadTime;
            if (_currentReloadTime < _reloadTime) {
                _currentReloadTime += Time.deltaTime;
                if (_currentReloadTime > _reloadTime) {
                    _currentReloadTime = _reloadTime;
                }
            }
            _text.text = _ammoCount.ToString();
        }

        private void UpdateStamina() {
            _staminaSlider.value = _currentStamina / _maxStamina;
            if (_isRuning) {
                _currentStamina -= Time.deltaTime * _staminaDecriseSpeed;
                if(_currentStamina <= 0) {
                    _timeBeforReduceStamina = _timeToReduceStaminaBeforRunAgain;
                }
            }
            if (_timeBeforReduceStamina > 0) {
                _timeBeforReduceStamina -= Time.deltaTime;
            }
            if (!_isRuning && _currentStamina < _maxStamina) {
                _currentStamina += Time.deltaTime * _staminaIncreaseSpeed;
                if (_currentStamina > _maxStamina) {
                    _currentStamina = _maxStamina;
                }
            }
        }



        private void Shoot() {
            if (_currentReloadTime < _reloadTime || _ammoCount <= 0) {
                return;
            }
            _ammoCount--;
            Instantiate(_bullet, _shootPoint.transform.position, Quaternion.LookRotation(_shootPoint.transform.forward, Vector3.up));
            _currentReloadTime = 0;
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
                if (_isRuning) {
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
            if (isDead) {
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
