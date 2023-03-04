using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperSkillUI : MonoBehaviour
{
    private TempUIInfo tempUIInfo;

    private void Start()
    {
        tempUIInfo = GetComponentInParent<TempUIInfo>();
    }

    public void CallSwapSkill()
    {
        tempUIInfo.SwapSkillIcon();
    }
}
