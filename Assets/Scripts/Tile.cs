using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor, _blockColor;
    [SerializeField] private SpriteRenderer _renderer;

    public void InitColor(int isOffset)
    {
        if(isOffset == 2)
        {
           _renderer.color = _blockColor; 
           return;
        }
        _renderer.color = isOffset == 0 ? _offsetColor : _baseColor;
    }

    /*void ManageBorders()
    {
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            Vector3 tileScale = transform.localScale;
            Vector3 tilePosition = transform.position;
            Quaternion tileRotation = transform.rotation;
            Transform curBorder = gameObject.transform.GetChild(i);
            curBorder.localScale = new Vector3(tileScale.x / 20
                , tileScale.y + tileScale.y / 20, tileScale.z);
            if(i == 0 || i == 1)
            {
                curBorder.rotation = new Quaternion(tileRotation.x, tileRotation.y, tileRotation.z + 90);
            }
            curBorder.position = new Vector3(tilePosition.x, tilePosition.y, tilePosition.z - 1);
            i % 2
        }
        TopBorder();
        BottomBorder();
        RightBorder();
        LeftBorder();
    }*/
}
