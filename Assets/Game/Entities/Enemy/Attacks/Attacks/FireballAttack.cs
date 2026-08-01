using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttack : BasicAttack {
    public enum Type { None, Honing, Boomerang };
    [Header("Fireball Stats")]
    [SerializeField] GameObject fireball;
    [SerializeField] Type type;
    [SerializeField] int numberOfFireballs;
    [SerializeField] float wait;
    [SerializeField] float speed;
    [SerializeField] float acceleration;
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
                fireballDirection = PlayerManager.instance.GetTransform().position;
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

    public override bool Attack() {
        if (!canAttack) return false;
        fireballs = new List<GameObject>();

        StartCoroutine(SpawnFireballs());
        StartCoroutine(AttackCooldown());
        return true;
    }

    public bool CompareType(Type _type) {
        return type == _type;
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
        yield return new WaitForSeconds(lifespan / 2);
        animationManager.SetAttacking(false);
        canComeback = true;
    }

    IEnumerator SpawnFireballs() {
        animationManager.SetWinding(true);
        animationManager.SetAttacking(false);
        yield return new WaitForSeconds(windup);
        animationManager.SetWinding(false);
        int currentNumberOfFireballs = 0;
        while (currentNumberOfFireballs < numberOfFireballs) {
            currentNumberOfFireballs++;

            fireballDirection = PlayerManager.instance.GetTransform().position;
            GameObject newFireball = Instantiate(fireball, transform.position, Quaternion.identity);
            Fireball newFireballComponent = newFireball.GetComponent<Fireball>();
            newFireballComponent.SetDirection(fireballDirection);
            newFireballComponent.SetSpeed(speed);
            newFireballComponent.SetAcceleration(acceleration);
            newFireballComponent.SetDamage(damage);
            newFireballComponent.SetLifeSpan(lifespan);

            if (type == Type.Boomerang) StartCoroutine(Comeback());


            fireballs.Add(newFireball);
            yield return new WaitForSeconds(wait);
        }
        animationManager.SetAttacking(false);
    }
}
