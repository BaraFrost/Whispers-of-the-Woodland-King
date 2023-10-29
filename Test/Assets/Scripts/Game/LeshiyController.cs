using UnityEngine;

namespace Game {

    public class LeshiyController : MonoBehaviour {

        private Rigidbody _rigidbody;
        private PlayerMoveController _player;

        [SerializeField]
        private float _jumpSpeed;

        [SerializeField]
        private float _moveSpeed;

        [SerializeField]
        private AnimationCurve _jumpCurve;

        [SerializeField]
        private float _minDistanceDifference;

        [SerializeField]
        private float _stayDelay;

        private Vector3 _playerPosition;

        private Vector3 _startPosition;

        private bool _isInited;

        private bool _isAttacked;

        private Animator _animator;

        [SerializeField]
        private float _rotationSpeed;

        public void Init(PlayerMoveController player) {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _player = player;
            _playerPosition = _player.transform.position;
            _startPosition = gameObject.transform.position;
            _isInited = true;
            _animator.Play("Attack");
        }

        private void FixedUpdate() {
            if (!_isInited) {
                return;
            }
            if (!_isAttacked) {

                _rigidbody.rotation = Quaternion.LookRotation(_playerPosition - gameObject.transform.position, Vector3.up);
                var modificator = _jumpCurve.Evaluate(Vector3.Distance(gameObject.transform.position, _startPosition) / Vector3.Distance(_startPosition, _playerPosition));
                _rigidbody.velocity = modificator * _jumpSpeed * transform.forward;

                var distanceToTarget = Vector3.Distance(gameObject.transform.position, _playerPosition);
                if (distanceToTarget <= _minDistanceDifference) {
                    _isAttacked = true;
                }
            } else {
                _stayDelay -= Time.deltaTime;
                var targetRotationVector = _startPosition - gameObject.transform.position;
                var rotationVector = Vector2.Lerp(new Vector2(transform.forward.x, transform.forward.z), new Vector2(targetRotationVector.x, targetRotationVector.z), Time.fixedDeltaTime * _rotationSpeed);
                _rigidbody.MoveRotation(Quaternion.LookRotation(new Vector3(rotationVector.x, 0, rotationVector.y), Vector3.up));
                if (_stayDelay <= 0) {

                    _animator.Play("Run");
                    _rigidbody.velocity = _moveSpeed * transform.forward;
                    var distanceToTarget = Vector3.Distance(gameObject.transform.position, _startPosition);
                    if (distanceToTarget <= _minDistanceDifference) {
                        Destroy(gameObject);
                    }
                }
            }
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.TryGetComponent<PlayerMoveController>(out var player)) {
                player.SetDamage();
            }
        }
    }
}

