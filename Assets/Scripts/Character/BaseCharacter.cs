using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private InputController _inputController;

    private HealthComponent _health;

    void Start()
    {
        _health = gameObject.AddComponent<HealthComponent>();
    }

}
