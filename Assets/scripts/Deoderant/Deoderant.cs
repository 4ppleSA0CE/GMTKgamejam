using UnityEngine;

public class Deoderant : MonoBehaviour
{
    public GameObject TextMessage_key;

    public float time_of_message = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnInteract()
    {
        StartCoroutine(ShowKeyMessage());
    }
    private System.Collections.IEnumerator ShowKeyMessage()
    {
        if (TextMessage_key != null) // Use comparison, not assignment
        {
            TextMessage_key.SetActive(true);
            yield return new WaitForSeconds(time_of_message);
            TextMessage_key.SetActive(false);
        }
    }
}
