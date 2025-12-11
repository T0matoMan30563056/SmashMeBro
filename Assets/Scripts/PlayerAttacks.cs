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
    [SerializeField] private float ResetTime;
    private float ResetTimeRemaining;

    public float Direction;
    public float VerticalDirection;


    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
 

    }

    // Update is called once per frame
    void Update()
    {

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log(VerticalDirection);
            if (Mathf.Abs(VerticalDirection) == 1)
            {
                Instantiate(AttackSequence[AttackOrder], new Vector2(transform.position.x, transform.position.y + Range * VerticalDirection), Quaternion.identity);
            }
            else
            {

                Instantiate(AttackSequence[AttackOrder], new Vector2(transform.position.x + Range * Direction, transform.position.y), Quaternion.identity);
            }
            AttackOrder++;

            ResetTimeRemaining = ResetTime;
            

            if(AttackOrder >= AttackSequence.Length)
            {
                AttackOrder = 0;
            }

        }
        ResetTimeRemaining = Mathf.Max(ResetTimeRemaining - Time.deltaTime, 0);
        if (ResetTimeRemaining == 0)
        {
            AttackOrder = 0;
        }

    }



}
