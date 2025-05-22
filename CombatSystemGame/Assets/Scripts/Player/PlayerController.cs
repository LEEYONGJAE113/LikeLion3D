using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed; // default 5 -> 7
    [SerializeField] float rotationSpeed; // default 500
    [SerializeField] float groundCheckRadius; // default 0.2f
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;

    Quaternion targetRotation;
    float ySpeed;
    CameraController cameraController;
    Animator anim;
    CharacterController characterController;
    MeleeFighter meleeFighter;

    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        meleeFighter = GetComponent<MeleeFighter>();
    }

    void Update()
    {
        if (meleeFighter.inAction)
        {
            anim.SetFloat("ForwardSpeed", 0f);
            return;
        }


        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput)); // 0~1로 제한

        var moveInput = new Vector3(horizontalInput, 0, verticalInput).normalized;

        var moveDir = cameraController.PlayerRotation * moveInput;

        GroundCheck();
        // Debug.Log(isGrounded);

        if(isGrounded)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;


        if (moveAmount > 0)
        {
            // 캐릭터 컨트롤러를 활용한 이동
            characterController.Move(velocity* Time.deltaTime);

            // transform.position += moveDir * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(moveDir); // 이동방향으로 회전
        }

        // 부드러운 회전 처리
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        anim.SetFloat("ForwardSpeed", moveAmount, 0.1f, Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}
