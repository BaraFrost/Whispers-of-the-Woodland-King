using UnityEngine;

namespace Game {

    public class Bullet : MonoBehaviour {

        [SerializeField]
        private Rigidbody _rigidbody;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _lifeTime;

        private void Start() {
            Destroy(gameObject, _lifeTime);
        }

        private void FixedUpdate() {
            _rigidbody.velocity = transform.forward * _speed;
        }

        private void OnTriggerEnter(Collider other) {
            if(other.TryGetComponent<HideItem>(out var hideItem)) {
                hideItem.SetDamage();
            }
        }
    }
}
