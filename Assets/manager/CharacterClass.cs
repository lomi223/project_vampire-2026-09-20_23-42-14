using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

[System.Serializable]
public class FrameData
{
    public float x;
    public float y;
    public float width;
    public float height;
    public float durationInSeconds;
}

[System.Serializable]
public class ImageInfo
{
    public string state;
    public bool isAnimation;
    public string spriteSheetFileName;
    public List<FrameData> frames = new List<FrameData>();

}

[System.Serializable]
public class CharacterInfo
{
    public string folderName;
    public string name;
    public string CharacterType;
    public int SpawnWeight = 0;
    public List<ImageInfo> Images = new List<ImageInfo>();
}