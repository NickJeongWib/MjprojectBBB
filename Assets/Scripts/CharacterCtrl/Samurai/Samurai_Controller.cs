using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_Controller : MonoBehaviour
{
    public Camera mainCam;
    private Animator animator;
    private Vector3 destination;
    [SerializeField]
    LayerMask groundLayer; // ������ ���̾�
    void Awake()
    {
        mainCam = Camera.main;
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame

}
