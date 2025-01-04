using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberField : MonoBehaviour
{
    Board board;
    //coords
    int x1, y1;
    int value;

    string identifier;

    public Text number;

    public void SetValues(int _x1, int _y1, int _value, string _identfier, Board _board)
    {
        x1 = _x1;
        y1 = _y1;
        value = _value;
        identifier = _identfier;
        board = _board;

        number.text = (value != 0) ? value.ToString() : ""; //if value is not 0 fill text value with "" (nothing)

        if (value != 0)
        {
            GetComponentInParent<Button>().transition = Selectable.Transition.None;
            GetComponentInParent<Button>().interactable = false;
        }
        else
        {
            number.color = Color.clear; //number.color = Color.blue;
        }
    }

    public void ButtonClick()
    {
        InputField.instance.ActivateInputField(this);
    }

    public void ReceiveInput(int newValue)
    {
        value = newValue;
        number.text = (value != 0) ? value.ToString() : "";
        //update riddle
        board.SetInputInRiddleGrid(x1, y1, value);
    }

    public int GetX()
    {
        return x1;
    }

    public int GetY()
    {
        return y1;
    }

    public void SetHint(int _value)
    {
        value = _value;
        number.text = value.ToString();
        number.color = Color.clear; //number.color = Color.green;
        GetComponentInParent<Button>().transition = Selectable.Transition.None;
        GetComponentInParent<Button>().interactable = false;
    }
}
