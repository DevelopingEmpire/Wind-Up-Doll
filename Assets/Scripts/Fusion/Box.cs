using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : NetworkBehaviour
{
    NetworkRigidbody nrb;
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        /*
         if (_isPushBox && _pushedBody != null)
        {
            _pushedBody.velocity = _pushDirection * pushPower;
            _isPushBox = false;
            _rootBox.FixedUpdateNetwork();
        }
         */

    }

    public void IsPushBox()
    {

    }
}
