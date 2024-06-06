using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] private int bossId;

    private void OnTriggerEnter(Collider other)
    {
        var boss = GameManager.Instance.GetBossCharacterByID(bossId);
        if (boss != null)
        {
            boss.WakeBossUp();
        }
    }
}
