using UnityEngine;

// This line adds a new option to Unity's Right-Click menu!
[CreateAssetMenu(fileName = "NewFighter", menuName = "HOA Dispute/Fighter Data")]
public class FighterData : ScriptableObject
{
    [Header("Character Info")]
    public string characterName;

    [Header("Base Stats")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;
    public float maxMeter = 100f;

    [Header("Moveset Prefabs")]
    // This is where Kyle's Sticky Note or Barbara's Paper Airplane goes
    public GameObject specialProjectilePrefab;

    // This is where Kyle's Subaru or Pam's HR Office Trap goes
    public GameObject ultimateEffectPrefab;
}