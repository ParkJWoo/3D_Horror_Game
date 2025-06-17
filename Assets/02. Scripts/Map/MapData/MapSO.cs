using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LightInfo
{
    [field: SerializeField] public float MinInterval { get; private set; }
    [field: SerializeField] public float MaxInterval { get; private set;}
    [field: SerializeField] public float MinIntense { get; private set; }
    [field: SerializeField] public float MaxIntense { get; private set; }
    [field: SerializeField] public Color Lightcolor { get; private set; }
}




[CreateAssetMenu(fileName = "MapData", menuName = "Map/MapData")]
public class MapSO : ScriptableObject
{
    [field :Header("맵 전등 설정")]
    [field :SerializeField] public LightInfo Lightinfo { get; private set; }

}
