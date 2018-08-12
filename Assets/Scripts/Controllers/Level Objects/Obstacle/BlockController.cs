using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour {

    public List<Vector3> movePoints = new List<Vector3>();

    public int currentMovePoint = 0;

    public float moveDampTime = 5f;
    public float maxSpeed = 5f;
    public float acceleration = 0.1f;

    private Vector3 _dampVelocity = Vector3.zero;
    private Rigidbody _rigidbody;
    private Vector3 _startPosition;

	void Start ()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;
	}
	
	void FixedUpdate ()
    {
        if (movePoints.Count > 0)
        {
            Vector3 movePoint = _startPosition + movePoints[currentMovePoint];

            if (transform.position.Approximately(movePoint))
            {
                if (movePoints.Count > 1)
                {
                    currentMovePoint = (currentMovePoint + 1) % movePoints.Count;

                    _dampVelocity = Vector3.zero;
                }
            }
            else
            {
                _rigidbody.MovePosition(Vector3.SmoothDamp(transform.position, movePoint, ref _dampVelocity, moveDampTime, maxSpeed, acceleration));
            }
        }
	}
}

public static class VectorExtension {

    public static bool Approximately(this Vector3 vectorFirst, Vector3 vectorSecond, float tolerance = 0.02f)
    {
        

        return Mathf.Abs(vectorFirst.x - vectorSecond.x) <= tolerance && Mathf.Abs(vectorFirst.y - vectorSecond.y) <= tolerance && Mathf.Abs(vectorFirst.z - vectorSecond.z) <= tolerance;
    }

}
