using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float moveSpeed = 3f;
    public float growSpeed = 0.1f;
    public float lightFactor = 3f;

    public float startSize = 0.8f;
    public float maxSize = 3f;
    
    public ParticleSystem particles;

    public GameObject popPrefab;
    public GameObject inactivePopPrefab;
    public GameObject winPrefab;

    public bool activePlayer = true;

    private Transform _transform;
    private Rigidbody _rigidbody;
    private Light _light;
    private Material _material;
    [SerializeField]
    private Material _particleMat;

    private Vector2 _moveVector;

    public bool isLevelExit = false;
    private float levelExitTime = 0.5f;
    private float levelExitDelay = 0.6f;
    private Vector3 _levelExitShrink = new Vector3();
    private Vector3 _levelExitVelocity = new Vector3();
    public Vector3 levelExitPos;
    private float _elapsedTimeLevelExit = 0f;

	void Start ()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _light = GetComponentInChildren<Light>();
        _material = GetComponent<Renderer>().material;

        GameController.AddPlayer(this);
	}
	
	void Update ()
    {

        /*if (!isLevelExit && activePlayer)
        {
            float xSpeed = Input.GetAxis("Horizontal");
            float ySpeed = Input.GetAxis("Vertical");

            _moveVector = new Vector2(xSpeed, ySpeed);
        }*/

        float healthFactor = (transform.localScale.x - startSize) / (maxSize - startSize);
        float active = 1f;
        if (!activePlayer)
            active = 0f;

        _light.range = lightFactor * transform.localScale.x;
        if (activePlayer)
            _light.color = Color.Lerp(Color.green, Color.red, healthFactor);
        else
            _light.color = new Color(0.655f, 0.655f, 0.655f);

        _material.SetFloat("_IsActive", active);
        if (activePlayer)
        {
            _material.SetFloat("_HealthFactor", healthFactor);
            _particleMat.SetFloat("_HealthFactor", healthFactor);
        }

        if(!activePlayer && !particles.isStopped)
            particles.Stop();
        else if (activePlayer && !particles.isPlaying)
            particles.Play();

    }

    private void FixedUpdate()
    {
        if (!isLevelExit)
        {
            if (Mathf.Abs(_rigidbody.velocity.x) > 0 || Mathf.Abs(_rigidbody.velocity.y) > 0)
            {
                float growth = Mathf.Max(_rigidbody.velocity.magnitude * growSpeed, growSpeed);

                Grow(growth);
            }

            _rigidbody.MovePosition(new Vector3(transform.position.x, transform.position.y, -transform.localScale.z / 2f));

            _rigidbody.velocity = _moveVector * moveSpeed;
        }
        else if(_elapsedTimeLevelExit < levelExitTime + levelExitDelay)
        {
            _rigidbody.velocity = Vector3.zero;

            transform.localScale = Vector3.SmoothDamp(transform.localScale, Vector3.zero, ref _levelExitShrink, levelExitTime);
            transform.position = Vector3.SmoothDamp(transform.position, levelExitPos, ref _levelExitVelocity, levelExitTime);

            _elapsedTimeLevelExit += Time.fixedDeltaTime;
        }
        else
        {
            Win();
        }


    }

    public void MovePlayer(Vector2 move)
    {
        if (move.sqrMagnitude > 1f)
            move.Normalize();

        _moveVector = move * moveSpeed;
    }

    public void Grow(float growth)
    {
        if (!isLevelExit)
        {
            if (transform.localScale.x >= maxSize)
            {
                Pop();
            }
            else
            {
                transform.localScale = Vector3.one * Mathf.Max(transform.localScale.x + growth, startSize);

                //_rigidbody.MovePosition(new Vector3(transform.position.x, transform.position.y, -transform.localScale.z / 2f));
            }
        }
    }

    public void Split()
    {
        Debug.Log("Split");

        PlayerController other = Instantiate(gameObject, transform.position, Quaternion.identity).GetComponent<PlayerController>();

        float newScale = Mathf.Max(transform.localScale.x / 2f, startSize);
        int intervals = 30;
        float intervalShrink = -newScale / intervals;

        other.StartCoroutine(other.GrowOverTime(intervals, intervalShrink));
        StartCoroutine(GrowOverTime(intervals, intervalShrink));

        other.activePlayer = false;
    }

    public void Pop()
    {
        GameController.PlayerNotDying = false;

        Debug.Log("Player Death");

        if(activePlayer)
            Instantiate(popPrefab, transform.position, Quaternion.identity);
        else
            Instantiate(inactivePopPrefab, transform.position, Quaternion.identity);

        GameController.OnPlayerDeath(this);

        Destroy(gameObject);
    }

    public void Win()
    {
        GameController.PlayerNotDying = false;
        GameController.notExitingLevel = false;

        Debug.Log("Level Exit");

        Instantiate(winPrefab, transform.position, Quaternion.identity);


        GameController.NextLevel();

        GameController.OnPlayerDeath(this);

        Destroy(gameObject);
    }

    public IEnumerator GrowOverTime(float intervals, float intervalGrowth)
    {
        for (int i = 0; i < intervals; i++)
        {
            Grow(intervalGrowth);

            yield return null;
        }
    }

}
