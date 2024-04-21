using System;
using UnityEngine;

namespace stickin.mathcross
{
    [Serializable]
    public class Cell
    {
        public Vector2Int CorrectIndex;
        public Vector2Int CurrentIndex;
        
        public int Value;
        public bool IsLocked;
        public bool IsCorrect { get; private set; }

        public bool IsNumber => !IsSign && !IsEqual;
        public bool IsEqual => Value == MathCrossGame.SignEql;
        public bool IsSign => Value == MathCrossGame.SignAdd || 
                              Value == MathCrossGame.SignSub || 
                              Value == MathCrossGame.SignMul || 
                              Value == MathCrossGame.SignDiv;

        public event Action OnChange;
        
        public Cell(int value, Vector2Int correctIndex)
        {
            Value = value;
            CorrectIndex = correctIndex;
            CurrentIndex = correctIndex;
        }

        public void SetCurrentIndex(Vector2Int currentIndex)
        {
            CurrentIndex = currentIndex;
        }
        
        public void SetValue(int value)
        {
            Value = value;
        }

        public void SetIsCorrect(bool isCorrect)
        {
            IsCorrect = isCorrect;
            OnChange?.Invoke();
        }

        public string ValueString()
        {
            if (Value == MathCrossGame.SignAdd)
                return "+";
            
            if (Value == MathCrossGame.SignSub)
                return "-";
            
            if (Value == MathCrossGame.SignMul)
                return "x";
            
            if (Value == MathCrossGame.SignDiv)
                return "/";
            
            if (Value == MathCrossGame.SignEql)
                return "=";
            
            return Value.ToString();
        }
    }
}