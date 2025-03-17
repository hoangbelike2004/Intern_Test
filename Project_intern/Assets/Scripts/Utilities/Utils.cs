using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

public class Utils
{
    public static int[] followType = new int[7];
    public static List<int> saveValuesRamdom = new List<int>();
    public static int[] valuesRamdom = new int[3] { 3, 6, 9 };
    public static void RamdomFlowXboardAndYboard(int amout)
    {
        int i = 0;
        while (amout > 0)
        {

            if (i > followType.Length - 1)
            {
                i = 0;
            }
            int valueRD = URandom.Range(0, valuesRamdom.Length);
            if (amout < valuesRamdom[valueRD])
            {
                followType[i] += amout;
                amout = 0;
            }
            amout -= valuesRamdom[valueRD];
            followType[i] += valuesRamdom[valueRD];
            i++;
        }
        for (int j = 0; j < followType.Length; j++)
        {
            if (followType[j] > 0)
            {
                saveValuesRamdom.Add(followType[j]);
                followType[j] = 0;
            }
        }
    }
    public static NormalItem.eNormalType GetRandomNormalType()
    {
        Array values = Enum.GetValues(typeof(NormalItem.eNormalType));
        NormalItem.eNormalType result = (NormalItem.eNormalType)values.GetValue(URandom.Range(0, values.Length));

        return result;
    }

    public static NormalItem.eNormalType GetRandomNormalTypeExcept(NormalItem.eNormalType[] types)//ramdom
    {
        List<NormalItem.eNormalType> list = Enum.GetValues(typeof(NormalItem.eNormalType)).Cast<NormalItem.eNormalType>().Except(types).ToList();


        int rnd = URandom.Range(0, saveValuesRamdom.Count);
        saveValuesRamdom[rnd] -= 1;
        while (saveValuesRamdom[rnd] <= 0)
        {
            rnd = URandom.Range(0, saveValuesRamdom.Count);
        }

            NormalItem.eNormalType result = list[rnd];

        return result;
    }
}
