using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private AnimationClip[] _clip;

    private int _currentNum;

    private void Update()
    {
        AnimationController animationController = GetComponent<AnimationController>();

        if (animationController == null) return;

        ChangeAnimation();
    }

    private void ChangeAnimation()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) _currentNum = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1)) _currentNum = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) _currentNum = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) _currentNum = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) _currentNum = 4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) _currentNum = 5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) _currentNum = 6;
        if (Input.GetKeyDown(KeyCode.Alpha7)) _currentNum = 7;
        if (Input.GetKeyDown(KeyCode.Alpha8)) _currentNum = 8;
        if (Input.GetKeyDown(KeyCode.Alpha9)) _currentNum = 9;


    }
}
