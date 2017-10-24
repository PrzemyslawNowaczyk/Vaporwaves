using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public AudioClip[] painMoans;

    public int Health;
    public int Bullets;
    public float Damage;
    public float Spread;
    public float BulletsSpeed;
    public float CritChance;

    internal void IncreaseHealth() {
        Health++;
        OnHealthChange.Invoke(Health);
    }

    public ValueChangeEvent OnHealthChange;
    public ValueChangeEvent OnBulletsChange;
    public ValueChangeEvent OnDamageChange;
    public ValueChangeEvent OnAttackSpeedChange;
    public ValueChangeEvent OnBulletSpeedChange;
    public ValueChangeEvent OnCritChange;
    public ValueChangeEvent OnSpreadChange;

    public float AttackSpeed {
        set { _animator.speed = value; }
        get { return _animator.speed; }
    }

    private float AttackSpeedIncrease {
        get {
            switch (Mathf.FloorToInt(AttackSpeed)) {
                case 1:
                    return 0.2f;
                case 2:
                    return 0.15f;
                case 3:
                    return 0.1f;
                case 4:
                    return 0.07f;
                default:
                    return 0.05f;
            }
        }
    }

    private float CritCHanceIncrease {
        get {
            switch (Mathf.CeilToInt(CritChance*10.0f)) {
                case 1:
                    return 0.05f;
                case 2:
                    return 0.03f;
                case 3:
                    return 0.02f;
                case 4:
                    return 0.01f;
                case 5:
                    return 0.005f;
                default:
                    return 0.002f;
            }
        }
    }

    private Animator _animator;
    private AudioSource _source;
    private float invulTime;

    public void IncreaseBullets() {
        Bullets++;
        Spread += 1.0f;
        OnBulletsChange.Invoke(Bullets);
        OnSpreadChange.Invoke(Spread);
    }

    public void IncreaseAttackSpeed() {
        AttackSpeed += AttackSpeedIncrease;
        OnAttackSpeedChange.Invoke(AttackSpeed);
    }

    public void IncreaseDamage() {
        Damage += 0.5f;
        OnDamageChange.Invoke(Damage);
    }

    public void IncreaseCritChance() {
        CritChance += CritCHanceIncrease;
        OnCritChange.Invoke(CritChance);
    }

    float _spreadModifier = 1.0f;
    float spreadModifier {
        get {
            _spreadModifier *= 0.9f;
            return _spreadModifier;
        }
    }

    public void DecreaseSpread() {
        Spread = (Bullets-1) * spreadModifier;
        OnSpreadChange.Invoke(Spread);
    }

    public void IncreaseBulletSpeed() {
        BulletsSpeed += 8.0f;
        OnBulletSpeedChange.Invoke(BulletsSpeed);
    }

    private void Awake() {
        _animator = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
    }

    private void Start() {
        OnHealthChange.Invoke(Health);
        OnBulletsChange.Invoke(Bullets);
        OnCritChange.Invoke(CritChance);
        OnAttackSpeedChange.Invoke(AttackSpeed);
        OnBulletSpeedChange.Invoke(BulletsSpeed);
        OnDamageChange.Invoke(Damage);
        OnSpreadChange.Invoke(Spread);
    }

    private void Update() {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.E)) {
            DecreaseSpread();
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            IncreaseBullets();
        }
#endif

        if(invulTime > 0.0f) {
            invulTime -= Time.deltaTime;
        }
    }

    public void TakeDamage(int dmg) {
        if (invulTime <= 0.0f) {
            Health -= dmg;
            OnHealthChange.Invoke(Health);
            _source.clip = painMoans[UnityEngine.Random.Range(0, painMoans.Length - 1)];
            _source.Play();

            if (Health <= 0) {
                Die();
            }
            else {
                invulTime = 0.5f;
            }

        }
    }

    public SceneField DeathScene;

    private void Die() {
        SceneManager.LoadScene(DeathScene);
    }
}

[Serializable]
public class ValueChangeEvent : UnityEvent<float> { }