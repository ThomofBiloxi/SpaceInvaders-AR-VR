using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerContBK : MonoBehaviour
{

    public CharacterController characterController;
    public float speed = 5f;

    public Projectile laserPrefab;

    private bool _laserActive;
    public Vector3 initialPosition { get; private set; }
    public Vector3 direction { get; private set; }

    public GameObject LeftBoundary;
    public GameObject RightBoundary;

    private GameObject playerPosition = null;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (playerPosition == null)
            playerPosition = GameObject.Find("playerBK");
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Keyboard.current.leftArrowKey.isPressed)
        {
            MoveLeft();
            // same as invaders movement also deltatime to be aware of frame rate
        }

        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Keyboard.current.rightArrowKey.isPressed)
        {
            MoveRight();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // GetKey vs GetKeyDown. GetKey returns True every single frame the key is pressed.
        // GetKeyDown will only return True the first frame the key is pressed down
        // this allows for the player to keep moving when the key is held down

        // moves the player to left or right when A or D or arrow keys are pressed
        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Keyboard.current.anyKey.wasPressedThisFrame)
        //{
        // same as invaders movement also deltatime to be aware of frame rate
        //MoveLeft();
        //}


        // if user presses another input shoot laser
        if (!PauseController.gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Keyboard.current.spaceKey.isPressed) // mousebuttondown 0 means left click
            {
                Shoot();
            }
        }

    }

    public void MoveLeft()
    {
        this.transform.position += Vector3.left * this.speed * Time.deltaTime;
    }

    public void MoveRight()
    {
        this.transform.position += Vector3.right * this.speed * Time.deltaTime;
    }

    public void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 move = transform.right * horizontal;
        characterController.Move(speed * Time.deltaTime * move);
    }

    public void Shoot()
    {
        // when user shoots it will instantiate a new instance of the prefab
        // the position should be psotion where you are shooting from this case the player position
        // rotation doesnt matter so Quaternion should be set to identity

        if (!_laserActive)
        {
            Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            // this subscribes the projectile instantiated object to the event
            projectile.destroyed += LaserDestroyed;
            _laserActive = true;

        }
    }

    // gets called when delegate gets invoked and then will set laserActive back to false which will let user
    // shoot again
    private void LaserDestroyed()
    {
        _laserActive = false;
    }

    // player collides with missile or invader
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") ||
            other.gameObject.layer == LayerMask.NameToLayer("Missile")) // Layers created in editor project settings
        {
            Debug.Log("Player hit something");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "LeftBoundary")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else if (collision.gameObject.name == "RightBoundary")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}