using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCubeController : MonoBehaviour {

    public float naturalShrinkRate = 0.0002f;

    public float minSize = 0.4f;
    public float minSizeShrinkFactor = 5f;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Shrink(naturalShrinkRate);
    }

    public void Shrink(float shrink)
    {
        if (transform.localScale.x <= 0.001f)
        {
            Melt();
        }
        else if (transform.localScale.x <= minSize)
        {
            shrink *= minSizeShrinkFactor;

            transform.localScale = new Vector3(transform.localScale.x - shrink, transform.localScale.y - shrink, transform.localScale.z - shrink);

            _rigidbody.MovePosition(new Vector3(transform.position.x, transform.position.y, -transform.localScale.z / 2f));
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x - shrink, transform.localScale.y - shrink, transform.localScale.z - shrink);

            _rigidbody.MovePosition(new Vector3(transform.position.x, transform.position.y, -transform.localScale.z / 2f));
        }
    }

    public void Melt()
    {
        Destroy(gameObject);
    }
}
