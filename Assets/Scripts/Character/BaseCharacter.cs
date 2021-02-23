using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CharacterType
{
    Base,
    Shadow,
    Slowdown,
}


public class BaseCharacter : MonoBehaviour
{

    public CharacterMovement CharacterMovement { get; private set; }
    public bool IsDead { get; private set; }

    public CharacterType Type;

    [SerializeField] private string[] DeathTags;

    void Awake()
    {
        CharacterMovement = GetComponent<CharacterMovement>();
    }

    public virtual void UseAbility()
    {
        Die();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        HandleCollision(collider);
    }

    protected virtual void HandleCollision(Collider2D collider)
    {
        if (DeathTags.Contains(collider.gameObject.tag) && !IsDead)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;
        CharacterMovement.PlayDieAnimation();
    }

    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
