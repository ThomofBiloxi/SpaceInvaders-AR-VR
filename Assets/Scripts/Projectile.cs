using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed; // public so we can change in editor

    // callback used to inform the player that projectile was destroyed
    public System.Action destroyed; // this is a C# delegate pattern


    private void Update()
    {
        this.transform.position += this.direction * this.speed * Time.deltaTime;
    }

    // Unity function for anytime collider is triggered
    private void OnTriggerEnter(Collider other)
    {
        if (this.destroyed != null)
        {
            this.destroyed.Invoke(); // a way to allow other scripts when an event happens
        }

        // anytime projectile collides with something destroy projectile game object
        // Destroy(this.gameObject);
    }
}
