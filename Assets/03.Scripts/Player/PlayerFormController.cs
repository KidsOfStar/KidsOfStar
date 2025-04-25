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
    [SerializeField, Tooltip("이펙트 재생용 오브젝트")] private GameObject formChangeEffectObj;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Dictionary<string, FormData> formDataDictionary = new Dictionary<string, FormData>();

    private PlayerController controller;
    private BoxCollider2D boxCollider;
    private FormData curFormData;
    public FormData CurFormData { get { return  curFormData; } }

    // 변신 이펙트 재생 시간
    private float fxDuration;

    public void Init(Player player, string formName)
    {
        boxCollider = GetComponent<BoxCollider2D>();
        SetFormData();
        controller = player.Controller;
        Animator fxAnim = formChangeEffectObj.GetComponent<Animator>();
        fxDuration = fxAnim.runtimeAnimatorController.animationClips[0].length;
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

        StartCoroutine(FormChangeSequence());
    }

    // 변신 이펙트 재생
    private IEnumerator FormChangeSequence()
    {
        controller.LockPlayer();
        spriteRenderer.enabled = false;

        formChangeEffectObj.SetActive(true);
        // 애니메이션 재생 상태 확보를 위한 한 프레임 대기
        yield return null;
        // 이펙트 재생 시간만큼 대기
        yield return new WaitForSeconds(fxDuration);

        formChangeEffectObj.SetActive(false);
        controller.IsControllable = true;
        spriteRenderer.sprite = curFormData.FormImage;
        boxCollider.offset = new Vector2(curFormData.OffsetX, curFormData.OffsetY);
        boxCollider.size = new Vector2(curFormData.SizeX, curFormData.SizeY);
        controller.JumpForce = curFormData.JumpForce;
        controller.Anim.runtimeAnimatorController = curFormData.FormAnim;
        spriteRenderer.enabled = true;
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