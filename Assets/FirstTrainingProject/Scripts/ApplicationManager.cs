using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "My Data", menuName = "ScriptableObjects/ApplicationManager", order = 1)]
public class ApplicationManager : ScriptableObject
{
    #region Serialize Fields

    //[SerializeField]
    private MapController _mapController;

    //[SerializeField]
    private PlayerController _playerController;

    //[SerializeField]
    private GameController _gameController;

    #endregion

    #region Properties

    public MapController MapController
    {
        get { return _mapController; }
        set { _mapController = value; }
    }

    public PlayerController PlayerController
    {
        get { return _playerController; }
        set { _playerController = value; }
    }

    public GameController GameController
    {
        get { return _gameController; }
        set { _gameController = value; }
    }

    #endregion
}
