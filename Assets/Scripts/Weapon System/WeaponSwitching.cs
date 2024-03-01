using System;
using System.Collections;
using UnityEngine;
using static System.TimeZoneInfo;

public class WeaponSwitching : MonoBehaviour
{

    //test comment
    public int selectedWeapon;
    public Transform gunTransform; // Reference to the gun's transform
    public Transform Camera;
    public Quaternion initialRotation; // Initial rotation of the gun
    //public Quaternion finalRotation; // Final rotation of the gun
    public float transitionDuration = 10f; // Duration of the transition in seconds


    private float transitionTimer = 0f;
    private bool isTransitioning = false;
    void Start()
    {
        SelectWeapon();
    }



    // Update is called once per frame
    void Update()
    {
        int previousWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)//up
        {
            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;//wrap around
            }
            else
                selectedWeapon++;

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)//down
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
                selectedWeapon--;

        }
        if (previousWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }
    private void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                gunTransform = weapon.gameObject.transform;
                //finalRotation = gunTransform.rotation;

                weapon.gameObject.transform.Rotate(Vector3.right, 30f);
                initialRotation = weapon.gameObject.transform.rotation;
                StartTransition();
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    public void StartTransition()
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            transitionTimer = 0f;
            StartCoroutine(TransitionCoroutine());
        }
    }

    private IEnumerator TransitionCoroutine()
    {
        while (transitionTimer < transitionDuration)
        {
            // Interpolate between initial and final rotations
            float t = transitionTimer / transitionDuration;
            gunTransform.rotation = Quaternion.Slerp(initialRotation, Camera.gameObject.transform.rotation, t);

            transitionTimer += Time.deltaTime;
            yield return null;
        }

        // Ensure final rotation is set
        gunTransform.rotation = Camera.gameObject.transform.rotation;

        isTransitioning = false;
    }
}
