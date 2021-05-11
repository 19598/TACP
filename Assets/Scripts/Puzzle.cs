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
    void Start()
    {
        //creates the operations randomly
        for (int i = 0; i < operations.Length; i++)
        {
            operations[i] = (int)Random.Range(1, 4);
        }
        //creates the number to use to find the answer randomly
        for (int i = 0; i < numbers.Length - 1; i++)
        {
            numbers[i] = (int)Random.Range(10, 100);
        }

        //does math with the numbers and operations in order to find the answer, and sets it to the last number in the array
        numbers[numbers.Length - 1] = DoOperation(operations[0], numbers[0], numbers[1]);
        numbers[numbers.Length - 1] = DoOperation(operations[1], numbers[numbers.Length - 1], numbers[2]);
    }

    //does an operation with the provided numbers
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

            //creates the objects
            myText = Instantiate(myText);
            drop1 = Instantiate(drop1);
            drop2 = Instantiate(drop2);

            //puts them on the canvas
            drop1.transform.SetParent(myCanvas.transform, false);
            drop2.transform.SetParent(myCanvas.transform, false);
            puzzle = myText.GetComponent<Text>();
            puzzle.transform.SetParent(myCanvas.transform, false);

            //sets the text to show the numbers and leave space for the dropdowns
            puzzle.text = "(" + numbers[0] + "     " + numbers[1] + ")     " + numbers[2] + " = " + numbers[3];
            puzzle.raycastTarget = false;

            //makes the dropdowns call the CheckPuzzle method whenever they are changed to see if the puzzle is right
            drop1.GetComponent<TMPro.TMP_Dropdown>().onValueChanged.AddListener(delegate { CheckPuzzle(); });
            drop2.GetComponent<TMPro.TMP_Dropdown>().onValueChanged.AddListener(delegate { CheckPuzzle(); });

            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void CheckPuzzle()
    {
        //gets the values of the dropdowns
        int val1 = drop1.GetComponent<TMPro.TMP_Dropdown>().value;
        int val2 = drop2.GetComponent<TMPro.TMP_Dropdown>().value;

        //figures out what number comes out of what the user enters in the dropdown
        int computedValue = DoOperation(val1, numbers[0], numbers[1]);
        computedValue = DoOperation(val2, computedValue, numbers[2]);

        if (computedValue == numbers[numbers.Length -1])//checks to see if they compute to the same number, and if they do, puts the menu away and collects the item the chest holds
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

    public bool isActive()
    {
        return active;
    }
}
