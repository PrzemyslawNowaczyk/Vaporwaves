using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : EnemyBase {

    Player player;
    Vector3 startPosition;
    AudioSource source;
    public Transform rotationPivot;
    public LayerMask CheckWalls;
    public Vector3 wallCheckSize;
    public int Damage;
    public float Speed;
    bool movePossible = true;

    public float PokeSpeed;

    [SerializeField]private float _hp;
    private float lastRotation;

    public override float HP {
        get { return _hp; }
        set { _hp = value; }
    }

    private void Awake() {
        player = FindObjectOfType<Player>();
        source = GetComponent<AudioSource>();
    }

    public override void Init(int level) {
        if (level <= 1) {
            level = 2;
        }
        int pointsToSpend = level;
        int speedPoints = (int) (Random.value * pointsToSpend);
        pointsToSpend -= speedPoints;
        int healthPoints = pointsToSpend;
        Speed = 135.0f + speedPoints * 20.0f * Random.Range(0.8f, 1.2f);
        HP = level / 2.0f + healthPoints;

        transform.localScale = Vector3.one * (1.0f + healthPoints * 0.1f);

        var pos = transform.position;
        pos.y = 0.51f + 3.0f * transform.localScale.y / 2.0f;
        transform.position = pos;
        startPosition = pos;
        StartCoroutine(AnimationStep());
    }

    IEnumerator AnimationStep() {
        movePossible = false;

        while (!movePossible) {
            SetDirection(out movePossible);
            yield return null;
        }
        

        for (float i = 0.0f; i <= 90.0f; i += Speed * Time.deltaTime) {
            var step = i - lastRotation;
            lastRotation = i;
            transform.RotateAround(rotationPivot.position, transform.right, step);
            yield return null;
        }
        lastRotation = 0.0f;

        source.Play();

        StartCoroutine(AnimationStep());
    }

    public void SetDirection(out bool figured) {

        figured = false;

        transform.rotation = Quaternion.identity;
        var direction = player.transform.position - transform.position;
        var absDir = new Vector3(Mathf.Abs(direction.x), 0.0f, Mathf.Abs(direction.z));
        if (absDir.x >= absDir.z) {
            transform.rotation = Quaternion.Euler(Vector3.up * 90.0f * Mathf.Sign(direction.x));
        }
        else {
            transform.rotation = Quaternion.Euler(Vector3.up * (90.0f - 90.0f * Mathf.Sign(direction.z)));
        }

        var alteredPosition = transform.position;
        alteredPosition.y = startPosition.y;

        transform.position = alteredPosition;


        for (int i = 0; i < 3; i++) {
            if (Physics.CheckBox(
                transform.position + transform.forward * transform.localScale.z * 1.5f + transform.forward * transform.localScale.z * wallCheckSize.z * 1.501f,
                Vector3.Scale(transform.localScale, wallCheckSize) * 3.0f / 2.0f, 
                Quaternion.identity, CheckWalls)) {

                transform.Rotate(Vector3.up * 90.0f);
            }
            else {
                figured = true;
                break;
            }
        }
        

    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.GetComponentInChildren<Player>() == player) {
            player.TakeDamage(Damage);
            var fpsController = player.GetComponent<RigidbodyFPSController>();
            if (fpsController) {
                fpsController.AddExternalForce(transform.forward.normalized * PokeSpeed, ForceMode.VelocityChange);
            }
        }
    }

    public override void TakeDamage(float damage) {
        HP -= damage;

        if (HP <= 0) {
            Die();
        }
    }

    public override void Die() {
        for (int i = 0; i < DropTable.Length; i++) {
            if (UnityEngine.Random.value < DropTable[i].DropChancePercent / 100.0f) {
                var drop = Instantiate(DropTable[i].DropPrefab, transform.position, Quaternion.identity);
                drop.GetComponent<Rigidbody>().velocity = UnityEngine.Random.insideUnitSphere * 5.0f;
                DropChance.ResetMultiplier();
            }
        }

        var particle = GetComponentInChildren<ParticleSystem>();
        if (particle) {
            particle.transform.SetParent(null, true);
            particle.transform.position = transform.position;
            particle.transform.rotation = transform.rotation;
            particle.transform.localScale = transform.lossyScale * 3.0f;
            particle.Emit(100);
            GameObject.Destroy(particle.gameObject, 2.0f);
        }
        Destroy(gameObject, 0.1f);
    }

}
