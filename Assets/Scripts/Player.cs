using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{

    public Projectile laserPrefab; // prefab var that holds the projectile script
    public float speed = 5.0f; // speed variable

    // in Space Invaders there can only be one laser bullet at a time on screen
    private bool _laserActive;


    // change position of player based on speed while checking input
    private void Update()
    {
        // GetKey vs GetKeyDown. GetKey returns True every single frame the key is pressed.
        // GetKeyDown will only return True the first frame the key is pressed down
        // this allows for the player to keep moving when the key is held down

        // moves the player to left or right when A or D or arrow keys are pressed
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            // same as invaders movement also deltatime to be aware of frame rate
            this.transform.position += Vector3.left * this.speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += Vector3.right * this.speed * Time.deltaTime;
        }

        // if user presses another input shoot laser
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // mousebuttondown 0 means left click
        {
            Shoot();
        }
    }

    private void Shoot()
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") ||
            other.gameObject.layer == LayerMask.NameToLayer("Missile")) // Layers created in editor project settings
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
