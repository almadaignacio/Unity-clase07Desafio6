using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    private PlayerData playerData;
    private PlayerMoveForce playerMove;
    [SerializeField] WeaponManager weaponManager;
    public static event Action Ondead;
    public static event Action<int> OnChangeHP;
    [SerializeField] Animator playerAnimator;
    private bool damage;

    public Image bloodEffectImage;

    private float r;
    private float g;
    private float b;
    private float a;


    //public static event Action Dying;




    private void Start()
    {
        playerData = GetComponent<PlayerData>();
        playerMove = GetComponent<PlayerMoveForce>();
        HUDManager.SetHPBar(playerData.HP);

        r = bloodEffectImage.color.r;
        g = bloodEffectImage.color.g;
        b = bloodEffectImage.color.b;
        a = bloodEffectImage.color.a;

    }

    void Update()
    {

    }



    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log("ENTRANDO EN COLISION CON ->" + other.gameObject.name);
       if (other.gameObject.CompareTag("Powerups"))
        {
            Destroy(other.gameObject);
             //sumar vida
            playerData.Healing(other.gameObject.GetComponent<Health>().HealPoints);
            //HUDManager.SetHPBar(playerData.HP);
            PlayerCollision.OnChangeHP?.Invoke(playerData.HP);


            //if (playerData.HP == 30)
            //{
              //  Debug.Log("ESTAS MURIENDO, DEBES CURARTE");
                //Dying?.Invoke();
            //}

            //SUMAS SCORE
            GameManager.Score++;
            Debug.Log(GameManager.Score);
        }

        if (other.gameObject.CompareTag("Munitions"))
        {
            Debug.Log("ENTRANDO EN COLISION CON " + other.gameObject.name);
            Destroy(other.gameObject);
            damage = true;
            playerData.Damage(other.gameObject.GetComponent<Munition>().DamagePoints);
            //HUDManager.SetHPBar(playerData.HP);
            PlayerCollision.OnChangeHP?.Invoke(playerData.HP);
            //PlayerCollision.Dying?.Invoke(playerData.HP);
            if (damage) playerAnimator.SetTrigger("DAMAGE");
            if(damage)
            {
                a += 0.01f;
            }
            else
            {
                a -= 0.01f;

            }

            a = Mathf.Clamp(a, 0, 1f);

            ChangeColor();


            if (playerData.HP <= 0)
            {
                Debug.Log("GAME OVER");
                Ondead?.Invoke();
               // SceneManager.LoadScene("Nivel 1");
            }

            //RESTAS SCORE
            GameManager.Score--;
            Debug.Log(GameManager.Score);
        }

        


        if (other.gameObject.CompareTag("Floor"))
        {
            playerMove.CanJump = true;
        }

        
    }

    private void OnCollisionExit(Collision other)
    {
        //Debug.Log("SALGO DE LA COLISION ->" + other.gameObject.name);
    }

    private void OnCollisionStay(Collision other)
    {
        //Debug.Log("EN CONTACO CON ->" + other.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Trampoline"))
        {
            /*
             Cambio de velocidad instant�neo (ForceMode.VelocityChange)
             Aqui la fuerza se traduce en un cambio de velocidad,
             por lo cual el movimiento se altera significantemente.
             El c�lculo Vector3.up + Vector3.forward permite al
             aplicar fuerza en "diagonal"(hacia arriba y adelante)
            */
            playerMove.MyRigidbody.AddForce((Vector3.up + Vector3.forward) * playerMove.MaxSpeed * 5f, ForceMode.VelocityChange);
        }

        if (other.gameObject.CompareTag("Weapons"))
        {
            other.gameObject.SetActive(false);
            weaponManager.WeaponList.Add(other.gameObject);

            //COLA
           // weaponManager.WeaponQueue.Enqueue(other.gameObject);
            //Debug.Log("ELEMENTOS EN LA COLA " + weaponManager.WeaponQueue.Count);
            
            //STACK
           // weaponManager.WeaponStack.Push(other.gameObject);
            //Debug.Log("ELEMENTOS EN LA STACK " + weaponManager.WeaponStack.Count);
            
            //DIC
            if (!weaponManager.WeaponDirectory.ContainsKey(other.gameObject.name))
            {
                weaponManager.WeaponDirectory.Add(other.gameObject.name, other.gameObject);
                Debug.Log(weaponManager.WeaponDirectory[other.gameObject.name]);
            }

        }

        if (other.gameObject.CompareTag("Goal"))
        {
            PlayerEvent.OnWinCall();
        }

        if (other.gameObject.CompareTag("Win"))
        {
            SceneManager.LoadScene("Win");
        }
    }

    private void ChangeColor()
    {
        Color c = new Color(r, g, b, a);
        bloodEffectImage.color = c;

    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {

    }
}
