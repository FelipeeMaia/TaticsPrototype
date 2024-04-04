using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _speed;
    private Vector3 _input;

    private void Update()
    {
        GatherInput();   
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GatherInput()
    {
        Vector3 input = Vector3.zero;
        input.x = Input.GetAxisRaw("Horizontal");
        input.z = Input.GetAxisRaw("Vertical");
        input.Normalize();

        var isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        _input = isoMatrix.MultiplyPoint3x4(input);
    }

    private void Move()
    {
        Vector3 newPosition = transform.position + _input * _speed * Time.deltaTime;
        transform.position = newPosition;
    }
}
