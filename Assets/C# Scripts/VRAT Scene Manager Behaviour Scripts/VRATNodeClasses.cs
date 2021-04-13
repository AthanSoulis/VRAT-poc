/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VRATNode
{
    public string type;
    public int id;
    public VRATData data;
    public VRATNode[] nodes;
    public VRATPresentation[] presentations;
    public bool autoProceed;
    public VRATTransform proceedTransform;
}

public class VRATData
{
    public string title;
    public string unityScene;
    public VRATImage image;
    public bool showTitle;
    public string long_description;
    public string short_description;
    public int presentation;
    public string template;
    public VRATAudio audio;
    public string text;
    public VRATCharacter[] characters;
    public VRATQuote[] quotes;
    public string prompt;
}

public class VRATPresentation
{
    public string title;
    public int id;
    public bool showTitle;
}

public class VRATQuote
{
    public int character;
    public string text;
    public VRATAudio audio;
}

public class VRATCharacter
{
    public string name;
    public VRATImage image;
    public VRATTransform transform;
}

public class VRATAudio
{
    public string src;
    public string name;
    public string type;
}

public class VRATImage
{
    public string src;
    public string name;
    public string type;
}

public class VRATTransform
{
    public float[] position;
    public float[] rotation;
    public float[] scale;
}

