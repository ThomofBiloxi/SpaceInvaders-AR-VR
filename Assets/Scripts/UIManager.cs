using UnityEngine;

public class UIManager : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        // Find the game object with the "Laser" tag and get the PlayerController component
        GameObject playerObject = GameObject.FindGameObjectWithTag("PlayerLaser");
        playerController = playerObject.GetComponent<PlayerController>();
    }

    public void CallMoveLeft()
    {
        playerController.MoveLeft();
    }

    public void CallMoveRight()
    {
        playerController.MoveRight();
    }

    public void CallShoot()
    {
        playerController.Shoot();
    }
}