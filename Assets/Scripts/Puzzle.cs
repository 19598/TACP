using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    public int[] operations = {0, 0};
    public int[] numbers = {0, 0, 0, 0};
    public GameObject myText;
    public GameObject drop1;
    public GameObject drop2;
    public Canvas myCanvas;
    public Text puzzle;
    public Dropdown op1;
    public Dropdown op2;
    public GameObject collectedItem;
    public PlayerController player;
    private bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < operations.Length; i++)
        {
            operations[i] = (int)Random.Range(1, 4);
        }
        for (int i = 0; i < numbers.Length - 1; i++)
        {
            numbers[i] = (int)Random.Range(10, 100);
        }
        numbers[numbers.Length - 1] = DoOperation(operations[0], numbers[0], numbers[1]);
        numbers[numbers.Length - 1] = DoOperation(operations[1], numbers[numbers.Length - 1], numbers[2]);
    }

    private int DoOperation(int operation, int num1, int num2)
    {
        int result = 0;
        switch (operation)
        {
            case 1:
                result = num1 + num2;
                break;
            case 2:
                result = num1 - num2;
                break;
            case 3:
                result = num1 * num2;
                break;
        }
        return result;
    }

    public void StartPuzzle()
    {
        if (active)
        {
            player.setActive(false);
            myText = Instantiate(myText);
            drop1 = Instantiate(drop1);
            drop1.transform.SetParent(myCanvas.transform, false);
            drop2 = Instantiate(drop2);
            drop2.transform.SetParent(myCanvas.transform, false);
            puzzle = myText.GetComponent<Text>();
            puzzle.transform.SetParent(myCanvas.transform, false);
            puzzle.text = "(" + numbers[0] + "     " + numbers[1] + ")     " + numbers[2] + " = " + numbers[3];
            puzzle.raycastTarget = false;
            drop1.GetComponent<TMPro.TMP_Dropdown>().onValueChanged.AddListener(delegate { CheckPuzzle(); });
            drop2.GetComponent<TMPro.TMP_Dropdown>().onValueChanged.AddListener(delegate { CheckPuzzle(); });
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void CheckPuzzle()
    {
        int val1 = drop1.GetComponent<TMPro.TMP_Dropdown>().value;
        int val2 = drop2.GetComponent<TMPro.TMP_Dropdown>().value;
        if (val1 == operations[0] && val2 == operations[1])
        {
            myText.SetActive(false);
            drop1.SetActive(false);
            drop2.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            collectedItem.SetActive(true);
            active = false;
            player.setActive(true);
        }
        else if ((operations[0] == 1 || operations [0] == 2) && (operations[1] == 1 || operations[1] == 2))
        {
            if ((val1 == 1 && val2 == 2) || (val1 == 2 && val2 == 1))
            {
                myText.SetActive(false);
                drop1.SetActive(false);
                drop2.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                collectedItem.SetActive(true);
                active = false;
                player.setActive(true);
            }
        }
        else
        {
            Debug.Log(drop1.GetComponent<TMPro.TMP_Dropdown>().value + " " + operations[0]);
            Debug.Log(drop2.GetComponent<TMPro.TMP_Dropdown>().value + " " + operations[1]);
        }
    }

    public bool isActive()
    {
        return active;
    }
}
