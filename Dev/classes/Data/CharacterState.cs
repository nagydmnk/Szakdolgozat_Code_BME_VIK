using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState
{
    private int stressLevel;
    private int workCommitment;
    private int trustInOther;

    public int StressLevel
    {
        get { return stressLevel; }
        set { stressLevel = Mathf.Clamp(value, 0, 11); }
    }

    public int WorkCommitment
    {
        get { return workCommitment; }
        set { workCommitment = Mathf.Clamp(value, 0, 11); }
    }

    public int TrustInOther
    {
        get { return trustInOther; }
        set { trustInOther = Mathf.Clamp(value, 0, 11); }
    }
}
