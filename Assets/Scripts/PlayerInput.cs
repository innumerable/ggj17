using UnityEngine;

public static class PlayerInput
{
    public static bool IsPressed
    {
        get
        {
            return Input.anyKey;
        }
    }
}
