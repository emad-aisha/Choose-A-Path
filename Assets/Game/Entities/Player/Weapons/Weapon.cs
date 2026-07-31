using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : Input {
    [Header("Weapon Stats")]
    [SerializeField] GameObject hitbox;
    [SerializeField] float cooldown;
    [SerializeField] float damageImage;
    [SerializeField] int damage;
    bool isAttacking;
    [SerializeField] float distance;

    [Header("Knockbacl")]
    [SerializeField] float knockback;
    [SerializeField] float knockbackTime;

    InputAction attackAction;

    void Start() {
        attackAction = InputManager.instance.GetAction(actionName, "Attack");
        hitbox.GetComponent<Hitbox>().SetDamage(damage);
    }


    void Update() {
        if (!isAttacking) StartCoroutine(AttackCooldown());
    }


    IEnumerator AttackCooldown() {
        if (!attackAction.WasPressedThisFrame()) yield break;
        AnimationManager.instance.SetIsAttacking(true);
        SlashAnimationManager.instance.SetIsAttacking(true);
        isAttacking = true;

        hitbox.transform.position = transform.position + (FacingDirectionManager.instance.GetAttackDirection() * distance);
        hitbox.SetActive(true);

        float time = 0;
        bool hit = false;
        while (time < cooldown) {
            time += Time.deltaTime;

            if (hitbox.GetComponent<Hitbox>().GetHitSomething() && !hit) {
                Vector3 direction = hitbox.GetComponent<Hitbox>().GetHitDirection();
                direction = (direction - PlayerManager.instance.GetTransform().position) * -knockback;

                PlayerManager.instance.GetMovementController().SetKnockback(direction, knockbackTime);
                hit = true;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        AnimationManager.instance.SetIsAttacking(false);
        SlashAnimationManager.instance.SetIsAttacking(false);
        hitbox.SetActive(false);


        yield return new WaitForSeconds(cooldown - damageImage);
        isAttacking = false;
    }


    void ResetDirections() {
        AnimationManager.instance.SetUpOrDown(0);
        SlashAnimationManager.instance.SetUpOrDown(0);
    }

}
