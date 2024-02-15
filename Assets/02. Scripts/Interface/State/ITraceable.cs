using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITracable : IStateable
{
    void TraceUpdate();
}
