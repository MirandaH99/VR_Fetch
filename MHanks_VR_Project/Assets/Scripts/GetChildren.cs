using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;



public class GetChildren : MonoBehaviour
{
    public TMP_Text myText;
    //GameObject hand;
    LeftControl leftHand;
    StringBuilder info = new StringBuilder();
    GameObject myObj;
        
    // Start is called before the first frame update
    void Start()
    {
        myObj = GameObject.Find("Floor");
        
        foreach (Transform g in myObj.transform.GetComponentsInChildren<Transform>())
        {
            if (g.name == "LeftText")
            {
                myText = g.GetComponent<TMP_Text>();
                print("myText found");
            }
        }

        //leftHand = hand.GetComponent<LeftControl>();

        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        {
            if (g.name == "LeftControls")
            {
                leftHand = g.GetComponent<LeftControl>();
            }
            //if (g.name == "LeftText")
            //{
            //    myText = g.GetComponent<Text>();
            //}
        }
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }
    }
        
    // Update is called once per frame
    void Update()
    {
        info.Clear();
        info.AppendLine("LeftController\n\n");

        info.AppendLine(string.Format("TriggerState: {0}", leftHand.triggerButton));
        info.AppendLine(string.Format("TriggerValue: {0:F3}", leftHand.triggerValue));

        info.AppendLine(string.Format("gripState: {0}", leftHand.gripButton));
        info.AppendLine(string.Format("gripValue: {0:F3}", leftHand.gripValue));

        info.AppendLine(string.Format("Joystick X: {0:F3},  Y: {1:F3}", leftHand.joyStickRaw.x, leftHand.joyStickRaw.y));
        info.AppendLine(string.Format("JoyStickClick: {0}", leftHand.joyStickClick));

        info.AppendLine(string.Format("TouchClicked: {0}", leftHand.touchButton));
        info.AppendLine(string.Format("TouchSensed: {0}", leftHand.touched));
        info.AppendLine(string.Format("TouchPad X: {0:F3},  Y: {1:F3}", leftHand.touchRaw.x, leftHand.touchRaw.y));

        myText.text = info.ToString();
    }
}
