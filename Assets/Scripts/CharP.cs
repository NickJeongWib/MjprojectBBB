using UnityEngine;

public class CharP : MonoBehaviour
{
    public Transform player; // player ������Ʈ�� Transform ������Ʈ�� Inspector â���� �Ҵ����ּ���.
    public Vector3 offset; // player�� character ������ �Ÿ��� �����մϴ�.

    // Update is called once per frame
    void Update()
    {
        // player ������Ʈ�� ���� ��ġ�� offset�� ���Ͽ� Character ������Ʈ�� ��ġ�� �����մϴ�.
        transform.position = player.position + offset;
    }
}