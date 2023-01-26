using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugShooting : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] gunshots;
    [SerializeField] private GameObject bulletCasing;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private CharacterController characterController;

    [SerializeField] private int bullets_in_clip = 20;

    int gunshotsClips;

    // Start is called before the first frame update
    void Start()
    {
        gunshotsClips = gunshots.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(bullets_in_clip == 0) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if(state.IsName("Shoot") || state.IsName("ShootLast")) return;

        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            if(bullets_in_clip == 1) {
                animator.Play("Base Layer.ShootLast", 0, 0);
            }
            else {
                animator.Play("Base Layer.Shoot", 0, 0);
            }
            bullets_in_clip--;
            audioSource.PlayOneShot(gunshots[Random.Range(0, gunshotsClips)]);
            SpawnBulletCasing(20f);
        }
    }

    void SpawnBulletCasing(float lifetime) {
        GameObject casing = Instantiate(bulletCasing, bulletSpawn.position, bulletSpawn.rotation  * Quaternion.Euler(90f,0f,0f));
        Rigidbody casingRB = casing.GetComponent<Rigidbody>();
        casingRB.AddForce(//characterController.transform.TransformDirection(characterController.velocity) +
        bulletSpawn.transform.TransformDirection(new Vector3(.6f,.5f,-.8f)), ForceMode.VelocityChange);
        Debug.Log(bulletSpawn.transform.TransformDirection(new Vector3(.6f,.5f,-.8f)));
        Destroy(casing, lifetime);
    }
}
