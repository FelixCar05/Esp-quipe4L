using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public GameObject Coin;
    private float coinForce = 10f;

    //controls
    public KeyCode fire = KeyCode.Mouse0;
    public KeyCode altfire = KeyCode.Mouse1;
    public KeyCode flick = KeyCode.F;

    bool isSpinning = false;

    //shooting
    //public GameObject flashPrefab; // Prefab for the muzzle flash effect
    public Transform flashSpawnPoint; // Point where the muzzle flash should spawn
    public float flashDuration = 0.1f; // Duration of the muzzle flash
    public Camera fpsCam;
    public Transform trigger;
    private bool isTwirlable = true;


    [SerializeField]
    List<GameObject> muzzleFlashes = new List<GameObject>();




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(fire))
        {
            Shoot();
            ShootAnimation();
        }
        if (Input.GetKeyDown(altfire) && !isSpinning && isTwirlable)
        {
            Debug.Log("spinny3");
            StartCoroutine(SpinAnimation());
            //on key down boost damage, on key up put back to normal
            //spin revolver (boosts shot damage)
        }
        if (Input.GetKeyUp(altfire))// Check for mouse release
        {
            // Stop spinning
            isSpinning = false;
            transform.rotation = transform.rotation = new Quaternion(fpsCam.transform.rotation.x, fpsCam.transform.rotation.y, fpsCam.transform.rotation.z, fpsCam.transform.rotation.w);
            transform.Rotate(Vector3.up, 90f);
        }
        if (Input.GetKeyDown(flick))
        {
            var coinInstance = Instantiate(Coin, fpsCam.transform.position, Quaternion.identity);//coin should spawn in front of player and shoot

            coinInstance.GetComponent<Rigidbody>().AddForce((fpsCam.transform.forward + fpsCam.transform.up) * coinForce, ForceMode.Impulse);
            coinInstance.GetComponent<Rigidbody>().AddTorque(new Vector3(0.5f, 0, 0.5f) * 1000f);//adjust vector based on cam orientation on both of these
        }

    }

    IEnumerator SpinAnimation()
    {
        isSpinning = true;

        Debug.Log("spinny2");
        while (isSpinning)
        {

            Debug.Log("spinny");
            // Get the current rotation
            Vector3 currentRotation = transform.rotation.eulerAngles;

            // Add rotation around the object's local up axis (Vector3.up) based on the spinSpeed and deltaTime
            //transform.RotateAround(Vector3.right, 900f * Time.deltaTime);
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z - 900f * Time.deltaTime);

            yield return null; // Wait for the next frame
        }
    }

    private void ShootAnimation()
    {
        isTwirlable = false;
        transform.rotation = transform.rotation = new Quaternion(fpsCam.transform.rotation.x, fpsCam.transform.rotation.y, fpsCam.transform.rotation.z, fpsCam.transform.rotation.w);
        transform.Rotate(Vector3.up, 90f);
        // Check for input to trigger the muzzle flash (for example, firing a gun)
        // Spawn the muzzle flash effect
        System.Random muzzleSelector = new System.Random();
        int selected = muzzleSelector.Next(0, muzzleFlashes.Count);


        GameObject muzzleFlash = Instantiate(muzzleFlashes[selected], flashSpawnPoint.position, transform.rotation);

        // Set the muzzle flash as a child of the flash spawn point to move and rotate with it
        muzzleFlash.transform.parent = flashSpawnPoint;
        muzzleFlash.transform.Rotate(Vector3.up, 180f);

        // Destroy the muzzle flash effect after the specified duration
        Destroy(muzzleFlash, flashDuration);

        StartCoroutine(ReactivateTwirl());
    }

    IEnumerator ReactivateTwirl()
    {
        yield return new WaitForSeconds(1f);
        isTwirlable = true;
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            LineRenderer trail = GetComponent<LineRenderer>();
            trail.enabled = true;
            trail.positionCount = 2;//can have multiple points for ricochets
                                    //Vector3 target = hit.point;//default
                                    //if (!(hit.rigidbody.gameObject.tag == "Coin"))
                                    //{
                                    //    trail.SetPosition(1, hit.rigidbody.gameobject.transform.position);

            //}

            trail.SetPosition(0, fpsCam.transform.position);
            trail.SetPosition(1, hit.point);

            StartCoroutine(deleteTrail(trail));

            Debug.Log(hit.transform);
        }

    }

    IEnumerator deleteTrail(LineRenderer trail)
    {
        yield return new WaitForSeconds(0.5f);
        trail.enabled = false;
    }
}
