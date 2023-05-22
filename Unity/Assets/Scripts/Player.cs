using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
   private Dictionary<string,Canvas> GUIs;
    private GameManager gameManager;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        objectCAM = GameObject.FindWithTag("MainCamera");
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        DELTA_TIME = Time.deltaTime;
            movimentacaoChao();
    }

    private void movimentacaoChao()
    {
        bool isRun = false;
        int anim = 0;
        int hDir = 0;
        int vDir = 0;
        float spd;
        bool idle=true;

        move = Vector3.zero;

        if (!gameManager.getIsGamePaused())
        {

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
        }

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
