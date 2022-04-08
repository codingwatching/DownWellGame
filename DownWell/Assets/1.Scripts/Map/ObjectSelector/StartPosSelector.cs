using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosSelector : ObjectSelector
{
    public StartPosSelector(int code) : base(code)
    {
    }

    public StartPosSelector(int min, int max) : base(min, max)
    {
    }

    protected override GameObject Select(int tileCode)
    {
        var startPosObject = new GameObject();
        startPosObject.name = "StageStart";
        startPosObject.tag = "StageStart";

        return Instantiate(startPosObject);
    }
}
