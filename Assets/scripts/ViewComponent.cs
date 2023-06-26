using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewComponent : MonoBehaviour
{
    public SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void SetSprite(Sprite sprite){
        this.sprite.sprite = sprite;
    }

    public void Animate(){
        GetComponent<ZoomAnimation>().BeginZoom();
    }
}
