using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class XMLLoader
{
    public XMLLoader(){ }

    public XmlDocument[] GetAllXMLDocumentsInDirectory(string directory)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/" + directory);
        FileInfo[] allFiles = directoryInfo.GetFiles("*.xml");
        //TextAsset[] xmlFiles = Resources.LoadAll<TextAsset>(Application.dataPath + "/" + directory + "/");
        
        XmlDocument[] foundDocuments = new XmlDocument[allFiles.Length];
        for (int foundIndex = 0; foundIndex < allFiles.Length; foundIndex++)
        {
            XmlDocument enemyType = new XmlDocument();
            enemyType.LoadXml(File.ReadAllText(allFiles[foundIndex].FullName));
            foundDocuments[foundIndex] = enemyType;
        }

        //Resources.UnloadUnusedAssets();

        return foundDocuments;
    }
    //void Start()
    //{
    //    TextAsset[] xmlFiles = Resources.LoadAll<TextAsset>("Enemies/");
    //    foreach (TextAsset file in xmlFiles)
    //    {
    //        XmlDocument enemyType = new XmlDocument();
    //        enemyType.LoadXml(file.text);
    //        ParseXmlFile(enemyType);
    //    }        
    //}
    //
    //void ParseXmlFile(XmlDocument xmlDoc)
    //{
    //    XmlNode parentNode = xmlDoc.FirstChild;
    //    XmlNode enemyName = parentNode.NextSibling.FirstChild;
    //    XmlNode attributesNode = enemyName.NextSibling;
    //    XmlNode meshNode = attributesNode.FirstChild;
    //    XmlNode speedNode = meshNode.NextSibling;
    //    XmlNode hitPointsNode = speedNode.NextSibling;
    //    XmlNode scoreNode = hitPointsNode.NextSibling;
    //    XmlNode attackNode = scoreNode.NextSibling;
    //
    //    print(enemyName.InnerText);
    //    print(meshNode.InnerText);
    //    print(speedNode.InnerText);
    //    print(hitPointsNode.InnerText);
    //    print(scoreNode.InnerText);
    //    print(attackNode.InnerText);
    //}
}
