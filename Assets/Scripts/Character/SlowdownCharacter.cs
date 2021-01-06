using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownCharacter : BaseCharacter
{

    public GameObject SlowdownArea;

    private GameObject _spawnedArea;

    public override void UseAbility()
    {
        _spawnedArea = Instantiate(SlowdownArea, transform.position, Quaternion.identity);
        base.UseAbility();
    }

}
