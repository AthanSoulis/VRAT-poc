/* Store the contents for ListBoxes to display.
 */
using UnityEngine;

public class ListBank : MonoBehaviour
{
	public static ListBank Instance;

    //A.S. The Prefab of a ListBox
    public ListBox ListBoxPrefab;

    [SerializeField]
    private int maximumListBoxArraySize = 4;

    /*A.S. 
	private int[] contents = {
		1, 2, 3, 4, 5, 6, 7, 8, 9, 10
	};
    */

    //A.S. 
    public VRATNode[] contents;
    private ListBox[] listBoxArray;

	void Awake()
	{
		Instance = this;
        contents = null;
        listBoxArray = null;
	}

    //A.S. 
    public VRATNode getListContentFNode(int index)
    {
        return contents[index];
    }

    //A.S. 
    public string getListContent(int index)
	{
		return contents[index].data.title;
	}

	public int getListLength()
	{
		return contents == null ? 0 : (contents.Length > maximumListBoxArraySize ? maximumListBoxArraySize : contents.Length);
	}

    public int getRealListLength()
    {
        return contents == null ? 0 : contents.Length ;
    }

    public void clearContents()
    {   
        if(listBoxArray != null)
            foreach (ListBox l in listBoxArray)
                Destroy(l.gameObject);

        listBoxArray = null;

        Debug.Log("Cleared Contents");
    }

    //A.S. Feeds the ListPositionCtrl the ListBox Array.
    public ListBox[] getListBoxArray()
    {   
        if(contents != null) {

            int listBoxArraySize = contents.Length;
            if (contents.Length > maximumListBoxArraySize)
                listBoxArraySize = maximumListBoxArraySize;

            listBoxArray = new ListBox[listBoxArraySize];

            for (int i = 0; i < listBoxArray.Length; i++) { 
                listBoxArray[i] = Instantiate(ListBoxPrefab,gameObject.transform) as ListBox;
                listBoxArray[i].transform.localRotation = Quaternion.identity;
                listBoxArray[i].transform.localScale = Vector3.one;
            }


            for (int i = 0; i < listBoxArray.Length; i++)
            {
                listBoxArray[i].listBoxID = i;
                if (i > 0)
                    listBoxArray[i].lastListBox = listBoxArray[i - 1];
                if(i < listBoxArray.Length -1)
                    listBoxArray[i].nextListBox = listBoxArray[i + 1];
            }
            listBoxArray[0].lastListBox = listBoxArray[listBoxArray.Length - 1];
            listBoxArray[listBoxArray.Length - 1].nextListBox = listBoxArray[0];
        }

        return listBoxArray;

    }
}
