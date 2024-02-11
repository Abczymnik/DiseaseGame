using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;

    private void OnValidate()
    {
        playerAttack = GameObject.Find("/Player").GetComponent<PlayerAttack>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        playerAttack.EnemyHit(collision);
    }
}
