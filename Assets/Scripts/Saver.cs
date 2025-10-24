using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Saver : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject _CapsulesPrefab;
    public CapsuleData _CapsulesDefault;
    private static string _saveFilePath;
    List<GameObject> _activeCapsules = new();


    void Awake()
    {
        _saveFilePath = Path.Combine(Application.dataPath, "Saves", "savefile.json");
        SaveChecker();
    }

    public void SaveChecker()
    {
        if (File.Exists(_saveFilePath))
        {
            string json = File.ReadAllText(_saveFilePath);

            CapsuleData capsulesData = new();
            JsonUtility.FromJsonOverwrite(json, capsulesData);

            Debug.Log("������ ������� �� ����: " + _saveFilePath);
            Debug.Log($"����������:\n" + json);

            LoadCapsuls(capsulesData);
        }
        else
        {
            Debug.LogWarning("���� ���������� �� ������");
            ResetCapsuls();
        }
    }

    public void SaveCapsuls()
    {
        try
        {
            Debug.Log($"����������...");

            CapsuleData capsulesData = new();
            foreach (var obj in _activeCapsules)
            {
                Capsule capsule = new();
                capsule.x = obj.transform.position.x;
                capsule.y = obj.transform.position.y;
                capsule.z = obj.transform.position.z;

                capsulesData._capsules.Add(capsule);
            }

            string json = JsonUtility.ToJson(capsulesData);
            File.WriteAllText(_saveFilePath, json);
            Debug.Log("���� ������� ������� �� ����: " + _saveFilePath);
            Debug.Log($"����������:\n" + json);
        }
        catch (IOException e)
        {
            Debug.LogError("������ ��� ���������� �����: " + e.Message);
        }
    }

    void LoadCapsuls(CapsuleData capsuleData)
    {
        foreach (var obj in _activeCapsules)
        {
            Destroy(obj);
        }
        _activeCapsules.Clear();
        foreach (var obj in capsuleData._capsules)
        {
            var newObject = Instantiate(_CapsulesPrefab, new Vector3(obj.x, obj.y, obj.z), Quaternion.identity);
            //newObject.transform.SetParent(parentObject.transform);
            _activeCapsules.Add(newObject);
        }

        Debug.Log("������ ��������� �� �����: " + _saveFilePath);
    }

    public void ResetCapsuls()
    {
        Debug.Log("��������� ����������� ���� ����������...");

        LoadCapsuls(_CapsulesDefault);

        Debug.Log("��� �������� ����������� ���� ����������");
    }
}
