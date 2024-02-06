using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AutomaticLayoutFitter : MonoBehaviour
{
    public enum FitMode
    {
        Vertical,
        Horizontal,
        VerticalGridFit,
        HorizontalGridFit
    }

    public FitMode fitMode;
    public float Space;

    // Update is called once per frame
    void Update()
    {
        RectTransform self = GetComponent<RectTransform>();
        RectTransform[] childs = GetComponentsInChildren<RectTransform>();

        if (fitMode == FitMode.Vertical)
        {
            float vSize = 0;

            foreach(RectTransform r in transform)
            {
                if(r == self)
                {
                    
                }
                else
                {
                    if (r.gameObject.activeInHierarchy)
                    {
                        vSize += r.rect.height + Space;
                    }               
                }
            }
            self.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, vSize);
        }
        else if (fitMode == FitMode.Horizontal)
        {
            float hSize = 0;

            foreach (RectTransform r in transform)
            {
                if (r == self)
                {

                }
                else
                {
                    if (r.gameObject.activeInHierarchy)
                    {
                        hSize += r.rect.width + Space;
                    }
                }
            }
            self.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hSize);
        }
        else if(fitMode == FitMode.VerticalGridFit)
        {
            if(GetComponent<GridLayoutGroup>() == null)
            {
                gameObject.AddComponent<GridLayoutGroup>();
            }
            GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
            int gridRows = 0;
            int gridColumns = 0;
            GetColumnAndRow(grid, out gridRows, out gridColumns);
            self.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (grid.cellSize.y * gridRows) + (grid.spacing.y * gridRows));
        }
    }
    #region Misc
    void GetColumnAndRow(GridLayoutGroup glg, out int column, out int row)
    {
        column = 0;
        row = 0;

        if (glg.transform.childCount == 0)
            return;

        //Column and row are now 1
        column = 1;
        row = 1;

        //Get the first child GameObject of the GridLayoutGroup
        RectTransform firstChildObj = glg.transform.
            GetChild(0).GetComponent<RectTransform>();

        Vector2 firstChildPos = firstChildObj.anchoredPosition;
        bool stopCountingRow = false;

        //Loop through the rest of the child object
        for (int i = 1; i < glg.transform.childCount; i++)
        {
            //Get the next child
            RectTransform currentChildObj = glg.transform.
           GetChild(i).GetComponent<RectTransform>();

            Vector2 currentChildPos = currentChildObj.anchoredPosition;

            //if first child.x == otherchild.x, it is a column, ele it's a row
            if (firstChildPos.x == currentChildPos.x)
            {
                column++;
                //Stop couting row once we find column
                stopCountingRow = true;
            }
            else
            {
                if (!stopCountingRow)
                    row++;
            }
        }
    }
    #endregion
}
