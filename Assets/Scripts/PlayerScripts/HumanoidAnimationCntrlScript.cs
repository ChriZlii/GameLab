using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Animator))]
public class HumanoidAnimationCntrlScript : MonoBehaviour
{
    // Public fields
    public bool EnableHeadIK = false;
    public bool EnableHandIK = false;
    

    // Privat fileds
    private Animator animator;

    private Transform IKrightHandle = null;
    private Transform IKleftHandle = null;
    private Transform IKlookObj = null;

    // Public fields
    public enum WalkTypes
    {
        Run_Forward,
        Walk_Forward,
        Idle,
        Walk_Backwards,
        Walk_Left,
        Walk_Left_Diagonal,
        Walk_Right,
        Walk_Right_Diagonal,
    }

    void Start()
    {
        animator = GetComponent<Animator>();


    }

    public void MoveBodyForward(float speed)
    {
        if (!animator) return;
        
        if (speed < -5) speed = -5;
        else if (speed > 10) speed = 10;
        animator.SetFloat("Forward", speed);
    }

    public void MoveBodySideward(float speed)
    {
        if (!animator) return;

        if (speed > 5) speed = 5;
        else if (speed < -5) speed = -5;
        animator.SetFloat("Sideward", speed);
    }

    public void MoveBody(WalkTypes walkTypes)
    {
        if (walkTypes == WalkTypes.Run_Forward) MoveBodyForward(10);
        else if (walkTypes == WalkTypes.Walk_Forward) MoveBodyForward(5);
        else if (walkTypes == WalkTypes.Idle) MoveBodyForward(0);
        else if (walkTypes == WalkTypes.Walk_Backwards) MoveBodyForward(-5);
        else if (walkTypes == WalkTypes.Walk_Left) MoveBodySideward(-5);
        else if (walkTypes == WalkTypes.Walk_Right) MoveBodySideward(5);
        else if (walkTypes == WalkTypes.Walk_Right_Diagonal)
        {
            MoveBodyForward(5);
            MoveBodySideward(5);
        }
        else if (walkTypes == WalkTypes.Walk_Left_Diagonal)
        {
            MoveBodyForward(5);
            MoveBodySideward(-5);
        }
    }

    public void RotateBody(float degree)
    {
        // no Animations
        transform.Rotate(Vector3.up * degree);
    }


    // IK stuff________________________________________

    public void setHandIKTargets(Transform rightHandle, Transform leftHandle)
    {
        this.IKleftHandle = leftHandle;
        this.IKrightHandle = rightHandle;
    }

    public void setHeadIKLockAtTarget(Transform target)
    {
        this.IKlookObj = target;
    }



    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {
            // Head IK--------------
            if (EnableHeadIK)
            {
                // Set the look target position, if one has been assigned
                if (IKlookObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(IKlookObj.position);
                }
            }
            else
            {
                animator.SetLookAtWeight(0);
            }

            // Hand IK--------------
            if (EnableHandIK)
            {
                // Set the right hand target position and rotation, if one has been assigned
                if (IKrightHandle != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, IKrightHandle.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, IKrightHandle.rotation);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (IKleftHandle != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, IKleftHandle.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, IKleftHandle.rotation);
                }

            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }






}
