using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordParticle_Eff : MonoBehaviour
{
    public ParticleSystem[] childParticleSystems;
    public ParticleSystem particleSystem;
    public bool isBig;
    public bool isShot;

    [SerializeField]
    float UpScale;

    private Vector3 StartScale;
    float timer;

    void Start()
    {
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
            StartScale = transform.localScale;
        }

        if (particleSystem == null)
        {
            Debug.LogError("Particle system is not assigned or found.");
        }
    }

    void Update()
    {
        // Ư�� ������ ������ �� ��ƼŬ �ý����� ����
        if (this.gameObject.activeSelf == true)
        {
            StartCoroutine(stopparticle());   
        }

        if (isBig == true)
        {
            // ���� ũ�⿡�� ���� Ŀ���� �ϱ�
            float scaleFactor = 1.0f + UpScale * Time.deltaTime;

            // ũ�� ����
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(scaleFactor, scaleFactor, scaleFactor));
            //this.transform.localScale = new Vector3(transform.localScale.x * UpScale * Time.deltaTime,
            //    transform.localScale.y * UpScale * Time.deltaTime,
            //    transform.localScale.z * UpScale * Time.deltaTime);
        }

        if (isShot == true)
        {
            // ���� ��ǥ �������� �̵�
            transform.Translate(Vector3.forward * 100.0f * Time.deltaTime, Space.Self);
        }
    }

    IEnumerator stopparticle()
    {
        // yield return new WaitForSeconds(0.06f);
        //yield return new WaitForSeconds(10.0f);
        //StopParticleSystem();

        // �ڽ� ��ƼŬ �ý��۵��� �Ͻ� ���� �Ǵ� ���
        //foreach (ParticleSystem childParticleSystem in childParticleSystems)
        //{
        //    if (childParticleSystem.isPlaying)
        //    {
        //        childParticleSystem.Pause();
        //    }
        //    else
        //    {
        //        childParticleSystem.Play();
        //    }
        //}

        yield return new WaitForSeconds(5.0f);
        this.gameObject.transform.localScale = StartScale;
        isShot = false;
        this.gameObject.SetActive(false);
    }

    void StopParticleSystem()
    {
        particleSystem.Pause();
    }
}
