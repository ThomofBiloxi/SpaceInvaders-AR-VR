using UnityEngine;

public class Invader : MonoBehaviour
{
    public Sprite[] splat; 
    public Sprite[] animationSprites; // an array of sprites its public meaning we can change it in the editor
    public float animationTime = 1.0f; // var for how often does it cycle to the next sprite

    private SpriteRenderer _spriteRenderer; // reference to our sprite renderer so we can actually change which sprite is being rendered
    private int _animationFrame; // which sprite we are using which will just be an index # in our sprite array

    // delegate to inform other scripts that invader was killed
    public System.Action killed;

    // establish a reference to our sprite render. Awake does the first life cycle of unity script.
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>(); //going to look at the component we specify
    }


    // Start executes the very first frame of game of an active game object
    private void Start()
    {
        // on our first frame we want to start our animation loop
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime); // invoke lets us invoke a method after a set number or seconds and repeat repeats invoke
    }

    // everytime this is called this update which frame we are on
    private void AnimateSprite()
    {
        _animationFrame++; // increments to next frame

        // exceded how many sprites provided then loop back to first sprite
        if (_animationFrame >= this.animationSprites.Length)
        {
            _animationFrame = 0;
        }

        _spriteRenderer.sprite = this.animationSprites[_animationFrame];
    }

    // when projectile hits invader gets destroyed
    private void OnTriggerEnter2D(Collider2D other)  // unity trgger function collides with other game object
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))  // this is the Layer "Laser" we made in editor
        {
            _spriteRenderer.sprite = this.splat[0];
            this.killed.Invoke();
            
            this.gameObject.SetActive(false);  // completely turns off game object so it stops rendering
        }
    }

}
