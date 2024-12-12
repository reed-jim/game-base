using UnityEngine;

public class BusOutLevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject busPrefab;


    [Header("CUSTOMIZE")]
    [SerializeField] private float tileSize;
    [SerializeField] private int areaRow;
    [SerializeField] private int areaColumn;
    [SerializeField] private float maxAngleVariationMagnitude;

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
                        bus.transform.eulerAngles = Vector3.zero + new Vector3(0, 90 + Random.Range(-maxAngleVariationMagnitude, maxAngleVariationMagnitude), 0);

                        if (direction == Direction.Left)
                        {
                            bus.transform.eulerAngles += new Vector3(0, 180, 0);
                        }

                        busIndex++;
                    }
                }
                else
                {
                    bool isValid = IsValidVerticleAreaForBus(yIndex, yIndex + 2, xIndex);

                    if (isValid)
                    {
                        for (int j = yIndex + 1; j <= yIndex + 2; j++)
                        {
                            isTilesChecked[xIndex + j * areaColumn] = true;
                        }

                        Vector3 position = new Vector3();

                        position.x = xIndex * tileSize;
                        position.z = (yIndex + 1) * tileSize;

                        GameObject bus = Instantiate(busPrefab);

                        bus.transform.position = position;
                        bus.transform.eulerAngles = Vector3.zero + new Vector3(0, Random.Range(-maxAngleVariationMagnitude, maxAngleVariationMagnitude), 0);

                        if (direction == Direction.Down)
                        {
                            bus.transform.eulerAngles += new Vector3(0, 180, 0);
                        }

                        busIndex++;
                    }
                }

                isTilesChecked[i] = true;
            }
        }
    }

    private bool IsValidVerticleAreaForBus(int startYIndex, int endYIndex, int xIndex)
    {
        bool isValid = true;

        for (int j = startYIndex; j <= endYIndex; j++)
        {
            if (isTilesChecked[xIndex + j * areaColumn])
            {
                isValid = false;

                break;
            }

            // if (j > areaColumn)
            // {
            //     isValid = false;

            //     break;
            // }
            // else
            // {
            //     if (isTilesChecked[j + yIndex * areaColumn])
            //     {
            //         isValid = false;

            //         break;
            //     }
            // }
        }

        return isValid;
    }
}
