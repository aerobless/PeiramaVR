using UnityEngine;

public class Mug : MonoBehaviour
{
    //0 - 100
    public int fillContent = 0;

    public ParticleSystem beerParticles;
    public GameObject drinkContent;
    private MeshRenderer _drinkContentMesh;
    private Transform _drinkingMeshTransform;

    private const float FillingStartPos = -0.0771f;
    private const float FillingEndPos = 0.0876f;

    // Start is called before the first frame update
    void Start()
    {
        _drinkContentMesh = drinkContent.GetComponent<MeshRenderer>();
        _drinkingMeshTransform = drinkContent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (fillContent <= 0)
        {
            _drinkContentMesh.enabled = false;
        }
        else
        {
            _drinkContentMesh.enabled = true;
        }
    }

    public void FillMugByIncrement()
    {
        if (fillContent < 100)
        {
            fillContent += 1;
            var fillingLerp = Mathf.Lerp(FillingStartPos, FillingEndPos,fillContent/100f);
            var fillingPos = _drinkingMeshTransform.localPosition;
            _drinkingMeshTransform.localPosition = new Vector3(fillingPos.x, fillingPos.y, fillingLerp);
        }
    }
}