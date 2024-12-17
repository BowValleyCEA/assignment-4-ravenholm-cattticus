using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dying : MonoBehaviour
{
    [SerializeField] GameObject player;
    private float dead;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(player);
    }
}
