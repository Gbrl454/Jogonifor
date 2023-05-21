using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera[] cameras;
    private int cameraAtual = 0;
    public float gravity;
    public float speed;
    public float run_fac;
    public float jump_force;
    private CharacterController controller;
    private Vector3 move;
    private float DELTA_TIME;
    private float rotateH;
    private float rotateV;
    private GameObject objectCAM;
    private Animator animator;
    public bool isCrawl = false;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        objectCAM = GameObject.FindWithTag("MainCamera");
    }

    private void Update()
    {
        DELTA_TIME = Time.deltaTime;

        movimentacaoChao();
        trocaCamera();
    }

    private void trocaCamera()
    {
        if (Input.GetKeyUp(KeyCode.F5))
        {
            cameraAtual++;
            if (cameraAtual >= cameras.Length)
            {
                cameraAtual = 0;
            }
        }

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].depth = (cameraAtual == i) ? 1 : 0;
            cameras[i].tag = "Untagged";
        }

        cameras[cameraAtual].tag = "MainCamera";

        objectCAM = GameObject.FindWithTag("MainCamera");

    }

    private void movimentacaoChao()
    {
        bool isRun = false;
        int anim = 0;
        int hDir = 0;
        int vDir = 0;
        float spd;
        bool idle;


        move = Vector3.zero;

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrawl = (isCrawl) ? false : true;
        }


        if (isCrawl)
        {
            anim = 0;
            hDir = 0;
            vDir = 0;
            idle = true;

            if (Input.GetKey(KeyCode.W))
            {
                move += Vector3.forward;
                anim = 1;
            }

            spd = speed * .5f;
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                move += Vector3.forward;
                vDir = 1;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    isRun = true;
                }
                anim = (isRun) ? 2 : 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                move -= Vector3.forward;
                vDir = -1;
                anim = 1;
            }

            if (Input.GetKey(KeyCode.A) && anim == 0)
            {
                move -= Vector3.right;
                hDir = -1;
            }

            if (Input.GetKey(KeyCode.D) && anim == 0)
            {
                move += Vector3.right;
                hDir = 1;
            }
            idle = (anim != 0 || hDir != 0 || vDir != 0) ? false : true;
            spd = (isRun) ? (speed * run_fac) : speed;
        }


        move *= spd * DELTA_TIME;

        if (rotateV > 90) rotateV = 90;

        if (rotateV < -90) rotateV = -90;

        rotateH += Input.GetAxis("Mouse X");
        rotateV -= Input.GetAxis("Mouse Y");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        objectCAM.transform.localRotation = Quaternion.Euler(rotateV, 0, 0);
        transform.eulerAngles = new Vector3(0, rotateH, 0);
        move = transform.TransformDirection(move);

        move -= Vector3.up * gravity * DELTA_TIME;
        animator.SetInteger("Animation", anim);
        animator.SetInteger("HDir", hDir);
        animator.SetInteger("VDir", vDir);
        animator.SetBool("Idle", idle);
        animator.SetBool("Crawl", isCrawl);
        controller.Move(move);
    }

}
