using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [SerializeField] GameObject[] weapons;
    
    [SerializeField] Transform playerHand;



    //2do TDA LISTA
    [SerializeField] List<GameObject> weaponList;
    public List<GameObject> WeaponList { get => weaponList; set => weaponList = value; }

    //3er TDA COLA
    private Queue weaponQueue;
    public Queue WeaponQueue { get => weaponQueue; set => weaponQueue = value; }


    //4to Stack

    private Stack weaponStack;
   public Stack WeaponStack { get => weaponStack; set => weaponStack = value; }
    public Dictionary<string, GameObject> WeaponDirectory { get => weaponDirectory; set => weaponDirectory = value; }


    //5to TDA DICCIONARIO
    private Dictionary<string, GameObject> weaponDirectory;


    void Start()
    {
        //DiseableAllWeapons();
        weaponList = new List<GameObject>();
        weaponQueue = new Queue();
      weaponStack = new Stack();
        weaponDirectory = new Dictionary<string, GameObject>();
    }

    void DiseableAllWeapons()
    {

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false); // 0 -> Weapon A  1->WB / 2 -> WC
        }
    }


    void EnableAllWeapon()
    {
        foreach (GameObject weapon in weaponList)
        {
            weapon.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q)) EnableAllWeapon();
        //INPUT QUEQ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (IsQueueEmpty())
            {
                GameObject weapon = weaponQueue.Dequeue() as GameObject;
                EquipWeapon(weapon);
            }
        }

        //INPUT STACK
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (IsStackEmpty())
            {
                GameObject weapon = weaponStack.Pop() as GameObject;
                EquipWeapon(weapon);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipWeapon(weaponDirectory["Bow1"]); else { Debug.Log("EL arma aún no ha sido obtenida");}
        if (Input.GetKeyDown(KeyCode.Alpha4)) EquipWeapon(weaponDirectory["Bow2"]); else { Debug.Log("EL arma aún no ha sido obtenida"); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) EquipWeapon(weaponDirectory["Bow3"]); else { Debug.Log("EL arma aún no ha sido obtenida"); }
        //if (Input.GetKeyDown(KeyCode.Alpha6)) EquipWeapon(weaponDirectory["WeaponC"]);
    }

    //Método para verificar si la cola está vacía.
    private bool IsQueueEmpty()
    {
        return weaponQueue.Count > 0;
    }

    //Método para verificar si la pila está vacía.
    private bool IsStackEmpty()
    {
        return weaponStack.Count > 0;
    }

    //Método que permite equipar el arma al Player
    private void EquipWeapon(GameObject weapon)
    {
        DetachWeapons();
        weapon.SetActive(true);
        weapon.transform.parent = playerHand;
        weapon.transform.localPosition = Vector3.zero;
    }

    //Método para remparentar todos los hijos.
    private void DetachWeapons()
    {
        //foreach para recorrer todos los hijos.
        foreach (Transform child in playerHand)
        {
            child.parent = null;
            child.transform.position = new Vector3(Random.Range(0f,3f), 1f,Random.Range(0f,3f));
            child.gameObject.SetActive(true);
        }
    }

}
