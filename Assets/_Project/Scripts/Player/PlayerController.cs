using UnityEngine;
using UnityEngine.InputSystem;

namespace Lastlight.Player
{
    /// 1인칭 캐릭터 이동 및 시점 컨트롤러
    /// (Unity 6 Project-wide Input Actions 사용)

    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 8f;
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private float gravity = -20f;

        [Header("Look")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float mouseSensitivity = 0.1f;
        [SerializeField] private float maxLookAngle = 80f;

        [Header("Animation")]
        [SerializeField] private Animator animator;

        [Header("Head Tracking")]
        [SerializeField] private Transform headBone;
        [SerializeField] private float headPitchRatio = -1.0f;
        [SerializeField] private Vector3 headRotationOffset = Vector3.zero;

        // Animator 파라미터 해시
        private static readonly int MoveMotionHash = Animator.StringToHash("MoveMotion");


        // Input Actions (Project-wide Actions 참조)
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction jumpAction;
        private InputAction sprintAction;

        // Movement
        private CharacterController controller;
        private Vector3 velocity;
        private float verticalRotation = 0f;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();

            // Animator 자동 찾기 (자식 오브젝트에서)
            if (animator == null)
                animator = GetComponentInChildren<Animator>();

            // 카메라가 비어있으면 자동으로 찾기
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;

            // Project-wide Actions에서 액션 참조 가져오기
            moveAction = InputSystem.actions.FindAction("Move");
            lookAction = InputSystem.actions.FindAction("Look");
            jumpAction = InputSystem.actions.FindAction("Jump");
            sprintAction = InputSystem.actions.FindAction("Sprint");
        }

        private void OnEnable()
        {
            // Jump는 이벤트로 처리 (한 번만 발동)
            jumpAction.performed += OnJump;
        }

        private void OnDisable()
        {
            // 메모리 누수 방지
            jumpAction.performed -= OnJump;
        }

        private void Start()
        {
            // 마우스 커서 잠그기 (FPS 표준)
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleLook();
            HandleMovement();
        }

        // ────────────── Input Callbacks ──────────────

        private void OnJump(InputAction.CallbackContext ctx)
        {
            if (controller.isGrounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // ────────────── Movement Logic ──────────────

        /// 마우스로 시점 회전
        private void HandleLook()
        {
            Vector2 lookInput = lookAction.ReadValue<Vector2>();

            float mouseX = lookInput.x * mouseSensitivity;
            float mouseY = lookInput.y * mouseSensitivity;

            // 좌우 회전: 본체 회전
            transform.Rotate(Vector3.up, mouseX);

            // 상하 회전: 카메라 회전 (각도 제한)
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);

            // 카메라 X축 회전 (Spine 자식이 아니므로 직접 회전)
            if (cameraTransform != null)
                cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }

        /// <summary>
        /// WASD 이동 + 중력 + 달리기
        /// </summary>
        private void HandleMovement()
        {
            // 바닥에 닿아있으면 수직 속도 리셋 (살짝 음수로 둬서 isGrounded 안정화)
            if (controller.isGrounded && velocity.y < 0)
                velocity.y = -2f;

            // 매 프레임 입력 값 읽기
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            bool isRunning = sprintAction.IsPressed();

            // 애니메이터에 이동 크기 전달
            if (animator != null)
                animator.SetFloat(MoveMotionHash, moveInput.magnitude);

            // 속도 결정
            float speed = isRunning ? runSpeed : walkSpeed;

            // 플레이어가 바라보는 방향 기준 이동 벡터
            Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y) * speed;

            // 중력 적용
            velocity.y += gravity * Time.deltaTime;

            // 최종 이동
            controller.Move((move + new Vector3(0, velocity.y, 0)) * Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (headBone != null)
            {
                // Spine 회전만 적용 (모델이 마우스 따라 굽힘)
                float pitch = verticalRotation * headPitchRatio;
                headBone.localRotation *= Quaternion.Euler(
                    headRotationOffset.x,
                    headRotationOffset.y,
                    pitch + headRotationOffset.z
                );
            }
            // 카메라 위치 추적 코드 삭제 - 카메라는 머리 높이에 고정
        }
    }
}