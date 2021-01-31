using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundScript : MonoBehaviour
{
    public void Step()
    {
        AkSoundEngine.PostEvent("PlayerFootstep", this.gameObject);
    }

    public void Jump()
    {
        AkSoundEngine.PostEvent("PlayerJump", this.gameObject);
    }
}
