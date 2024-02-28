using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MuzzleFlash : MonoBehaviour
{
    public GameObject flashPrefab; // Prefab for the muzzle flash effect
    public Transform flashSpawnPoint; // Point where the muzzle flash should spawn
    public float flashDuration = 0.1f; // Duration of the muzzle flash

    // Update is called once per frame
    void Update()
    {
        // Check for input to trigger the muzzle flash (for example, firing a gun)
        if (Input.GetMouseButtonDown(0)) // Change this to your desired input
        {
            // Spawn the muzzle flash effect
            GameObject muzzleFlash = Instantiate(flashPrefab, flashSpawnPoint.position, flashSpawnPoint.rotation);

            // Set the muzzle flash as a child of the flash spawn point to move and rotate with it
            muzzleFlash.transform.parent = flashSpawnPoint;

            // Destroy the muzzle flash effect after the specified duration
            Destroy(muzzleFlash, flashDuration);
        }
    }
}

