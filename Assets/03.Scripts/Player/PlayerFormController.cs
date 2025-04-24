using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPusher
{
    float GetPushPower();
}

public class PlayerFormController : MonoBehaviour, IWeightable, IPusher
{
    [SerializeField, Tooltip("형태변환 데이터 모음집")] private PlayerFormData formData;
    private Dictionary<string, FormData> formDataDictionary = new Dictionary<string, FormData>();

    private PlayerController controller;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private FormData curFormData;
    public FormData CurFormData { get { return  curFormData; } }

    public void Init(Player player, string formName)
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        SetFormData();
        controller = player.Controller;
    }

    void SetFormData()
    {
        for(int i = 0; i < formData.PlayerFromDataList.Count; i++)
        {
            formDataDictionary.Add(formData.PlayerFromDataList[i].FormName, formData.PlayerFromDataList[i]);
        }
    }

    // 형태변화 함수
    // 플레이어 캐릭터의 여러 속성을 변경해준다
    public void FormChange(string formName)
    {
        FormData nextFormData = formDataDictionary[formName];
        
        if (nextFormData == null || !nextFormData.IsActive || !controller.IsControllable 
            || (curFormData != null && !controller.IsGround)) return;

        if (curFormData == null)
        {
            curFormData = nextFormData;
        }
        else
        {
            if (formName == curFormData.FormName)
            {
                curFormData = formDataDictionary["Human"];
            }
            else
            {
                curFormData = nextFormData;
            }
        }

        spriteRenderer.sprite = curFormData.FormImage;
        boxCollider.offset = new Vector2(curFormData.OffsetX, curFormData.OffsetY);
        boxCollider.size = new Vector2(curFormData.SizeX, curFormData.SizeY);
        controller.JumpForce = curFormData.JumpForce;
    }

    // 스프라이트 렌더러 플립
    public void FlipControl(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            if (curFormData.Direction == DefaultDirection.Right)
            {
                spriteRenderer.flipX = dir.x < 0;
            }
            else
            {
                spriteRenderer.flipX = dir.x > 0;
            }
        }
    }

    /// <summary>
    /// 돌과 사람을 제외한 모든 형태의 해금 상태를 반환
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, bool> GetAllFormLock()
    {
        Dictionary<string, bool> activeDic =
            new Dictionary<string, bool>()
            {
                {"Squirrel", formDataDictionary["Squirrel"].IsActive },
                {"Dog", formDataDictionary["Dog"].IsActive },
                {"Cat", formDataDictionary["Cat"].IsActive },
                {"Hide", formDataDictionary["Hide"].IsActive },
            };

        return activeDic;
    }

    // 형태의 해금 상태를 반환
    public bool GetFormLock(string formName)
    {
        bool active = formDataDictionary[formName].IsActive;
        return active;
    }

    // 형태의 해금 상태를 변경
    public void SetFormActive(string formName)
    {
        formDataDictionary[formName].IsActive = !formDataDictionary[formName].IsActive;
    }

    // 현재 형태의 이름을 반환
    public string ReturnCurFormName()
    {
        return curFormData.FormName;
    }

    public float GetWeight()
    {
        return curFormData.Weight;
    }

    public float GetPushPower()
    {
        return curFormData.Force;
    }
}