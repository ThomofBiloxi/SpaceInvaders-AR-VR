using UnityEngine;

public class Bunker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            // don't do this when implementing scoring and rounds instead turn off or on
            // for in between rounds
            this.gameObject.SetActive(false);   
        }
    }
}
