using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneNameSetter : MonoBehaviour
{
    Canvas canvas;
    Text nametext;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        nametext = GetComponentInChildren<Text>();
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(10 * gameObject.transform.localScale.x, 10 * gameObject.transform.localScale.y);
        nametext.text = gameObject.name;
    }
}
