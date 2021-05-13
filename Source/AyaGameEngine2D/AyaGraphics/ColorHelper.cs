using System;

namespace AyaGameEngine2D
{
    /// <summary>
    /// 类      名：ColorHelper
    /// 功      能：提供从RGB到HSV/HSL色彩空间的相互转换
    /// 日      期：2015-02-08
    /// 修      改：2016-01-03
    /// 作      者：ls9512
    /// </summary>
    public static class ColorHelper
    {
        #region 颜色模型转换
        /// <summary>
        /// RGB转换HSV
        /// </summary>
        /// <param name="rgb">RGB</param>
        /// <returns>HSV</returns>
        public static ColorHSV RgbToHsv(ColorRGB rgb)
        {
            float s;
            float r = rgb.R * 1.0f / 255, G = rgb.G * 1.0f / 255, B = rgb.B * 1.0f / 255;
            float tmp = Math.Min(r, G);
            float min = Math.Min(tmp, B);
            tmp = Math.Max(r, G);
            float max = Math.Max(tmp, B);
            // H
            float H = 0;
            if (max == min)
            {
                H = 0;
            }
            else if (max == r && G > B)
            {
                H = 60 * (G - B) * 1.0f / (max - min) + 0;
            }
            else if (max == r && G < B)
            {
                H = 60 * (G - B) * 1.0f / (max - min) + 360;
            }
            else if (max == G)
            {
                H = H = 60 * (B - r) * 1.0f / (max - min) + 120;
            }
            else if (max == B)
            {
                H = H = 60 * (r - G) * 1.0f / (max - min) + 240;
            }
            // S
            if (max == 0)
            {
                s = 0;
            }
            else
            {
                s = (max - min) * 1.0f / max;
            }
            // V
            var V = max;
            return new ColorHSV((int)H, (int)(s * 255), (int)(V * 255));
        }

        /// <summary>
        /// HSV转换RGB
        /// </summary>
        /// <param name="hsv">HSV</param>
        /// <returns>RGB</returns>
        public static ColorRGB HsvToRgb(ColorHSV hsv)
        {
            if (hsv.H == 360) hsv.H = 359; // 360为全黑，原因不明
            float R = 0f, G = 0f, B = 0f;
            if (hsv.S == 0)
            {
                return new ColorRGB(hsv.V, hsv.V, hsv.V);
            }
            float S = hsv.S * 1.0f / 255, V = hsv.V * 1.0f / 255;
            int H1 = (int)(hsv.H * 1.0f / 60), H = hsv.H;
            float F = H * 1.0f / 60 - H1;
            float P = V * (1.0f - S);
            float Q = V * (1.0f - F * S);
            float T = V * (1.0f - (1.0f - F) * S);
            switch (H1)
            {
                case 0: R = V; G = T; B = P; break;
                case 1: R = Q; G = V; B = P; break;
                case 2: R = P; G = V; B = T; break;
                case 3: R = P; G = Q; B = V; break;
                case 4: R = T; G = P; B = V; break;
                case 5: R = V; G = P; B = Q; break;
            }
            R = R * 255;
            G = G * 255;
            B = B * 255;
            while (R > 255) R -= 255;
            while (R < 0) R += 255;
            while (G > 255) G -= 255;
            while (G < 0) G += 255;
            while (B > 255) B -= 255;
            while (B < 0) B += 255;
            return new ColorRGB((int)R, (int)G, (int)B);
        }

        ///// <summary>
        ///// RGB转换HSL
        ///// </summary>
        ///// <param name="rgb">RGB</param>
        ///// <returns>HSL</returns>
        //public static ColorHSL RgbToHsl(ColorRGB rgb)
        //{
        //    float min, max, tmp, H, S, L;
        //    float R = rgb.R * 1.0f / 255, G = rgb.G * 1.0f / 255, B = rgb.B * 1.0f / 255;
        //    tmp = Math.Min(R, G);
        //    min = Math.Min(tmp, B);
        //    tmp = Math.Max(R, G);
        //    max = Math.Max(tmp, B);
        //    // H
        //    H = 0;
        //    if (max == min)
        //    {
        //        H = 0;  // 此时H应为未定义，通常写为0
        //    }
        //    else if (max == R && G > B)
        //    {
        //        H = 60 * (G - B) * 1.0f / (max - min) + 0;
        //    }
        //    else if (max == R && G < B)
        //    {
        //        H = 60 * (G - B) * 1.0f / (max - min) + 360;
        //    }
        //    else if (max == G)
        //    {
        //        H = H = 60 * (B - R) * 1.0f / (max - min) + 120;
        //    }
        //    else if (max == B)
        //    {
        //        H = H = 60 * (R - G) * 1.0f / (max - min) + 240;
        //    }
        //    // L 
        //    L = 0.5f * (max + min);
        //    // S
        //    S = 0;
        //    if (L == 0 || max == min)
        //    {
        //        S = 0;
        //    }
        //    else if (0 < L && L < 0.5)
        //    {
        //        S = (max - min) / (L * 2);
        //    }
        //    else if (L > 0.5)
        //    {
        //        S = (max - min) / (2 - 2 * L);
        //    }
        //    return new ColorHSL((int)H, (int)(S * 255), (int)(L * 255));
        //}

        ///// <summary>
        ///// HSL转换RGB
        ///// </summary>
        ///// <param name="hsl">HSL</param>
        ///// <returns>RGB</returns>
        //public static ColorRGB HslToRgb(ColorHSL hsl)
        //{
        //    float R = 0f, G = 0f, B = 0f;
        //    float S = hsl.S * 1.0f / 255, L = hsl.L * 1.0f / 255;
        //    float temp1, temp2, temp3;
        //    if (S == 0f) // 灰色
        //    {
        //        R = L;
        //        G = L;
        //        B = L;
        //    }
        //    else
        //    {
        //        if (L < 0.5f)
        //        {
        //            temp2 = L * (1.0f + S);
        //        }
        //        else
        //        {
        //            temp2 = L + S - L * S;
        //        }
        //        temp1 = 2.0f * L - temp2;
        //        float H = hsl.H * 1.0f / 360;
        //        // R
        //        temp3 = H + 1.0f / 3.0f;
        //        if (temp3 < 0) temp3 += 1.0f;
        //        if (temp3 > 1) temp3 -= 1.0f;
        //        R = temp3;
        //        // G
        //        temp3 = H;
        //        if (temp3 < 0) temp3 += 1.0f;
        //        if (temp3 > 1) temp3 -= 1.0f;
        //        G = temp3;
        //        // B
        //        temp3 = H - 1.0f / 3.0f;
        //        if (temp3 < 0) temp3 += 1.0f;
        //        if (temp3 > 1) temp3 -= 1.0f;
        //        B = temp3;
        //    }
        //    R = R * 255;
        //    G = G * 255;
        //    B = B * 255;
        //    return new ColorRGB((int)R, (int)G, (int)B);
        //} 
        #endregion
    }
}
