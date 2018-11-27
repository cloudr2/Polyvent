using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance = null;
    [HideInInspector]
    public UIButton shootButton, jumpButton, grenadeButton;
    private Image hpBar;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SetHPBar(float value) {
        hpBar.rectTransform.localScale = new Vector3(value, 1, 1);
    }

    public void SetHPBar() {
        hpBar.rectTransform.localScale = Vector3.one;
    }

    public void GetUIObjects() {
        shootButton = GameObject.Find("ShootButton").GetComponent<UIButton>();
        jumpButton = GameObject.Find("JumpButton").GetComponent<UIButton>();
        grenadeButton = GameObject.Find("GrenadeButton").GetComponent<UIButton>();
        hpBar = GameObject.Find("currentHp").GetComponent<Image>();
    }
}
