using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget {
    Vector2Int Pos { get; }
    Transform Obj { get; }
}
