using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerMovement : MonoBehaviour
{
    [SerializeField] private GameObject plane;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private int numberOfRoadsLines = 5;

    private float _rightLimitPosition;
    private float _leftLimitPosition;
    private float _horizontalStepWidth;
    private Vector3 _velocity;
    private CharacterController _characterController;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        SetHorizontalStep();
    }

    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float xPosition = 0f;
        if (Input.GetKeyDown(KeyCode.A) && transform.position.x > _leftLimitPosition)
        {
            xPosition -= _horizontalStepWidth;
        }
        if (Input.GetKeyDown(KeyCode.D) && transform.position.x < _rightLimitPosition)
        {
            xPosition += _horizontalStepWidth;
        }

        var playerMovementInput = new Vector3(xPosition, 0f, 0f);

        Vector3 moveVector = transform.TransformDirection(playerMovementInput);

        if (_characterController.isGrounded)
        {
            _velocity.y = -1f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _velocity.y = jumpForce;
            }
        }
        else
        {
            _velocity.y += gravity * Time.deltaTime;
        }

        _characterController.Move(moveVector);
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void SetHorizontalStep()
    {
        if (plane == null)
        {
            throw new NullReferenceException(nameof(plane));
        }

        Bounds bounds = plane.GetComponent<MeshFilter>().mesh.bounds;
        float planeWidth = plane.transform.localScale.x * bounds.size.x;

        transform.position = new Vector3(plane.transform.position.x, transform.position.y);

        _horizontalStepWidth = planeWidth / numberOfRoadsLines;
        _rightLimitPosition = transform.position.x + _horizontalStepWidth * (numberOfRoadsLines / 2);
        _leftLimitPosition = transform.position.x - _horizontalStepWidth * (numberOfRoadsLines / 2);

        Debug.Log(string.Format("planeWidth={3} xStepWidth = {0}, xRightLimit = {1}, xLeftLimit ={2}", _horizontalStepWidth, _rightLimitPosition, _leftLimitPosition, planeWidth));
    }
}
