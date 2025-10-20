using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class interactive_alarm : MonoBehaviour
{
    int mode = 0;
    // Start is called before the first frame update
    void Start()
    {
        mode = 0;
    }

    private void FixedUpdate()
    {
        if (mode == 0)
        {
            transform.localScale = new Vector3(this.transform.localScale.x + 0.01f,  this.transform.localScale.y + 0.01f,this.transform.localScale.z);
            if (transform.localScale.x >=0.5f)
            {
                mode = 1;
            }
        }else if (mode == 1)
        {
            transform.localScale = new Vector3(this.transform.localScale.x - 0.01f, this.transform.localScale.y - 0.01f, this.transform.localScale.z);
            if (transform.localScale.x <=0.4f)
            {
                mode=0;
            }
        }
    }
}
