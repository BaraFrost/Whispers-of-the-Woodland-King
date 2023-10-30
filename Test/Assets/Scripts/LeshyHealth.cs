using System;
using UnityEngine;

public class LeshyHealth : MonoBehaviour
{
    [SerializeField]
    private int _leshiyHealth;
    public int LeshiyHealth => _leshiyHealth;
    public Action _onHealtChanged;

    [SerializeField]
    private AudioSource _audioSource;

    public static bool isDead;

    private void Awake() {
        isDead = false;
    }

    public void DecreaseLeshiyHelth() {
        if(isDead) {
            return;
        }
        _leshiyHealth--;
        _onHealtChanged?.Invoke();
        if(_leshiyHealth <= 0) {
            isDead = true;
            _audioSource.Play();
        }
    }
}
