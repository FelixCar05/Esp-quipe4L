using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    public ParticleSystem MuzzleflashP;
    public float fireRate = 1f;

    public Camera fpsCam;
    public Transform trigger;
    private bool isTwirlable = true;


    [SerializeField]
    List<GameObject> muzzleFlashes = new List<GameObject>();

    public Transform gunTransform; // The transform of the gun object
    private Vector3 recoilRotation = new Vector3(0f, 100f, 0f); // Amount of recoil rotation
    private float recoilSpeed = 10f; // Speed of recoil

    private Quaternion initialRotation;
    private Quaternion recoilRotationQuaternion;
    private bool isRecoiling = false;



    void Start()
    {
        initialRotation = gunTransform.localRotation;
        recoilRotationQuaternion = Quaternion.Euler(recoilRotation);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRecoil();
        if (Input.GetKeyDown(fire))
        {
            //Shoot();
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
            transform.Rotate(Vector3.back, 400f * Time.deltaTime);
            //transform.RotateAround(trigger.position, Vector3.back, 100f * Time.deltaTime);
            //transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z - 900f * Time.deltaTime);

            yield return null; // Wait for the next frame
        }
    }

    private void ShootAnimation()
    {
        isTwirlable = false;

        //set rotation back to normals
        transform.rotation = transform.rotation = new Quaternion(fpsCam.transform.rotation.x, fpsCam.transform.rotation.y, fpsCam.transform.rotation.z, fpsCam.transform.rotation.w);
        transform.Rotate(Vector3.up, 90f);

        System.Random muzzleSelector = new System.Random();
        int selected = muzzleSelector.Next(0, muzzleFlashes.Count);


        GameObject muzzleFlash = Instantiate(muzzleFlashes[selected], flashSpawnPoint.position, transform.rotation);
        MuzzleflashP.Play();
        // Set the muzzle flash as a child of the flash spawn point to move and rotate with it
        muzzleFlash.transform.parent = flashSpawnPoint;
        muzzleFlash.transform.Rotate(Vector3.up, 180f);

        // Destroy the muzzle flash effect after the specified duration
        Destroy(muzzleFlash, flashDuration);

        StartCoroutine(MotionCoroutine());

        StartCoroutine(ReactivateTwirl());
    }


    IEnumerator MotionCoroutine()
    {
        int i = 0;
        if (i <= 10)
            transform.Rotate(Vector3.back, 100f * Time.deltaTime);
        if (i > 10)
            transform.Rotate(Vector3.back, -100f * Time.deltaTime);
        if (i <= 20)
        {
            transform.rotation = transform.rotation = new Quaternion(fpsCam.transform.rotation.x, fpsCam.transform.rotation.y, fpsCam.transform.rotation.z, fpsCam.transform.rotation.w);
            transform.Rotate(Vector3.up, 90f);
            StopCoroutine(MotionCoroutine());
        }
     

        // Wait for the next frame
        yield return null;

    }





    IEnumerator ReactivateTwirl()
    {
        yield return new WaitForSeconds(1f);
        isTwirlable = true;
    }

    private void Shoot()
    {
        ApplyRecoil();
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
