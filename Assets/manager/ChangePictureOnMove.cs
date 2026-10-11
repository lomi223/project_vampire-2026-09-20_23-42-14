using UnityEngine;
using System.IO;

// 1. Create a class that exactly matches the layout of your JSON file.
// [System.Serializable] tells Unity it can pack/unpack this data.
[System.Serializable]
public class ConfigData
{ 
    public string imagePath;
}

public class ChangePictureOnMove : MonoBehaviour
{
    // Sprite & SpritrRenderer 是unity自帶的class object 
    private Sprite newPicture;
    private SpriteRenderer mySpriteRenderer;
    private bool hasChanged = false;

    void Start()
    {
        //取得使用該script的gameObject中SpriteRenderer的內容
        //mySpriteRenderer是gameObject "風好大" 的 SpriteRenderer (in current version)
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        // 2. Define where your JSON file is located
        // NOTE: Application.datapath = "C:/Users/<your usr name>/project_vampire/Assets" 注意斜線方向
        string jsonFilePath = Application.dataPath + "/Json/TestImgPath.json";
        // Json (命名為：TestImgPath) 存在於 C:/Users/<your usr name>/project_vampire/Assets/Json 時執行
        if (File.Exists(jsonFilePath))
        {

            // 3和4創建名為loadedConfig的 ConfigData class內容為TestImgPath.json的內容
            // 3. Read the JSON file into a standard string
            string jsonString = File.ReadAllText(jsonFilePath);
            // 4. Convert the JSON string into our C# ConfigData object
            ConfigData loadedConfig = JsonUtility.FromJson<ConfigData>(jsonString);

            // result: "C:/Users/<your usr name>/project_vampire/Assets/TmpPicFolder/newPhoto.PNG"
            // 5. Combine the dataPath with the path we just pulled from the JSON
            string fullImagePath = Application.dataPath + loadedConfig.imagePath;

            // 6. Load the image using the new dynamic path
            LoadSpriteFromFile(fullImagePath);
        }
        else
        {
            Debug.LogError("Could not find the JSON config file at: " + jsonFilePath);
        }
    }

    void LoadSpriteFromFile(string path)
    {
        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            // Texture2D(2,2)創建了一個2*2 pixel的空白正方形，在tex.LoadImage(fileData)之後會被覆蓋(仍建議設定為2的指數次方以避免錯誤)
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            // Sprite.Create(讀取資料來源, 擷取部分, 軸心(pivot)位置)
            // Rect(照片起始點(x),照片起始點(y), 讀入pixel(x), 讀入pixel(y))
            // Vector2(0.5f, 0.5f)設定 pivot 在中心位置(50%, 50%)
            newPicture = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.LogError("Could not find the image file at: " + path);
        }
    }

    void Update()
    {
        if (!hasChanged && transform.position.x > 5f)
        {
            if (mySpriteRenderer != null && newPicture != null)
            {
                mySpriteRenderer.sprite = newPicture;
                hasChanged = true;
            }
        }
    }
}