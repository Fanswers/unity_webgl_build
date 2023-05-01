using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Generators
{
    public static float RandomBinomial => Random.Range(0f, 1f) - Random.Range(0f, 1f);
}
