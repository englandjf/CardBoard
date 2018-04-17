using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreationManager : MonoBehaviour {

    [SerializeField] private TileScript _BlankTile;
    [SerializeField] private DragCamera _DragCamera;

    private List<List<TileScript>> _CreatedTiles;
    private List<TileScript> _CreatedBankTiles;

	// Use this for initialization
	void Start () {
        CreateBoardWithDimensions(5, 5);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CreateBoardWithDimensions(int x, int y)
    {
        //Play Area
        _CreatedTiles = new List<List<TileScript>>();
        for(int i = 0; i < x; i++)
        {
            _CreatedTiles.Add(new List<TileScript>());
            for(int j = 0;j < y;j++)
            {           
                TileScript TS = (TileScript)Instantiate(_BlankTile, new Vector3(i * 2, 0, j * 2), _BlankTile.transform.rotation);
                _CreatedTiles[i].Add(TS);
                TS.gameObject.name = "TILE"+ i + "-" + j;
                TS.ListPosX = i;
                TS.ListPosY = j;
                if(i == (int)x/2)
                {
                    TS.SetAsWaterTile();
                    TS.gameObject.name += "W";
                }

                if(j == 0)
                {
                    TS.SetAsSpawnTile();
                }
            }
        }
        TileAssistant.Board = _CreatedTiles;

        //Bank Area, hard limit of 5 for now
        _CreatedBankTiles = new List<TileScript>();
        for (int i = 0; i < 5; i++)
        {
            TileScript TS = (TileScript)Instantiate(_BlankTile, new Vector3(i * 2, 0, -4), _BlankTile.transform.rotation);
            _CreatedBankTiles.Add(TS);
            TS.SetAsBankTile();
            TS.gameObject.name = "BTILE" + i;
            TS.ListPosX = i;
            TS.ListPosY = -4;
        }

        _DragCamera.CreateCameraBarrierValues(-4,-8,(x * 2) +4,(y * 2)+4);

    }
}
