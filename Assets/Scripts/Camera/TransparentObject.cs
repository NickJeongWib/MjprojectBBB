//using UnityEngine;

//public class TransparentObject : MonoBehaviour
//{
//    public Transform player; // �÷��̾� ������Ʈ
//    public float transparency = 0.5f; // ������ ������ ���� ����

//    void Update()
//    {
//        // ī�޶�� �÷��̾� ������ ���� ����
//        Vector3 direction = player.position - transform.position;

//        // ī�޶󿡼� �÷��̾������ �Ÿ�
//        float distanceToPlayer = direction.magnitude;

//        // ī�޶󿡼� �÷��̾������ ����ĳ��Ʈ
//        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distanceToPlayer);

//        foreach (RaycastHit hit in hits)
//        {
//            Renderer renderer = hit.transform.GetComponent<Renderer>();
//            if (renderer != null)
//            {
//                // MeshRenderer���� ��� Material ��������
//                Material[] materials = renderer.materials;

//                // ��� Material �������ϰ� �����
//                foreach (Material material in materials)
//                {
//                    // ������ ����
//                    material.SetFloat("_Surface", 1); // 1�� Transparent�� �ǹ��մϴ�.
//                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
//                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
//                    material.SetInt("_ZWrite", 0);
//                    material.DisableKeyword("_ALPHATEST_ON");
//                    material.EnableKeyword("_ALPHABLEND_ON");
//                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
//                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

//                    // ��Ƽ������ ������ ������ ���� ���� �����մϴ�.
//                    Color color = material.color;
//                    color.a = transparency;
//                    material.color = color;
//                }
//            }
//        }
//    }
//}

using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    public Transform player; // �÷��̾� ������Ʈ
    public float transparency = 0.5f; // ������ ������ ���� ����

    private bool isTransparent = false; // ������Ʈ�� ������ �������� ���θ� ��Ÿ���� ����
    private Material[] originalMaterials; // ������Ʈ�� ���� ��Ƽ������ ������ �迭

    void Update()
    {
        // ī�޶�� �÷��̾� ������ ���� ����
        Vector3 direction = player.position - transform.position;

        // ī�޶󿡼� �÷��̾������ �Ÿ�
        float distanceToPlayer = direction.magnitude;

        // ī�޶󿡼� �÷��̾������ ����ĳ��Ʈ
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distanceToPlayer);

        bool hitTransparentObject = false; // ���̰� ������ ������Ʈ�� ��Ҵ��� ���θ� ��Ÿ���� ����

        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.transform.GetComponent<Renderer>();
            if (renderer != null) // ������Ʈ ��ü�� ���̰� ����� ��
            {
                originalMaterials = renderer.materials;
                SetObjectTransparent(renderer);
                hitTransparentObject = true;
            }
        }

        // ���̰� ������ ������Ʈ�� ���� ���� ��� ������ ���·� �ǵ�����
        if (!hitTransparentObject && isTransparent)
        {
            SetObjectOpaque();
        }
    }

    // ������Ʈ�� �������ϰ� ����� �Լ�
    void SetObjectTransparent(Renderer renderer)
    {
        Material[] materials = renderer.materials;
        foreach (Material material in materials)
        {
            material.SetFloat("_Surface", 1); // 1�� Transparent�� �ǹ��մϴ�.
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            Color color = material.color;
            color.a = transparency; // ������ ����
            material.color = color;
        }
        isTransparent = true; // ������ ���·� ����
    }

    // ������Ʈ�� �������ϰ� ����� �Լ�
    void SetObjectOpaque()
    {
        Material[] materials = originalMaterials; // ��� ��Ƽ���� �迭 ��������
        foreach (Material material in materials)
        {
            material.SetFloat("_Surface", 0); // 0�� Opaque�� �ǹ��մϴ�.
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

            Color color = material.color;
            color.a = 1f; // ������ ����
            material.color = color;
        }
        isTransparent = false; // ������ ���·� ����
    }
}


