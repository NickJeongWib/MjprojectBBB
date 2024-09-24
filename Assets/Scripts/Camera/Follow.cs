using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Follow : MonoBehaviour
{

    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QarterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, 5.0f);

    [SerializeField]
    GameObject _player = null;

    public float wallDistance = 2.0f; // ������ �Ÿ�

    // Y���� ������ �� ����� ����
    public bool lockYPosition = false;
    public float fixedYPosition;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        // �Ǵ� _player�� �÷��̾� ������Ʈ�� ���� �Ҵ��ص� �˴ϴ�.
    }

    // �� �����Ӹ��� ���� 
    void Update()
    {

        //if (!(SceneManager.GetActiveScene().name == ""))
        //{

        //}
        // �÷��̾ ��ų�� ����ϴ� ���� Y���� ����
        if (lockYPosition)
        {
            transform.GetComponent<CinemachineBrain>().enabled = false;
            Vector3 newPosition = _player.transform.position + _delta;
            newPosition.y = fixedYPosition; // Y���� ����
            transform.position = newPosition;
        }
        else
        {
            transform.position = _player.transform.position + _delta;

            transform.LookAt(_player.transform);
        }


        //if (_mode == Define.CameraMode.QarterView && _player != null)
        //{
        //    RaycastHit hit;
        //    if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
        //    {
        //        float dist = (hit.point - _player.transform.position).magnitude * wallDistance;
        //        transform.position = _player.transform.position + _delta.normalized * dist;
        //    }
        //    else
        //    {
        //        transform.position = _player.transform.position + _delta;
        //        transform.LookAt(_player.transform);
        //    }
        //}
    }

    // ��ų ��� �� ī�޶� Y�� ����
    public void LockCameraYPosition()
    {
        lockYPosition = true;
        fixedYPosition = transform.position.y; // ���� Y���� ����
    }

    // ��ų�� ���� �� ī�޶� Y�� �ٽ� ������Ʈ
    public void UnlockCameraYPosition()
    {
        lockYPosition = false;
        transform.GetComponent<CinemachineBrain>().enabled = true;
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QarterView;
        _delta = delta;
    }
}