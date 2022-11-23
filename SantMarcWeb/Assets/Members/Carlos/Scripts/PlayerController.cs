using UnityEngine;
using UnityEngine.SceneManagement;

namespace Members.Carlos.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        //Variables
        [SerializeField] private CharacterController controller;
        [SerializeField] private Transform cam;
    
        [Header("--- PLAYER BASIC VALUES ---")] 
        [Space(10)] 
        [SerializeField] private float speed = 6f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float jumpHeight = 3f;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] private float turnSmoothVelocity;
        public float horizontal;
        public float vertical;

        public float Speed
        {
            get => speed;
        }

        [Header("--- PLAYER GROUND VALUES ---")] 
        [Space(10)]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.4f;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private Vector3 velocity;
        public bool isGrounded;

        [Header("--- ANIMATIONS ---")] 
        [Space(10)] 
        public Animator playerAnimator;

        private static readonly int X = Animator.StringToHash("X");
        private static readonly int Y = Animator.StringToHash("Y");

        private void Update()
        {
            PlayerControl();
            AnimationsController();
        }

        private void PlayerControl()
        {
            gameObject.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * (speed * Time.deltaTime));
            }
        }

        private void AnimationsController()
        {
            playerAnimator.SetFloat(X, horizontal);
            playerAnimator.SetFloat(Y, vertical);
        }
    }
}