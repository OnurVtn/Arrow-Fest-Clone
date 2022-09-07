using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            enemyAnimator.SetBool("isTriggerArrow", true);
            GameManager.Instance.OnGameCompleted();
        }
    }
}
