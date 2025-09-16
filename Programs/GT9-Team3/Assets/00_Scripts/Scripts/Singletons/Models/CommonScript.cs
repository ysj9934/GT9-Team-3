using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CommonScript
{
    /// <summary>
    /// 특정 숫자 가져오기
    /// </summary>
    /// <param name="number">타겟 번호</param>
    /// <param name="position">타겟 위치</param>
    /// <returns></returns>
    public int getDigit(int targetNumber, int targetPosition)
    {
        string numStr = targetNumber.ToString();
        int position = numStr.Length - targetPosition;
        if (position < 0 || position >= numStr.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(position), "타겟 위치가 잘 못 되었습니다.");
        }

        return int.Parse(numStr[position].ToString());
    }

}
