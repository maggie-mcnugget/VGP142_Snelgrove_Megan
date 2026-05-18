using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    public bool canMove = true;

    private float originalWalkSpeed;
    private float originalRunSpeed;

    private Vector3 originalCenter;
    private float originalHeight;

    [Header("Combat")]
    public float punchRange = 2f;
    public float kickRange = 3f;

    public float punchDamage = 25f;
    public float kickDamage = 50f;

    [Header("Ranged Attack")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootForce = 20f;
    public float shootCooldown = 1f;

    private float shootTimer;




    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalWalkSpeed = walkSpeed;
        originalRunSpeed = runSpeed;

        originalHeight = characterController.height;
        originalCenter = characterController.center;
    } 

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;

            characterController.center = new Vector3(
                originalCenter.x,
                originalCenter.y - (originalHeight - crouchHeight) / 2f,
                originalCenter.z
            );

            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;

            animator.SetBool("isCrouching", true);
        }
        else
        {
            characterController.height = originalHeight;
            characterController.center = originalCenter;

            walkSpeed = originalWalkSpeed;
            runSpeed = originalRunSpeed;

            animator.SetBool("isCrouching", false);
        }

        characterController.Move(moveDirection * Time.deltaTime);

        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);

        float speed = horizontalVelocity.magnitude;
        speed = (speed < 0.1f) ? 0f : speed;

        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);

        animator.SetBool("isRunning", isRunning);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (canMove && Input.GetMouseButtonDown(0))
        {
            Punch();
        }

        if (canMove && Input.GetMouseButtonDown(1))
        {
            Kick();
        }

        shootTimer -= Time.deltaTime;

        if (canMove && Input.GetKeyDown(KeyCode.E) && shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootCooldown;
        }

    }
        void Punch()
        {
            animator.SetTrigger("Punch");

            Collider[] hits = Physics.OverlapSphere(transform.position, punchRange);

            foreach (Collider hit in hits)
            {
                MeleeEnemy enemy = hit.GetComponentInParent<MeleeEnemy>();

                if (enemy != null)
                {
                enemy.TakeDamage(punchDamage, "Punch");
                }
            }
        }

        void Kick()
        {
            animator.SetTrigger("Kick");

            Collider[] hits = Physics.OverlapSphere(transform.position, kickRange);

            foreach (Collider hit in hits)
            {
                MeleeEnemy enemy = hit.GetComponentInParent<MeleeEnemy>();

                if (enemy != null)
                {
                enemy.TakeDamage(kickDamage, "Kick");
                }
            }
        }

    void Shoot()
    {
        animator.SetTrigger("Shoot");

        if (projectilePrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = proj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = playerCamera.transform.forward * shootForce;
        }
    }

}