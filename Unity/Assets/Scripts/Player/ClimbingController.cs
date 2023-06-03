using StarterAssets;
using UnityEngine;

public class ClimbingController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] string vertParam = "Vert", horizParam = "Horiz";
    [Space]
    [SerializeField] Transform cameraTarget;
    [SerializeField] StarterAssetsInputs inputs;
    [SerializeField] CharacterController controller;
    [Space]
    [SerializeField] Vector2 speed = new(1, 1);

    private Vector2 move;

    public bool turninCamera;


    void Update()
    {
        Move();
        Rotate();
        Animate();
    }


    void Move()
    {
        move = inputs.move;

        float vert = move.y * speed.y;
        float horiz = move.x * speed.x;

        controller.Move(new Vector3(horiz, vert, 0) * Time.deltaTime);
    }
    void Rotate()
    {
        if (!turninCamera)
            return;

        Vector3 eulerLoc = cameraTarget.localEulerAngles;
        eulerLoc = Vector3.Slerp(eulerLoc, Vector3.zero, Time.deltaTime);

        turninCamera = eulerLoc.magnitude > 0.1f;

        if (turninCamera)
            cameraTarget.localEulerAngles = eulerLoc;
        else
            cameraTarget.localEulerAngles = Vector3.zero;
    }
    void Animate()
    {
        var mInput = ClampNormalize(move);
        anim.SetFloat(vertParam, mInput.y);
        anim.SetFloat(horizParam, mInput.x);
    }


    Vector2 ClampNormalize(Vector2 source)
    {
        float x = Mathf.Clamp(source.x, -1, 1);
        float y = Mathf.Clamp(source.y, -1, 1);

        return new Vector2(x, y);
    }
}
