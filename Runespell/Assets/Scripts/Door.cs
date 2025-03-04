using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private bool open;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        animator.SetBool("OpenDoor", true);
        open = true;
    }

    public bool Open { get { return open; } set {  open = value; } }
}
