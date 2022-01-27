/*
 * Copyright 2021 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Used in <see cref="CarBehaviour"/> to determine a collision with a package. 
 */
public class PackageBehaviour : MonoBehaviour
{   

    private int itemcode;
    public GameObject explosion;
    public GameObject nexplosion;

    void OnTriggerEnter(Collider cogi) {

        if(cogi.gameObject.tag == "agent") {
            //아이템 별 코드를 만들어놓고 비교하면 좋을듯
            addItem();
            Destroy(gameObject);
        }

    }

    void OnTriggerExit(Collider cogi) {

    }

    private void addItem() {
        //아이템 코드에 따라서 아이템을 인벤토리에 추가한다.
        //itemcode=Random.Range(1,11);

        if(gameObject.tag == "bomb") { //폭탄
            var nexplo= GameObject.Instantiate(nexplosion);
            nexplo.transform.position= gameObject.transform.position;

        }

        else if(gameObject.tag=="package") {
                //폭탄x
                var explo= GameObject.Instantiate(explosion);
                explo.transform.position= gameObject.transform.position;
                //explo.gameObject.tag="package";
        }
    }
    
}
