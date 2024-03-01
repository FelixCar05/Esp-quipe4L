using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MuzzleFlash : MonoBehaviour
{
  

    public Transform gunTransform; // The transform of the gun object
    public Vector3 recoilRotation = new Vector3(3f, 0f, 0f); // Amount of recoil rotation
    public float recoilSpeed = 5f; // Speed of recoil

    private Quaternion initialRotation;
    private Quaternion recoilRotationQuaternion;
    private bool isRecoiling = false;



    void Start()
    {
        initialRotation = gunTransform.localRotation;
        recoilRotationQuaternion = Quaternion.Euler(recoilRotation);
    }


  

   


    void UpdateRecoil()
    {
        if (isRecoiling)
        {
            gunTransform.localRotation = Quaternion.Slerp(gunTransform.localRotation, recoilRotationQuaternion, Time.deltaTime * recoilSpeed);
            if (Quaternion.Angle(gunTransform.localRotation, recoilRotationQuaternion) < 0.1f)
            {
                isRecoiling = false;
            }
        }
        else
        {
            // Return to initial rotation
            gunTransform.localRotation = Quaternion.Slerp(gunTransform.localRotation, initialRotation, Time.deltaTime * recoilSpeed);
        }

    }



    void ApplyRecoil()
    {
        isRecoiling = true;
    }





}

