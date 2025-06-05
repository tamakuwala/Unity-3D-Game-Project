using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphMovement : MonoBehaviour
{
    public Vector3[] positions;
    private float movementSpeed = 15.0f;

    private int index = 0;
    private Vector3 scaleChange;
   

void Awake(){
    transform.localScale = new Vector3(3f, 0.1f, 3f);
    scaleChange = new Vector3(-0.1f, 0, -0.1f);
    
    positions = this.transform.parent.parent.GetComponent<MeteorTelegraph>().GetPositions();
    

}
    void Start(){
        
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.y > 20f) {
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        } else if ( this.gameObject.transform.position.y < 0){
            Destroy(this.gameObject);
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, positions[index], movementSpeed * Time.deltaTime);
        if (this.transform.position == positions[index]){
            index++;
        }
        if (index == positions.Length) index = 0;
        

    }
    private void FixedUpdate() {
        
        // Shrink
        float localScaleX = transform.localScale.x;
        float localScaleY = transform.localScale.y;
        float localScaleZ = transform.localScale.z;
        float positionY = transform.position.y;
        float aproximateMaxY = 20f;
        float ratio = 1 - positionY/aproximateMaxY;

        if (ratio > 0.1 ) transform.localScale = new Vector3(ratio * 3f, 0 , ratio * 3f );
    }
}
