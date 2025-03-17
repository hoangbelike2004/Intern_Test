using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;
using System.Reflection;

public class Utils
{
    public static List<int> nomaltypes = new List<int>();
    public static int index = 0;
    public static void RamdomFlowXboardAndYboard(int amout)
    {
        if(amout % 7 == 0)//lay du loai ca khi thoa man dk amout chia het cho 7 va so luong cac loai ca trung chia het cho 3
        {
            int tmp = amout / 7;
            if(tmp % 3 == 0)
            {
                for (int j = 0; j < 7; j++)
                {

                    for (int k = 0; k < tmp; k++)
                    {
                        nomaltypes.Add(j);
                    }
                }
                index = nomaltypes.Count - 1;
                MixType(nomaltypes);
                return;
            }
        }
        for (int i = 7; i > 0; i--)
        {
            if (amout % i == 0)
            {
                int tmp = amout / i;
                if (tmp % 3 == 0)
                {
                    int rnd = URandom.Range(i, 8);
                    for (int j = rnd - i; j < rnd; j++)
                    {

                        for (int k = 0; k < tmp; k++)
                        {
                            nomaltypes.Add(j);
                        }
                    }
                    index = nomaltypes.Count - 1;
                    MixType(nomaltypes);
                    break;
                }
            }
        }
    }
    public static void MixType(List<int> normaltypes)
    {
        for (int i = 0; i < normaltypes.Count - 1; i++)
        {
            int rnd = URandom.Range(i, normaltypes.Count);
            int tmp = nomaltypes[i];
            nomaltypes[i] = nomaltypes[rnd];
            nomaltypes[rnd] = tmp;
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
        List<NormalItem.eNormalType> list = Enum.GetValues(typeof(NormalItem.eNormalType)).Cast<NormalItem.eNormalType>().ToList();
        NormalItem.eNormalType result = list[nomaltypes[index]];
        nomaltypes.RemoveAt(index);
        index--;
        return result;
    }
}
