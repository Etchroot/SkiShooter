using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using System;

public class AuthenticationManager : MonoBehaviour
{
    async void Start()
    {
        try
        {
            // Unity Authentication 서비스 초기화
            await UnityServices.InitializeAsync();

            // 로그인 처리 (자동으로 로그인하는 방식)
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            Debug.Log("로그인 성공: " + AuthenticationService.Instance.PlayerId);
        }
        catch (Exception e)
        {
            Debug.LogError("로그인 실패: " + e.Message);
        }
    }
}
