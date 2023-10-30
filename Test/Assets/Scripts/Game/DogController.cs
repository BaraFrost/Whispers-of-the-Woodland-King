using System.Collections;
using UnityEngine;

namespace Game {

    public class DogController : MonoBehaviour {

        [SerializeField]
        private AudioSource _startAudio;

        [SerializeField]
        private AudioSource _wovAudio;
        [SerializeField]
        private AudioSource _attackAudio;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _lifeTime;

        private PlayerMoveController _player;

        [SerializeField]
        private float _delayBeforeAttack;

        [SerializeField]
        private float _distanceToAttack;

        private Rigidbody _rigidbody;

        private Animator _animator;

        [SerializeField]
        private int _lifeCount;

        [SerializeField]
        private float _jumpTime;

        [SerializeField]
        private float _jumpForce;

        private bool _isJumping;

        private bool _inited;

        public static bool isActive;

        [SerializeField]
        public DestroyableEffect _damageEffect;
        [SerializeField]
        public DestroyableEffect _destoyEffect;


        private void Start() {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            StartCoroutine(LifeTimeCoroutnie());
          //  Init(FindObjectOfType<PlayerMoveController>());
        }

        private IEnumerator LifeTimeCoroutnie() {
            yield return new WaitForSeconds(_lifeTime);
            Destroy(gameObject);
            Instantiate(_destoyEffect, gameObject.transform.position, Quaternion.identity);
        }

        public void Init(PlayerMoveController player) {
            _player = player;
            isActive = true;
            StartCoroutine(StartCoroutine());
        }

        private IEnumerator StartCoroutine() {
            _startAudio.Play();
            yield return new WaitForSeconds(_delayBeforeAttack);
            _wovAudio.Play();
            _inited = true;
        }

        private void FixedUpdate() {
            if(!_inited) {
                return;
            }
            if (_isJumping) {
                return;
            }
            _rigidbody.rotation = Quaternion.LookRotation(_player.gameObject.transform.position - gameObject.transform.position, Vector3.up);
            _rigidbody.velocity = _speed * transform.forward;
            if (Vector3.Distance(gameObject.transform.position, _player.transform.position) <= _distanceToAttack) {
                StartCoroutine(JumpCoroutine());
            }
        }

        private IEnumerator JumpCoroutine() {
            _isJumping = true;
            _animator.Play("Jump");
            _attackAudio.Play();
            _rigidbody.AddForce(gameObject.transform.forward * _jumpForce, ForceMode.Force);
            yield return new WaitForSeconds(_jumpTime);
            _isJumping = false;
            //_animator.Play("Crowl");
        }

        private void OnDestroy() {
            isActive = false;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<Bullet>(out var bullet)) {
                _lifeCount--;
                if (_lifeCount <= 0) {
                    Instantiate(_destoyEffect, gameObject.transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                else {
                    Instantiate(_damageEffect, gameObject.transform.position, Quaternion.identity);
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

