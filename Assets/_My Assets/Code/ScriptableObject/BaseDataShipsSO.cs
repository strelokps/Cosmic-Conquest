using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDataShipsSO : ScriptableObject
{
    private DataShip _dataShip = new DataShip();

    public DataShip GetDataShipLight()
    {

        return _dataShip;
    }
    public DataShip GetDataShipMedium()
    {
        return _dataShip;
    }
    public DataShip GetDataShipHeavy()
    {
        return _dataShip;
    }
}
