using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownCharacter : BaseCharacter
{

    public GameObject SlowdownArea;

    private GameObject _spawnedArea;

    public override void Action()
    {
        _spawnedArea = Instantiate(SlowdownArea, transform.position, Quaternion.identity);
        base.Action();
    }

    void OnDestroy()
    {
        if (_spawnedArea != null)
        {
            Destroy(_spawnedArea);
        }
    }
}
