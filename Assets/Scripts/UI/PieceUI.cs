using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PieceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI normalText;
    [SerializeField] private TextMeshProUGUI heliumText;
    [SerializeField] private TextMeshProUGUI reinforcedText;

    private void OnEnable()
    {
        PieceManager.Instance.OnPieceCountChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        PieceManager.Instance.OnPieceCountChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        normalText.text = $"기본 조각 : {PieceManager.Instance.GetPieceCount(ItemType.NormalPiece).ToString()}";
        heliumText.text = $"헬륨 조각 : {PieceManager.Instance.GetPieceCount(ItemType.HeliumPiece).ToString()}";
        reinforcedText.text = $"강화 조각 : {PieceManager.Instance.GetPieceCount(ItemType.ReinforcedPiece).ToString()}";
    }
}
