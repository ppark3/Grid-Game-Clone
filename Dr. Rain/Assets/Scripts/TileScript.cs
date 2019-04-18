using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public int x;
    public int y;

    public int type;
    public Color[] colors;
    public Color[] selectedColors;
    public Color[] stoneColors;
    private bool inSlide = false;

    public bool selected;
    public bool canBeSelected;
    public bool isStone;

    public GameObject top;
    public GameObject bottom;
    public GameObject right;
    public GameObject left;
     
    public Vector3 startPosition;
    public Vector3 destPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (selected && !isStone)
        {
            GetComponent<SpriteRenderer>().color = selectedColors[type];
            if (top != null && top.GetComponent<TileScript>().type == type)
            {
                top.GetComponent<TileScript>().selected = true;
            }
            if (bottom != null && bottom.GetComponent<TileScript>().type == type)
            {
                bottom.GetComponent<TileScript>().selected = true;
            }
            if (right != null && right.GetComponent<TileScript>().type == type)
            {
                right.GetComponent<TileScript>().selected = true;
            }
            if (left != null && left.GetComponent<TileScript>().type == type)
            {
                left.GetComponent<TileScript>().selected = true;
            }
        }
        else if (!selected && !isStone)
        {
            GetComponent<SpriteRenderer>().color = colors[type];
        }
        else
        {
            GetComponent<SpriteRenderer>().color = stoneColors[type];
        }

        if (inSlide)
        {
            if (GridMaker.slideLerp < 0)
            {
                transform.localPosition = destPosition;
                inSlide = false;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(startPosition, destPosition, GridMaker.slideLerp);
            }
        }

    }

    public void SetSprite(int rand)
    {
        type = rand;
        GetComponent<SpriteRenderer>().color = colors[type];
    }

    public void SetupSlide(Vector2 newDestPost)
    {
        inSlide = true;
        startPosition = transform.localPosition;
        destPosition = newDestPost;
    }

    public bool TouchingSelected()
    {
        int count = 0;
        if (top != null)
        {
            if (top.GetComponent<TileScript>().selected)
            {
                count++;
            }
        }
        if (bottom != null)
        {
            if (bottom.GetComponent<TileScript>().selected)
            {
                count++;
            }
        }
        if (right != null)
        {
            if (right.GetComponent<TileScript>().selected)
            {
                count++;
            }
        }
        if (left != null)
        {
            if (left.GetComponent<TileScript>().selected)
            {
                count++;
            }
        }
        return count >= 1;
    }
}
