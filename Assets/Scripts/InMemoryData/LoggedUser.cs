using UnityEngine;

public class LoggedUser : MonoBehaviour
{
    private static LoggedUser _instance;
    public static LoggedUser Instance { get { return _instance; } }

    User _user = null;
    public User User
    {
        get => _user;
        set => _user = value;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void Logout()
    {
        User = null;
    }
}
