/* 
 * Author: Athanasios Soulis 
 * Part of Bsc Thesis in the Department of Informatics & Telecommunications 
 * University of Athens
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class VRATImagePlacer : MonoBehaviour {

    [SerializeField] int materialToBeReplaced;
    
    public bool placeImage(VRATImage image, string fileDirectoryPath)
    {
        bool ret = false;

        if (image != null && image.src != null)
        {

            //Trim the picture suffix from the imageSrc [.jpg , .png]
            string suffix = image.src.Split('.').Last();
            string imageSrcPath = fileDirectoryPath + image.src.Substring(0, image.src.Length - (suffix.Length +1));

            Debug.Log("Path: " + imageSrcPath);            

            Texture2D tex = Resources.Load<Texture2D>(imageSrcPath);
            Debug.Log("FileData: "+ tex.name);

            if (tex != null)
            {                   
                Material[] materials = transform.GetComponent<Renderer>().materials;
                foreach(Material mat in materials)
                    Debug.Log(mat.name);
                materials[materialToBeReplaced].mainTexture = tex;
                ret = true;
            }
        }
        else
            Debug.Log("Image NOT Found: " + (image == null ? "Image is null" : "No Image src"));

        return ret;
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
