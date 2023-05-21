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
    private Animator anim;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        objectCAM = GameObject.FindWithTag("MainCamera");
    }

    private void Update()
    {
        DELTA_TIME = Time.deltaTime;
        movimentacao();
    }

    private void movimentacao()
    {
        float spd = speed;
        int animF = 0;
        move = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            move += Vector3.forward;
            animF = 1;
        }

        if (Input.GetKey(KeyCode.A)) move -= Vector3.right;

        if (Input.GetKey(KeyCode.S))
        {
            move -= Vector3.forward;
            animF -= 1;
        }

        if (Input.GetKey(KeyCode.D)) move += Vector3.right;

        if (Input.GetKey(KeyCode.Space) && controller.isGrounded)
        {
            print("Pula");
        }

        if (Input.GetKey(KeyCode.LeftShift) && animF >= 0)
        {
            spd = speed * run_fac;
            animF += 1;
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
        anim.SetInteger("Transition", animF);
        controller.Move(move);
    }
}
