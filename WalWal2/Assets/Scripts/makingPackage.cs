


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makingPackage : MonoBehaviour
{

    public GameObject PackagePrefab;
    public GameObject Dog;
    private int maxPackage = 20;
    private Vector3 initialPos;
    private int xdist=15;
    private int zdist=60;
    private int itemcode;

    // Start is called before the first frame update
    void Start()
    {
        initialPos= Dog.transform.position;
        for (int i=0; i<maxPackage; i++)
            SetLocation();
    }


    //선물을 설정할 적당한 location을 찾는 함수
    public Vector3 FindLocation(){
        float u=Random.Range(-1.0f,1.0f);
        float v=Random.Range(0.2f,1.0f);

        return new Vector3(initialPos.x+(u*xdist),initialPos.y -0.5f,initialPos.z+(v*zdist));

    }

    //찾은 location에 선물을 놓는 함수
    public void SetLocation() {

        var newPackage = GameObject.Instantiate(PackagePrefab);
        newPackage.transform.position= FindLocation();

        itemcode=Random.Range(1,11);

        switch(itemcode) {
            case int n when (n<=4): //폭탄
                newPackage.gameObject.tag="bomb";
                break;

            default:
                //폭탄x
                newPackage.gameObject.tag="package";
                break;

        }
    }
}
