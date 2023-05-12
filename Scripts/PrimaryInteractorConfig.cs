using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryInteractorConfig : MonoBehaviour
{
    [SerializeField] GameObject leftInteractor;
    [SerializeField] GameObject rightInteractor;

    [SerializeField] bool rightHanded = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetInteractor() {
        if(rightHanded) return rightInteractor;
        return leftInteractor;
    }
}
