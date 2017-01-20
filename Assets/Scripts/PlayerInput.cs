using UnityEngine;

public static class PlayerInput
{
    public static bool IsPressed
    {
        get
        {
            // If the button is pressed.
            return Input.GetAxis("Input") >= 0.5f;
        }
    }
}
