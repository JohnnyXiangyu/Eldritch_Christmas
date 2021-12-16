using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMachineState
{
    IMachineState nextState = null;

    public abstract void Initialize();

    public abstract void Update();

    public IMachineState Next()
    {
        if (nextState == null)
            return this;
        else
            return nextState;
    }
}
