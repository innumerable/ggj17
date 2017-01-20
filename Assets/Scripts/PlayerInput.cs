using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool IsPressed
    {
        get
        {
            // If the button is pressed.
            return Input.GetAxis("Input") >= 0.5f;
        }
    }
}
