using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private bool _isTriggered;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == StringTags.Player && !_isTriggered)
        {
            var baseCharacter = collider.gameObject.GetComponent<BaseCharacter>();
            if (baseCharacter.Type == CharacterType.Base)
            {
                _isTriggered = true;
                GameInstance.Instance.FinishLevel();
            }
        }
    }
}
