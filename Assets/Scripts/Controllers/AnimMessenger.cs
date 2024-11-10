using UnityEngine;

public class AnimMessenger : MonoBehaviour
{
    [SerializeField] PlayerController player;
    public void Jump(int direction) {
        player.Jump(direction);
    }
}
