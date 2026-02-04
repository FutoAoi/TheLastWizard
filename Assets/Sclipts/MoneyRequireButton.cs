using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyRequireButton : MonoBehaviour
{
    [Header("価格設定")]
    [SerializeField, Tooltip("初期価格")]
    private int _basePrice = 100;

    [SerializeField, Tooltip("購入ごとに上がる価格")]
    private int _priceIncrease = 50;

    [SerializeField, Tooltip("最大購入回数（1なら買い切り）")]
    private int _maxBuyCount = 1;

    [Header("UI")]
    [SerializeField, Tooltip("対象ボタン")]
    private Button _button;

    [SerializeField, Tooltip("価格表示テキスト（任意）")]
    private TMP_Text _priceText;
    [SerializeField, Tooltip("最終テキスト")]
    private string _buyText;

    // 現在の購入回数
    private int _currentBuyCount = 0;

    /// <summary>
    /// 今の購入価格
    /// </summary>
    private int CurrentPrice =>
        _basePrice + (_priceIncrease * _currentBuyCount);

    private void Start()
    {
        // ソウル変化を監視
        SoulManager.Instance.OnSoulChanged += UpdateButton;

        // 初期表示
        UpdateUI();
    }

    private void OnDestroy()
    {
        SoulManager.Instance.OnSoulChanged -= UpdateButton;
    }

    /// <summary>
    /// ボタンが押された時に呼ぶ
    /// </summary>
    public void OnClickBuy()
    {
        // すでに最大まで買っている
        if (_currentBuyCount >= _maxBuyCount)
        {
            return;
        }

        // ソウルが足りなければ何もしない
        if (!SoulManager.Instance.UseSoul(CurrentPrice))
        {
            return;
        }

        // 購入成功
        _currentBuyCount++;

        // ここに購入時の効果を書く
        OnPurchased();

        // UI更新
        UpdateUI();
    }

    /// <summary>
    /// 購入成功時の処理（必要なら拡張）
    /// </summary>
    protected virtual void OnPurchased()
    {
        Debug.Log($"購入！ 現在 {_currentBuyCount} 回目");
    }

    /// <summary>
    /// ボタンの有効状態更新
    /// </summary>
    private void UpdateButton(int currentSoul)
    {
        UpdateUI();
    }

    /// <summary>
    /// UI全体の更新
    /// </summary>
    private void UpdateUI()
    {
        // 最大購入済みなら完全に無効
        if (_currentBuyCount >= _maxBuyCount)
        {
            _button.interactable = false;

            if (_priceText != null)
            {
                _priceText.text = _buyText;
            }
            return;
        }

        // ソウルが足りているか
        bool canBuy = SoulManager.Instance.Soul >= CurrentPrice;
        _button.interactable = canBuy;

        // 価格表示
        if (_priceText != null)
        {
            _priceText.text = CurrentPrice + " Soul";
            
        }
    }
}