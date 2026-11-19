
## In store:

1. chmgr: spawn obj
2. obj: decide which character to spawn
	- decide which character to be based on weight (seed control)
	- image to sprite function void ImgToSprite(CharacterInfo Ch, string state)
	- decide how to move (cuatom movement using json?)
	- a way to change image after character is on screen(how to trigger
	- how a character despawn(move out screen? fade?)
3. Character class design:
	- Add List movement class
	- The movement class contain:
		- speed
		- fuction(x = ... , y = ...)
		- image on move (the state)
	this is harder than i thought, will use primetween instead.
4. events:
	- Show dialogue
	- Choose dialogue
	- scan item
	- buy item
	- restock item
5. Control flow:
	- every `x` time add character to queue (Max character in queue?) 
	- if incomming customer is normal -> trigger scan event
	- if incomming customer is special -> trigger dialogue (could be the last customer)
	- items bought by the customer should decrease the amount of item on stock(random? specific?)
## Character JSON
```JSON
{
	"folderName": //Name of the folder character is in ex: CH/<folderName>
	"name": //Name of the character
	"CharacterType": //Spectial Customer/Normal Customer etc. NOTE: I did not think of how to utilize this yet, could be changed in the future
	"SpawnWeight": //how frequent the character will spawn NOTE: I did not think of how to set weight for special characters
	"Images": [
		{
			"state": //state of the image, the scripe decide which image to show based on the state
			"isAnimation": //determine if the image is a frame by frame animation
			"spriteSheetFileName": //file name of the image
			"frames": [
				{
					"x": //starting point x of the image
					"y": //starting point y of the image
					"width": //width of the image
					"hight": //hight of the image
					"durationInSeconeds": //the period of a frame
				},
				//...
			]
		},
		//...
	] 
}
```

## Note
- 腳色存放位置：`Assets/StreamtingAssets/Ch`
- 角色資料（圖片, 名字）：`Ch/<folderName>/*.json`
- 文本：`csv` (暫時)
