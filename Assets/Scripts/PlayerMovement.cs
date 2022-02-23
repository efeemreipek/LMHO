using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float playerMass = 3f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    private bool isGrounded;
    private float xAxis;
    private float zAxis;
    private float gravity = -9.81f;
    private float groundDistance = 0.4f;
    private Vector3 velocity;
    private Vector3 moveVelocity;
    private Vector3 turnVelocity;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        xAxis = Input.GetAxis("Horizontal");
        zAxis = Input.GetAxis("Vertical");

        moveVelocity = transform.forward * movementSpeed * zAxis;
        turnVelocity = transform.up * rotationSpeed * xAxis;

        _controller.Move(moveVelocity * Time.deltaTime);
        _animator.SetFloat("Speed", zAxis * movementSpeed);
        transform.Rotate(turnVelocity * Time.deltaTime);

        velocity.y += gravity * playerMass * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("People"))
        {
            _animator.SetBool("isTalking", true);
        }
        else if (other.CompareTag("Bound"))
        {
            StartCoroutine(GameManager.Instance.EndGame());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("People"))
        {
            _animator.SetBool("isTalking", false);
        }
    }

}
