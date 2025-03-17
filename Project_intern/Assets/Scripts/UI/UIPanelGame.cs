using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPanelGame : MonoBehaviour,IMenu
{
    public Text LevelConditionView;

    [SerializeField] private Button btnPause;
    [SerializeField] private Button btnAutoWin;
    [SerializeField] private Button btnAutoLose;
    public static UnityAction eventAutoWin;
    public static UnityAction eventAutoLose;

    private UIMainManager m_mngr;

    private void Awake()
    {
        btnPause.onClick.AddListener(OnClickPause);
        btnAutoWin.onClick.AddListener(OnclickWin);
        btnAutoLose.onClick.AddListener(OnClickLose);
    }

    private void OnclickWin()
    {
        eventAutoWin.Invoke();
    }
    private void OnClickLose()
    {
        eventAutoLose.Invoke();
    }
    private void OnClickPause()
    {
        m_mngr.ShowPauseMenu();
    }

    public void Setup(UIMainManager mngr)
    {
        m_mngr = mngr;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
