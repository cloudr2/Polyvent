using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour {

    public float maxHP, speed;
    protected float currentHP;
    protected Animator anim;

    protected virtual void Awake() {
        currentHP = maxHP;
        anim = GetComponent<Animator>();
    }

    protected virtual void ReceiveDamage(float damage) {
        currentHP -= damage;
        if (currentHP <= 0)
            Death();
    }

    protected virtual void RestoreHealth(float restoration) {
        if (currentHP + restoration < maxHP)
            currentHP += restoration;
        else if (currentHP + restoration == maxHP)
            currentHP = maxHP;
        else
            return;
    }

    protected virtual void Death() {
        gameObject.SetActive(false);
    }
}
