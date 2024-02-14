using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Find_Target : MonoBehaviour
{
    [SerializeField]
    Reaper_Controller ReaperCtrl;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReaperCtrl.Target = other.gameObject;

            ReaperCtrl.isTargetFind = true; // �÷��̾� Ȯ��
            ReaperCtrl.Reaper_animator.SetBool("isFind", ReaperCtrl.isTargetFind);
            ReaperCtrl.reaperState = ReaperState.RaidStart;
            this.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
