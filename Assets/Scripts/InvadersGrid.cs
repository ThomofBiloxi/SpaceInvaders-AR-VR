using UnityEngine;
using UnityEngine.SceneManagement;

public class InvadersGrid : MonoBehaviour
{
    [Header("InvadersGrid")]
    public Invader3Dupdate[] prefabs = new Invader3Dupdate[5];
    public AnimationCurve speed = new AnimationCurve();
   // public float speed = 0.2f;
    public Vector3 direction { get; private set; } = Vector3.right;
    public Vector3 initialPosition { get; private set; }
    public System.Action<Invader3Dupdate> killed;
    
    public int AmountKilled { get; private set; }
    public int AmountAlive => TotalAmount - AmountKilled;
    public int TotalAmount => rows * columns;
    public float PercentKilled => (float)AmountKilled / (float)TotalAmount;

    [Header("Grid")]
    public int rows = 5;
    public int columns = 11;

    [Header("Missiles")]
    public Projectile missilePrefab;
    public float missileSpawnRate = 1f;

    [Header("Boundaries")]
    public GameObject LeftBoundary;
    public GameObject RightBoundary;

    private void Awake()
    {
        initialPosition = transform.position;

        // Form the grid of invaders
        for (int i = 0; i < rows; i++)
        {
            float width = 0.1f * (columns - 1);
            float height = 0.1f * (rows - 1);

            Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (1.4f * i) + centerOffset.y, 0f);

            for (int j = 0; j < columns; j++)
            {
                // Create an invader and parent it to this transform
                Invader3Dupdate invader = Instantiate(prefabs[i], transform);
                invader.killed += OnInvaderKilled;

                // Calculate and set the position of the invader in the row
                Vector3 position = rowPosition;
                position.x += 1.5f * j;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), missileSpawnRate, missileSpawnRate);
    }

    private void MissileAttack()
    {
        int amountAlive = AmountAlive;

        // No missiles should spawn when no invaders are alive
        if (amountAlive == 0)
        {
            return;
        }

        foreach (Transform invader3D in transform)
        {
            // Any invaders that are killed cannot shoot missiles
            if (!invader3D.gameObject.activeInHierarchy)
            {
                continue;
            }

            // Random chance to spawn a missile based upon how many invaders are
            // alive (the more invaders alive the lower the chance)
            if (Random.value < (1f / (float)amountAlive))
            {
                Instantiate(missilePrefab, invader3D.position, Quaternion.identity);
                break;
            }
        }
    }

    private void Update()
    {
        // Evaluate the speed of the invaders based on how many have been killed
        float invaderSpeed = this.speed.Evaluate(PercentKilled);
        transform.position += direction * invaderSpeed * Time.deltaTime * 1.0f;

        // The invaders will advance to the next row after reaching the edge of
        // the screen
        foreach (Transform invader3D in transform)
        {
            //// Skip any invaders that have been killed
            if (!invader3D.gameObject.activeInHierarchy)
            {
                continue;
            }

            // Check the left edge or right edge based on the current direction
            if (direction == Vector3.right && invader3D.position.x >= RightBoundary.transform.position.x)
            {
                AdvanceRow();
                break;
            }
            else if (direction == Vector3.left && invader3D.position.x <= LeftBoundary.transform.position.x)
            {
                AdvanceRow();
                break;
            }
        }
    }

    private void AdvanceRow()
    {
        // Flip the direction the invaders are moving
        direction = new Vector3(-direction.x, 0f, 0f);

        // Move the entire grid of invaders down a row
        Vector3 position = transform.position;
        position.y -= 0.1f;
        transform.position = position;
    }

    private void OnInvaderKilled(Invader3Dupdate invader3Dupdate)
    {
        invader3Dupdate.gameObject.SetActive(false);
        AmountKilled++;
        killed?.Invoke(invader3Dupdate); // Safely invoke the event
    }

    public void ResetInvaders()
    {
        AmountKilled = 0;
        direction = Vector3.right;
        transform.position = initialPosition;

        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    foreach (Transform invader3D in transform)
    //    {
    //        //// Skip any invaders that have been killed
    //        if (!invader3D.gameObject.activeInHierarchy)
    //        {
    //            continue;
    //        }

    //        // Check the left edge or right edge based on the current direction
    //        if (direction == Vector3.right && invader3D.position.x >= RightBoundary.transform.position.x)
    //        {
    //            AdvanceRow();
    //            break;
    //        }
    //        else if (direction == Vector3.left && invader3D.position.x <= LeftBoundary.transform.position.x)
    //        {
    //            AdvanceRow();
    //            break;
    //        }
    //    }
    //}

}
