using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Invader3Dupdate : MonoBehaviour
{
    //public Mesh[] splat;
    // MeshFilter currentMesh;
    private MeshFilter _currentMesh;
    public Mesh[] animationMeshes; // an array of sprites its public meaning we can change it in the editor
    public float animationTime = 1.0f; // var for how often does it cycle to the next sprite

    private MeshRenderer _meshRenderer; // reference to our sprite renderer so we can actually change which sprite is being rendered
    private int _animationFrame; // which sprite we are using which will just be an index # in our sprite array

    // delegate to inform other scripts that invader was killed
    public System.Action<Invader3Dupdate> killed;

    //public GameObject model1;
    //public GameObject model2;

    private int animationFrame;

    // establish a reference to our sprite render. Awake does the first life cycle of unity script.
    private void Awake()
    {
       MeshFilter _currentMesh = GetComponent<MeshFilter>(); //going to look at the component we specify
    }


    // Start executes the very first frame of game of an active game object
    private void Start()
    {
        // on our first frame we want to start our animation loop
        InvokeRepeating(nameof(AnimateMesh), this.animationTime, this.animationTime); // invoke lets us invoke a method after a set number or seconds and repeat repeats invoke

        //model1.SetActive(true);
        //model2.SetActive(true);
        // Debug.Log("object active");
    }

    // everytime this is called this update which frame we are on
    private void AnimateMesh()
    {
        // MeshFilter _currentMesh = GetComponent<MeshFilter>();
        _animationFrame++; // increments to next frame

        // exceded how many sprites provided then loop back to first sprite
        if (_animationFrame >= this.animationMeshes.Length)
        {
           
            _animationFrame = 0;
        }

        GetComponent<MeshFilter>().mesh = this.animationMeshes[_animationFrame];

        Debug.Log("Animation frame: " + animationFrame);

        //_meshRenderer.mesh = this.animationMeshes[_animationFrame];
    }

    // when projectile hits invader gets destroyed
    //private void OnTriggerEnter(Collider other)  // unity trgger function collides with other game object
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))  // this is the Layer "Laser" we made in editor
    //    {
    //        //_meshRenderer.mesh = this.splat[0];
    //        this.killed.Invoke();

    //        this.gameObject.SetActive(false);  // completely turns off game object so it stops rendering
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))  // this is the Layer "Laser" we made in editor
        {
            //_meshRenderer.mesh = this.splat[0];
            this.killed?.Invoke(this);

            this.gameObject.SetActive(false);  // completely turns off game object so it stops rendering
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //void Update()
    //{
    //    if (Keyboard.current.spaceKey.wasPressedThisFrame) // mousebuttondown 0 means left click
    //    {
    //        AnimateMesh();
    //        Debug.Log("object animate");
    //    }
    //}

}
