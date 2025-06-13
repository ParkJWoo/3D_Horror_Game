using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] MapSO[] MapDatas = new MapSO[5];
    MapSO CurrentMapData;
    [SerializeField] private GameObject MapLightContianor;
    List<Light> Lights = new List<Light>();
    List<float> Targetintenses = new List<float>();
    float LerpSpeed = 5f;
    void Start()
    {
        Init();
        GetAllLights();
        StartCoroutine(Lightcorutine());
    }

    private void Update()
    {
        for (int i = 0; i < Lights.Count; i++)
        {
            Lights[i].intensity = Mathf.Lerp(Lights[i].intensity, Targetintenses[i], Time.deltaTime * LerpSpeed);
        }
    }

    private void Init() // 요기 이제 저장된 맵정보를 불러오거나 없으면 처음맵정보 적용
    {
        if(true) // 만약 저장된 정보가 없다면... 수정해야함 나중에
        {
            CurrentMapData = MapDatas[0];
        }
    }

    private void GetAllLights() // 모든 광원 정보를 리스트에 저장
    {
        MapLightContianor.GetComponentsInChildren(Lights);
        foreach(Light light in Lights)
        {
            Targetintenses.Add(light.intensity);
        }
    }

    IEnumerator Lightcorutine()
    {
        while(true)
        {
            for (int i = 0; i < Targetintenses.Count; i++)
            {
                Targetintenses[i] = Random.Range(CurrentMapData.Lightinfo.MinIntense, CurrentMapData.Lightinfo.MaxIntense);
            }
            float waittime = Random.Range(CurrentMapData.Lightinfo.MinInterval, CurrentMapData.Lightinfo.MaxInterval);
            yield return new WaitForSeconds(waittime);
        }
    }


}
