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

        private const float X_SIZE = 1;
        private const float Y_SIZE = X_SIZE * 1.5f;
        private readonly Vector2 _defaultCellSize = new Vector2(X_SIZE, Y_SIZE);
        private int _height = 0, _width = 0;
        private int _cardCount = 0;
        private Quaternion _rotation;
        private Grid<CardObject> _grid = null;
        private Hand _cachedHand = null;

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
                    float scale = _grid.CellSize.x;
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
            return createGrid(_height, _width, _defaultCellSize);
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
            float scale = _grid.CellSize.x;
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

        private IEnumerator scaleLerper(float duration, float targetRatio)
        {
            var newGrid = createGrid(_height, _width, _defaultCellSize * targetRatio);

            float oldRatio = _grid.CellSize.x / _defaultCellSize.x;
            float newRatio = newGrid.CellSize.x / _defaultCellSize.x;

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
                    break;
                }
            }
        }

        // Public methods

        public static CardDock CreateCardDock(Vector3 position, Hand hand)
        {
            var instance = new GameObject("CardDock", typeof(CardDock));
            var cardDock = instance.GetComponent<CardDock>();
            cardDock.transform.position = position;
            cardDock._cachedHand = hand;
            return cardDock;
        }

        public void AddCard(Card card)
        {
            while (true)
            {
                if (_cardCount == gridCapacity())
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
                _cachedHand?.AddCard(card);
                ++_cardCount;
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
            _cachedHand?.RemoveCard(card);
            _cardCount = Mathf.Max(0, _cardCount - 1);
            cleanupGrid(x, y);
        }

        public void ClearDock()
        {
            _cardCount = 0;
            for (int y = 0; y < _grid.Width; ++y)
                for (int x = 0; x < _grid.Height; ++y)
                {
                    var instance = _grid.GetValue(x, y);
                    Destroy(instance.gameObject);
                }
            _grid = null;
            _cachedHand.TruncateHand();
            updateCardGrid();
        }

        public void ApplyRotation(float rotation, Vector3 rotationAxis)
        {
            rotationAxis *= rotation;
            _rotation = Quaternion.Euler(rotationAxis.x, rotationAxis.y, rotationAxis.z);
            updateCardGrid();
        }

        public void LerpScale(float duration, float targetRatio) => StartCoroutine(scaleLerper(duration, targetRatio));
    }
}