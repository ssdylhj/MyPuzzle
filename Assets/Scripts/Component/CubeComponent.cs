using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MyPuzzle;

public class CubeComponent
    : MonoBehaviour
    , IPointerDownHandler
    , IPointerUpHandler
    , IDragHandler
{
    [SerializeField] private Image UpLine;
    [SerializeField] private Image RightLine;
    [SerializeField] private Image DownLine;
    [SerializeField] private Image LeftLine;

    private RectTransform RectTransform;
    private float halfRectWidth;
    private float halfRectHeight;
    private Vector2 UpPoint;
    private Vector2 RightPoint;
    private Vector2 DownPoint;
    private Vector2 LeftPoint;
    private void Start()
    {
        this.RectTransform = this.GetComponent<RectTransform>();
        this.halfRectWidth = this.RectTransform.rect.width / 2;
        this.halfRectHeight = this.RectTransform.rect.height / 2;

        this.InitPoint();
        Debug.Log(string.Format("rectWidth: {0} rectHeight: {1}", this.halfRectWidth, this.halfRectHeight));
    }

    private void InitPoint()
    {
        this.UpPoint.y = this.halfRectHeight;
        this.RightPoint.x = this.halfRectWidth;
        this.DownPoint.y = -this.halfRectHeight;
        this.LeftPoint.x = -this.halfRectWidth;
    }

    private Cube Cube;
    private int Row;
    private int Col;
    public void Setup(Cube cube, int r, int c)
    {
        this.Cube = cube;
        this.Row = r;
        this.Col = c;

        this.Cube.IsDirty = true;
    }

    private void Update()
    {
        if (!this.Cube.IsDirty)
            return;

        this.Cube.IsDirty = false;

        if (this.Cube.UpColor != MyColor.None)
        {
            this.UpLine.color = this.Cube.UpColor.ToColor();
        }

        if (this.Cube.RightColor != MyColor.None)
        {
            this.RightLine.color = this.Cube.RightColor.ToColor();
        }

        if (this.Cube.DownColor != MyColor.None)
        {
            this.DownLine.color = this.Cube.DownColor.ToColor();
        }

        if (this.Cube.LeftColor != MyColor.None)
        {
            this.LeftLine.color = this.Cube.LeftColor.ToColor();
        }
    }

    #region Event Handler
    private Vector2 startPos;
    public void OnDrag(PointerEventData eventData)
    {
        var localPos = this.transform.InverseTransformPoint(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var localPos = this.transform.InverseTransformPoint(eventData.position);
        this.startPos = this.SnapPoint(localPos);

        Debug.Log(string.Format("Down Point: {0} Snap To: (x: {1}, y: {2})",
            localPos,
            this.startPos.x,
            this.startPos.y));
    }

    public Action<int, int, Direction> OnDraw;
    public void OnPointerUp(PointerEventData eventData)
    {
        var localPos = this.transform.InverseTransformPoint(eventData.position);
        var snapPoint = this.SnapPoint(localPos);

        var dir = (snapPoint - this.startPos).ToDirection();
        if (dir == Direction.None)
            return;

        Debug.Log(string.Format("Down Point: {0} Snap To: (x: {1}, y: {2}), dir: {3}, Direction: {4}",
            localPos,
            snapPoint.x,
            snapPoint.y,
            snapPoint - this.startPos,
            dir
            ));

        OnDraw.SafeInvoke(this.Row, this.Col, this.GetFinalDirection(this.startPos, snapPoint));
    }

    private Direction GetFinalDirection(Vector2 start, Vector2 end)
    {
        var dir = (end - start).ToDirection();
        if (dir == Direction.None)
            return Direction.None;

        if (dir == Direction.Up && (start - this.DownPoint).sqrMagnitude < float.Epsilon)
            return Direction.Down;

        if (dir == Direction.Right && (start - this.LeftPoint).sqrMagnitude < float.Epsilon)
            return Direction.Left;

        if (dir == Direction.Down && (start - this.UpPoint).sqrMagnitude < float.Epsilon)
            return Direction.Up;

        if (dir == Direction.Left && (start - this.RightPoint).sqrMagnitude < float.Epsilon)
            return Direction.Right;

        return dir;
    }

    private Vector2 SnapPosition = Vector2.zero;
    private Vector2 SnapPoint(Vector2 p)
    {
        this.SnapPosition.x = (int)(p.x/this.halfRectWidth + Mathf.Sign(p.x) * 0.5f) * this.halfRectWidth;
        this.SnapPosition.y = (int)(p.y/this.halfRectHeight + Mathf.Sign(p.y) * 0.5f) * this.halfRectHeight;

        return this.SnapPosition;
    }
    #endregion
}
