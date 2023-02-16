# Mod Class Dumper
Reads and serialize all the public(?) fields/properties in the dll file by class
![image](https://user-images.githubusercontent.com/11086210/219307472-1b856051-55ed-45b9-b49f-2b73c70f57d1.png)

<h2>How to Use</h2>
Place compiled .exe in RimWorldWin64_Data/Managed
Drag & Drop your dll to the .exe
The serialized xml file should be in where your dll file is
Cannot load dll reference automatically for now, so if it needs other dlls (e.g. Harmony, Hugslib) you will have to copy them to the same folder as Assembly-CSharp.dll for it to work
