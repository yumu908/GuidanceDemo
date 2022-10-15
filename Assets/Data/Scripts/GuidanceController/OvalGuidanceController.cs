using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OvalGuidanceController : MonoBehaviour
{
    private Canvas canvas;

    /// <summary>
    /// 要高亮显示的目标
    /// </summary>
    [HideInInspector]
    public RectTransform target;

    /// <summary>
    /// 区域范围缓存
    /// </summary>
    private Vector3[] _corners = new Vector3[4];

    /// <summary>
    /// 镂空区域圆心
    /// </summary>
    private Vector4 _center;

    /// <summary>
    /// 椭圆长轴和短轴
    /// </summary>
    private float _longAxis;
    private float _shortAxis;

    /// <summary>
    /// 遮罩材质
    /// </summary>
    private Material _material;

    /// <summary>
    /// 当前长轴和短轴
    /// </summary>
    private float _currentLongAxisX;
    private float _currentshortAxisX;


    /// <summary>
    /// 高亮区域缩放的动画时间
    /// </summary>
    private float _shrinkTime = 0.5f;
    /// <summary>
    /// Button穿透点击的类
    /// </summary>
    private EventPermeate eventPermeateClass;

    private void Awake()
    {
        eventPermeateClass = GetComponent<EventPermeate>();
    }
    /// <summary>
    /// 世界坐标向画布坐标转换
    /// </summary>
    /// <param name="canvas">画布</param>
    /// <param name="world">世界坐标</param>
    /// <returns>返回画布上的二维坐标</returns>
    private Vector2 WorldToCanvasPos(Canvas canvas, Vector3 world)
    {
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            world, canvas.GetComponent<Camera>(), out position);
        return position;
    }

    public void SetTarget(RectTransform target)
    {
        this.target = target;
        RefreshMask();
        eventPermeateClass.target = target.gameObject;
    }

    public void RefreshMask()
    {
        //获取画布
        canvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        //获取高亮区域的四个顶点的世界坐标
        target.GetWorldCorners(_corners);
        //计算最终高亮显示区域的半径
        _longAxis = Vector2.Distance(WorldToCanvasPos(canvas, _corners[0]), WorldToCanvasPos(canvas, _corners[3])) / 2f;
        _shortAxis = Vector2.Distance(WorldToCanvasPos(canvas, _corners[0]), WorldToCanvasPos(canvas, _corners[1])) / 2f; 
        //计算高亮显示区域的圆心
        float x = _corners[0].x + ((_corners[3].x - _corners[0].x) / 2f);
        float y = _corners[0].y + ((_corners[1].y - _corners[0].y) / 2f);
        Vector3 centerWorld = new Vector3(x, y, 0);
        Vector2 center = WorldToCanvasPos(canvas, centerWorld);
        //设置遮罩材料中的圆心变量
        Vector4 centerMat = new Vector4(center.x, center.y, 0, 0);
        _material = GetComponent<Image>().material;
        _material.SetVector("_Center", centerMat);
        //计算当前高亮显示区域的半径
        RectTransform canRectTransform = canvas.transform as RectTransform;
        if (canRectTransform != null)
        {
            //获取画布区域的四个顶点
            canRectTransform.GetWorldCorners(_corners);
            //将画布顶点距离高亮区域中心最远的距离作为当前高亮区域半径的初始值
            for (int i = 0; i < _corners.Length; i++)
            {
                if (i % 2 == 0)
                    _currentLongAxisX = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, _corners[i]), center), _currentLongAxisX);
                else
                    _currentshortAxisX = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, _corners[i]), center), _currentshortAxisX);
            }
        }
        _material.SetFloat("_SliderX", _currentLongAxisX);
        _material.SetFloat("_SliderY", _currentshortAxisX);
    }

    /// <summary>
    /// 收缩速度
    /// </summary>
    private float _shrinkVelocityX = 0f;
    private float _shrinkVelocityY = 0f;

    private void Update()
    {
        //从当前半径到目标半径差值显示收缩动画
        float valueX = Mathf.SmoothDamp(_currentLongAxisX, _longAxis + 10, ref _shrinkVelocityX, _shrinkTime);
        float valueY = Mathf.SmoothDamp(_currentshortAxisX, _shortAxis + 10, ref _shrinkVelocityY, _shrinkTime);
        if (!Mathf.Approximately(valueX, _currentLongAxisX))
        {
            _currentLongAxisX = valueX;
            _material.SetFloat("_SliderX", _currentLongAxisX);
        }

        if (!Mathf.Approximately(valueY, _currentshortAxisX))
        {
            _currentshortAxisX = valueY;
            _material.SetFloat("_SliderY", _currentshortAxisX);
        }
    }
}