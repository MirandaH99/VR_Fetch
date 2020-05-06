using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public float score;
    public Vector3 startPosition;

    public Image LGrabImage;
    public Image RGrabImage;
    public Image LTeleportImage;
    public Image RTeleportImage;
    public Text scoreTxt;

    public bool UIisEnabled;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;

        score = 0;
        startPosition = gameObject.transform.position;

        if(UIisEnabled)
        {
            LGrabImage.enabled = false;
            RGrabImage.enabled = false;
            LTeleportImage.enabled = false;
            RTeleportImage.enabled = false;
        }
    }

    public void setImageToFalse(Image image)
    {
        image.enabled = false;
    }

    public void AddScore()
    {
        score++;
        scoreTxt.text = "Score: " + score;
    }

    public void MinusScore()
    {
        score--;
        scoreTxt.text = "Score: " + score;
    }
}
