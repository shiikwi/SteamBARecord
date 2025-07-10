# Steam BA Record

对steam版碧蓝档案的一些记录

## il2cpp dump

Use: [frida-il2cpp-bridge](https://github.com/vfsfitvnm/frida-il2cpp-bridge)

```bash
npm install frida-il2cpp-bridge
npx frida-il2cpp-bridge -- -p PID dump
```

## Unity资源
`BlueArchive_Data\StreamingAssets\PUB\Resource\GameData\Windows`  
`BlueArchive_Data\StreamingAssets\PUB\Resource\Preload\Windows`  
Use: [AssetStudio](https://github.com/Perfare/AssetStudio)

## 解压ZIP
```csharp
class TableService
{
    static bool HasChanged;

    static TableService();
    static void ClearCache();
    static byte[] LoadBytes(string key, string extension, bool showFailedPopup);
    static string Load(string key, string extension);
    static string CreatePassword(string key, int length);
}
```
`CreatePassword`将带扩展文件名(*.zip)进行xxHash，转化为十进制作为梅森旋转伪随机数生成器的seed(Mersenne Twister MT19937)  
将传入的`length`(貌似固定为20)进行运算`(3 * length) >> 2`作为字节数组长度  
梅森旋转算法生成伪随机数存入字节数组，将其进行base64编码  
Use: `DecpTableZip`
```bash
DecpTableZip.exe <InputZIPFolder>
```

## 解密bytes文件
```csharp
class TableEncryptionService
{
static byte[] CreateKey(string name);
static byte[] XOR(string name, byte[] bytes);
...
}
```
`XOR`用区分单词大小写不带扩展名的文件名(大驼峰法)进行xxHash，用梅森旋转算法生成伪随机字节流进行XOR  
bytes文件名可以在il2cpp dump文件中找到对应的类  
`GenXORKey.py`可以生成bytes文件对应的类名作为Key  
Use: `Decryptbytes`
```bash
Decryptbytes.exe <InputBytesFolder>
```
解密后部分文件内容依然是经过base64编码存储的
