using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MessageSystem : MonoBehaviour
{
    public Text tf;
    public static Text textField;
    // Start is called before the first frame update

    private void Start()
    {
        textField = tf;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            textField.text = "";
            ScenePersistantData.paused = false;
        }
    }

    public static void message(string message)
    {
        textField.text = message;
        ScenePersistantData.paused = true;
    }
}