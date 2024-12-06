using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;
using YG;
using System.Collections;
using System.Collections.Generic;

public class Saver : MonoBehaviour
{
    [SerializeField] Main _main;
    public SavesYG savesData= new();
    [SerializeField] string _playerName;
    [SerializeField] Market _market;

    [ContextMenu("Start")]
    void Start()
    {
        StartCoroutine(StartDo());
    }

    IEnumerator StartDo()
    {
        string _playerNamePath = Application.persistentDataPath + "/PlayerName.txt";

        if(PlayerPrefs.HasKey("name"))
        {
            _playerName = PlayerPrefs.GetString("name");
            File.WriteAllText(_playerNamePath, _playerName);
            Load();
        }
        else
        {
            MarketData _data = new();
            for(int i=0;i<_market._units.Length;i++)
            {
                _data.units.Add(_market._units[i]._data);
            }
            File.WriteAllText(Application.persistentDataPath+"/input.txt", JsonUtility.ToJson(_data));
            StartProcess("C:/Users/Misha/AppData/LocalLow/DefaultCompany/Strategy/CreatePlayer.py", "");

            while(File.ReadAllText(_playerNamePath) == "")
                yield return new WaitForSeconds(0.2f);
            
            _playerName = File.ReadAllText(_playerNamePath);
            PlayerPrefs.SetString("name", _playerName);
            PlayerPrefs.Save();
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        string arguments = JsonUtility.ToJson(savesData);

        File.WriteAllText(Application.persistentDataPath + "/input.txt", arguments);

        StartProcess("C:/Users/Misha/AppData/LocalLow/DefaultCompany/Strategy/PutData.py", arguments);
        UnityEngine.Debug.Log("Save");
    }

    [ContextMenu("Load")]
    public void Load()
    {
        StartCoroutine(LoadDo());
    }

    IEnumerator LoadDo()
    {
        string arguments = "";
        string dataPath = Application.persistentDataPath + "/Out.txt";

        File.WriteAllText(dataPath, "");
        StartProcess("C:/Users/Misha/AppData/LocalLow/DefaultCompany/Strategy/GetData.py", arguments);
        while(File.ReadAllText(dataPath) == "")
            yield return new WaitForSeconds(0.3f);
        
        savesData = JsonUtility.FromJson<SavesYG>(File.ReadAllText(dataPath));
        _main.Load();
        UnityEngine.Debug.Log("Load");
    }

    public void SendLog(string message, string resource)
    {
        UnityEngine.Debug.Log("Log: "+message);
        File.WriteAllText(Application.persistentDataPath + "/LogInp", message + " : " + resource);
        StartProcess("C:/Users/Misha/AppData/LocalLow/DefaultCompany/Strategy/Log.py", "");
    }

    void StartProcess(string pythonScriptPath, string arguments)
    {
        ProcessStartInfo start = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = $"\"{pythonScriptPath}\" {arguments}",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(start))
        {
            using (System.IO.StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd(); // Читаем результат выполнения скрипта
                Console.WriteLine(result); // Выводим результат в консоль
            }
        }
    }
}

public class MarketData: MonoBehaviour
{
    public List<Market.UnitD> units=new List<Market.UnitD>();
}