using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public event Action OnMoveEvent = delegate { };

    public bool IsBusy { get; private set; }

    private Board m_board;

    private GameManager m_gameManager;

    private bool m_isDragging;

    private Camera m_cam;

    private Collider2D m_hitCollider;

    private GameSettings m_gameSettings;

    private List<Cell> m_potentialMatch;

    private float m_timeAfterFill;

    private bool m_hintIsShown;

    private bool m_gameOver;

    public void StartGame(GameManager gameManager, GameSettings gameSettings)
    {
        m_gameManager = gameManager;

        m_gameSettings = gameSettings;

        m_gameManager.StateChangedAction += OnGameStateChange;

        m_cam = Camera.main;

        m_board = new Board(this.transform, gameSettings);

        Fill();
    }

    private void Fill()
    {
        m_board.Fill();
        //FindMatchesAndCollapse();
    }

    private void OnGameStateChange(GameManager.eStateGame state)
    {
        switch (state)
        {
            case GameManager.eStateGame.GAME_STARTED:
                IsBusy = false;
                break;
            case GameManager.eStateGame.PAUSE:
                IsBusy = true;
                break;
            case GameManager.eStateGame.GAME_OVER:
                m_gameOver = true;
                //StopHints();
                break;
        }
    }


    public void Update()
    {
        if (m_gameOver) return;
        if (IsBusy) return;

        //if (!m_hintIsShown)
        //{
        //    m_timeAfterFill += Time.deltaTime;
        //    if (m_timeAfterFill > m_gameSettings.TimeForHint)
        //    {
        //        m_timeAfterFill = 0f;
        //        ShowHint();
        //    }
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    var hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        //    if (hit.collider != null)
        //    {
        //        m_isDragging = true;
        //        m_hitCollider = hit.collider;
        //    }
        //}

        //if (Input.GetMouseButtonUp(0))
        //{
        //    ResetRayCast();
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_gameManager.SetState(GameManager.eStateGame.GAME_WIN);
        }
        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.Raycast(m_cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                m_hitCollider = hit.collider;
                Cell c1 = m_hitCollider.GetComponent<Cell>();
                Cell c2 = null;
                List<Cell> cells = m_board.GetRowCollectedItem();
                for (int i = 0; i < cells.Count; i++)
                {
                    if (cells[i].Item == null)
                    {
                        c2 = cells[i];
                        break;
                    }
                }
                //if (AreItemsNeighbor(c1, c2))
                //{
                IsBusy = true;
                //SetSortingLayer(c1, c2);
                m_board.Swap(c1, c2, () =>
                {
                    AfterMatches();
                    //FindMatchesAndCollapse(c1, c2);
                });
                //}

            }
        }
    }
    public void AfterMatches()
    {

        IsBusy = false;
        int tmp = 0;
        List<Cell> cells = new List<Cell>();
        List<Cell> row_Collected_item = m_board.GetRowCollectedItem();
        for (int i = 0; i < row_Collected_item.Count; i++)
        {
            if (row_Collected_item[i].Item.nameItem == row_Collected_item[i + 1].Item.nameItem)
            {
                tmp++;
                cells.Add(row_Collected_item[i]);
                if (tmp == 2)
                {
                    cells.Add(row_Collected_item[i + 1]);
                    for (int j = 0; j < cells.Count; j++)
                    {
                        cells[j].ExplodeItem();
                    }
                    tmp = 0;
                    cells.Clear();
                    for (int j = 0; j < row_Collected_item.Count; j++)
                    {
                        if (row_Collected_item[j].Item == null)
                        {
                            for (int k = j; k < row_Collected_item.Count; k++)
                            {
                                if (row_Collected_item[k].Item != null)
                                {
                                    Item item = row_Collected_item[k].Item;
                                    row_Collected_item[k].Free();
                                    row_Collected_item[j].Assign(item);
                                    item.View.DOMove(row_Collected_item[j].transform.position, 0.3f);
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
            }
            else
            {
                tmp = 0;
                cells.Clear();
            }
            if (i == row_Collected_item.Count - 2&& row_Collected_item[row_Collected_item.Count-1].Item != null)
            {
                m_gameManager.SetState(GameManager.eStateGame.GAME_OVER);
            }
        }

    }
    private void ResetRayCast()
    {
        m_isDragging = false;
        m_hitCollider = null;
    }

    private void FindMatchesAndCollapse(Cell cell1, Cell cell2)
    {
        if (cell1.Item is BonusItem)
        {
            cell1.ExplodeItem();
            StartCoroutine(ShiftDownItemsCoroutine());
        }
        else if (cell2.Item is BonusItem)
        {
            cell2.ExplodeItem();
            StartCoroutine(ShiftDownItemsCoroutine());
        }
        else
        {
            List<Cell> cells1 = GetMatches(cell1);
            List<Cell> cells2 = GetMatches(cell2);

            List<Cell> matches = new List<Cell>();
            matches.AddRange(cells1);
            matches.AddRange(cells2);
            matches = matches.Distinct().ToList();

            //if (matches.Count < m_gameSettings.MatchesMin)
            //{
            //    m_board.Swap(cell1, cell2, () =>
            //    {
            //        IsBusy = false;
            //    });
            //}
            //else
            //{
            OnMoveEvent();

            CollapseMatches(matches, cell2);
            //}
        }
    }

    private void FindMatchesAndCollapse()
    {
        List<Cell> matches = m_board.FindFirstMatch();

        if (matches.Count > 0)
        {
            CollapseMatches(matches, null);
        }
        else
        {
            m_potentialMatch = m_board.GetPotentialMatches();
            if (m_potentialMatch.Count > 0)
            {
                IsBusy = false;

                m_timeAfterFill = 0f;
            }
            else
            {
                //StartCoroutine(RefillBoardCoroutine());
                StartCoroutine(ShuffleBoardCoroutine());
            }
        }
    }

    private List<Cell> GetMatches(Cell cell)
    {
        List<Cell> listHor = m_board.GetHorizontalMatches(cell);
        if (listHor.Count < m_gameSettings.MatchesMin)
        {
            listHor.Clear();
        }

        List<Cell> listVert = m_board.GetVerticalMatches(cell);
        if (listVert.Count < m_gameSettings.MatchesMin)
        {
            listVert.Clear();
        }

        return listHor.Concat(listVert).Distinct().ToList();
    }

    private void CollapseMatches(List<Cell> matches, Cell cellEnd)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            matches[i].ExplodeItem();
        }

        if (matches.Count > m_gameSettings.MatchesMin)
        {
            m_board.ConvertNormalToBonus(matches, cellEnd);
        }

        StartCoroutine(ShiftDownItemsCoroutine());
    }

    private IEnumerator ShiftDownItemsCoroutine()//
    {
       //m_board.ShiftDownItems();

        yield return new WaitForSeconds(0.2f);

        //m_board.FillGapsWithNewItems();

        yield return new WaitForSeconds(0.2f);

        //FindMatchesAndCollapse();
    }

    private IEnumerator RefillBoardCoroutine()
    {
        m_board.ExplodeAllItems();

        yield return new WaitForSeconds(0.2f);

        m_board.Fill();

        yield return new WaitForSeconds(0.2f);

        FindMatchesAndCollapse();
    }

    private IEnumerator ShuffleBoardCoroutine()
    {
        m_board.Shuffle();

        yield return new WaitForSeconds(0.3f);

        FindMatchesAndCollapse();
    }


    private void SetSortingLayer(Cell cell1, Cell cell2)
    {
        if (cell1.Item != null) cell1.Item.SetSortingLayerHigher();
        if (cell2.Item != null) cell2.Item.SetSortingLayerLower();
    }

    private bool AreItemsNeighbor(Cell cell1, Cell cell2)
    {
        return cell1.IsNeighbour(cell2);
    }

    internal void Clear()
    {
        m_board.Clear();
    }

    private void ShowHint()
    {
        m_hintIsShown = true;
        foreach (var cell in m_potentialMatch)
        {
            cell.AnimateItemForHint();
        }
    }

    private void StopHints()
    {
        m_hintIsShown = false;
        foreach (var cell in m_potentialMatch)
        {
            cell.StopHintAnimation();
        }

        m_potentialMatch.Clear();
    }
}
