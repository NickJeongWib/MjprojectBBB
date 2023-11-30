using UnityEngine;

public class CharP : MonoBehaviour
{
    public Transform player;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    private Vector3 currentRotation; // ���� ���Ϸ� ������ ������ ����

    // LateUpdate is called after all Update functions have been called.
    void LateUpdate()
    {
        // ����� ���� ���
        Debug.Log($"Player Position: {player.position}");
        Debug.Log($"Position Offset: {positionOffset}");

        // ���� ��ǥ��� ��ȯ�� �������� ����Ͽ� �÷��̾��� ��ġ�� ���մϴ�.
        transform.position = player.TransformPoint(positionOffset);
        Debug.Log($"New Position: {transform.position}");

        // �÷��̾ �ٶ󺸵��� ȸ���� ����
        transform.LookAt(player.position);

        // ���Ϸ� ������ �����ϰ� ���ο� ������ ���� �Ŀ� ����
        currentRotation += rotationOffset;
        transform.rotation *= Quaternion.Euler(currentRotation);

        Debug.Log($"New Rotation: {transform.rotation.eulerAngles}");
    }
}