using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvWeapon : MonoBehaviour
{
    [SerializeField] private GameObject pistolGameObject;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform barrel;
    [SerializeField] private Light lightFlash;


    [SerializeField] private AudioClip gunshot;


    [Header("Settings")]
    [SerializeField] private int maxBulletsInClip = 7;



    private int bulletsInClip;
    private bool pistolEquiped = true;
    private bool reloading = false;
    private bool helpu = false;



    // Start is called before the first frame update
    void Start()
    {
        bulletsInClip = maxBulletsInClip;
    }

    // Update is called once per frame
    void Update()
    {
        if(!pistolEquiped) return;
        
        lightFlash.intensity -= 10 * Time.deltaTime;


        animator.SetBool("Aim", Input.GetKey(KeyCode.Mouse1));

        if(Input.GetKeyDown(KeyCode.Mouse0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot") && bulletsInClip > 0 && !reloading){
            animator.SetTrigger("Shoot");
            audioSource.pitch = Random.Range(1f,1.1f);
            audioSource.PlayOneShot(gunshot);
            bulletsInClip--;
            lightFlash.intensity = 1;
            Debug.DrawRay(barrel.transform.position, barrel.transform.forward * 10, Color.red, 3);

            RaycastHit hit;
            if(Physics.Raycast(barrel.transform.position, barrel.transform.forward, out hit, 300)){
                if(hit.transform.gameObject.tag == "Shootable"){
                    hit.rigidbody.AddForceAtPosition(barrel.transform.forward * 3, hit.point, ForceMode.Impulse);
                }
            }
        }

        if(reloading){
            bool desudesu = animator.GetCurrentAnimatorStateInfo(0).IsName("reload") || animator.GetCurrentAnimatorStateInfo(0).IsName("reloadNoammo");
            if(desudesu || helpu) {
                helpu = true;
                reloading = desudesu;
            }
            if(!reloading){
                bulletsInClip = maxBulletsInClip;
            }
        }

        if(Input.GetKeyDown(KeyCode.R) && !reloading){
            animator.SetBool("Empty", bulletsInClip == 0);
            animator.SetTrigger("Reload");
            bulletsInClip = 0;
            reloading = true;
            helpu = false;
        }

    }

    public void EquipPistol(){
        pistolGameObject.SetActive(true);
        pistolEquiped = true;
    }
    public void DeEquipPistol(){
        animator.SetTrigger("deDraw");
        pistolGameObject.SetActive(false);
        pistolEquiped = false;
    }
}
