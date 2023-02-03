using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DebugShooting : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] gunshots;
    [SerializeField] private GameObject bulletCasing;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private CharacterController characterController;

    [SerializeField] private Transform weaponTransform;
    [SerializeField] private Transform aimTransform;
    [SerializeField] private Transform sideTransform;
    [SerializeField] private Transform viewTransform;
    [SerializeField] private Camera playerCam;

    [SerializeField] private int bulletsInClip = 20;

    int gunshotsClips;

    Sequence shootSeq = null;

    // Start is called before the first frame update
    void Start()
    {
        gunshotsClips = gunshots.Length - 1;
        DOTween.Init(null, false, LogBehaviour.ErrorsOnly);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1)){
            weaponTransform.transform.DOLocalMove(aimTransform.localPosition, .4f).SetEase(Ease.InOutSine);
            playerCam.DOFieldOfView(30f, .3f).SetEase(Ease.InOutSine);
        }
        if(Input.GetKeyUp(KeyCode.Mouse1)){
            weaponTransform.transform.DOLocalMove(sideTransform.localPosition, .4f).SetEase(Ease.InOutSine);
            playerCam.DOFieldOfView(60f, .3f).SetEase(Ease.InOutSine);
        }

        if(bulletsInClip == 0) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if(state.IsName("Shoot") || state.IsName("ShootLast")) return;

        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            if(bulletsInClip == 1) {
                animator.Play("Base Layer.ShootLast", 0, 0);
            }
            else {
                animator.Play("Base Layer.Shoot", 0, 0);
            }
            bulletsInClip--;
            audioSource.PlayOneShot(gunshots[Random.Range(0, gunshotsClips)]);
            SpawnBulletCasing(20f);


            //tweening for recoil effect, will change in the future
            shootSeq = DOTween.Sequence()
                .Join(weaponTransform.DOPunchPosition(new Vector3(0f,.1f,-.25f), .3f))
                .Join(weaponTransform.DOPunchRotation(new Vector3(-25f,0f,0f), .2f))
                .Join(viewTransform.DOPunchRotation(new Vector3(-8f,0f,0f), .7f, 0, 0f));

            shootSeq.Restart();

        }
    }

    void SpawnBulletCasing(float lifetime) {
        GameObject casing = Instantiate(bulletCasing, bulletSpawn.position, bulletSpawn.rotation  * Quaternion.Euler(90f,0f,0f));
        Rigidbody casingRB = casing.GetComponent<Rigidbody>();
        casingRB.AddForce(//characterController.transform.TransformDirection(characterController.velocity) +
        bulletSpawn.transform.TransformDirection(new Vector3(.6f,.5f,-.8f)), ForceMode.VelocityChange);
        Destroy(casing, lifetime);
    }
}
