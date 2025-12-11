using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private GameObject[] AttackSequence;
    [SerializeField] private float Range;

    private int AttackOrder = 0;
    private float ResetTime = 0.75f;

    public float Direction;


    private bool ResetIsRunning = false;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.V].wasPressedThisFrame)
        {

            Instantiate(AttackSequence[AttackOrder], new Vector2(transform.position.x + Range * Direction, transform.position.y), Quaternion.identity);
            AttackOrder++;

            if (ResetIsRunning)
            {
                StopCoroutine(AttackOrderReset());
                ResetIsRunning = false;
            }
            
            StartCoroutine(AttackOrderReset());

            if(AttackOrder >= AttackSequence.Length)
            {
                AttackOrder = 0;
            }

        }
    }

    private IEnumerator AttackOrderReset()
    {
        ResetIsRunning = true;
        yield return new WaitForSeconds(ResetTime);
        AttackOrder = 0;
        ResetIsRunning = false;
    }

}
