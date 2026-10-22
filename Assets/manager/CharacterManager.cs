using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


public class CharacterManager : MonoBehaviour
{
    public List<CharacterInfo> characters = new List<CharacterInfo>();
    private HashSet<string> loadedFolderPath = new HashSet<string> ();

    void Start()
    {
        LoadCharacters();
    }
    void LoadCharacters()
    {
        string CharacterFolderPath = Application.streamingAssetsPath + "/Ch";
        if (Directory.Exists(CharacterFolderPath))
        {
            string[] jsonFiles = Directory.GetFiles(CharacterFolderPath, "*.json", SearchOption.AllDirectories);
            foreach (string jsonFilePath in jsonFiles) 
            {
                string content = File.ReadAllText(jsonFilePath);
                CharacterInfo Config = JsonUtility.FromJson<CharacterInfo>(content);
                if (Validate(Config, jsonFilePath))
                {
                    characters.Add(Config);
                }
            }
        }
        else
        {
            Debug.LogError("Could not find folder 'Ch' at: " + Application.streamingAssetsPath);
        }

    }   
    
    private bool Validate(CharacterInfo config, string filePath)
    {
        //character setting validation
        bool isValid = true;
        if (string.IsNullOrEmpty(config.folderName))
        {
            Debug.LogError($"[Error] missing data (folderPath) in: {filePath}");
            isValid = false;
        }
        if (string.IsNullOrEmpty(config.name))
        {
            Debug.LogError($"[Error] missing data (name) in: {filePath}");
            isValid = false;
        }
        if (string.IsNullOrEmpty(config.CharacterType))
        {
            Debug.LogError($"[Error] missing data (CharacterType) in: {filePath}");
            isValid = false;
        }
        if (!string.IsNullOrEmpty(config.folderName))
        {
            if (loadedFolderPath.Contains(config.folderName))
            {
                Debug.LogError($"[Error] Duplicate folderName '{config.folderName}' found in: {filePath}");
                isValid = false;
            }
            else
            {
                loadedFolderPath.Add(config.folderName);
            }
        }
        
        // image validation
        HashSet<string> states = new HashSet<string>();
        foreach(ImageInfo i in config.Images)
        {
            if (!string.IsNullOrEmpty(i.state))
            {
                if (states.Contains(i.state))
                {
                    Debug.LogError($"[Error] Duplicate image state '{i.state}' found in character '{config.name}'");
                    isValid = false;
                }
                else
                {
                    states.Add(i.state);
                }
            }
            else
            {
                Debug.LogError($"[Error] Image state missing for character '{config.name}'");
                isValid = false;
            }

            if (string.IsNullOrEmpty(i.spriteSheetFileName))
            {
                Debug.LogError($"[Error] Missing spriteSheetFileName for state '{i.state}' in character '{config.name}'");
                isValid = false;
            }

            if(!string.IsNullOrEmpty(config.folderName) && !string.IsNullOrEmpty(i.spriteSheetFileName))
            {
                if(!ImageCheck(config.folderName, i.spriteSheetFileName))
                {
                    Debug.LogError($"[Error] Image with name '{i.spriteSheetFileName} can not be found in {config.folderName}'");
                    isValid = false;
                }
            }


            if (i.frames != null)
            {
                foreach(FrameData f in i.frames)
                {
                    if (f.width <= 0 || f.height <= 0)
                    {
                        Debug.LogError($"[Error] Invalid or missing width/height in frame for state '{i.state}'");
                        isValid = false;
                    }
                    if (i.isAnimation)
                    {
                        if (f.durationInSeconds <= 0)
                        {
                            Debug.LogError($"[Error] isAnimation is true, but durationInSeconds is missing or 0 in state '{i.state}'");
                            isValid = false;
                        }
                    }
                    else
                    {
                        if (f.durationInSeconds > 0)
                        {
                            f.durationInSeconds = 0;
                        }
                    }
                }
            }
        }
        return isValid;
    }

    private bool ImageCheck(string folderPath, string imgName)
    {
        bool isValid = true;
        string imgPath = Path.Combine(Application.streamingAssetsPath, "Ch", folderPath, imgName);
        if (!File.Exists(imgPath)) { isValid = false; }
        return isValid;
    }
}