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

            Debug.Log("Данные найдены по пути: " + _saveFilePath);
            Debug.Log($"Содержимое:\n" + json);

            LoadCapsuls(capsulesData);
        }
        else
        {
            Debug.LogWarning("Файл сохранения не найден");
            ResetCapsuls();
        }
    }

    public void SaveCapsuls()
    {
        try
        {
            Debug.Log($"Сохранение...");

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
            Debug.Log("Файл успешно сохранён по пути: " + _saveFilePath);
            Debug.Log($"Содержимое:\n" + json);
        }
        catch (IOException e)
        {
            Debug.LogError("Ошибка при сохранении файла: " + e.Message);
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

        Debug.Log("Данные загружены из файла: " + _saveFilePath);
    }

    public void ResetCapsuls()
    {
        Debug.Log("Загружаем стандартный файл сохранения...");

        LoadCapsuls(_CapsulesDefault);

        Debug.Log("Был загружен стандартный файл сохранения");
    }
}
