using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SequenceSliderView : SliderView
{
    [SerializeField]
    private float snapSpeed = 3f;

    private List<Base> sequence;
    private int selectionSize;
    private int marginSize;
    private List<GameObject> baseModels;
    private float sliderWorldStartX;
    private Coroutine snapCoroutine;

    public override void Init()
    {
        base.Init();
        sliderWorldStartX = sliderGameObject.transform.position.x;
        var touchDetector = sliderGameObject.GetComponent<TouchDetectorView>();
        touchDetector.OnDownSignal.AddListener(
            () => {
                if (snapCoroutine != null)
                    StopCoroutine(snapCoroutine);
            });
        touchDetector.OnUpSignal.AddListener(() => SnapToNearestBase(false));
    }

    private void SnapToNearestBase(bool immediately)
    {
        if(snapCoroutine != null)
            StopCoroutine(snapCoroutine);
        snapCoroutine = StartCoroutine(SnapToNearestBaseCoroutine(immediately ? 100000f : snapSpeed));
    }

    private IEnumerator SnapToNearestBaseCoroutine(float speed)
    {
        var destination = GetNearestBaseSliderLocation();

        while(sliderGameObject.transform.position != destination)
        {
            var pos = Vector3.MoveTowards(sliderGameObject.transform.position, destination, speed * Time.deltaTime);
            sliderGameObject.gameObject.transform.position = pos;
            yield return null;
        }

        sliderGameObject.gameObject.transform.position = destination;
        snapCoroutine = null;
    }

    private Vector3 GetNearestBaseSliderLocation()
    {
        float modelX = 0f;
        float minDistance = float.MaxValue;
        int skipMarginSize = marginSize + selectionSize / 2;

        for (int i = skipMarginSize; i < baseModels.Count - skipMarginSize; i++)
        {
            var model = baseModels[i];
            var dist = Mathf.Abs(model.transform.position.x - sliderWorldStartX);
            if(dist < minDistance)
            {
                minDistance = dist;
                modelX = model.transform.position.x;
            }
        }

        float delta = sliderWorldStartX - modelX;
        var position = base.sliderGameObject.transform.position;
        position.x += delta;
        return position;
    }

    private void InitializeSliderBounds()
    {
        var modelBounds = sequence[0].displayPrefab.GetComponent<Renderer>().bounds;
        float sliderHalfSize = modelBounds.size.x * sequence.Count * 0.5f;
        float delta = 0f;

        bool sequenceIsEven = sequence.Count % 2 == 0;
        if (sequenceIsEven)
        {
            float limit = ((selectionSize - 1) * 0.5f + marginSize) * modelBounds.size.x;
            delta = sliderHalfSize - limit;
        }
        else
        {
            float limit = (selectionSize / 2 + marginSize) * modelBounds.size.x;
            delta = sliderHalfSize - limit;
        }

        // Set upper bound
        var upos = highEndTransform.position;
        upos.x += delta;
        highEndTransform.position = upos;

        // Set lower bound
        var lpos = lowEndTransform.position;
        lpos.x -= delta;
        lowEndTransform.position = lpos;
    }

    #region public interface

    public void DisplaySequence(List<Base> sequence, int selectionSize, int marginSize)
    {
        if (selectionSize % 2 != 1)
            throw new Exception("SequenceSliderView only supports odd selection sizes for now");
        if (sequence.Count < selectionSize + marginSize * 2)
            throw new Exception("Sequence needs to be larger than selectionSize + marginSize * 2");

        this.sequence = sequence;
        this.selectionSize = selectionSize;
        this.marginSize = marginSize;
        this.baseModels = new List<GameObject>();

        var modelBounds = sequence[0].displayPrefab.GetComponent<Renderer>().bounds;

        for (int i = 0; i < sequence.Count; i++)
        {
            var @base = sequence[i];
            var model = Instantiate(@base.displayPrefab);
            model.GetComponent<Renderer>().material.color = @base.color;
            model.transform.SetParent(base.sliderGameObject.transform, false);
            model.transform.localPosition = new Vector3(i * modelBounds.size.x - (sequence.Count - 1) * 0.5f * modelBounds.size.x, 0);
            model.layer = gameObject.layer;
            baseModels.Add(@model);
        }

        var touchSize = base.sliderGameObject.GetComponent<BoxCollider>().size;
        touchSize.x = modelBounds.size.x * sequence.Count;
        touchSize.y = modelBounds.size.y;
        touchSize.z = 0.01f;
        base.sliderGameObject.GetComponent<BoxCollider>().size = touchSize;

        InitializeSliderBounds();
        SnapToNearestBase(true);
    }

    public void ClearSequence()
    {
        var children = sliderGameObject.GetComponentsInChildren<Transform>();
        foreach (var child in children)
            if (child != sliderGameObject.transform)
                Destroy(child.gameObject);

        var pos = sliderGameObject.transform.position;
        pos.x = sliderWorldStartX;
        sliderGameObject.transform.position = pos;
        sliderGameObject.GetComponent<BoxCollider>().size = Vector3.zero;
        lowEndTransform.localPosition = Vector3.zero;
        highEndTransform.localPosition = Vector3.zero;

        StopAllCoroutines();
    }

    public List<Base> GetLeftMargin()
    {
        int centerIndex = GetCenterIndex();
        int leftIndex = centerIndex - selectionSize / 2 - marginSize;

        return sequence.GetRange(leftIndex, marginSize);
    }

    public List<Base> GetRightMargin()
    {
        int centerIndex = GetCenterIndex();
        int leftIndex = centerIndex + selectionSize / 2 + 1;

        return sequence.GetRange(leftIndex, marginSize);
    }

    public List<Base> GetSelectedSequence()
    {
        int centerIndex = GetCenterIndex();
        int leftIndex = centerIndex - selectionSize / 2;

        return sequence.GetRange(leftIndex, selectionSize);
    }

    // Gets the index of the centered base
    private int GetCenterIndex()
    {
        float minDistance = float.MaxValue;
        int centerIndex = -1;
        for (int i = 0; i < baseModels.Count; i++)
        {
            var model = baseModels[i];
            float dist = Mathf.Abs(model.transform.position.x - sliderWorldStartX);
            if (dist < minDistance)
            {
                minDistance = dist;
                centerIndex = i;
            }
        }

        return centerIndex;
    }

    #endregion
}
