using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LiveMapPlacer : EditorWindow
{
    private string prefabFolder = "Prefabs/";
    private GameObject[] prefabList;
    private string[] prefabNames;
    private int selectedPrefabIndex = 0;
    private float gridSnap = 0.5f;
    private bool placingMode = false;

    private float yOffset = 0f;
    private float yMoveStep = 1f;
    private float rotationY = 0f;
    private float rotationStep = 90f;

    private GameObject previewInstance;

    [MenuItem("Tools/Live Map Placer")]
    public static void ShowWindow()
    {
        GetWindow<LiveMapPlacer>("Live Map Placer");
    }

    private void OnEnable()
    {
        LoadPrefabs();
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        DestroyPreview();
    }

    private void LoadPrefabs()
    {
        prefabList = Resources.LoadAll<GameObject>(prefabFolder);
        prefabNames = prefabList.Select(p => p.name).ToArray();
    }

    private void OnGUI()
    {
        GUILayout.Label("실시간 맵 배치툴", EditorStyles.boldLabel);

        prefabFolder = EditorGUILayout.TextField("Prefab 폴더 경로", prefabFolder);

        if (GUILayout.Button("프리팹 다시 불러오기"))
            LoadPrefabs();

        if (prefabList.Length == 0)
        {
            GUILayout.Label("프리팹이 없습니다.");
            return;
        }

        selectedPrefabIndex = EditorGUILayout.Popup("Prefab 선택", selectedPrefabIndex, prefabNames);
        gridSnap = EditorGUILayout.FloatField("그리드 스냅", gridSnap);
        yMoveStep = EditorGUILayout.FloatField("Y 이동 단위", yMoveStep);
        rotationStep = EditorGUILayout.FloatField("회전 단위", rotationStep);

        bool newPlacingMode = GUILayout.Toggle(placingMode, "배치 모드");
        if (newPlacingMode != placingMode)
        {
            placingMode = newPlacingMode;
            yOffset = 0f;
            rotationY = 0f;
            UpdatePreview();
        }

        GUILayout.Label("PageUp : 위로 이동 | PageDown : 높이 이동 | R : 회전 | 좌클릭 배치");
        GUILayout.Label("바닥은 그리드스냅 0.5로하니깐 잘 맞음");
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!placingMode || previewInstance == null) return;

        Event e = Event.current;

        HandleKeyInput(e);
        UpdatePreviewTransform();

        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            PlacePrefab(previewInstance.transform.position, rotationY);
            e.Use();
        }
    }

    private void HandleKeyInput(Event e)
    {
        if (e.type != EventType.KeyDown) return;

        if (e.keyCode == KeyCode.PageUp)
        {
            yOffset += yMoveStep;
            e.Use();
        }
        else if (e.keyCode == KeyCode.PageDown)
        {
            yOffset -= yMoveStep;
            e.Use();
        }
        else if (e.keyCode == KeyCode.R)
        {
            rotationY += rotationStep;
            if (rotationY >= 360f) rotationY -= 360f;
            e.Use();
        }
    }

    private void UpdatePreview()
    {
        DestroyPreview();

        if (!placingMode) return;

        GameObject prefab = prefabList[selectedPrefabIndex];
        previewInstance = Instantiate(prefab);
        previewInstance.name = "PreviewInstance";

        ApplyTransparentMaterial(previewInstance);
        UpdatePreviewTransform();
    }

    private void UpdatePreviewTransform()
    {
        Vector3 snappedPosition = GetMouseSnappedPosition();
        snappedPosition.y += yOffset;

        previewInstance.transform.position = snappedPosition;
        previewInstance.transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }

    private Vector3 GetMouseSnappedPosition()
    {
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null || sceneView.camera == null)
            return Vector3.zero;

        Vector2 mousePos = Event.current != null ? Event.current.mousePosition : new Vector2(sceneView.position.width / 2, sceneView.position.height / 2);
        mousePos.y = sceneView.position.height - mousePos.y;

        Ray ray = sceneView.camera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            return Snap(hit.point);
        }
        else
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out float distance))
            {
                return Snap(ray.GetPoint(distance));
            }
        }
        return Vector3.zero;
    }

    private Vector3 Snap(Vector3 point)
    {
        return new Vector3(
            Mathf.Round(point.x / gridSnap) * gridSnap,
            Mathf.Round(point.y / gridSnap) * gridSnap,
            Mathf.Round(point.z / gridSnap) * gridSnap
        );
    }

    private void ApplyTransparentMaterial(GameObject instance)
    {
        foreach (var renderer in instance.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat != null)
                {
                    mat.SetFloat("_Mode", 2);
                    Color color = mat.color;
                    color.a = 0.3f;
                    mat.color = color;
                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.EnableKeyword("_ALPHABLEND_ON");
                    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = 3000;
                }
            }
        }
    }

    private void PlacePrefab(Vector3 position, float rotationY)
    {
        GameObject prefab = prefabList[selectedPrefabIndex];
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.transform.position = position;
        instance.transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        Undo.RegisterCreatedObjectUndo(instance, "Prefab Placed");
    }

    private void DestroyPreview()
    {
        if (previewInstance != null)
        {
            DestroyImmediate(previewInstance);
            previewInstance = null;
        }
    }
}