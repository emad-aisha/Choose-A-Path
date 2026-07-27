using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttack : BasicAttack {
    enum Type { None, Honing, Boomerang };
    [Header("Fireball Stats")]
    [SerializeField] GameObject fireball;
    [SerializeField] Type type;
    [SerializeField] int numberOfFireballs;
    [SerializeField] float speed;
    [SerializeField] float lifespan;

    Vector3 fireballDirection;
    Vector3 endDestination;
    bool canComeback;

    List<GameObject> fireballs;

    void Start() {
        fireballs = new List<GameObject>();
        canAttack = true;
        canComeback = false;
    }

    void Update() {
        // throw fireball towards player
        // if isHoning, following towards player
        switch (type) {
            case Type.Honing:
                fireballDirection = PlayerManager.instance.GetPlayerTransform().position;
                UpdateDirections(fireballDirection);
                break;
            case Type.Boomerang:
                if (canComeback) {
                    fireballDirection = transform.position;
                    endDestination = transform.position;
                    UpdateDirections(fireballDirection);
                    UpdateEndDirections(endDestination);
                }
                break;
        }
    }

    public override void Attack() {
        if (!canAttack) return;
        fireballs = new List<GameObject>();

        StartCoroutine(SpawnFireballs());
        StartCoroutine(AttackCooldown());
    }

    void UpdateDirections(Vector3 direction) {
        for (int i = 0; i < fireballs.Count; i++) {
            if (!fireballs[i]) {
                fireballs.Remove(fireballs[i]);
                i--;
                continue;
            }
            fireballs[i].GetComponent<Fireball>().ResetOriginalPosition();
            fireballs[i].GetComponent<Fireball>().SetDirection(direction);
        }
    }

    void UpdateEndDirections(Vector3 direction) {
        for (int i = 0; i < fireballs.Count; i++) {
            if (!fireballs[i]) {
                fireballs.Remove(fireballs[i]);
                i--;
                continue;
            }
            fireballs[i].GetComponent<Fireball>().SetEndDirection(direction);
        }
    }

    IEnumerator Comeback() {
        canComeback = false;
        yield return new WaitForSeconds(cooldown / 2);
        canComeback = true;
    }

    IEnumerator SpawnFireballs() {
        int currentNumberOfFireballs = 0;
        while (currentNumberOfFireballs < numberOfFireballs) {
            currentNumberOfFireballs++;

            fireballDirection = PlayerManager.instance.GetPlayerTransform().position;
            GameObject newFireball = Instantiate(fireball, transform.position, Quaternion.identity);
            newFireball.GetComponent<Fireball>().SetDirection(fireballDirection);
            newFireball.GetComponent<Fireball>().SetLifeSpan(lifespan);
            newFireball.GetComponent<Fireball>().SetSpeed(speed);
            newFireball.GetComponent<Fireball>().SetDamage(damage);
            if (type == Type.Boomerang) StartCoroutine(Comeback());


            fireballs.Add(newFireball);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
