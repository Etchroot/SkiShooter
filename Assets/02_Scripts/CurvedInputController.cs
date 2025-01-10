using System;
using System.Collections;
using CurvedUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class CurvedInputController : MonoBehaviour
{
    [SerializeField] private NearFarInteractor nearFarInteractor;

    public Canvas canvas;

    // 비활성화 시켜야할 CurvedUI 컴포넌트
    public CurvedUIInputModule curvedUIInputModule;
    public CurvedUIRaycaster curvedUIRaycaster;

    // 활성화 시켜야 할 컴포넌트
    public GraphicRaycaster graphicRaycaster;
    public TrackedDeviceGraphicRaycaster trackedDeviceGraphicRaycaster;
    public XRUIInputModule xrUIInputModule;

    public GameObject selectedObject;

    IEnumerator Start()
    {
        nearFarInteractor = GetComponent<NearFarInteractor>();
        BindingEvents();

        yield return new WaitForSeconds(2.0f);
        DeactivateProperties();

        yield return new WaitForSeconds(2.0f);
        ActivateProperties();
    }

    private void ActivateProperties()
    {
        // GraphicRaycaster 컴포넌트 활성화
        //graphicRaycaster = GameObject.FindAnyObjectByType<GraphicRaycaster>();
        graphicRaycaster.enabled = true;

        // TrackedDeviceGraphicRaycaster 컴포넌트 활성화
        //trackedDeviceGraphicRaycaster = GameObject.FindAnyObjectByType<TrackedDeviceGraphicRaycaster>();
        trackedDeviceGraphicRaycaster.enabled = true;
        xrUIInputModule.enabled = true;
    }

    private void DeactivateProperties()
    {
        // CurvedUIInputModule 컴포넌트 비활성화
        //curvedUIInputModule = GameObject.FindAnyObjectByType<CurvedUIInputModule>();
        curvedUIInputModule.enabled = false;

        // CurvedUIRaycaster 컴포넌트 비활성화
        //curvedUIRaycaster = GameObject.FindAnyObjectByType<CurvedUIRaycaster>();
        curvedUIRaycaster = canvas.GetComponent<CurvedUIRaycaster>();
        curvedUIRaycaster.enabled = false;
    }

    private void BindingEvents()
    {
        nearFarInteractor.selectEntered.AddListener(OnSelectEntered);
        nearFarInteractor.selectExited.AddListener(OnSelectExited);

        nearFarInteractor.uiHoverEntered.AddListener(OnUIHoverEntered);
        nearFarInteractor.uiHoverExited.AddListener(OnUIHoverExited);
    }

    private void OnUIHoverExited(UIHoverEventArgs obj)
    {
        selectedObject = null;
    }

    private void OnUIHoverEntered(UIHoverEventArgs obj)
    {
        selectedObject = obj.uiObject.gameObject;
    }

    private void OnSelectExited(SelectExitEventArgs arg0)
    {
        if (selectedObject != null)
        {
            SendMouseReleaseEvent(selectedObject);
        }

    }

    private void OnSelectEntered(SelectEnterEventArgs arg0)
    {
        if (selectedObject != null)
        {
            SendMouseClickEvent(selectedObject);
        }
    }

    private void OnHoverEntered(HoverEnterEventArgs obj)
    {
        selectedObject = obj.interactableObject.transform.gameObject;
    }

    // 마우스 클릭 이벤트 생성 및 전달
    private void SendMouseClickEvent(GameObject target)
    {
        // PointerEventData 생성
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1, // 마우스 왼쪽 버튼
            position = Input.mousePosition // 현재 마우스 위치
        };

        // 클릭 이벤트 전달
        ExecuteEvents.Execute(target, pointerEventData, ExecuteEvents.pointerClickHandler);
    }

    // 마우스 릴리즈 이벤트 생성 및 전달
    private void SendMouseReleaseEvent(GameObject target)
    {
        // PointerEventData 생성
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1, // 마우스 왼쪽 버튼
            position = Input.mousePosition // 현재 마우스 위치
        };

        // 클릭 이벤트 전달
        ExecuteEvents.Execute(target, pointerEventData, ExecuteEvents.pointerUpHandler);
    }
}