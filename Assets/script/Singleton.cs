using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;

                if (_instance == null)
                {
                    Debug.LogError("���� ������ " + typeof(T) + "�� Ȱ��ȭ �� �� �����ϴ�.");
                }


            }
            return _instance;
        }
    }

    // �ı����� �ʴ� ������Ʈ�� ������� �ּ��� ǰ
    public void Awake()
    {
        T[] obj = FindObjectsOfType(typeof(T)) as T[];
        if (obj.Length > 1)
        {
            Destroy(obj[0].gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}