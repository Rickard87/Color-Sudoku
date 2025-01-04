using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public enum Difficulties
    {
        DEBUG,
        EASY,
        MEDIUM,
        HARD,
        NIGHTMARE
    }

    public static Difficulties difficulty;
}
