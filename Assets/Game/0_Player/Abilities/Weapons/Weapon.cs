using System.Collections;
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

    InputAction attackAction;

    void Start() {
        attackAction = InputManager.instance.GetAction(actionName, "Attack");
        hitbox.GetComponent<Hitbox>().SetDamage(damage);
    }


    void Update() {
        Debug.DrawRay(transform.position, FacingDirectionManager.instance.GetAttackDirection() * distance, Color.red);
        if (!isAttacking) StartCoroutine(AttackCooldown());
    }


    IEnumerator AttackCooldown() {
        if (!attackAction.WasPressedThisFrame()) yield break;
        isAttacking = true;

        hitbox.transform.position = transform.position + (FacingDirectionManager.instance.GetAttackDirection() * distance);
        hitbox.SetActive(true);
        yield return new WaitForSeconds(damageImage);
        hitbox.SetActive(false);


        yield return new WaitForSeconds(cooldown - damageImage);
        isAttacking = false;
    }

}
