using UnityEngine;
using System.Collections;

public class PushedEnemyExplosion : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ExplosionPush"))
        {
            Debug.Log("pushed enemy");
            transform.parent.GetComponent<FollowTargetTiled>().Push((transform.position - other.transform.position).normalized);
        }
    }
}
