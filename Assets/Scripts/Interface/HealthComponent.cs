using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(StringTags.Danger))
        {
            print("Die");
            Destroy(gameObject);
        }
    }
}
