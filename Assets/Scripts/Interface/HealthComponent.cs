using UnityEngine;

public class HealthComponent : MonoBehaviour
{

    public bool IsUnhittable = false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!IsUnhittable && collider.gameObject.CompareTag(StringTags.Danger))
        {
            print("Die");
            Destroy(gameObject);
        }
    }
}
