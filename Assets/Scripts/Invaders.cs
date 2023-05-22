using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    // 
    public Invader3D[] prefabs; // an array of prefbas that correspond to each row within our grid each row instatiates each prefab

    // for the invader grid
    public int rows = 5;
    public int columns = 11;
    private Vector3 _direction = Vector2.right; // initially they move to the right

    // speed is determined by how many invaders are left on the screen
    // AnimationCurve is basically an XY graph, where x will be percentageKilled and y will be speed
    // AnimationCurve turns on a curve graph in our speed setting window in our editor
    public AnimationCurve speed;

    // Create a prefab for Missile graphic
    // uses Projectile script
    public Projectile missilePrefab;

    public float missileAttackRate = 1.0f; // how often will there be missiles

    public int amountAlive => this.totalInvaders - this.amountKilled;

    public int amountKilled { get; private set; }
    public int totalInvaders => this.rows * this.columns;   // => makes it a calculated property

    // need to know percentage of invaders killed to calculate the speed of remaining invaders
    public float percentKilled => (float)this.amountKilled / (float)this.totalInvaders;

    
    private void Awake()
    {
        // 2d for loop to loop through each row and column
        for (int row = 0; row < this.rows; row++)
        {
            float width = 1.0f * (this.columns - 1); // spacing of invaders * total amount of columns
            float height = 10.0f * (this.rows - 1);  // spacing (2.0) * total amount of rows

            // centering need to subrtract half of the total of our grid
            Vector2 centering = new Vector2(-width /2, -height /2); // center is halfway of each edge of grid

            // apply centering to center the grid and start a starting position
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 2.0f), 0.0f); // setting the y axis here

            for (int col = 0; col < this.columns; col++)
            {
                // this.transform will make Invaders class the Parent so we can move the entire grid of invaders
                // not just each indivdual invader
                Invader3D invader = Instantiate(this.prefabs[row], this.transform);

                // when killed delegate gets called will invoke InvaderKilled method
                invader.killed += InvaderKilled;

                // Do math to set invader positions. We need to know the postion of the row
                // then we can offset that based on the column
                Vector3 position = rowPosition;

                // this will be the offset. size of invader is 1 unit plus 1 pad = 2
                // column is x axis and rows is y axis
                position.x += col * 2.0f;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }

    // update is called every single frame the game is running. Commenly used for input or movement 
    private void Update()
    {
        // we need to know direction they are moving * speed. Delta time to account
        // for frame rate this will make sure movement is consistent regardless of framerate 
        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

        // Unity provides functions to calculate the viewport coordinats to world point coordinates
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        // find the position of each invader so we know when it touches the edge of
        // the screen, so they can bounce back to the center

        // loops through every child of transform object
        foreach (Transform invader in this.transform)
        {
            // check if our invader has been disabled (when it is shot down)
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            // checks if invader hits a right edge or leftedge of screen then flip direction and advance invaders down a row
            if (_direction == Vector3.right && invader.position.x >= (rightEdge.x - 1.0f)) // rightEdge plus 1 of padding so invaders don't clip off screen
            {
                AdvanceRow();
            } else if (_direction == Vector3.left && invader.position.x <= (leftEdge.x + 1.0f)) // leftEdge plus 1 of padding
            {
                AdvanceRow();
            }
        
        }
    }

    private void AdvanceRow()
    {
        _direction.x *= -1.0f; // filps by multiplying a negative to be positive or negative

        // takes current position and updates it
        Vector3 position = this.transform.position;
        // takes one less in y direction which moves invaders down to the next row
        position.y -= 1.0f;

        // send new invader position to the transform
        this.transform.position = position;

    }


    // spawns missile depending on amount of invaders alive.
    // Inverse relationship. The more invaders alive the less missile spawning
    private void MissileAttack()
    {
        // loops through all invaders in grid
        foreach (Transform invader in this.transform)
        {
            // check if our invader has been disabled (when it is shot down)
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            // Random.value returns a value between 0 and 1
            // dividing 1 by amountAlive will return a small number below one
            // for Random.value to compare to
            // as more and more die the chances will increase
            if (Random.value < (1.0f / (float)this.amountAlive))
            {
                // Spawns 1 missle then break so there is only 1 missile at a time on scene
                Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void InvaderKilled()
    {
        this.amountKilled++;

        // for no scoring. Just reloads scene. Acts to restart game
        if (this.amountKilled >= this.totalInvaders)
        {
            // unity function to reload original scene 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
