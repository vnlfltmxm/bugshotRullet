using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Singleton<T> : NetworkBehaviour where T :NetworkBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            //�ν��Ͻ��� null�̸�
            if (instance == null)
            {
                //���� ���� �̹� �ش�Ÿ���� �ν��Ͻ��� ������
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<T>();

                    DontDestroyOnLoad(singletonObject);//���� ����Ǵ��� �ش������Ʈ�� ����������
                }
            }

            return instance;
        }
    }
}
