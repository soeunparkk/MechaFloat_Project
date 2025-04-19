using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 풍선 조각을 관리하고, 조각 3개를 모으면 해당 풍선을 생성해 인벤토리에 추가하는 매니저.
/// </summary>
public class PieceManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static PieceManager Instance;

    // 아이템 데이터베이스 (ScriptableObject)
    public ItemDatabaseSO itemDatabase;

    // 각 풍선 타입에 대응하는 프리팹
    public GameObject normalBalloonPrefab;
    public GameObject heliumBalloonPrefab;
    public GameObject reinforcedBalloonPrefab;

    // 각 풍선 조각 개수
    private int normalPieceCount = 0;
    private int heliumPieceCount = 0;
    private int reinforcedPieceCount = 0;

    // 조각 개수가 바뀔 때 호출되는 이벤트
    public event Action OnPieceCountChanged;

    private void Awake()
    {
        // 싱글톤 설정
        Instance = this;
    }

    /// <summary>
    /// 풍선 조각을 1개 추가하고, 3개가 되면 해당 풍선을 업그레이드하여 생성.
    /// </summary>
    public void AddPiece(ItemType pieceType)
    {
        switch (pieceType)
        {
            case ItemType.NormalPiece:
                normalPieceCount++;
                if (normalPieceCount == 3) UpgradeBalloon(ItemType.NormalBalloon);
                break;
            case ItemType.HeliumPiece:
                heliumPieceCount++;
                if (heliumPieceCount == 3) UpgradeBalloon(ItemType.HeliumBalloon);
                break;
            case ItemType.ReinforcedPiece:
                reinforcedPieceCount++;
                if (reinforcedPieceCount == 3) UpgradeBalloon(ItemType.ReinforcedBalloon);
                break;
        }

        // UI 등 외부에 알림
        OnPieceCountChanged?.Invoke();
    }

    /// <summary>
    /// 조각이 3개 모였을 때 해당 풍선을 생성하고 인벤토리에 추가.
    /// </summary>
    private void UpgradeBalloon(ItemType balloonType)
    {
        // 풍선 데이터 가져오기
        ItemSO balloonData = itemDatabase.GetItemByType(balloonType).FirstOrDefault();
        if (balloonData == null)
        {
            Debug.LogError($"해당 풍선 타입에 대한 데이터가 없습니다: {balloonType}");
            return;
        }

        // 풍선 프리팹 가져오기
        GameObject prefab = GetBalloonPrefab(balloonType);
        if (prefab == null)
        {
            Debug.LogError($"프리팹이 설정되지 않았습니다: {balloonType}");
            return;
        }

        // 프리팹 인스턴스 생성 (즉시 활성화 방지)
        GameObject balloonObj = Instantiate(prefab);
        balloonObj.SetActive(false);
        balloonObj.transform.SetParent(null);

        // BalloonController 설정
        var balloon = balloonObj.GetComponent<BalloonController>();
        if (balloon == null)
        {
            Debug.LogError("BalloonController가 프리팹에 없습니다.");
            return;
        }

        // 데이터 설정
        balloon.balloonData = balloonData;
        balloon.currentHP = balloonData.maxHP;

        // 인벤토리에 추가
        bool added = InventoryManager.Instance.AddToInventory(balloon);
        Debug.Log($"인벤토리 추가 결과: {added}");

        // 조각 개수 초기화
        switch (balloonType)
        {
            case ItemType.NormalBalloon:
                normalPieceCount = 0;
                break;
            case ItemType.HeliumBalloon:
                heliumPieceCount = 0;
                break;
            case ItemType.ReinforcedBalloon:
                reinforcedPieceCount = 0;
                break;
        }

        // UI 등 외부에 알림
        OnPieceCountChanged?.Invoke();
    }

    /// <summary>
    /// 풍선 타입에 맞는 프리팹 반환.
    /// </summary>
    private GameObject GetBalloonPrefab(ItemType balloonType)
    {
        return balloonType switch
        {
            ItemType.NormalBalloon => normalBalloonPrefab,
            ItemType.HeliumBalloon => heliumBalloonPrefab,
            ItemType.ReinforcedBalloon => reinforcedBalloonPrefab,
            _ => null
        };
    }

    /// <summary>
    /// 특정 조각의 현재 개수를 반환.
    /// </summary>
    public int GetPieceCount(ItemType pieceType)
    {
        return pieceType switch
        {
            ItemType.NormalPiece => normalPieceCount,
            ItemType.HeliumPiece => heliumPieceCount,
            ItemType.ReinforcedPiece => reinforcedPieceCount,
            _ => 0
        };
    }
}
