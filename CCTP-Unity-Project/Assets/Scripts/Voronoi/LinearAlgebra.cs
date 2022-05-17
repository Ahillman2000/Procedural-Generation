using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearAlgebra
{
    public static bool AreLinesIntersecting(Vector2 line1_point1, Vector2 line1_point2, Vector2 line2_point1, Vector2 line2_point2, bool shouldIncludeEndPoints)
    {
        bool isIntersecting = false;

        float denominator = (line2_point2.y - line2_point1.y) * (line1_point2.x - line1_point1.x) - (line2_point2.x - line2_point1.x) * (line1_point2.y - line1_point1.y);

        if (denominator != 0f)
        {
            float u_a = ((line2_point2.x - line2_point1.x) * (line1_point1.y - line2_point1.y) - (line2_point2.y - line2_point1.y) * (line1_point1.x - line2_point1.x)) / denominator;
            float u_b = ((line1_point2.x - line1_point1.x) * (line1_point1.y - line2_point1.y) - (line1_point2.y - line1_point1.y) * (line1_point1.x - line2_point1.x)) / denominator;

            if (shouldIncludeEndPoints)
                if (u_a >= 0f && u_a <= 1f && u_b >= 0f && u_b <= 1f) isIntersecting = true;

                else
                    if (u_a > 0f && u_a < 1f && u_b >= 0f && u_b <= 1f) isIntersecting = true;
        }

        return isIntersecting;
    }
}
