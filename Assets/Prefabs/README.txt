How to Add NPC Assets

import fbx file
put it on the scene. 
scale it to 3 in xyz (collider match)
then adjust the scale through package scale size 1-> 0.9, 0.8 (Whatever)
Copy paste all components from existing prefabs such as ghost.
Set the layer and tag as "NPC"
Change the NPC_SO to either human or monster version 
**if custom assets**
set "isCustomAsset" and "AnimOn" variables to true in "NPC" script
add animator component (copy paste from existings)
change the animator avatar to according model's avatar
(if doesn't exist, go to asset package and go to "rig" tab. Then set model type from "general" to 'Humanoid"

Add "NPCAnim" script (Use the existings)
then change the animator controller to the accordings.