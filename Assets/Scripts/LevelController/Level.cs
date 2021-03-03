using UnityEngine;

public class Level : MonoBehaviour
{
    public CharacterType[] AllowedCharacters;
    public SpriteRenderer CharacterView;
    public Vector3 SpawnPoint => CharacterView.transform.position;
}
