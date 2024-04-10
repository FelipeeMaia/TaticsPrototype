using Brisanti.Tactics.Core;
using Brisanti.Tactics.Managment;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] TurnManager _turnManager;
    [SerializeField] float _speed;
    [SerializeField] float _focusTime;

    private Vector3 _input;
    private bool _isFocusing;

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
        if (_isFocusing) return;
        Vector3 newPosition = transform.position + _input * _speed * Time.deltaTime;
        transform.position = newPosition;
    }

    private void FocusUnit(Unit unit)
    {
        Vector3 targetPos = unit.transform.position;
        targetPos.y = 0;
        
        StartCoroutine(FocusingOn(targetPos));
    }

    private IEnumerator FocusingOn(Vector3 endPos)
    {
        _isFocusing = true;
        Vector3 startPos = transform.position;
        float lerpTime = 0;

        while (lerpTime < 1)
        {
            lerpTime += Time.fixedDeltaTime * _focusTime;
            transform.position = Vector3.Slerp(startPos, endPos, lerpTime);
            yield return new WaitForFixedUpdate();
        }

        _isFocusing = false;
    }

    private void Awake()
    {
        if(_turnManager)
            _turnManager.OnTurnStart += FocusUnit;
    }
}
