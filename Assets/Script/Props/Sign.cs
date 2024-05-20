using UnityEngine;
using TMPro;

public class Sign : MonoBehaviour
{
    [Header("Instruction Text")]
    [SerializeField] TextMeshProUGUI instructionText;
    [SerializeField] string instruction;
    // Start is called before the first frame update
    void Start()
    {
        instructionText.text = "!"; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            instructionText.text = instruction;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            instructionText.text = "!";
        }
    }
}
