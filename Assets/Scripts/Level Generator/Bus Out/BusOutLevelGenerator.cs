using UnityEngine;

public class BusOutLevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject busPrefab;


    [Header("CUSTOMIZE")]
    [SerializeField] private float tileSize;
    [SerializeField] private int areaRow;
    [SerializeField] private int areaColumn;

    #region PRIVATE FIELD
    private bool[] isTilesChecked;
    #endregion

    private void Awake()
    {
        isTilesChecked = new bool[areaRow * areaColumn];

        Generate();
    }

    private void Generate()
    {
        int busIndex = 0;

        for (int i = 0; i < isTilesChecked.Length; i++)
        {
            int xIndex = i % areaColumn;
            int yIndex = (i - xIndex) / areaColumn;

            if (!isTilesChecked[i])
            {
                Direction direction = (Direction)Random.Range(0, 4);

                if (direction == Direction.Left || direction == Direction.Right)
                {
                    bool isValid = true;

                    for (int j = xIndex; j <= xIndex + 2; j++)
                    {
                        if (j > areaColumn)
                        {
                            isValid = false;

                            break;
                        }
                        else
                        {
                            if (isTilesChecked[j + yIndex * areaColumn])
                            {
                                isValid = false;

                                break;
                            }
                        }
                    }

                    if (isValid)
                    {
                        for (int j = xIndex + 1; j <= xIndex + 2; j++)
                        {
                            isTilesChecked[j + yIndex * areaColumn] = true;
                        }

                        Vector3 position = new Vector3();

                        position.x = (xIndex + 1) * tileSize;
                        position.z = yIndex * tileSize;

                        GameObject bus = Instantiate(busPrefab);

                        bus.transform.position = position;

                        Debug.Log(busIndex + "/" + new Vector2(xIndex, yIndex) + "/" + new Vector2(xIndex + 2, yIndex));

                        busIndex++;
                    }
                }
                else
                {

                }

                isTilesChecked[i] = true;
            }
        }
    }
}
