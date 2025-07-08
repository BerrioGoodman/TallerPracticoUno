using UnityEngine;

public class MovementHandler
{
    private Transform _transform;
    private Transform _camera;
    private float _rotationSpeed;
    public MovementHandler(Transform transform, Transform camera, float rotationSpeed)
    {
        _transform = transform;
        _camera = camera;
        _rotationSpeed = rotationSpeed;
    }
    public void Rotate(Vector2 moveInput)
    {
        if (moveInput == Vector2.zero) return;

        Vector3 direction = _camera.forward * moveInput.y + _camera.right * moveInput.x;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
    }
    public void SetTransform(Transform newTransform)
    {
        _transform = newTransform;
    }
    public void SetCamera(Transform newCamera)
    {
        _camera = newCamera;
    }
    public void SetRotationSpeed(float newSpeed)
    {
        _rotationSpeed = newSpeed;
    }
}
