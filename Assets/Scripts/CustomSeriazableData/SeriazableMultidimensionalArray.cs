using Ink.Parsed;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SeriazableMultidimensionalArray: ISerializationCallbackReceiver
{
    public RoomData[,] elements = new RoomData[3, 5];

    private List<Package<RoomData>> list = new List<Package<RoomData>>();

    [System.Serializable]
    struct Package<T>
    {
        public int Index0;
        public int Index1;
        public T element;
        public Package(int index0, int index1, T element)
        {
            Index0 = index0;
            Index1 = index1;
            this.element = element;
        }

    }

    public void OnBeforeSerialize()
    {
        list = new List<Package<RoomData>>();
        for(int i = 0; i < elements.GetLength(0); i++)
        {
            for(int j = 0; j < elements.GetLength(1); j++)
            {
                list.Add(new Package<RoomData>(i,j, elements[i,j]));
            }
        }
    }

    public void OnAfterDeserialize()
    {
        elements = new RoomData[3, 5];
        foreach(var package in list)
        {
            elements[package.Index0, package.Index1] = package.element;
        }
    }

}
