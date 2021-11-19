using System.Collections;
using UnityEngine;
using FickleFrameGames.Systems;
using System.Collections.Generic;
using Cameo.NonMono;


namespace Cameo.Mono
{
    public class CardDock : MonoBehaviour
    {
        // Private fields

        private readonly Vector2 _defaultCellSize = new Vector2(1, 1.5f);
        private int _height = 0, _width = 0;
        private Quaternion _rotation;
        private Vector2 _cellSize = new Vector2(1, 1.5f);
        private List<Card> _cachedCards = new List<Card>();
        private Grid<CardObject> _grid = null;

        // Private methods

        private void updateCardGrid(bool shouldCreateGrid = false)
        {
            // Updating the grid alone
            if (shouldCreateGrid)
            {
                if (_grid == null)
                    _grid = createGrid();
                else
                    _grid = mergeGrid(createGrid());
            }

            // Update all cards inside the grid
            for (int y = 0; y < _grid.Height; ++y)
                for (int x = 0; x < _grid.Width; ++x)
                {
                    Quaternion rotation = _rotation;
                    Vector3 position = getCardPosition(x, y);
                    float scale = _grid.CellSize.magnitude / _defaultCellSize.magnitude;
                    var instance = _grid.GetValue(x, y);
                    if (instance)
                    {
                        instance.transform.rotation = rotation;
                        instance.transform.position = position;
                        instance.transform.localScale = new Vector3(scale, scale, 0);
                    }
                }
        }

        private Grid<CardObject> createGrid()
        {
            return createGrid(_height, _width, _cellSize);
        }

        private Grid<CardObject> createGrid(int height, int width, Vector2 cellSize)
        {
            Vector3 gridPosition = transform.position - new Vector3(cellSize.x * width / 2, cellSize.y * height / 2);
            return new Grid<CardObject>(gridPosition, height, width, cellSize, false, false);
        }

        private Grid<CardObject> mergeGrid(Grid<CardObject> newGrid)
        {
            int height = Mathf.Min(newGrid.Height, _grid.Height);
            int width = Mathf.Min(newGrid.Width, _grid.Width);
            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                    newGrid.SetValue(j, i, _grid.GetValue(j, i));
            _grid = newGrid;
            return newGrid;
        }

        private Vector3 getCardPosition(int x, int y, Grid<CardObject> grid = null)
        {
            if (grid == null)
                grid = _grid;
            var rawPosition = grid.GetWorldPosition(x, y) + new Vector3(grid.CellSize.x / 2, grid.CellSize.y / 2);
            var position = (_rotation * rawPosition.normalized) * rawPosition.magnitude;
            return position;
        }

        private CardObject createCardInstance(Vector3 position, Card card)
        {
            var instance = CardObject.CreateNewCardObject(card);
            float scale = _grid.CellSize.magnitude / _defaultCellSize.magnitude;
            instance.transform.SetParent(transform);
            instance.transform.position = _rotation * position.normalized * position.magnitude;
            instance.transform.rotation = _rotation;
            instance.transform.localScale = new Vector3(scale, scale);
            return instance;
        }

        private int gridCapacity()
        {
            return _height * _width;
        }

        private Vector2 getAvailableSlot()
        {
            if (_grid == null)
                _grid = createGrid();

            for (int i = 0; i < _grid.Height; ++i)
                for (int j = 0; j < _grid.Width; ++j)
                    if (_grid.GetValue(j, i) == false)
                        return new Vector2(j, i);
            return default;
        }

        private Vector2 findCard(Card card)
        {
            for (int i = 0; i < _grid.Height; ++i)
                for (int j = 0; j < _grid.Width; ++j)
                    if (_grid.GetValue(j, i)?.Card == card)
                        return new Vector2(j, i);
            return new Vector2(-1, 0);
        }

        private void cleanupGrid(int x, int y)
        {
            bool flag = true;
            for (int i = 0; i < _grid.Height; ++i)
                flag &= !_grid.GetValue(x, i);
            if (flag == true)
                truncateGridColumn(x);
            for (int i = 0; i < _grid.Width; ++i)
                flag &= !_grid.GetValue(i, y);
            if (flag == true)
                truncateGridRow(y);
        }

        private void truncateGridColumn(int X)
        {
            _width = Mathf.Max(0, _width - 1);
            var grid = createGrid();

            for (int y = 0; y < grid.Height; ++y)
            {
                for (int x = 0; x < grid.Width; ++x)
                {
                    if (x >= X)
                    {
                        grid.SetValue(x, y, _grid.GetValue(x + 1, y));
                    }
                    else
                    {
                        grid.SetValue(x, y, _grid.GetValue(x, y));
                    }
                }
            }
            _grid = grid;
            updateCardGrid();
        }

        private void truncateGridRow(int Y)
        {
            _height = Mathf.Max(0, (_height - 1));
            var grid = createGrid();
            for (int y = 0; y < grid.Height; ++y)
            {
                for (int x = 0; x < grid.Width; ++x)
                {
                    if (y >= Y)
                    {
                        grid.SetValue(x, y, _grid.GetValue(x, y + 1));
                    }
                    else
                    {
                        grid.SetValue(x, y, _grid.GetValue(x, y));
                    }
                }
            }
            _grid = grid;
            updateCardGrid();
        }

        // Public methods

        public static CardDock CreateCardDock(Vector3 position)
        {
            var instance = new GameObject("CardDock", typeof(CardDock));
            instance.transform.position = position;
            return instance.GetComponent<CardDock>();
        }

        public void AddCard(Card card)
        {
            while (true)
            {
                if (_cachedCards.Count == gridCapacity())
                {
                    if (gridCapacity() == 0)
                    {
                        _height = 1;
                        _width = 1;
                    }
                    else if (_height != 2)
                        ++_height;
                    else
                        ++_width;
                    updateCardGrid(true);
                    continue;
                }
                var slot = getAvailableSlot();
                var instance = createCardInstance(getCardPosition((int)slot.x, (int)slot.y), card);
                _grid.SetValue((int)slot.x, (int)slot.y, instance);
                _cachedCards.Add(card);
                break;
            }
        }

        public void RemoveCard(Card card)
        {
            var slot = findCard(card);
            int x = (int)slot.x;
            int y = (int)slot.y;

            if (x == -1)
                return;

            var instance = _grid.GetValue(x, y);
            _grid.SetValue(x, y, null);
            Destroy(instance.gameObject);
            _cachedCards.Remove(card);
            cleanupGrid(x, y);
        }

        public void ApplyRotation(float rotation, Vector3 rotationAxis)
        {
            rotationAxis *= rotation; 
            _rotation = Quaternion.Euler(rotationAxis.x, rotationAxis.y, rotationAxis.z);
            updateCardGrid();
        }

        public IEnumerator LerpCellSize(float duration, float targetRatio)
        {
            var newGrid = createGrid(_height, _width, _defaultCellSize * targetRatio);

            float oldRatio = _grid.CellSize.magnitude / _defaultCellSize.magnitude;
            float newRatio = newGrid.CellSize.magnitude / _defaultCellSize.magnitude;

            Vector3 oldScale = new Vector3(oldRatio, oldRatio);
            Vector3 newScale = new Vector3(newRatio, newRatio);
            List<Vector3> oldPositions = new List<Vector3>();
            List<Vector3> newPositions = new List<Vector3>();

            // cache all positions
            for (int y = 0; y < _height; ++y)
                for (int x = 0; x < _width; ++x)
                {
                    newPositions.Add(getCardPosition(x, y, newGrid));
                    oldPositions.Add(getCardPosition(x, y, _grid));
                }

            float timer = 0;
            float ratio = 0;

            while (true)
            {
                yield return new WaitForEndOfFrame();

                ratio = Mathf.Clamp(timer / duration, 0, 1);
                timer += Time.deltaTime;

                // apply lerp
                int k = 0;
                for (int y = 0; y < _height; ++y)
                    for (int x = 0; x < _width; ++x)
                    {
                        var instance = _grid.GetValue(x, y);
                        if (instance != null)
                        {
                            instance.transform.position = Vector3.Lerp(oldPositions[k], newPositions[k], ratio);
                            instance.transform.localScale = Vector3.Lerp(oldScale, newScale, ratio);
                            instance.transform.rotation = _rotation;
                        }
                        ++k;
                    }

                if (ratio == 1)
                {
                    mergeGrid(newGrid);
                    _height = newGrid.Height;
                    _width = newGrid.Width;
                    _cellSize = newGrid.CellSize;
                    break;
                }
            }
        }
    }
}