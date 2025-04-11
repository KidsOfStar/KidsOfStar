using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideFormState : BaseFormState
{
    public HideFormState(PlayerStateContext _context, FormData data) : base(_context, data) { }

    public override void OnJump() { }
}
