using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageSystem : MonoBehaviour
{
    public Text tf;
    public Image img;
    public static Text textField;
    public static Image image;
    // Start is called before the first frame update

    private void Start()
    {
        textField = tf;
        image = img;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            textField.text = "";
            ScenePersistantData.paused = false;
            image.enabled = false;
        }
    }

    public static void message(string message)
    {
        image.enabled = true;
        textField.text = message;
        ScenePersistantData.paused = true;
    }
}