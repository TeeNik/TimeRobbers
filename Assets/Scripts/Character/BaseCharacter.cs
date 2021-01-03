using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{

    public CharacterMovement CharacterMovement { get; private set; }
    public InputController InputController { get; private set; }
    public bool IsDead { get; private set; }

    protected HealthComponent Health;

    void Awake()
    {
        CharacterMovement = GetComponent<CharacterMovement>();
        InputController = GetComponent<InputController>();
    }

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        //Health = gameObject.AddComponent<HealthComponent>();
    }

    public virtual void Action()
    {
        IsDead = true;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        HandleCollision(collider);
    }

    protected virtual void HandleCollision(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(StringTags.Danger) && !IsDead)
        {
            //TODO refactor messing with recording
            if (InputController.enabled)
            {
                InputController.StopRecording();
            }
            print("Die");
            IsDead = true;
            gameObject.SetActive(false);
        }
    }
}
