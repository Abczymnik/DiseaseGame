using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;

    private void Start()
    {
        if (playerAttack == null) playerAttack = GameObject.Find("/Player").GetComponent<PlayerAttack>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        playerAttack.EnemyHit(collision);
    }
}
